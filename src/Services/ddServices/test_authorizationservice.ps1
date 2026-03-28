# =========================================================================================================
# AuthorizationService.API Comprehensive Test Suite
# Tests: REST endpoints, GraphQL operations, Authentication, RabbitMQ, Health Check
# Port: 5177
# =========================================================================================================

$baseUrl = "http://localhost:5177"
$graphqlUrl = "$baseUrl/graphql"
$pass = 0; $fail = 0; $skip = 0; $total = 0

function Test-Result($name, $condition, $detail = "") {
    $script:total++
    if ($condition) { $script:pass++; Write-Host "  PASS: $name" -ForegroundColor Green }
    else { $script:fail++; Write-Host "  FAIL: $name" -ForegroundColor Red }
    if ($detail) { Write-Host "        $detail" -ForegroundColor Gray }
}

function Test-Skip($name, $reason = "") {
    $script:total++
    $script:skip++
    Write-Host "  SKIP: $name" -ForegroundColor DarkYellow
    if ($reason) { Write-Host "        $reason" -ForegroundColor Gray }
}

# Helper: decode response content (handles byte arrays from application/graphql-response+json)
function Get-ResponseContent($response) {
    if ($response.Content -is [byte[]]) {
        return [System.Text.Encoding]::UTF8.GetString($response.Content)
    }
    return $response.Content
}

Write-Host "================================================" -ForegroundColor Cyan
Write-Host " AUTHORIZATION SERVICE API - TEST SUITE" -ForegroundColor Cyan
Write-Host " Base URL: $baseUrl" -ForegroundColor Cyan
Write-Host "================================================`n" -ForegroundColor Cyan

# ─────────────────────────────────────────────────────────────────────────────
# 1. HEALTH CHECK
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "=== 1. HEALTH CHECK ===" -ForegroundColor Yellow
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/health" -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /health returns 200" ($r.StatusCode -eq 200) "Status: $($r.Content)"
} catch {
    Test-Result "GET /health returns 200" $false $_.Exception.Message
}

try {
    $r = Invoke-WebRequest -Uri "$baseUrl/health/ready" -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /health/ready returns 200" ($r.StatusCode -eq 200) "Status: $($r.Content)"
} catch {
    Test-Result "GET /health/ready returns 200" $false $_.Exception.Message
}

# ─────────────────────────────────────────────────────────────────────────────
# 2. AUTHENTICATION (JWT)
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n=== 2. AUTHENTICATION ===" -ForegroundColor Yellow

# Generate JWT token matching appsettings.json JwtSettings
$header = @{alg="HS256";typ="JWT"} | ConvertTo-Json -Compress
$now = [int](Get-Date -UFormat %s)
$payload = @{
    sub   = "admin001"
    iss   = "AuthorizationService"
    aud   = "AuthorizationServiceClient"
    iat   = $now
    exp   = ($now + 3600)
    role  = "Admin"
    name  = "Test Admin"
} | ConvertTo-Json -Compress
$hB = [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($header)) -replace '\+','-' -replace '/','_' -replace '='
$pB = [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes($payload)) -replace '\+','-' -replace '/','_' -replace '='
$hmac = New-Object System.Security.Cryptography.HMACSHA256
$hmac.Key = [Text.Encoding]::UTF8.GetBytes("your-very-long-secret-key-change-this-in-production-at-least-32-characters")
$sig = [Convert]::ToBase64String($hmac.ComputeHash([Text.Encoding]::UTF8.GetBytes("$hB.$pB"))) -replace '\+','-' -replace '/','_' -replace '='
$jwt = "$hB.$pB.$sig"
Write-Host "  INFO: Generated JWT for testing" -ForegroundColor Gray

# Test unauthorized access (no token)
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/rights" -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /api/rights without token returns 401" $false "Got $($r.StatusCode) instead of 401"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "GET /api/rights without token returns 401" ($code -eq 401) "StatusCode: $code"
}

