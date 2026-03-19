# MobileAppManagement API Test Script

$BaseUrl = "http://localhost:5154"

Write-Host "=== Testing Authentication ==="
$authResp = Invoke-WebRequest -Uri "$BaseUrl/api/auth/token" -Method POST -ContentType "application/json" -Body '{"username":"admin","password":"admin"}' -UseBasicParsing
$authData = $authResp.Content | ConvertFrom-Json
$token = $authData.token
Write-Host "Token obtained: $($token.Substring(0,40))..."
Write-Host ""

$headers = @{"Authorization" = "Bearer $token"; "Content-Type" = "application/json"}

Write-Host "=== Testing POST Endpoints ==="

Write-Host "1. POST /api/devices/register (Controller)"
try {
    $resp = Invoke-WebRequest -Uri "$BaseUrl/api/devices/register" -Method POST -Headers $headers -Body '{"employeeSysId":1001,"deviceId":"TEST001","deviceType":"A","imeiNo":"1234","updatedBy":1}' -UseBasicParsing
    Write-Host "   Status: $($resp.StatusCode) - $($resp.Content)" -ForegroundColor Green
} catch {
    Write-Host "   Error: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

Write-Host ""
Write-Host "2. POST /api/minimal/devices/register (Minimal API)"
try {
    $resp = Invoke-WebRequest -Uri "$BaseUrl/api/minimal/devices/register" -Method POST -Headers $headers -Body '{"employeeSysId":1002,"deviceId":"TEST002","deviceType":"I","imeiNo":"5678","updatedBy":1}' -UseBasicParsing
    Write-Host "   Status: $($resp.StatusCode) - $($resp.Content)" -ForegroundColor Green
} catch {
    Write-Host "   Error: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

Write-Host ""
Write-Host "3. POST /api/logins (No Auth)"
try {
    $resp = Invoke-WebRequest -Uri "$BaseUrl/api/logins" -Method POST -ContentType "application/json" -Body '{"userSysId":1,"deviceId":"DEV1","imeiNo":"1234","deviceType":"A"}' -UseBasicParsing
    Write-Host "   Status: $($resp.StatusCode) - $($resp.Content)" -ForegroundColor Green
} catch {
    Write-Host "   Error: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

Write-Host ""
Write-Host "4. POST /api/registrations (Create)"
try {
    $resp = Invoke-WebRequest -Uri "$BaseUrl/api/registrations" -Method POST -Headers $headers -Body '{"registrationId":0,"employeeSysId":1001,"userId":"USR001","userSysId":1,"userType":"E","mobileNo":"9876543210","imeiNo":"1234","deviceId":"TEST001","deviceType":"A"}' -UseBasicParsing
    Write-Host "   Status: $($resp.StatusCode) - $($resp.Content)" -ForegroundColor Green
} catch {
    Write-Host "   Error: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== Testing PUT Endpoints ==="

Write-Host "1. PUT /api/registrations/1/status"
try {
    $resp = Invoke-WebRequest -Uri "$BaseUrl/api/registrations/1/status" -Method PUT -Headers $headers -Body '{"newStatus":"A"}' -UseBasicParsing
    Write-Host "   Status: $($resp.StatusCode) - $($resp.Content)" -ForegroundColor Green
} catch {
    Write-Host "   Error: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

Write-Host ""
Write-Host "2. PUT /api/minimal/registrations/1/status"
try {
    $resp = Invoke-WebRequest -Uri "$BaseUrl/api/minimal/registrations/1/status" -Method PUT -Headers $headers -Body '{"newStatus":"R"}' -UseBasicParsing
    Write-Host "   Status: $($resp.StatusCode) - $($resp.Content)" -ForegroundColor Green
} catch {
    Write-Host "   Error: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== Testing GET Endpoints ==="

Write-Host "1. GET /api/devices/employee/1001"
try {
    $resp = Invoke-WebRequest -Uri "$BaseUrl/api/devices/employee/1001" -Headers $headers -UseBasicParsing
    Write-Host "   Status: $($resp.StatusCode) - Found devices" -ForegroundColor Green
} catch {
    Write-Host "   Error: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

Write-Host ""
Write-Host "2. GET /health"
try {
    $resp = Invoke-WebRequest -Uri "$BaseUrl/health" -UseBasicParsing
    Write-Host "   Status: $($resp.StatusCode) - API is healthy" -ForegroundColor Green
} catch {
    Write-Host "   Error: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== Testing GraphQL ==="

Write-Host "1. GraphQL Query (GetDevicesByEmployee)"
try {
    $resp = Invoke-WebRequest -Uri "$BaseUrl/graphql" -Method POST -ContentType "application/json" -Body '{"query":"query { getDevicesByEmployee(employeeSysId: 1001) { employeeSysId deviceId deviceType } }"}' -UseBasicParsing
    $data = $resp.Content | ConvertFrom-Json
    Write-Host "   Status: $($resp.StatusCode) - Query executed" -ForegroundColor Green
} catch {
    Write-Host "   Error: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

Write-Host ""
Write-Host "2. GraphQL Mutation (RegisterDevice)"
try {
    $resp = Invoke-WebRequest -Uri "$BaseUrl/graphql" -Method POST -ContentType "application/json" -Body '{"query":"mutation { registerDevice(employeeSysId: 1003, deviceId: \"GQL001\", deviceType: \"A\", imeiNo: \"999\", updatedBy: 1) }"}' -UseBasicParsing
    $data = $resp.Content | ConvertFrom-Json
    Write-Host "   Status: $($resp.StatusCode) - Mutation executed" -ForegroundColor Green
} catch {
    Write-Host "   Error: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== Test Complete ===" -ForegroundColor Cyan
