#!/usr/bin/env pwsh

# Colors for output
$Green = "`e[32m"
$Red = "`e[31m"
$Yellow = "`e[33m"
$Blue = "`e[34m"
$Reset = "`e[0m"

$BaseUrl = "http://localhost:5154"
$AuthToken = $null

Write-Host "$Blue=== MobileAppManagement API - Comprehensive Test Suite ===$Reset`n"

# Step 1: Authentication
Write-Host "$Yellow[1/5] Testing Authentication Endpoint$Reset"
try {
    $authResponse = Invoke-WebRequest -Uri "$BaseUrl/api/auth/token" `
        -Method POST `
        -ContentType 'application/json' `
        -Body '{"username":"admin","password":"admin"}' `
        -UseBasicParsing -ErrorAction Stop
    
    $authData = $authResponse.Content | ConvertFrom-Json
    $AuthToken = $authData.token
    
    Write-Host "$Green✓ Authentication successful$Reset"
    Write-Host "  Token: $($AuthToken.Substring(0, 50))..."
    Write-Host "  Expiration: $($authData.expiration)`n"
} catch {
    Write-Host "$Red✗ Authentication failed$Reset"
    Write-Host "Error: $($_.Exception.Message)`n"
    exit 1
}

$headers = @{"Authorization" = "Bearer $AuthToken"; "Content-Type" = "application/json"}

# Step 2: Test POST Endpoints
Write-Host "$Yellow[2/5] Testing POST Endpoints$Reset"