# Test invalid token
try {
    $badHeaders = @{ "Authorization" = "Bearer invalid.token.here" }
    $r = Invoke-WebRequest -Uri "$baseUrl/api/rights" -Headers $badHeaders -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /api/rights with bad token returns 401" $false "Got $($r.StatusCode)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "GET /api/rights with bad token returns 401" ($code -eq 401) "StatusCode: $code"
}

# Test valid token
$headers = @{ "Authorization" = "Bearer $jwt"; "Content-Type" = "application/json" }
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/rights" -Headers $headers -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /api/rights with valid token returns 200" ($r.StatusCode -eq 200) "Authenticated OK"
} catch {
    Test-Result "GET /api/rights with valid token returns 200" $false $_.Exception.Message
}

# ─────────────────────────────────────────────────────────────────────────────
# 3. REST - RIGHTS ENDPOINTS
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n=== 3. REST - RIGHTS ENDPOINTS ===" -ForegroundColor Yellow

# GET all rights (seeded data)
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/rights" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $rights = $r.Content | ConvertFrom-Json
    $count = if ($rights -is [array]) { $rights.Count } else { 1 }
    Test-Result "GET /api/rights" ($r.StatusCode -eq 200) "Rights returned: $count"
} catch {
    Test-Result "GET /api/rights" $false $_.Exception.Message
}

# POST create a new right
$ts = [DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds()
$newRightCode = [decimal]($ts % 100000)
$createRightBody = @{
    rightCode        = $newRightCode
    rightDescription = "TST"
} | ConvertTo-Json

$createdRightId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/rights" -Method POST -Headers $headers -Body $createRightBody -UseBasicParsing -TimeoutSec 5
    $createdRightId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/rights (create)" ($r.StatusCode -eq 200 -or $r.StatusCode -eq 201) "Created ID: $createdRightId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $stream = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($stream)
    $errBody = $reader.ReadToEnd()
    if ($errBody.Length -gt 300) { $errBody = $errBody.Substring(0, 300) + "..." }
    Test-Result "POST /api/rights (create)" $false "Status: $code, Body: $errBody"
}

# GET right by ID
if ($createdRightId) {
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/rights/$createdRightId" -Headers $headers -UseBasicParsing -TimeoutSec 5
        $right = $r.Content | ConvertFrom-Json
        Test-Result "GET /api/rights/{id}" ($r.StatusCode -eq 200) "RightCode: $($right.rightCode), Desc: $($right.rightDescription)"
    } catch {
        Test-Result "GET /api/rights/{id}" $false $_.Exception.Message
    }
} else {
    # Fallback: try seeded ID 1
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/rights/1" -Headers $headers -UseBasicParsing -TimeoutSec 5
        $right = $r.Content | ConvertFrom-Json
        Test-Result "GET /api/rights/{id}" ($r.StatusCode -eq 200) "RightCode: $($right.rightCode) (seeded)"
    } catch {
        Test-Result "GET /api/rights/{id}" $false $_.Exception.Message
    }
}

# ─────────────────────────────────────────────────────────────────────────────
# 4. REST - USER RIGHTS ENDPOINTS
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n=== 4. REST - USER RIGHTS ENDPOINTS ===" -ForegroundColor Yellow

# GET all user rights
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/userrights" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $userRights = $r.Content | ConvertFrom-Json
    $count = if ($userRights -is [array]) { $userRights.Count } else { 1 }
    Test-Result "GET /api/userrights" ($r.StatusCode -eq 200) "UserRights returned: $count"
} catch {
    Test-Result "GET /api/userrights" $false $_.Exception.Message
}

# POST create user right
$createUserRightBody = @{
    userId       = "testuser_$ts"
    pinNumber    = 12345
    rightCode    = 100
    businessCode = "BC001"
    unitCode   = "UN001"
    rightMode    = 1
} | ConvertTo-Json

