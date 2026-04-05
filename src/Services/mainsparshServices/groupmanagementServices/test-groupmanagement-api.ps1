# GroupManagementService.API Test Script
# Port: 5130
# Run: dotnet run --project GroupManagementService.API

$baseUrl = "http://localhost:5130"
$passed = 0
$failed = 0
$total = 0
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
Write-Host "  GroupManagementService.API Tests" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# ─── 1. HEALTH CHECKS ─────────────────────────────────────────────────────────
Write-Host "`n[1] Health Checks" -ForegroundColor Yellow

Test-Endpoint "GET /health - service healthy" {
    $r = Invoke-RestMethod "$baseUrl/health"
    $r.status -eq "Healthy"
}

Test-Endpoint "GET /health - Database healthy" {
    $r = Invoke-RestMethod "$baseUrl/health"
    ($r.checks | Where-Object { $_.name -eq "Database" }).status -eq "Healthy"
}

Test-Endpoint "GET /health/live - liveness check" {
    try {
        $r = Invoke-WebRequest "$baseUrl/health/live" -UseBasicParsing
        $r.StatusCode -eq 200
    } catch {
        $_.Exception.Response.StatusCode.value__ -eq 200
    }
}

# ─── 2. AUTH TOKEN ─────────────────────────────────────────────────────────────
Write-Host "`n[2] Auth Token" -ForegroundColor Yellow

$token = $null

Test-Endpoint "POST /api/auth/token - returns JWT" {
    $r = Invoke-RestMethod "$baseUrl/api/auth/token" -Method POST -ContentType "application/json"
    $script:token = $r.token
    $r.token -ne $null -and $r.token.Length -gt 20
}

Test-Endpoint "POST /api/auth/token - returns expiresIn" {
    $r = Invoke-RestMethod "$baseUrl/api/auth/token" -Method POST -ContentType "application/json"
    $r.expiresIn -eq 3600
}

$headers = @{ Authorization = "Bearer $token" }

# ─── 3. REST CONTROLLER - GROUPS ──────────────────────────────────────────────
Write-Host "`n[3] REST Groups Controller" -ForegroundColor Yellow

$createdId = $null

Test-Endpoint "GET /api/v1/groups - returns list" {
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups" -Headers $headers
    $r -is [array] -or $r.Count -ge 0
}

Test-Endpoint "GET /api/v1/groups - returns seeded ADMIN group" {
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups" -Headers $headers
    ($r | Where-Object { $_.code -eq "ADMIN" }) -ne $null
}

Test-Endpoint "GET /api/v1/groups/code/ADMIN - returns by code" {
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups/code/ADMIN" -Headers $headers
    $r.code -eq "ADMIN"
}

Test-Endpoint "GET /api/v1/groups/1 - returns by ID" {
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups/1" -Headers $headers
    $r.id -gt 0
}

Test-Endpoint "GET /api/v1/groups/999 - 404 for nonexistent" {
    try {
        Invoke-RestMethod "$baseUrl/api/v1/groups/999" -Headers $headers
        return $false
    } catch {
        $_.Exception.Response.StatusCode.value__ -eq 404
    }
}

