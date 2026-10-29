# ============================================================
# TitanPlanner MSI 一键打包脚本（全自动版）
# 功能：
#   1) 检查 Release 构建产物（bin\Release\net461\MissionPlanner.exe）
#   2) 清理 bin\Release\net461 内重复的插件副本（避免 MSI 重复打包）
#   3) 自动编译 wix.exe（生成 installer.wxs 的扫描工具，位于 Msi\net472\wix.exe）
#   4) 自动定位 WiX Toolset；本机没有时自动下载解压（GitHub + 国内加速源轮询）
#   5) 生成 installer.wxs -> candle 编译 -> light 链接 -> 输出 MSI 到 packaging 目录
# 用法：
#   方式A（推荐）：双击同目录下的「打包.bat」
#   方式B：powershell -NoProfile -ExecutionPolicy Bypass -File "packaging\build - MSI.ps1"
# 前置条件：已用 Visual Studio 构建 Release（bin\Release\net461\MissionPlanner.exe 存在）
# 产物：packaging\TitanPlanner-Installer.msi
# 说明：WiX 下载到 %LOCALAPPDATA%\wix314，一次下载后本机复用
# ============================================================
$ErrorActionPreference = 'Stop'
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = Split-Path -Parent $scriptDir   # 仓库根 = packaging 的上一级
Set-Location -LiteralPath $root

function Write-Step($msg) { Write-Host "[*] $msg" -ForegroundColor Cyan }
function Write-Ok($msg)    { Write-Host "    $msg" -ForegroundColor Green }
function Write-Err($msg)  { Write-Host "[错误] $msg" -ForegroundColor Red }