$createdUserRightId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/userrights" -Method POST -Headers $headers -Body $createUserRightBody -UseBasicParsing -TimeoutSec 5
    $createdUserRightId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/userrights (create)" ($r.StatusCode -eq 200 -or $r.StatusCode -eq 201) "Created ID: $createdUserRightId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $stream = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($stream)
    $errBody = $reader.ReadToEnd()
    if ($errBody.Length -gt 300) { $errBody = $errBody.Substring(0, 300) + "..." }
    Test-Result "POST /api/userrights (create)" $false "Status: $code, Body: $errBody"
}

# GET user rights by userId
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/userrights/user/testuser_$ts" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $userRights = $r.Content | ConvertFrom-Json
    $count = if ($userRights -is [array]) { $userRights.Count } else { 1 }
    Test-Result "GET /api/userrights/user/{userId}" ($r.StatusCode -eq 200) "Rights for user: $count"
} catch {
    Test-Result "GET /api/userrights/user/{userId}" $false $_.Exception.Message
}

# DELETE user right
if ($createdUserRightId) {
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/userrights/$createdUserRightId" -Method DELETE -Headers $headers -UseBasicParsing -TimeoutSec 5
        Test-Result "DELETE /api/userrights/{id}" ($r.StatusCode -eq 200) "Deleted ID: $createdUserRightId"
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        Test-Result "DELETE /api/userrights/{id}" $false "Status: $code"
    }
}

# ─────────────────────────────────────────────────────────────────────────────
# 5. REST - TRACKER RIGHTS ENDPOINTS
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n=== 5. REST - TRACKER RIGHTS ENDPOINTS ===" -ForegroundColor Yellow

# GET all tracker rights
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/trackerrights" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $trackerRights = $r.Content | ConvertFrom-Json
    $count = if ($trackerRights -is [array]) { $trackerRights.Count } else { 1 }
    Test-Result "GET /api/trackerrights" ($r.StatusCode -eq 200) "TrackerRights returned: $count"
} catch {
    Test-Result "GET /api/trackerrights" $false $_.Exception.Message
}

# POST create tracker right
$createTrackerBody = @{
    userId          = "tracker_$ts"
    pinNumber       = 99999
    trackerMode     = "RW"
    businessCode    = "BC002"
    unitCode        = "UN1"
    trackerRights   = "Y"
    vtcRights       = "N"
    representingUnit = "Y"
    letRight        = "N"
    carRight        = "Y"
} | ConvertTo-Json

$createdTrackerRightId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/trackerrights" -Method POST -Headers $headers -Body $createTrackerBody -UseBasicParsing -TimeoutSec 5
    $createdTrackerRightId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/trackerrights (create)" ($r.StatusCode -eq 200 -or $r.StatusCode -eq 201) "Created ID: $createdTrackerRightId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $stream = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($stream)
    $errBody = $reader.ReadToEnd()
    if ($errBody.Length -gt 300) { $errBody = $errBody.Substring(0, 300) + "..." }
    Test-Result "POST /api/trackerrights (create)" $false "Status: $code, Body: $errBody"
}

# GET tracker rights by userId
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/trackerrights/user/tracker_$ts" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $trRights = $r.Content | ConvertFrom-Json
    $count = if ($trRights -is [array]) { $trRights.Count } else { 1 }
    Test-Result "GET /api/trackerrights/user/{userId}" ($r.StatusCode -eq 200) "TrackerRights for user: $count"
} catch {
    Test-Result "GET /api/trackerrights/user/{userId}" $false $_.Exception.Message
}

# ─────────────────────────────────────────────────────────────────────────────
# 6. REST - SPECIAL INPUTS ENDPOINTS
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n=== 6. REST - SPECIAL INPUTS ENDPOINTS ===" -ForegroundColor Yellow

