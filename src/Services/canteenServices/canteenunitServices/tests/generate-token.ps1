# ─────────────────────────────────────────────────────────────────────────────
# generate-token.ps1
# Generates a signed HS256 JWT for local testing of CanteenUnit API
# Run: .\generate-token.ps1
# Copy the printed token into canteenunit-api.http  @token = <paste here>
# ─────────────────────────────────────────────────────────────────────────────

param(
    [string]$Role = "Admin",          # Admin | User
    [int]   $ExpiresInHours = 2
)

$jwtKey      = "CanteenUnit_SuperSecretKey_ChangeInProduction_MinLength32Chars!"
$jwtIssuer   = "CanteenUnitAPI"
$jwtAudience = "CanteenUnitClients"

function ConvertTo-Base64Url([byte[]]$bytes) {
    [Convert]::ToBase64String($bytes) -replace '\+','-' -replace '/','_' -replace '=',''
}

$hmac    = [System.Security.Cryptography.HMACSHA256]::new([System.Text.Encoding]::UTF8.GetBytes($jwtKey))
$now     = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds()
$exp     = $now + ($ExpiresInHours * 3600)

$headerJson  = '{"alg":"HS256","typ":"JWT"}'
$payloadJson = "{`"sub`":`"testuser`",`"name`":`"Test User`",`"role`":`"$Role`",`"iss`":`"$jwtIssuer`",`"aud`":`"$jwtAudience`",`"iat`":$now,`"exp`":$exp}"

$headerB64  = ConvertTo-Base64Url ([System.Text.Encoding]::UTF8.GetBytes($headerJson))
$payloadB64 = ConvertTo-Base64Url ([System.Text.Encoding]::UTF8.GetBytes($payloadJson))
$sigInput   = "$headerB64.$payloadB64"
$sigB64     = ConvertTo-Base64Url ($hmac.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($sigInput)))

$token = "$headerB64.$payloadB64.$sigB64"

Write-Host ""
Write-Host "JWT Token (role=$Role, expires in ${ExpiresInHours}h):" -ForegroundColor Cyan
Write-Host $token -ForegroundColor Green
Write-Host ""
Write-Host "Paste into canteenunit-api.http as:" -ForegroundColor Yellow
Write-Host "@token = $token" -ForegroundColor Yellow
Write-Host ""

# Auto-copy to clipboard if available
if (Get-Command Set-Clipboard -ErrorAction SilentlyContinue) {
    Set-Clipboard $token
    Write-Host "Token copied to clipboard." -ForegroundColor DarkGreen
}
