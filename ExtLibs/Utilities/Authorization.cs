using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Net.NetworkInformation;

namespace MissionPlanner.Utilities
{
    /// <summary>
    /// LAI GCS / TitanPlanner 本地 RSA 授权系统（纯离线）。
    /// 激活码格式：Base64(有效小时数|随机数|RSA-SHA1签名(机器码|有效小时数|随机数))
    /// license.dat：第1行机器码 / 第2行激活码 / 第3行激活时间(yyyy-MM-dd HH:mm:ss)
    /// revoked.dat：已作废激活码的 MD5 黑名单（每行一个）
    /// </summary>
    public static class Authorization
    {
        // 公钥（对应 LicenseGenerator/private_key.xml，私钥绝不入库）
        private const string PublicKeyXml =
            "<RSAKeyValue><Modulus>9pmjx0PI3XkYx+Lm3xlBk/HiXJBwIwPfgNZjWikM0NSC4rF32xfjx5t3X0d0mWkftzdYyAlw2Q6bq5xoNMC18BEF8AHrjN6DKKnqExzqKVGyCyLysh78/fwCz8E4iNJbmKD+L7fkungk7cU8tQli1MBrPfAzzrEC7uz0xkdTaMKM8FNjlCEAmRUu7pPKZ+ppq0vzeJqWmyXnspJDGPDzekW1nMI/d8DLROELWGKDiGVb+wINgMVGrY9j6bLXMIlBF+oAMZWUnspYizHJsY1dOvgxmJE8PLMlWfDp6Ao+wEwsGuh74whZ27OCoU5ZKMhT0Ria+93Ag+fZgPxEhCBi/Q==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        private static string DataDir =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LAIGCS");

        private static string LicenseFile => Path.Combine(DataDir, "license.dat");
        private static string RevokedFile => Path.Combine(DataDir, "revoked.dat");
        private static string UsedFile => Path.Combine(DataDir, "used.dat");

        /// <summary>生成机器码：MAC + CPU核数 + 机器名 → MD5 → XXXX-XXXX-XXXX</summary>
        public static string GetMachineCode()
        {
            string raw = "";

            // MAC 地址（第一个可用网卡）
            try
            {
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                    if (nic.OperationalStatus != OperationalStatus.Up) continue;
                    byte[] mac = nic.GetPhysicalAddress().GetAddressBytes();
                    if (mac.Length > 0)
                    {
                        raw += BitConverter.ToString(mac);
                        break;
                    }
                }
            }
            catch { }

            // CPU 逻辑核数
            try { raw += Environment.ProcessorCount.ToString(); } catch { }

            // 机器名
            try { raw += Environment.MachineName; } catch { }