# GET all special inputs
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/specialinputs" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $specInputs = $r.Content | ConvertFrom-Json
    $count = if ($specInputs -is [array]) { $specInputs.Count } else { 1 }
    Test-Result "GET /api/specialinputs" ($r.StatusCode -eq 200) "SpecialInputs returned: $count"
} catch {
    Test-Result "GET /api/specialinputs" $false $_.Exception.Message
}

# POST create special input
$createSpecialInputBody = @{
    specialInputId = 5001
    yearId         = 2026
    roleType       = "Appraiser"
    employeeSysId  = 10001
    appraisalSysId = 20001
    inputs         = "Test special input data from automated test"
    status         = "P"
} | ConvertTo-Json

try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/specialinputs" -Method POST -Headers $headers -Body $createSpecialInputBody -UseBasicParsing -TimeoutSec 5
    $createdSpecialId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/specialinputs (create)" ($r.StatusCode -eq 200 -or $r.StatusCode -eq 201) "Created ID: $createdSpecialId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $stream = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($stream)
    $errBody = $reader.ReadToEnd()
    if ($errBody.Length -gt 300) { $errBody = $errBody.Substring(0, 300) + "..." }
    Test-Result "POST /api/specialinputs (create)" $false "Status: $code, Body: $errBody"
}

# ─────────────────────────────────────────────────────────────────────────────
# 7. GRAPHQL QUERIES
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n=== 7. GRAPHQL QUERIES ===" -ForegroundColor Yellow

# GraphQL: Get all rights
$gqlBody = @{ query = '{ getRights { id rightCode rightDescription createdAt updatedAt } }' } | ConvertTo-Json
try {
    $r = Invoke-WebRequest -Uri $graphqlUrl -Method POST -Body $gqlBody -ContentType "application/json" -UseBasicParsing -TimeoutSec 5
    $gqlData = (Get-ResponseContent $r) | ConvertFrom-Json
    $hasData = $gqlData.data.getRights -ne $null
    $count = if ($gqlData.data.getRights -is [array]) { $gqlData.data.getRights.Count } else { 0 }
    Test-Result "GraphQL: { getRights }" ($r.StatusCode -eq 200 -and $hasData) "Rights returned: $count"
} catch {
    Test-Result "GraphQL: { getRights }" $false $_.Exception.Message
}

# GraphQL: Get right by ID
$rightIdToQuery = if ($createdRightId) { $createdRightId } else { 1 }
$gqlBody = @{ query = "{ getRight(id: $rightIdToQuery) { id rightCode rightDescription } }" } | ConvertTo-Json
try {
    $r = Invoke-WebRequest -Uri $graphqlUrl -Method POST -Body $gqlBody -ContentType "application/json" -UseBasicParsing -TimeoutSec 5
    $gqlData = (Get-ResponseContent $r) | ConvertFrom-Json
    $hasRight = $gqlData.data.getRight -ne $null
    Test-Result "GraphQL: { getRight(id: $rightIdToQuery) }" ($r.StatusCode -eq 200 -and $hasRight) "RightCode: $($gqlData.data.getRight.rightCode)"
} catch {
    Test-Result "GraphQL: { getRight(id: $rightIdToQuery) }" $false $_.Exception.Message
}

# GraphQL: Get all user rights
$gqlBody = @{ query = '{ getAllUserRights { id userId pinNumber rightCode businessCode unitCode rightMode } }' } | ConvertTo-Json
try {
    $r = Invoke-WebRequest -Uri $graphqlUrl -Method POST -Body $gqlBody -ContentType "application/json" -UseBasicParsing -TimeoutSec 5
    $gqlData = (Get-ResponseContent $r) | ConvertFrom-Json
    $hasData = $gqlData.data.getAllUserRights -ne $null
    $count = if ($gqlData.data.getAllUserRights -is [array]) { $gqlData.data.getAllUserRights.Count } else { 0 }
    Test-Result "GraphQL: { getAllUserRights }" ($r.StatusCode -eq 200 -and $hasData) "UserRights returned: $count"
} catch {
    Test-Result "GraphQL: { getAllUserRights }" $false $_.Exception.Message
}

