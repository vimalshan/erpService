$secret = "YourSuperSecretKeyFor256BitHmacSha256AlgorithmMustBeAtLeast32Characters"
$header = '{"alg":"HS256","typ":"JWT"}'
$now = [int]([DateTimeOffset]::UtcNow.ToUnixTimeSeconds())
$exp = $now + 3600
$payload = "{`"sub`":`"1`",`"name`":`"TestUser`",`"role`":`"Admin`",`"iss`":`"OrganizationSetupAPI`",`"aud`":`"OrganizationSetupClients`",`"exp`":$exp,`"iat`":$now}"

function ConvertTo-Base64Url($bytes) {
    [Convert]::ToBase64String($bytes).TrimEnd('=').Replace('+','-').Replace('/','_')
}

$h = ConvertTo-Base64Url([System.Text.Encoding]::UTF8.GetBytes($header))
$p = ConvertTo-Base64Url([System.Text.Encoding]::UTF8.GetBytes($payload))
$hmac = New-Object System.Security.Cryptography.HMACSHA256
$hmac.Key = [System.Text.Encoding]::UTF8.GetBytes($secret)
$sig = ConvertTo-Base64Url($hmac.ComputeHash([System.Text.Encoding]::UTF8.GetBytes("$h.$p")))
$token = "$h.$p.$sig"

$headers = @{
    "Content-Type" = "application/json"
    "Authorization" = "Bearer $token"
}

Write-Host "=== Test: Create Role (triggers RabbitMQ event) ==="
$body = '{"roleId":701,"roleName":"RabbitMQEventTest","roleLevel":7}'
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5099/api/roles" -Method POST -Body $body -Headers $headers -UseBasicParsing
    Write-Host "Status: $($response.StatusCode)"
    Write-Host "Body: $($response.Content)"
} catch {
    Write-Host "Status: $($_.Exception.Response.StatusCode)"
    try {
        $stream = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($stream)
        Write-Host "Body: $($reader.ReadToEnd())"
    } catch {
        Write-Host "Error reading response: $_"
    }
}

Write-Host ""
Write-Host "=== Test: GET Roles ==="
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5099/api/roles" -Headers $headers -UseBasicParsing
    Write-Host "Status: $($response.StatusCode)"
    Write-Host "Body: $($response.Content)"
} catch {
    Write-Host "Status: $($_.Exception.Response.StatusCode)"
}