# ------------------------------------------------------------
# [1/5] 检查 Release 构建产物
# ------------------------------------------------------------
Write-Step "1/5 检查 Release 构建产物"
$release = Join-Path $root 'bin\Release\net461'
if (-not (Test-Path -LiteralPath (Join-Path $release 'MissionPlanner.exe'))) {
    Write-Err "未找到 Release 构建产物：bin\Release\net461\MissionPlanner.exe"
    Write-Host "      请先用 Visual Studio 打开 MissionPlanner.sln，配置=Release，执行「生成」" -ForegroundColor Yellow
    Write-Host "      或命令行： & 'F:\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe' MissionPlanner.sln -restore -t:Build -p:Configuration=Release -p:Platform=`"Any CPU`"" -ForegroundColor Yellow
    exit 1
}
Write-Ok "已找到：$release"

# ------------------------------------------------------------
# [2/5] 清理 plugins 内与主目录重复的副本（官方 installer.bat 同款逻辑）
# ------------------------------------------------------------
Write-Step "2/5 清理 plugins 重复副本"
$pluginsDir = Join-Path $release 'plugins'
if (Test-Path -LiteralPath $pluginsDir) {
    Get-ChildItem -LiteralPath $pluginsDir -File | ForEach-Object {
        $target = Join-Path $release $_.Name
        if (Test-Path -LiteralPath $target -PathType Leaf) {
            Remove-Item -LiteralPath $_.FullName -Force
            Write-Ok "已删除重复副本：$($_.Name)"
        }
    }
}

# ------------------------------------------------------------
# [3/5] 编译 wix.exe（Msi\net472\wix.exe 不存在时自动编译）
# ------------------------------------------------------------
Write-Step "3/5 准备 wix.exe（installer.wxs 生成工具）"
$wixexe = Join-Path $root 'Msi\net472\wix.exe'
if (-not (Test-Path -LiteralPath $wixexe)) {
    Write-Host "    Msi\net472\wix.exe 不存在，开始编译 wix\wix.csproj ..."
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (-not (Test-Path -LiteralPath $vswhere)) {
        Write-Err "未找到 vswhere.exe（${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe），无法定位 MSBuild"
        Write-Err "请安装 Visual Studio 2019/2022/2026（含「使用 C++ 的桌面开发」或 MSBuild 组件）后重试"
        exit 1
    }
    $msbuild = & $vswhere -latest -requires Microsoft.Component.MSBuild -find MSBuild\Current\Bin\MSBuild.exe | Select-Object -First 1
    if (-not $msbuild) {
        Write-Err "vswhere 未找到 MSBuild，请确认已安装 Visual Studio 并勾选 MSBuild 组件"
        exit 1
    }
    Write-Ok "MSBuild：$msbuild"
    & $msbuild (Join-Path $root 'wix\wix.csproj') -restore -t:Build -p:Configuration=Release -p:Platform="Any CPU" -v:m
    if ($LASTEXITCODE -ne 0) {
        Write-Err "wix\wix.csproj 编译失败（退出码 $LASTEXITCODE）"
        exit 1
    }
    if (-not (Test-Path -LiteralPath $wixexe)) {
        Write-Err "编译完成但未生成 Msi\net472\wix.exe，请检查 wix\wix.csproj 的 OutputPath（应输出到 Msi\）"
        exit 1
    }
}
Write-Ok "已就绪（检测到已有 wix.exe，跳过编译）：$wixexe"

# ------------------------------------------------------------
# [4/5] 定位 / 自动下载 WiX Toolset
# ------------------------------------------------------------
Write-Step "4/5 准备 WiX Toolset（candle / light）"
$wixCandidates = @()
if ($env:WIX) { $wixCandidates += $env:WIX }
$wixCandidates += (Join-Path $env:LOCALAPPDATA 'wix314')
$wixCandidates += 'C:\Program Files (x86)\WiX Toolset v3.14'
$wixCandidates += 'C:\Program Files (x86)\WiX Toolset v3.11'
$wixCandidates += 'C:\Program Files (x86)\WiX Toolset v3.10'
$wixDir = $null
foreach ($c in $wixCandidates) {
    if ((Test-Path -LiteralPath (Join-Path $c 'candle.exe')) -and (Test-Path -LiteralPath (Join-Path $c 'light.exe'))) {
        $wixDir = $c
        Write-Ok "检测到已有打包环境（WiX Toolset），跳过下载：$c"
        break
    }
}
if (-not $wixDir) {
    $dlDir = Join-Path $env:LOCALAPPDATA 'wix314'
    if (-not (Test-Path -LiteralPath $dlDir)) { New-Item -ItemType Directory -Path $dlDir | Out-Null }
    $zip = Join-Path $dlDir 'wix314-binaries.zip'
    # 已有下载好的 zip 时直接解压复用，不再重复下载
    if (Test-Path -LiteralPath $zip) {
        Write-Host "    检测到已下载的 WiX 压缩包，直接解压（跳过下载）..."
        Expand-Archive -LiteralPath $zip -DestinationPath $dlDir -Force
        if (Test-Path -LiteralPath (Join-Path $dlDir 'candle.exe')) {
            $wixDir = $dlDir
        } else {
            Write-Err "已存在的 $zip 解压后缺少 candle.exe，将重新下载"
            Remove-Item -LiteralPath $zip -Force
        }
    }
    if (-not $wixDir) {
        Write-Host "    本机未找到 WiX Toolset，开始自动下载（约 41 MB）..."
        $urls = @(
            'https://github.com/wixtoolset/wix3/releases/download/wix3141rtm/wix314-binaries.zip',
            'https://ghfast.top/https://github.com/wixtoolset/wix3/releases/download/wix3141rtm/wix314-binaries.zip',
            'https://gh-proxy.com/https://github.com/wixtoolset/wix3/releases/download/wix3141rtm/wix314-binaries.zip'
        )
        $downloaded = $false
        foreach ($u in $urls) {
            try {
                Write-Host "    尝试下载：$u"
                if (Get-Command curl.exe -ErrorAction SilentlyContinue) {
                    & curl.exe -L --connect-timeout 15 --max-time 300 -o $zip $u
                    if ($LASTEXITCODE -eq 0) { $downloaded = $true; break }
                } else {
                    Invoke-WebRequest -Uri $u -OutFile $zip -UseBasicParsing -TimeoutSec 300
                    $downloaded = $true; break
                }
            } catch {
                Write-Host "    该源失败：$($_.Exception.Message)"
            }
        }
        if (-not $downloaded) {
            Write-Err "WiX 下载失败（GitHub 及国内加速源均不可达）"
            Write-Err "请手动下载 wix314-binaries.zip 解压到 $dlDir（需包含 candle.exe / light.exe / difxapp_x86.wixlib）后重试"
            Write-Host "      下载地址：https://github.com/wixtoolset/wix3/releases/download/wix3141rtm/wix314-binaries.zip" -ForegroundColor Yellow
            exit 1
        }
        Write-Ok "下载完成：$zip"
        Expand-Archive -LiteralPath $zip -DestinationPath $dlDir -Force
        if (-not (Test-Path -LiteralPath (Join-Path $dlDir 'candle.exe'))) {
            Write-Err "解压后仍找不到 candle.exe，请手动解压 $zip 到 $dlDir"
            exit 1
        }
        $wixDir = $dlDir
    }
}
$candle = Join-Path $wixDir 'candle.exe'
$light  = Join-Path $wixDir 'light.exe'
$difx   = Join-Path $wixDir 'difxapp_x86.wixlib'
if (-not (Test-Path -LiteralPath $difx)) {
    Write-Err "WiX 目录缺少 difxapp_x86.wixlib（$wixDir），请确认解压完整"
    exit 1
}
Write-Ok "WiX 目录：$wixDir"

# ------------------------------------------------------------
# [5/5] 生成 installer.wxs -> candle -> light -> MSI
# ------------------------------------------------------------
Write-Step "5/5 生成 installer.wxs 并打包 MSI"
$msiDir = Join-Path $root 'Msi'
Set-Location -LiteralPath $msiDir
if (Test-Path -LiteralPath 'installer.wxs') { Remove-Item -LiteralPath 'installer.wxs' -Force }
& $wixexe "$release\"
if ($LASTEXITCODE -ne 0) { Write-Err "wix.exe 生成 installer.wxs 失败（退出码 $LASTEXITCODE）"; exit 1 }

& $candle 'installer.wxs' -ext WiXNetFxExtension -ext WixDifxAppExtension -ext WixUIExtension.dll -ext WixUtilExtension -ext WixIisExtension
if ($LASTEXITCODE -ne 0) { Write-Err "candle.exe 编译 installer.wxs 失败（退出码 $LASTEXITCODE）"; exit 1 }

& $light 'installer.wixobj' $difx -sval -o 'TitanPlanner-Installer.msi' -ext WiXNetFxExtension -ext WixDifxAppExtension -ext WixUIExtension.dll -ext WixUtilExtension -ext WixIisExtension
if ($LASTEXITCODE -ne 0) { Write-Err "light.exe 链接 MSI 失败（退出码 $LASTEXITCODE）"; exit 1 }

$msiOut = Join-Path $scriptDir 'TitanPlanner-Installer.msi'
if (Test-Path -LiteralPath $msiOut) { Remove-Item -LiteralPath $msiOut -Force }
Move-Item -LiteralPath (Join-Path $msiDir 'TitanPlanner-Installer.msi') -Destination $msiOut

Write-Host ""
Write-Ok "打包完成！MSI 已生成：$msiOut"
Write-Ok "体积：$([math]::Round((Get-Item -LiteralPath $msiOut).Length / 1MB, 1)) MB"