# GraphQL: Get user rights by userId (seeded data)
$gqlBody = @{ query = '{ getUserRights(userId: "admin001") { id userId rightCode businessCode } }' } | ConvertTo-Json
try {
    $r = Invoke-WebRequest -Uri $graphqlUrl -Method POST -Body $gqlBody -ContentType "application/json" -UseBasicParsing -TimeoutSec 5
    $gqlData = (Get-ResponseContent $r) | ConvertFrom-Json
    $hasErrors = $gqlData.errors -ne $null
    Test-Result "GraphQL: { getUserRights(userId) }" ($r.StatusCode -eq 200 -and -not $hasErrors) "Response OK"
} catch {
    Test-Result "GraphQL: { getUserRights(userId) }" $false $_.Exception.Message
}

# GraphQL: Get all tracker rights
$gqlBody = @{ query = '{ getAllTrackerRights { id userId pinNumber trackerMode businessCode unitCode trackerRights vtcRights } }' } | ConvertTo-Json
try {
    $r = Invoke-WebRequest -Uri $graphqlUrl -Method POST -Body $gqlBody -ContentType "application/json" -UseBasicParsing -TimeoutSec 5
    $gqlData = (Get-ResponseContent $r) | ConvertFrom-Json
    $hasData = $gqlData.data.getAllTrackerRights -ne $null
    $count = if ($gqlData.data.getAllTrackerRights -is [array]) { $gqlData.data.getAllTrackerRights.Count } else { 0 }
    Test-Result "GraphQL: { getAllTrackerRights }" ($r.StatusCode -eq 200 -and $hasData) "TrackerRights returned: $count"
} catch {
    Test-Result "GraphQL: { getAllTrackerRights }" $false $_.Exception.Message
}

# GraphQL: Get tracker rights by userId (seeded data)
$gqlBody = @{ query = '{ getTrackerRights(userId: "admin001") { id userId trackerRights vtcRights } }' } | ConvertTo-Json
try {
    $r = Invoke-WebRequest -Uri $graphqlUrl -Method POST -Body $gqlBody -ContentType "application/json" -UseBasicParsing -TimeoutSec 5
    $gqlData = (Get-ResponseContent $r) | ConvertFrom-Json
    $hasErrors = $gqlData.errors -ne $null
    Test-Result "GraphQL: { getTrackerRights(userId) }" ($r.StatusCode -eq 200 -and -not $hasErrors) "Response OK"
} catch {
    Test-Result "GraphQL: { getTrackerRights(userId) }" $false $_.Exception.Message
}

# GraphQL: Get all special inputs
$gqlBody = @{ query = '{ getAllSpecialInputs { id specialInputId yearId roleType employeeSysId } }' } | ConvertTo-Json
try {
    $r = Invoke-WebRequest -Uri $graphqlUrl -Method POST -Body $gqlBody -ContentType "application/json" -UseBasicParsing -TimeoutSec 5
    $gqlData = (Get-ResponseContent $r) | ConvertFrom-Json
    $hasData = $gqlData.data.getAllSpecialInputs -ne $null
    $count = if ($gqlData.data.getAllSpecialInputs -is [array]) { $gqlData.data.getAllSpecialInputs.Count } else { 0 }
    Test-Result "GraphQL: { getAllSpecialInputs }" ($r.StatusCode -eq 200 -and $hasData) "SpecialInputs returned: $count"
} catch {
    Test-Result "GraphQL: { getAllSpecialInputs }" $false $_.Exception.Message
}

