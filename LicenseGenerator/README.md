# LAI GCS / TitanPlanner 授权系统说明

纯本地 RSA 2048 签名授权，**不联网**。激活码一次性使用，从用户激活时刻起算有效期。

## 文件清单与作用

| 文件 | 作用 | 是否入库 |
|------|------|---------|
| `ExtLibs/Utilities/Authorization.cs` | **客户端核心类**。机器码生成（WMI 读 CPU ID+主板序列号+MAC→MD5）、RSA 公钥验签、license.dat 读写、revoked.dat 黑名单、倒计时计算、状态查询。公钥硬编码在此。 | ✅ 入库 |
| `LicenseGenerator/LicenseGenerator.ps1` | **注册机脚本**。开发者双击运行，输入机器码+天数，用私钥签名生成 Base64 激活码。 | ✅ 入库 |
| `LicenseGenerator/注册机.bat` | 双击启动注册机（自动切 UTF-8 代码页并调 PS1）。 | ✅ 入库 |
| `LicenseGenerator/private_key.xml` | **RSA 2048 私钥**。保密，绝不分发/不入库（.gitignore 已排除）。 | ❌ 不入库 |
| `LicenseGenerator/public_key.xml` | 公钥备份（客户端公钥已硬编码进 Authorization.cs，此文件仅作备份）。 | ✅ 入库 |
| `LicenseGenerator/*.txt` | 注册机生成的激活码，按机器码命名。 | ❌ 不入库 |

## 用户端自动生成文件（`%APPDATA%\LAIGCS\`）

| 文件 | 内容 |
|------|------|
| `license.dat` | 3 行：机器码 / 激活码 / 激活时间(yyyy-MM-dd HH:mm:ss) |
| `revoked.dat` | 已作废激活码的 MD5 黑名单（每行一个） |

## 激活码格式

```
Base64(有效小时数|随机数|RSA-SHA1签名(机器码|有效小时数|随机数))
```

- 有效小时数：`24`=1天，`168`=7天，`0`=永久
- 随机数：8 位十六进制，每次生成不同
- 签名内容：`机器码|小时数|随机数`，用私钥 RSA-SHA1 签名

## 工作流程

```
用户电脑                                    开发者
   │ 1. WMI 读 CPU ID+主板+MAC → MD5 → 机器码
   │ ──── 把机器码发给开发者 ────────→
   │                                2. 注册机输入机器码+天数
   │                                3. 私钥签名 → 生成激活码
   │ ←────── 发回激活码 ─────────────
   │ 4. 公钥验签 + 查黑名单 + 写 license.dat
   │ 5. 从激活时刻开始倒计时
```

## 授权状态（右上角每秒刷新）

| 状态 | 颜色 | 显示 |
|------|------|------|
| 永久授权 | 绿色 | 授权状态: 永久授权 |
| 剩余 >3天 | 蓝色 | 授权剩余: X 天 X 小时 |
| 剩余 1~3天 | 黄色 | 授权剩余: X 天 X 小时 |
| 剩余 <1天 | 红色 | 授权剩余: X 小时 |
| 未授权 | 红色 | 未授权，请激活 |
| 已过期 | 红色 | 授权已过期，请重新激活 |

## 安全机制

1. **RSA 2048 / SHA1**：私钥只在开发者手里，用户无法伪造激活码
2. **机器码绑定**：激活码只能在指定硬件上使用，换电脑失效
3. **一次性激活**：撤销后激活码 MD5 进黑名单，永久作废
4. **随机数**：同一机器码同一天数每次生成的码都不同
5. **每次启动重新验签**：防止用户手改 license.dat
6. **纯离线**：不联网，无服务器依赖

**已知限制**：本地校验无法防止用户改系统时间来延长授权。

## 注册机用法

双击 `注册机.bat`，按提示输入：
```
机器码:  7E81-0229-ED9B
天数:    1   （0=永久，1=从激活时刻起24小时，30=30天）
```
激活码自动显示并保存为 `LicenseGenerator/<机器码>.txt`。

## 客户端集成接口

```csharp
// 获取机器码（显示给用户，让他发给开发者）
string machine = Authorization.GetMachineCode();

// 用户输入激活码后激活
var (ok, err) = Authorization.Activate(code);

// 查询状态（右上角 label 每秒刷新）
var st = Authorization.GetStatus();
// st.display 直接显示，st.state 判断颜色

// 撤销（用户点"撤销授权"按钮）
Authorization.Revoke();
Application.Exit();

// 快速检查是否授权
if (!Authorization.IsAuthorized()) { /* 弹窗要求激活 */ }
```