# 2a: POST /api/devices/register (Controller)
Write-Host "  Testing POST /api/devices/register (Controller)..."
try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/api/devices/register" `
        -Method POST `
        -Headers $headers `
        -Body '{"employeeSysId":1001,"deviceId":"TEST_CTRL_001","deviceType":"A","imeiNo":"123456789012345","updatedBy":1}' `
        -UseBasicParsing -ErrorAction Stop
    Write-Host "    $Green✓ Status $($response.StatusCode): $($response.Content)$Reset"
} catch {
    Write-Host "    $Yellow⚠ Status $($_.Exception.Response.StatusCode): $($_.Exception.Message)$Reset"
}

# 2b: POST /api/minimal/devices/register (Minimal API)
Write-Host "  Testing POST /api/minimal/devices/register (Minimal API)..."
try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/api/minimal/devices/register" `
        -Method POST `
        -Headers $headers `
        -Body '{"employeeSysId":1002,"deviceId":"TEST_MIN_001","deviceType":"I","imeiNo":"998765432109876","updatedBy":1}' `
        -UseBasicParsing -ErrorAction Stop
    Write-Host "    $Green✓ Status $($response.StatusCode): $($response.Content)$Reset"
} catch {
    Write-Host "    $Yellow⚠ Status $($_.Exception.Response.StatusCode): $($_.Exception.Message)$Reset"
}

# 2c: POST /api/logins (No Auth)
Write-Host "  Testing POST /api/logins (No Auth Required)..."
try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/api/logins" `
        -Method POST `
        -ContentType 'application/json' `
        -Body '{"userSysId":1,"deviceId":"TEST_001","imeiNo":"123456789012345","deviceType":"A"}' `
        -UseBasicParsing -ErrorAction Stop
    Write-Host "    $Green✓ Status $($response.StatusCode): $($response.Content)$Reset"
} catch {
    Write-Host "    $Yellow⚠ Status $($_.Exception.Response.StatusCode): $($_.Exception.Message)$Reset"
}

# 2d: POST /api/registrations (Create)
Write-Host "  Testing POST /api/registrations (Create)..."
try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/api/registrations" `
        -Method POST `
        -Headers $headers `
        -Body '{"registrationId":0,"employeeSysId":1001,"userId":"USR001","userSysId":1,"userType":"E","mobileNo":"9876543210","imeiNo":"123456789012345","deviceId":"TEST_001","deviceType":"A"}' `
        -UseBasicParsing -ErrorAction Stop
    Write-Host "    $Green✓ Status $($response.StatusCode): $($response.Content)$Reset"
} catch {
    Write-Host "    $Yellow⚠ Status $($_.Exception.Response.StatusCode): $($_.Exception.Message)$Reset"
}

Write-Host ""

# Step 3: Test GET Endpoints
Write-Host "$Yellow[3/5] Testing GET Endpoints$Reset"

# 3a: GET /api/devices/employee/{id}
Write-Host "  Testing GET /api/devices/employee/1001..."
try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/api/devices/employee/1001" `
        -Headers $headers `
        -UseBasicParsing -ErrorAction Stop
    Write-Host "    $Green✓ Status $($response.StatusCode): $(($response.Content | ConvertFrom-Json | Measure-Object).Count) devices found$Reset"
} catch {
    Write-Host "    $Yellow⚠ Status $($_.Exception.Response.StatusCode): $($_.Exception.Message)$Reset"
}

# 3b: GET /api/health
Write-Host "  Testing GET /api/health..."
try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/health" -UseBasicParsing -ErrorAction Stop
    Write-Host "    $Green✓ Status $($response.StatusCode)$Reset"
} catch {
    Write-Host "    $Yellow⚠ Status $($_.Exception.Response.StatusCode)$Reset"
}

Write-Host ""

# Step 4: Test PUT Endpoints
Write-Host "$Yellow[4/5] Testing PUT Endpoints$Reset"

# 4a: PUT /api/registrations/{id}/status
Write-Host "  Testing PUT /api/registrations/1/status..."
try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/api/registrations/1/status" `
        -Method PUT `
        -Headers $headers `
        -Body '{"newStatus":"A"}' `
        -UseBasicParsing -ErrorAction Stop
    Write-Host "    $Green✓ Status $($response.StatusCode): $($response.Content)$Reset"
} catch {
    Write-Host "    $Yellow⚠ Status $($_.Exception.Response.StatusCode): $($_.Exception.Message)$Reset"
}

# 4b: PUT /api/minimal/registrations/{id}/status
Write-Host "  Testing PUT /api/minimal/registrations/1/status..."
try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/api/minimal/registrations/1/status" `
        -Method PUT `
        -Headers $headers `
        -Body '{"newStatus":"R"}' `
        -UseBasicParsing -ErrorAction Stop
    Write-Host "    $Green✓ Status $($response.StatusCode): $($response.Content)$Reset"
} catch {
    Write-Host "    $Yellow⚠ Status $($_.Exception.Response.StatusCode): $($_.Exception.Message)$Reset"
}

Write-Host ""

# Step 5: Test GraphQL Endpoints
Write-Host "$Yellow[5/5] Testing GraphQL Endpoints$Reset"

# 5a: GraphQL Query
Write-Host "  Testing GraphQL Query (GetDevicesByEmployee)..."
$queryBody = '{"query":"query { getDevicesByEmployee(employeeSysId: 1001) { employeeSysId deviceId deviceType } }"}'

try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/graphql" `
        -Method POST `
        -Headers @{"Content-Type" = "application/json"} `
        -Body $queryBody `
        -UseBasicParsing -ErrorAction Stop
    
    $result = $response.Content | ConvertFrom-Json
    if ($result.data) {
        Write-Host "    $Green✓ Status $($response.StatusCode): Query returned data$Reset"
    } elseif ($result.errors) {
        Write-Host "    $Yellow⚠ GraphQL Error: $($result.errors[0].message)$Reset"
    }
} catch {
    Write-Host "    $Yellow⚠ Error: $($_.Exception.Message)$Reset"
}

# 5b: GraphQL Mutation
Write-Host "  Testing GraphQL Mutation (RegisterDevice)..."
$mutationBody = '{"query":"mutation { registerDevice(employeeSysId: 1003, deviceId: \"GQL_TEST_001\", deviceType: \"A\", imeiNo: \"555555555555555\", updatedBy: 1) }"}'

try {
    $response = Invoke-WebRequest -Uri "$BaseUrl/graphql" `
        -Method POST `
        -Headers @{"Content-Type" = "application/json"} `
        -Body $mutationBody `
        -UseBasicParsing -ErrorAction Stop
    
    $result = $response.Content | ConvertFrom-Json
    if ($result.data -and $result.data.registerDevice) {
        Write-Host "    $Green✓ Status $($response.StatusCode): $($result.data.registerDevice)$Reset"
    } elseif ($result.errors) {
        Write-Host "    $Yellow⚠ GraphQL Error: $($result.errors[0].message)$Reset"
    } else {
        Write-Host "    $Green✓ Status $($response.StatusCode): Response received$Reset"
    }
} catch {
    Write-Host "    $Yellow⚠ Error: $($_.Exception.Message)$Reset"
}

Write-Host ""
Write-Host "$Green=== All Endpoint Tests Completed ===$Reset"