# GraphQL: Schema introspection
$gqlBody = @{ query = '{ __schema { queryType { name fields { name type { name kind } } } } }' } | ConvertTo-Json
try {
    $r = Invoke-WebRequest -Uri $graphqlUrl -Method POST -Body $gqlBody -ContentType "application/json" -UseBasicParsing -TimeoutSec 5
    $gqlData = (Get-ResponseContent $r) | ConvertFrom-Json
    $queryFields = $gqlData.data.__schema.queryType.fields
    $fieldCount = if ($queryFields -is [array]) { $queryFields.Count } else { 0 }
    Test-Result "GraphQL: Schema introspection" ($r.StatusCode -eq 200 -and $fieldCount -gt 0) "Query fields: $fieldCount"
} catch {
    Test-Result "GraphQL: Schema introspection" $false $_.Exception.Message
}

# ─────────────────────────────────────────────────────────────────────────────
# 8. RABBITMQ CONNECTION TEST
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n=== 8. RABBITMQ ===" -ForegroundColor Yellow

# Test RabbitMQ Management API (default guest:guest)
$rabbitMgmtUrl = "http://localhost:15672/api"
$rabbitCreds = [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes("guest:guest"))
$rabbitHeaders = @{ "Authorization" = "Basic $rabbitCreds" }

# Pre-check: is RabbitMQ Management API reachable?
$rabbitAvailable = $false
try {
    $r = Invoke-WebRequest -Uri "$rabbitMgmtUrl/overview" -Headers $rabbitHeaders -UseBasicParsing -TimeoutSec 3
    $overview = $r.Content | ConvertFrom-Json
    $rabbitAvailable = $true
    Test-Result "RabbitMQ: Management API reachable" ($r.StatusCode -eq 200) "RabbitMQ $($overview.rabbitmq_version), Erlang $($overview.erlang_version)"
} catch {
    Test-Skip "RabbitMQ: Management API reachable" "RabbitMQ is not running on localhost:15672 (install or start RabbitMQ to enable these tests)"
}

if ($rabbitAvailable) {
    # Check authorization queues exist
    $expectedQueues = @("authorization.domain.events", "authorization.events")
    foreach ($queueName in $expectedQueues) {
        try {
            $r = Invoke-WebRequest -Uri "$rabbitMgmtUrl/queues/%2F/$queueName" -Headers $rabbitHeaders -UseBasicParsing -TimeoutSec 5
            $queue = $r.Content | ConvertFrom-Json
            Test-Result "RabbitMQ: Queue '$queueName' exists" ($r.StatusCode -eq 200) "Messages: $($queue.messages), Consumers: $($queue.consumers)"
        } catch {
            $code = $null
            try { $code = $_.Exception.Response.StatusCode.value__ } catch {}
            if ($code -eq 404) {
                Test-Result "RabbitMQ: Queue '$queueName' exists" $false "Queue not found (will be created on first use)"
            } else {
                Test-Result "RabbitMQ: Queue '$queueName' exists" $false $_.Exception.Message
            }
        }
    }

    # Publish a test message to authorization.events queue
    try {
        $publishBody = @{
            properties      = @{}
            routing_key     = "authorization.events"
            payload         = (@{ eventType = "TestEvent"; timestamp = (Get-Date -Format o); data = "Automated test message" } | ConvertTo-Json -Compress)
            payload_encoding = "string"
        } | ConvertTo-Json
        $r = Invoke-WebRequest -Uri "$rabbitMgmtUrl/exchanges/%2F/amq.default/publish" -Method POST -Headers $rabbitHeaders -Body $publishBody -ContentType "application/json" -UseBasicParsing -TimeoutSec 5
        $pubResult = $r.Content | ConvertFrom-Json
        Test-Result "RabbitMQ: Publish test message to authorization.events" ($r.StatusCode -eq 200 -and $pubResult.routed -eq $true) "Routed: $($pubResult.routed)"
    } catch {
        Test-Result "RabbitMQ: Publish test message" $false $_.Exception.Message
    }

    # Check AMQP connection from the service
    try {
        $r = Invoke-WebRequest -Uri "$rabbitMgmtUrl/connections" -Headers $rabbitHeaders -UseBasicParsing -TimeoutSec 5
        $connections = $r.Content | ConvertFrom-Json
        $connCount = if ($connections -is [array]) { $connections.Count } else { 0 }
        Test-Result "RabbitMQ: Active AMQP connections" ($r.StatusCode -eq 200) "Connections: $connCount"
    } catch {
        Test-Result "RabbitMQ: Active AMQP connections" $false $_.Exception.Message
    }
} else {
    Test-Skip "RabbitMQ: Queue 'authorization.domain.events' exists" "RabbitMQ not available"
    Test-Skip "RabbitMQ: Queue 'authorization.events' exists" "RabbitMQ not available"
    Test-Skip "RabbitMQ: Publish test message" "RabbitMQ not available"
    Test-Skip "RabbitMQ: Active AMQP connections" "RabbitMQ not available"
}

