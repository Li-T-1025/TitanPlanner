# LAI GCS / TitanPlanner License Generator
$ErrorActionPreference = "Stop"
[Console]::OutputEncoding = [Text.Encoding]::UTF8
chcp 65001 >$null

$privKeyPath = Join-Path $PSScriptRoot "private_key.xml"
if (-not (Test-Path $privKeyPath)) {
    Write-Host "[错误] 找不到 private_key.xml（应与本脚本同目录）" -ForegroundColor Red
    Read-Host "按回车退出"
    exit 1
}
$privXml = Get-Content $privKeyPath -Raw -Encoding UTF8

while ($true) {
    Write-Host ""
    Write-Host "===== LAI GCS 注册机 =====" -ForegroundColor Cyan
    $machine = Read-Host "请输入机器码 (格式 XXXX-XXXX-XXXX, 输入 q 退出)"
    if ($machine -eq "q") { break }
    if ($machine -notmatch '^[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}$') {
        Write-Host "[错误] 机器码格式不对，应为 XXXX-XXXX-XXXX（12 位十六进制）" -ForegroundColor Yellow
        continue
    }

    Write-Host "请选择授权时长单位："
    Write-Host "  1. 按天"
    Write-Host "  2. 按小时"
    $unit = Read-Host "输入选项 (1/2)"

    $hours = 0
    if ($unit -eq "1") {
        $daysStr = Read-Host "授权天数 (0=永久, 如 1=累计使用 24 小时)"
        $days = 0
        if (-not [int]::TryParse($daysStr, [ref]$days)) {
            Write-Host "[错误] 请输入数字" -ForegroundColor Yellow
            continue
        }
        $hours = $days * 24
        if ($days -eq 0) { $hours = 0 }
        Write-Host "授权时长: $days 天 = $hours 小时（累计使用）" -ForegroundColor Cyan
    } elseif ($unit -eq "2") {
        $hrsStr = Read-Host "授权小时数 (如 8=累计使用 8 小时)"
        if (-not [int]::TryParse($hrsStr, [ref]$hours)) {
            Write-Host "[错误] 请输入数字" -ForegroundColor Yellow
            continue
        }
        Write-Host "授权时长: $hours 小时（累计使用）" -ForegroundColor Cyan
    } else {
        Write-Host "[错误] 请输入 1 或 2" -ForegroundColor Yellow
        continue
    }

    $nonce = -join ((1..8) | ForEach-Object { '{0:X}' -f (Get-Random -Max 16) })
    $content = "$machine|$hours|$nonce"
    $contentBytes = [Text.Encoding]::UTF8.GetBytes($content)

    $rsa = New-Object System.Security.Cryptography.RSACryptoServiceProvider(2048)
    $rsa.FromXmlString($privXml)
    $sigBytes = $rsa.SignData($contentBytes, [Security.Cryptography.CryptoConfig]::MapNameToOID("SHA1"))
    $rsa.Dispose()
    $sigB64 = [Convert]::ToBase64String($sigBytes)

    $payload = "$hours|$nonce|$sigB64"
    $code = [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($payload))

    Write-Host ""
    Write-Host "激活码：" -ForegroundColor Green
    Write-Host $code
    Write-Host ""

    $outFile = Join-Path $PSScriptRoot "$machine.txt"
    [IO.File]::WriteAllText($outFile, $code, [Text.Encoding]::UTF8)
    Write-Host "已保存到: $outFile" -ForegroundColor DarkGray
}
Read-Host "按回车退出"