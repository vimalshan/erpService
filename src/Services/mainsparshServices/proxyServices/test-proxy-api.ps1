# ProxyModule.API Test Script
# Port: 5237

$baseUrl = "http://localhost:5237"
$script:passed = 0
$script:failed = 0
$script:total = 0
$runId = (Get-Date -Format "HHmmss")

function Test-Endpoint {
    param([string]$Name, [scriptblock]$Block)
    $script:total++
    try {
        $result = & $Block
        if ($result) {
            Write-Host "  [PASS] $Name" -ForegroundColor Green
            $script:passed++
        } else {
            Write-Host "  [FAIL] $Name" -ForegroundColor Red
            $script:failed++
        }
    } catch {
        Write-Host "  [FAIL] $Name - $_" -ForegroundColor Red
        $script:failed++
    }
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  ProxyModule.API Tests" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# ─── 1. HEALTH CHECK ──────────────────────────────────────────────────────────
Write-Host "`n[1] Health Check" -ForegroundColor Yellow

Test-Endpoint "GET /health - service healthy" {
    $r = Invoke-RestMethod "$baseUrl/health"
    $r -eq "Healthy" -or $r.status -eq "Healthy"
}

# ─── 2. AUTH TOKEN ─────────────────────────────────────────────────────────────
Write-Host "`n[2] Auth Token" -ForegroundColor Yellow

$script:token = $null

Test-Endpoint "POST /api/auth/token - returns JWT" {
    $body = @{ userId = 1; userName = "admin"; roles = @("Admin") } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/auth/token" -Method POST -Body $body -ContentType "application/json"
    $script:token = $r.token
    $r.token -ne $null -and $r.token.Length -gt 20
}

Test-Endpoint "POST /api/auth/token - works without roles" {
    $body = @{ userId = 2; userName = "testuser" } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/auth/token" -Method POST -Body $body -ContentType "application/json"
    $r.token -ne $null -and $r.token.Length -gt 20
}

$headers = @{ Authorization = "Bearer $($script:token)" }

# ─── 3. REST - PROXY RIGHTS CONTROLLER ────────────────────────────────────────
Write-Host "`n[3] REST ProxyRights Controller" -ForegroundColor Yellow

$script:proxyId = $null

Test-Endpoint "GET /api/proxyrights/active - returns active list" {
    $r = Invoke-RestMethod "$baseUrl/api/proxyrights/active" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "POST /api/proxyrights - creates proxy right" {
    $body = @{
        proxyUserId    = 100
        delegatedUserId = 200
        proxyStartDate = (Get-Date).ToString("o")
        proxyEndDate   = (Get-Date).AddDays(30).ToString("o")
        proxyType      = "APPROVAL"
        scope          = "ALL"
        notes          = "Test proxy $runId"
        createdBy      = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/proxyrights" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:proxyId = $r.proxyId
    $r.proxyId -gt 0 -and $r.proxyType -eq "APPROVAL"
}

Test-Endpoint "GET /api/proxyrights/{id} - find by ID" {
    if (-not $script:proxyId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/proxyrights/$($script:proxyId)" -Headers $headers
    $r.proxyId -eq $script:proxyId
}

Test-Endpoint "GET /api/proxyrights/user/100 - find by user" {
    $r = Invoke-RestMethod "$baseUrl/api/proxyrights/user/100" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "PUT /api/proxyrights/{id} - updates proxy right" {
    if (-not $script:proxyId) { return $false }
    $body = @{
        proxyId       = $script:proxyId
        proxyType     = "FULL"
        scope         = "DEPARTMENT"
        notes         = "Updated proxy $runId"
        updatedBy     = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/proxyrights/$($script:proxyId)" -Method PUT -Body $body -ContentType "application/json" -Headers $headers
    $r.proxyType -eq "FULL"
}

Test-Endpoint "GET /api/proxyrights/999999 - 404 for nonexistent" {
    try { Invoke-RestMethod "$baseUrl/api/proxyrights/999999" -Headers $headers; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

Test-Endpoint "POST /api/proxyrights - 401 without auth" {
    $body = @{ proxyUserId = 1; delegatedUserId = 2; proxyStartDate = (Get-Date).ToString("o"); proxyType = "APPROVAL"; createdBy = 1 } | ConvertTo-Json
    try { Invoke-RestMethod "$baseUrl/api/proxyrights" -Method POST -Body $body -ContentType "application/json"; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 401 }
}

# Create a second proxy for deactivation test
$script:proxyId2 = $null
Test-Endpoint "POST /api/proxyrights - create second proxy for delete" {
    $body = @{
        proxyUserId    = 300
        delegatedUserId = 400
        proxyStartDate = (Get-Date).ToString("o")
        proxyEndDate   = (Get-Date).AddDays(10).ToString("o")
        proxyType      = "READONLY"
        scope          = "LOCATION"
        notes          = "Delete test $runId"
        createdBy      = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/proxyrights" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:proxyId2 = $r.proxyId
    $r.proxyId -gt 0
}

Test-Endpoint "DELETE /api/proxyrights/{id} - deactivates proxy" {
    if (-not $script:proxyId2) { return $false }
    $resp = Invoke-WebRequest "$baseUrl/api/proxyrights/$($script:proxyId2)" -Method DELETE -Headers $headers -UseBasicParsing
    $resp.StatusCode -eq 204
}

Test-Endpoint "GET /api/proxyrights/{id} - verify deactivated" {
    if (-not $script:proxyId2) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/proxyrights/$($script:proxyId2)" -Headers $headers
    $r.proxyStatus -eq "I"
}

Test-Endpoint "POST /api/proxyrights - validates self-delegation" {
    $body = @{ proxyUserId = 999; delegatedUserId = 999; proxyStartDate = (Get-Date).ToString("o"); proxyType = "APPROVAL"; createdBy = 1 } | ConvertTo-Json
    try { Invoke-RestMethod "$baseUrl/api/proxyrights" -Method POST -Body $body -ContentType "application/json" -Headers $headers; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 400 }
}

# ─── 4. MINIMAL API (/api/v2/proxy-rights) ────────────────────────────────────
Write-Host "`n[4] Minimal API Endpoints" -ForegroundColor Yellow

$script:minProxyId = $null

Test-Endpoint "GET /api/v2/proxy-rights/active - returns active list" {
    $r = Invoke-RestMethod "$baseUrl/api/v2/proxy-rights/active" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "POST /api/v2/proxy-rights - creates proxy right" {
    $body = @{
        proxyUserId    = 500
        delegatedUserId = 600
        proxyStartDate = (Get-Date).ToString("o")
        proxyEndDate   = (Get-Date).AddDays(60).ToString("o")
        proxyType      = "SUBMISSION"
        scope          = "SPECIFIC"
        notes          = "Minimal API test $runId"
        createdBy      = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/v2/proxy-rights" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:minProxyId = $r.proxyId
    $r.proxyId -gt 0 -and $r.proxyType -eq "SUBMISSION"
}

Test-Endpoint "GET /api/v2/proxy-rights/{id} - find by ID" {
    if (-not $script:minProxyId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/v2/proxy-rights/$($script:minProxyId)" -Headers $headers
    $r.proxyId -eq $script:minProxyId
}

Test-Endpoint "GET /api/v2/proxy-rights/user/500 - find by user" {
    $r = Invoke-RestMethod "$baseUrl/api/v2/proxy-rights/user/500" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "PUT /api/v2/proxy-rights/{id} - updates proxy right" {
    if (-not $script:minProxyId) { return $false }
    $body = @{
        proxyId   = $script:minProxyId
        proxyType = "READONLY"
        scope     = "ALL"
        notes     = "Updated via minimal $runId"
        updatedBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/v2/proxy-rights/$($script:minProxyId)" -Method PUT -Body $body -ContentType "application/json" -Headers $headers
    $r.proxyType -eq "READONLY"
}

Test-Endpoint "DELETE /api/v2/proxy-rights/{id} - deactivates" {
    if (-not $script:minProxyId) { return $false }
    $resp = Invoke-WebRequest "$baseUrl/api/v2/proxy-rights/$($script:minProxyId)?updatedBy=1" -Method DELETE -Headers $headers -UseBasicParsing
    $resp.StatusCode -eq 204 -or $resp.StatusCode -eq 200
}

# ─── 5. GRAPHQL ────────────────────────────────────────────────────────────────
Write-Host "`n[5] GraphQL Endpoints" -ForegroundColor Yellow

$gqlUrl = "$baseUrl/graphql"
$gqlHeaders = @{ "Content-Type" = "application/json"; Authorization = "Bearer $($script:token)" }

function Invoke-GQL($query) {
    $body = (@{ query = $query } | ConvertTo-Json)
    return Invoke-RestMethod $gqlUrl -Method POST -Body $body -Headers $gqlHeaders
}

Test-Endpoint "GraphQL query activeProxyRights" {
    $r = Invoke-GQL 'query { activeProxyRights { proxyId proxyUserId delegatedUserId proxyType proxyStatus } }'
    $r.data.activeProxyRights -is [array]
}

Test-Endpoint "GraphQL query proxyRightById" {
    if (-not $script:proxyId) { return $false }
    $id = $script:proxyId
    $r = Invoke-GQL "query { proxyRightById(proxyId: $id) { proxyId proxyUserId proxyType proxyStatus isCurrentlyActive } }"
    $r.data.proxyRightById.proxyId -eq $id
}

Test-Endpoint "GraphQL query proxyRightsByUser" {
    $r = Invoke-GQL 'query { proxyRightsByUser(proxyUserId: 100) { proxyId proxyType proxyStatus } }'
    $r.data.proxyRightsByUser -is [array]
}

Test-Endpoint "GraphQL mutation createProxyRight" {
    $r = Invoke-GQL "mutation { createProxyRight(input: { proxyUserId: 700, delegatedUserId: 800, proxyStartDate: `"$(Get-Date -Format 'yyyy-MM-ddTHH:mm:ss')Z`", proxyEndDate: `"$((Get-Date).AddDays(90).ToString('yyyy-MM-ddTHH:mm:ss'))Z`", proxyType: `"FULL`", scope: `"ALL`", notes: `"GraphQL test $runId`", createdBy: 1 }) { proxyId proxyType proxyStatus } }"
    $r.data.createProxyRight.proxyId -gt 0 -and $r.data.createProxyRight.proxyType -eq "FULL"
}

Test-Endpoint "GraphQL mutation updateProxyRight" {
    if (-not $script:proxyId) { return $false }
    $id = $script:proxyId
    $r = Invoke-GQL "mutation { updateProxyRight(proxyId: $id, input: { proxyId: $id, proxyType: `"SUBMISSION`", notes: `"GQL updated $runId`", updatedBy: 1 }) { proxyId proxyType notes } }"
    $r.data.updateProxyRight.proxyType -eq "SUBMISSION"
}

Test-Endpoint "GraphQL mutation deactivateProxyRight" {
    if (-not $script:proxyId) { return $false }
    $id = $script:proxyId
    $r = Invoke-GQL "mutation { deactivateProxyRight(proxyId: $id, updatedBy: 1) }"
    $r.data.deactivateProxyRight -eq $true
}

Test-Endpoint "GraphQL query verify deactivated" {
    if (-not $script:proxyId) { return $false }
    $id = $script:proxyId
    $r = Invoke-GQL "query { proxyRightById(proxyId: $id) { proxyId proxyStatus isCurrentlyActive } }"
    $r.data.proxyRightById.proxyStatus -eq "I"
}

# ─── 6. RABBITMQ ──────────────────────────────────────────────────────────────
Write-Host "`n[6] RabbitMQ" -ForegroundColor Yellow

Test-Endpoint "GET /api/rabbitmq/test - returns status" {
    $r = Invoke-RestMethod "$baseUrl/api/rabbitmq/test"
    $r.service -eq "RabbitMQ" -and ($r.status -eq "Available" -or $r.status -eq "Disconnected")
}

# ─── SUMMARY ──────────────────────────────────────────────────────────────────
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  Results: $($script:passed)/$($script:total) passed" -ForegroundColor $(if ($script:failed -eq 0) { "Green" } else { "Yellow" })
if ($script:failed -gt 0) {
    Write-Host "  Failed: $($script:failed)" -ForegroundColor Red
}
Write-Host "========================================`n" -ForegroundColor Cyan