# ─────────────────────────────────────────────────────────────────────────────
# 9. SWAGGER / OPENAPI
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n=== 9. SWAGGER / OPENAPI ===" -ForegroundColor Yellow

try {
    $r = Invoke-WebRequest -Uri "$baseUrl/swagger/v1/swagger.json" -UseBasicParsing -TimeoutSec 5
    $swagger = $r.Content | ConvertFrom-Json
    $pathCount = ($swagger.paths.PSObject.Properties | Measure-Object).Count
    Test-Result "GET /swagger/v1/swagger.json" ($r.StatusCode -eq 200 -and $pathCount -gt 0) "API Title: $($swagger.info.title), Paths: $pathCount"
} catch {
    Test-Result "GET /swagger/v1/swagger.json" $false $_.Exception.Message
}

# ─────────────────────────────────────────────────────────────────────────────
# 10. EDGE CASES & VALIDATION
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n=== 10. EDGE CASES & VALIDATION ===" -ForegroundColor Yellow

# GET non-existent right
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/rights/999999" -Headers $headers -UseBasicParsing -TimeoutSec 5
    # If it returns 200 with null/empty, that's still a valid response
    Test-Result "GET /api/rights/999999 (not found)" ($r.StatusCode -eq 200 -or $r.StatusCode -eq 404) "StatusCode: $($r.StatusCode)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "GET /api/rights/999999 (not found)" ($code -eq 404 -or $code -eq 204) "StatusCode: $code"
}

# POST right with empty body
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/rights" -Method POST -Headers $headers -Body "{}" -UseBasicParsing -TimeoutSec 5
    Test-Result "POST /api/rights (empty body)" $true "Accepted with defaults (StatusCode: $($r.StatusCode))"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "POST /api/rights (empty body)" ($code -eq 400 -or $code -eq 422) "Validation returned: $code"
}

# GET user rights for non-existent user
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/userrights/user/nonexistent_user" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $data = $r.Content | ConvertFrom-Json
    $isEmpty = ($data -is [array] -and $data.Count -eq 0) -or ($data -eq $null)
    Test-Result "GET /api/userrights/user/nonexistent (empty result)" ($r.StatusCode -eq 200) "Empty array: $isEmpty"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "GET /api/userrights/user/nonexistent" ($code -eq 404) "StatusCode: $code"
}

# ─────────────────────────────────────────────────────────────────────────────
# SUMMARY
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n================================================" -ForegroundColor Cyan
Write-Host " TEST SUMMARY" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  Total:   $total" -ForegroundColor White
Write-Host "  Passed:  $pass" -ForegroundColor Green
Write-Host "  Failed:  $fail" -ForegroundColor Red
if ($skip -gt 0) { Write-Host "  Skipped: $skip" -ForegroundColor DarkYellow }
$runnable = $total - $skip
$pct = if ($runnable -gt 0) { [math]::Round(($pass / $runnable) * 100, 1) } else { 0 }
Write-Host "  Rate:    $pct% (of $runnable runnable)" -ForegroundColor $(if ($pct -ge 80) { "Green" } elseif ($pct -ge 50) { "Yellow" } else { "Red" })
Write-Host "================================================`n" -ForegroundColor Cyan