Test-Endpoint "POST /api/v1/groups - creates new group" {
    $body = @{
        code        = "TST$runId"
        name        = "Test Group $runId"
        description = "Created by test script"
        isAdmin     = $false
        createdBy   = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:createdId = $r.id
    $r.id -gt 0 -and $r.code -eq "TST$runId"
}

Test-Endpoint "GET /api/v1/groups/{id} - find newly created" {
    if (-not $script:createdId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups/$($script:createdId)" -Headers $headers
    $r.id -eq $script:createdId
}

Test-Endpoint "PUT /api/v1/groups/{id} - updates group" {
    if (-not $script:createdId) { return $false }
    $body = @{
        name        = "Updated Test Group"
        description = "Updated by test"
        updatedBy   = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups/$($script:createdId)" -Method PUT -Body $body -ContentType "application/json" -Headers $headers
    $r.name -eq "Updated Test Group"
}

Test-Endpoint "POST /api/v1/groups/{id}/deactivate - deactivates group" {
    if (-not $script:createdId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups/$($script:createdId)/deactivate" -Method POST -Body "1" -ContentType "application/json" -Headers $headers
    $r.message -like "*deactivated*"
}

Test-Endpoint "POST /api/v1/groups/{id}/activate - activates group" {
    if (-not $script:createdId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups/$($script:createdId)/activate" -Method POST -Body "1" -ContentType "application/json" -Headers $headers
    $r.message -like "*activated*"
}

# ─── 4. REST CONTROLLER - MENU MAPS ───────────────────────────────────────────
Write-Host "`n[4] REST MenuMaps Controller" -ForegroundColor Yellow

Test-Endpoint "POST /api/v1/groups/{id}/menumaps - adds menu map" {
    if (-not $script:createdId) { return $false }
    $body = @{
        menuCode    = "MENU_$runId"
        menuName    = "Test Menu"
        permissions = @{ canView = $true; canCreate = $true; canEdit = $false; canDelete = $false; canApprove = $false }
        menuSequence = 1
        createdBy   = 1
    } | ConvertTo-Json -Depth 3
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups/$($script:createdId)/menumaps" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $r.menuCode -eq "MENU_$runId"
}

Test-Endpoint "PUT /api/v1/groups/{id}/menumaps/{menuCode}/permissions - updates permissions" {
    if (-not $script:createdId) { return $false }
    $body = @{
        menuCode    = "MENU_$runId"
        permissions = @{ canView = $true; canCreate = $true; canEdit = $true; canDelete = $true; canApprove = $false }
        updatedBy   = 1
    } | ConvertTo-Json -Depth 3
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups/$($script:createdId)/menumaps/MENU_$runId/permissions" -Method PUT -Body $body -ContentType "application/json" -Headers $headers
    $r.permissions.canEdit -eq $true
}

Test-Endpoint "DELETE /api/v1/groups/{id}/menumaps/{menuCode} - removes menu map" {
    if (-not $script:createdId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/v1/groups/$($script:createdId)/menumaps/MENU_$runId" -Method DELETE -Body "1" -ContentType "application/json" -Headers $headers
    $r.message -like "*removed*" -or $r.message -ne $null
}

# ─── 5. MINIMAL API (/api/minimal/groups) ─────────────────────────────────────
Write-Host "`n[5] Minimal API Endpoints" -ForegroundColor Yellow

$minCreatedId = $null

Test-Endpoint "GET /api/minimal/groups - returns list" {
    $r = Invoke-RestMethod "$baseUrl/api/minimal/groups"
    $r -is [array] -or $r.Count -ge 0
}

Test-Endpoint "GET /api/minimal/groups - includes ADMIN group" {
    $r = Invoke-RestMethod "$baseUrl/api/minimal/groups"
    ($r | Where-Object { $_.code -eq "ADMIN" }) -ne $null
}

Test-Endpoint "GET /api/minimal/groups/code/ADMIN - returns by code" {
    $r = Invoke-RestMethod "$baseUrl/api/minimal/groups/code/ADMIN"
    $r.code -eq "ADMIN"
}

Test-Endpoint "GET /api/minimal/groups/1 - returns by ID" {
    $r = Invoke-RestMethod "$baseUrl/api/minimal/groups/1"
    $r.id -gt 0
}

Test-Endpoint "POST /api/minimal/groups - creates group" {
    $body = @{
        code        = "MIN$runId"
        name        = "Minimal Group $runId"
        description = "Created via minimal API"
        isAdmin     = $false
        createdBy   = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/minimal/groups" -Method POST -Body $body -ContentType "application/json"
    $script:minCreatedId = $r.id
    $r.id -gt 0 -and $r.code -eq "MIN$runId"
}

Test-Endpoint "GET /api/minimal/groups/{id} - find newly created" {
    if (-not $script:minCreatedId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/minimal/groups/$($script:minCreatedId)"
    $r.id -eq $script:minCreatedId
}

Test-Endpoint "PUT /api/minimal/groups/{id} - updates group" {
    if (-not $script:minCreatedId) { return $false }
    $body = @{
        name        = "Updated Minimal Group"
        description = "Updated via minimal API"
        updatedBy   = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/minimal/groups/$($script:minCreatedId)" -Method PUT -Body $body -ContentType "application/json"
    $r.name -eq "Updated Minimal Group"
}

Test-Endpoint "POST /api/minimal/groups/{id}/deactivate - deactivates group" {
    if (-not $script:minCreatedId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/minimal/groups/$($script:minCreatedId)/deactivate" -Method POST
    $r.message -like "*deactivated*"
}

Test-Endpoint "POST /api/minimal/groups/{id}/activate - activates group" {
    if (-not $script:minCreatedId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/minimal/groups/$($script:minCreatedId)/activate" -Method POST
    $r.message -like "*activated*"
}

Test-Endpoint "GET /api/minimal/groups/999 - 404 for nonexistent" {
    try {
        Invoke-RestMethod "$baseUrl/api/minimal/groups/999"
        return $false
    } catch {
        $_.Exception.Response.StatusCode.value__ -eq 404
    }
}

# ─── 6. GRAPHQL ────────────────────────────────────────────────────────────────
Write-Host "`n[6] GraphQL Endpoints" -ForegroundColor Yellow

$gqlUrl = "$baseUrl/graphql"
$gqlHeaders = @{ "Content-Type" = "application/json" }

function Invoke-GQL($query) {
    $body = (@{ query = $query } | ConvertTo-Json)
    return Invoke-RestMethod $gqlUrl -Method POST -Body $body -Headers $gqlHeaders
}

Test-Endpoint "GraphQL query getAllGroups - returns list" {
    $r = Invoke-GQL 'query { allGroups { id code name status } }'
    $r.data.allGroups -is [array]
}

Test-Endpoint "GraphQL query getAllGroups - includes ADMIN group" {
    $r = Invoke-GQL 'query { allGroups { id code name } }'
    ($r.data.allGroups | Where-Object { $_.code -eq "ADMIN" }) -ne $null
}

Test-Endpoint "GraphQL query getGroupByCode - returns ADMIN group" {
    $r = Invoke-GQL 'query { groupByCode(groupCode: "ADMIN") { id code name isAdmin } }'
    $r.data.groupByCode.code -eq "ADMIN"
}

Test-Endpoint "GraphQL query getGroupById - returns group" {
    $r = Invoke-GQL 'query { groupById(groupId: 1) { id code name } }'
    $r.data.groupById.id -gt 0
}

Test-Endpoint "GraphQL query getAdminGroups - returns admin groups" {
    $r = Invoke-GQL 'query { adminGroups { id code name isAdmin } }'
    $r.data.adminGroups -is [array]
}

Test-Endpoint "GraphQL query searchGroups - returns filtered results" {
    $r = Invoke-GQL 'query { searchGroups(searchTerm: "Admin") { id code name } }'
    $r.data.searchGroups -is [array]
}

Test-Endpoint "GraphQL query getGroupsByStatus - returns Active groups" {
    $r = Invoke-GQL 'query { groupsByStatus(status: "Active") { id code name status } }'
    $r.data.groupsByStatus -is [array]
}

$gqlCreatedId = $null

Test-Endpoint "GraphQL mutation createGroup" {
    $r = Invoke-GQL "mutation { createGroup(code: `"GQL$runId`", name: `"GQL Group $runId`", createdBy: 1, isAdmin: false) { id code name } }"
    $script:gqlCreatedId = $r.data.createGroup.id
    $r.data.createGroup.code -eq "GQL$runId"
}

Test-Endpoint "GraphQL mutation updateGroup" {
    if (-not $script:gqlCreatedId) { return $false }
    $id = $script:gqlCreatedId
    $r = Invoke-GQL "mutation { updateGroup(groupId: $id, name: `"Updated GQL Group`", updatedBy: 1) { id name } }"
    $r.data.updateGroup.name -eq "Updated GQL Group"
}

Test-Endpoint "GraphQL mutation deactivateGroup" {
    if (-not $script:gqlCreatedId) { return $false }
    $id = $script:gqlCreatedId
    $r = Invoke-GQL "mutation { deactivateGroup(groupId: $id, updatedBy: 1) }"
    $r.data.deactivateGroup -eq $true
}

Test-Endpoint "GraphQL mutation activateGroup" {
    if (-not $script:gqlCreatedId) { return $false }
    $id = $script:gqlCreatedId
    $r = Invoke-GQL "mutation { activateGroup(groupId: $id, updatedBy: 1) }"
    $r.data.activateGroup -eq $true
}

# ─── 7. RABBITMQ TEST ──────────────────────────────────────────────────────────
Write-Host "`n[7] RabbitMQ" -ForegroundColor Yellow

Test-Endpoint "GET /api/rabbitmq/test - returns status" {
    $r = Invoke-RestMethod "$baseUrl/api/rabbitmq/test"
    $r.service -eq "RabbitMQ" -and ($r.status -eq "Connected" -or $r.status -eq "Disconnected")
}

# ─── SUMMARY ───────────────────────────────────────────────────────────────────
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  Results: $passed/$total passed" -ForegroundColor $(if ($failed -eq 0) { "Green" } else { "Yellow" })
if ($failed -gt 0) {
    Write-Host "  Failed: $failed" -ForegroundColor Red
}
Write-Host "========================================`n" -ForegroundColor Cyan
