# generate-token.ps1
# Generates a signed HS256 JWT for the DeductionService API
#
# Usage:
#   .\generate-token.ps1
#   .\generate-token.ps1 -Role "PayrollAdmin" -ExpiresInHours 4
#
param(
    [string]$Role          = "PayrollAdmin",
    [int]   $ExpiresInHours = 8
)

$key      = "CHANGE_THIS_TO_A_SECURE_256BIT_SECRET_KEY_AT_LEAST_32CHARS"
$issuer   = "DeductionService"
$audience = "DeductionServiceClients"

$expiresAt = [DateTimeOffset]::UtcNow.AddHours($ExpiresInHours).ToUnixTimeSeconds()

$header  = [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes('{"alg":"HS256","typ":"JWT"}')) `
               -replace '=+$','' -replace '\+','-' -replace '/','_'

$payloadJson = "{`"sub`":`"1`",`"name`":`"admin`",`"role`":`"$Role`",`"jti`":`"$(New-Guid)`",`"exp`":$expiresAt,`"iss`":`"$issuer`",`"aud`":`"$audience`"}"
$payload = [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($payloadJson)) `
               -replace '=+$','' -replace '\+','-' -replace '/','_'

$toSign  = "$header.$payload"
$hmac    = New-Object System.Security.Cryptography.HMACSHA256
$hmac.Key = [Text.Encoding]::UTF8.GetBytes($key)
$sig     = [Convert]::ToBase64String($hmac.ComputeHash([Text.Encoding]::UTF8.GetBytes($toSign))) `
               -replace '=+$','' -replace '\+','-' -replace '/','_'

$token = "$toSign.$sig"

Write-Host ""
Write-Host "JWT Token (Role=$Role, ExpiresIn=${ExpiresInHours}h):"
Write-Host $token
Write-Host ""
Write-Host "Authorization header:"
Write-Host "Bearer $token"
