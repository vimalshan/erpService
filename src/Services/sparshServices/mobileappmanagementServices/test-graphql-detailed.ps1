$BaseUrl = "http://localhost:5154"

# Get JWT token
Write-Host "=== Getting JWT Token ===" -ForegroundColor Cyan
$authResponse = Invoke-WebRequest -Uri "$BaseUrl/api/auth/token" `
  -Method POST `
  -ContentType "application/json" `
  -Body '{}' `
  -ErrorAction SilentlyContinue

if ($authResponse.StatusCode -eq 200) {
    $token = ($authResponse.Content | ConvertFrom-Json).token
    Write-Host "✓ Token obtained: $($token.Substring(0, 50))..."
} else {
    Write-Host "✗ Failed to get token. Status: $($authResponse.StatusCode)"
    exit
}

# Test 1: GraphQL Query
Write-Host "`n=== Testing GraphQL Query ===" -ForegroundColor Cyan
$graphqlQuery = @{
    query = 'query { getDevicesByEmployee(employeeSysId: 1001) { deviceId deviceType } }'
} | ConvertTo-Json

$response = Invoke-WebRequest -Uri "$BaseUrl/graphql" `
  -Method POST `
  -ContentType "application/json" `
  -Body $graphqlQuery `
  -Headers @{ "Authorization" = "Bearer $token" } `
  -ErrorAction SilentlyContinue

Write-Host "Status: $($response.StatusCode)"
$jsonResponse = $response.Content | ConvertFrom-Json
if ($jsonResponse.data) {
    Write-Host "✓ Query executed successfully"
    Write-Host "Devices count: $($jsonResponse.data.getDevicesByEmployee.Count)"
} elseif ($jsonResponse.errors) {
    Write-Host "✗ Query error:"
    $jsonResponse.errors | ForEach-Object { Write-Host "  - $($_.message)" }
}

# Test 2: GraphQL Mutation
Write-Host "`n=== Testing GraphQL Mutation ===" -ForegroundColor Cyan
$graphqlMutation = @{
    query = 'mutation { registerDevice(employeeSysId: 1004, deviceId: "TEST_004", deviceType: "A", updatedBy: 1) }'
} | ConvertTo-Json

$response = Invoke-WebRequest -Uri "$BaseUrl/graphql" `
  -Method POST `
  -ContentType "application/json" `
  -Body $graphqlMutation `
  -Headers @{ "Authorization" = "Bearer $token" } `
  -ErrorAction SilentlyContinue

Write-Host "Status: $($response.StatusCode)"
$jsonResponse = $response.Content | ConvertFrom-Json
if ($jsonResponse.data) {
    Write-Host "✓ Mutation executed: $($jsonResponse.data.registerDevice)"
} elseif ($jsonResponse.errors) {
    Write-Host "✗ Mutation error:"
    $jsonResponse.errors | ForEach-Object { Write-Host "  - $($_.message)" }
}