            using (var md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(raw));
                string hex = BitConverter.ToString(hash).Replace("-", "").ToUpper();
                return $"{hex.Substring(0, 4)}-{hex.Substring(4, 4)}-{hex.Substring(8, 4)}";
            }
        }

        /// <summary>尝试用激活码激活。返回 (成功?, 错误信息)</summary>
        public static (bool ok, string error) Activate(string activationCode)
        {
            if (string.IsNullOrWhiteSpace(activationCode))
                return (false, "激活码为空");

            string machine = GetMachineCode();

            try
            {
                byte[] bytes = Convert.FromBase64String(activationCode.Trim());
                string payload = Encoding.UTF8.GetString(bytes);
                string[] parts = payload.Split('|');
                // 格式：小时数|随机数|签名Base64
                if (parts.Length != 3)
                    return (false, "激活码格式错误");

                if (!int.TryParse(parts[0], out int hours))
                    return (false, "激活码小时数无效");

                string nonce = parts[1];
                byte[] signature = Convert.FromBase64String(parts[2]);

                // 验签：签名内容 = 机器码|小时数|随机数
                string signedContent = $"{machine}|{hours}|{nonce}";

                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(PublicKeyXml);
                    if (!rsa.VerifyData(Encoding.UTF8.GetBytes(signedContent), CryptoConfig.MapNameToOID("SHA1"), signature))
                        return (false, "激活码签名验证失败");
                }

                // 检查黑名单（已撤销）
                if (IsRevoked(activationCode.Trim()))
                    return (false, "该激活码已被作废");

                // 检查一次性：该激活码是否已被使用过
                if (IsUsed(activationCode.Trim()))
                    return (false, "该激活码已被使用，不可重复使用");

                // 写 license.dat（第3行=已用秒数，从0开始累计）
                Directory.CreateDirectory(DataDir);
                File.WriteAllText(LicenseFile,
                    machine + Environment.NewLine +
                    activationCode.Trim() + Environment.NewLine +
                    "0",
                    new UTF8Encoding(false));

                // 一次性：把激活码记入 used.dat，防止重复使用
                MarkAsUsed(activationCode.Trim());

                return (true, "激活成功");
            }
            catch (FormatException)
            {
                return (false, "激活码不是合法 Base64");
            }
            catch (Exception ex)
            {
                return (false, "激活失败：" + ex.Message);
            }
        }

        /// <summary>每次软件启动时调用：把当前时间写为本次倒计时起点。</summary>
        public static void RefreshStartTime()
        {
            try
            {
                if (!File.Exists(LicenseFile)) return;
                string[] lines = File.ReadAllLines(LicenseFile);
                if (lines.Length < 2) return;
                // 保留机器码和激活码，仅更新第3行为本次启动时间
                File.WriteAllText(LicenseFile,
                    lines[0] + Environment.NewLine +
                    lines[1] + Environment.NewLine +
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    new UTF8Encoding(false));
            }
            catch { }
        }

        /// <summary>撤销当前授权：把当前激活码加入黑名单，下次启动即失效</summary>
        public static void Revoke()
        {
            try
            {
                if (!File.Exists(LicenseFile)) return;
                string[] lines = File.ReadAllLines(LicenseFile);
                if (lines.Length < 2) return;
                string code = lines[1].Trim();

                Directory.CreateDirectory(DataDir);
                string hash = Md5Of(code);
                var list = new System.Collections.Generic.HashSet<string>();
                if (File.Exists(RevokedFile))
                    foreach (var l in File.ReadAllLines(RevokedFile))
                        if (!string.IsNullOrWhiteSpace(l)) list.Add(l.Trim());
                list.Add(hash);
                File.WriteAllLines(RevokedFile, list, new UTF8Encoding(false));

                File.Delete(LicenseFile);
            }
            catch { }
        }

        /// <summary>当前是否已授权且未过期</summary>
        public static bool IsAuthorized()
        {
            var st = GetStatus();
            return st.state == AuthState.Permanent || st.state == AuthState.Valid;
        }

        public enum AuthState { Permanent, Valid, NotActivated, Expired, Revoked }

        public struct AuthStatus
        {
            public AuthState state;
            public string display;       // 直接显示给用户的中文文本（精确到秒）
            public int remainingSeconds;  // -1 = 永久
        }

        /// <summary>软件运行时每秒调用一次：已用秒数 +1 写回 license.dat。</summary>
        public static void Tick()
        {
            try
            {
                if (!File.Exists(LicenseFile)) return;
                string[] lines = File.ReadAllLines(LicenseFile);
                if (lines.Length < 3) return;
                if (!long.TryParse(lines[2].Trim(), out long used)) return;
                used++;
                File.WriteAllText(LicenseFile,
                    lines[0] + Environment.NewLine +
                    lines[1] + Environment.NewLine +
                    used.ToString(),
                    new UTF8Encoding(false));
            }
            catch { }
        }

        /// <summary>读取并计算当前授权状态（剩余时间精确到秒）</summary>
        public static AuthStatus GetStatus()
        {
            var st = new AuthStatus { state = AuthState.NotActivated, display = "未授权，请激活", remainingSeconds = -1 };

            if (!File.Exists(LicenseFile)) return st;

            try
            {
                string[] lines = File.ReadAllLines(LicenseFile);
                if (lines.Length < 3) return st;

                string machine = lines[0].Trim();
                string code = lines[1].Trim();
                if (!long.TryParse(lines[2].Trim(), out long usedSeconds)) return st;

                // 机器码不匹配
                if (!string.Equals(machine, GetMachineCode(), StringComparison.OrdinalIgnoreCase))
                {
                    st.state = AuthState.Revoked;
                    st.display = "授权机器不匹配，请重新激活";
                    return st;
                }

                // 黑名单检查
                if (IsRevoked(code))
                {
                    st.state = AuthState.Revoked;
                    st.display = "授权已作废，请重新激活";
                    return st;
                }

                // 重新验签
                try
                {
                    byte[] bytes = Convert.FromBase64String(code);
                    string payload = Encoding.UTF8.GetString(bytes);
                    string[] parts = payload.Split('|');
                    if (parts.Length != 3) return st;
                    int hours = int.Parse(parts[0]);
                    string signedContent = $"{machine}|{hours}|{parts[1]}";
                    using (var rsa = new RSACryptoServiceProvider())
                    {
                        rsa.FromXmlString(PublicKeyXml);
                        if (!rsa.VerifyData(Encoding.UTF8.GetBytes(signedContent),
                            CryptoConfig.MapNameToOID("SHA1"), Convert.FromBase64String(parts[2])))
                        {
                            st.state = AuthState.Revoked;
                            st.display = "授权文件校验失败";
                            return st;
                        }
                    }

                    if (hours == 0)
                    {
                        st.state = AuthState.Permanent;
                        st.remainingSeconds = -1;
                        st.display = "授权状态: 永久授权";
                    }
                    else
                    {
                        long totalSeconds = hours * 3600L;
                        long remain = totalSeconds - usedSeconds;
                        if (remain <= 0)
                        {
                            st.state = AuthState.Expired;
                            st.display = "授权已用完，请重新激活";
                        }
                        else
                        {
                            st.state = AuthState.Valid;
                            st.remainingSeconds = (int)remain;
                            long d = remain / 86400;
                            long h = (remain % 86400) / 3600;
                            long m = (remain % 3600) / 60;
                            long s = remain % 60;
                            if (d >= 1)
                                st.display = $"授权剩余: {d} 天 {h} 时 {m} 分 {s} 秒";
                            else if (h >= 1)
                                st.display = $"授权剩余: {h} 时 {m} 分 {s} 秒";
                            else
                                st.display = $"授权剩余: {m} 分 {s} 秒";
                        }
                    }
                }
                catch { }
            }
            catch { }

            return st;
        }

        private static bool IsRevoked(string activationCode)
        {
            if (!File.Exists(RevokedFile)) return false;
            string hash = Md5Of(activationCode.Trim());
            foreach (var l in File.ReadAllLines(RevokedFile))
                if (string.Equals(l.Trim(), hash, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        /// <summary>该激活码是否已被使用过（一次性机制）</summary>
        private static bool IsUsed(string activationCode)
        {
            if (!File.Exists(UsedFile)) return false;
            string hash = Md5Of(activationCode.Trim());
            foreach (var l in File.ReadAllLines(UsedFile))
                if (string.Equals(l.Trim(), hash, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        /// <summary>激活成功后标记为已使用</summary>
        private static void MarkAsUsed(string activationCode)
        {
            try
            {
                Directory.CreateDirectory(DataDir);
                string hash = Md5Of(activationCode.Trim());
                var list = new System.Collections.Generic.List<string>();
                if (File.Exists(UsedFile))
                    list.AddRange(File.ReadAllLines(UsedFile));
                list.Add(hash);
                File.WriteAllLines(UsedFile, list, new UTF8Encoding(false));
            }
            catch { }
        }

        private static string Md5Of(string s)
        {
            using (var md5 = MD5.Create())
                return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(s))).Replace("-", "").ToUpper();
        }
    }
}
