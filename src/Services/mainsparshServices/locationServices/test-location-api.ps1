# LocationService.API Test Script
# Port: 5000
# Run: dotnet run --project LocationService.API

$baseUrl = "http://localhost:5000"
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
Write-Host "  LocationService.API Tests" -ForegroundColor Cyan
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
    $r = Invoke-RestMethod "$baseUrl/api/auth/token" -Method POST -ContentType "application/json"
    $script:token = $r.token
    $r.token -ne $null -and $r.token.Length -gt 20
}

Test-Endpoint "POST /api/auth/token - returns expiresIn 3600" {
    $r = Invoke-RestMethod "$baseUrl/api/auth/token" -Method POST -ContentType "application/json"
    $r.expiresIn -eq 3600
}

$headers = @{ Authorization = "Bearer $($script:token)" }

# ─── 3. REST - LOCATIONS ──────────────────────────────────────────────────────
Write-Host "`n[3] REST Locations Controller" -ForegroundColor Yellow

$script:createdLocId = $null

Test-Endpoint "GET /api/locations - returns list" {
    $r = Invoke-RestMethod "$baseUrl/api/locations" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "POST /api/locations - creates location" {
    $body = @{
        locationCode = "LOC$runId"
        locationName = "Test Location $runId"
        city         = "TestCity"
        state        = "TS"
        country      = "India"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/locations" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:createdLocId = $r.locationId
    $r.locationId -gt 0 -and $r.locationCode -eq "LOC$runId"
}

Test-Endpoint "GET /api/locations/{id} - find newly created" {
    if (-not $script:createdLocId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/locations/$($script:createdLocId)" -Headers $headers
    $r.locationId -eq $script:createdLocId
}

Test-Endpoint "GET /api/locations/code/LOC$runId - get by code" {
    $r = Invoke-RestMethod "$baseUrl/api/locations/code/LOC$runId" -Headers $headers
    $r.locationCode -eq "LOC$runId"
}

Test-Endpoint "GET /api/locations/active - returns active locations" {
    $r = Invoke-RestMethod "$baseUrl/api/locations/active" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "GET /api/locations/search?searchText=Test - search returns results" {
    $r = Invoke-RestMethod "$baseUrl/api/locations/search?searchText=Test" -Headers $headers
    $r -is [array]
}

Test-Endpoint "PUT /api/locations/{id} - updates location" {
    if (-not $script:createdLocId) { return $false }
    $body = @{
        locationName = "Updated Location $runId"
        city         = "UpdatedCity"
        country      = "India"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/locations/$($script:createdLocId)" -Method PUT -Body $body -ContentType "application/json" -Headers $headers
    $r.locationName -eq "Updated Location $runId"
}

Test-Endpoint "GET /api/locations/999 - 404 for nonexistent" {
    try { Invoke-RestMethod "$baseUrl/api/locations/999" -Headers $headers; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

# ─── 4. REST - ROOMS ──────────────────────────────────────────────────────────
Write-Host "`n[4] REST Rooms Controller" -ForegroundColor Yellow

$script:createdRoomId = $null

Test-Endpoint "POST /api/rooms - creates room" {
    if (-not $script:createdLocId) { return $false }
    $body = @{
        locationId   = $script:createdLocId
        roomCode     = "RM$runId"
        roomName     = "Test Room $runId"
        roomCapacity = 20
        roomType     = "Conference"
        floorNumber  = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/rooms" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:createdRoomId = $r.roomId
    $r.roomId -gt 0 -and $r.roomCode -eq "RM$runId"
}

Test-Endpoint "GET /api/rooms/{id} - find by ID" {
    if (-not $script:createdRoomId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/rooms/$($script:createdRoomId)" -Headers $headers
    $r.roomId -eq $script:createdRoomId
}

Test-Endpoint "GET /api/rooms/location/{locId} - get by location" {
    if (-not $script:createdLocId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/rooms/location/$($script:createdLocId)" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "PUT /api/rooms/{id} - updates room" {
    if (-not $script:createdRoomId) { return $false }
    $body = @{ roomName = "Updated Room $runId"; roomCapacity = 30 } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/rooms/$($script:createdRoomId)" -Method PUT -Body $body -ContentType "application/json" -Headers $headers
    $r.roomName -eq "Updated Room $runId"
}

# ─── 5. REST - ROOM RESOURCES ─────────────────────────────────────────────────
Write-Host "`n[5] REST Room Resources Controller" -ForegroundColor Yellow

$script:createdResId = $null

Test-Endpoint "POST /api/roomresources - creates resource" {
    if (-not $script:createdRoomId -or -not $script:createdLocId) { return $false }
    $body = @{
        roomId           = $script:createdRoomId
        locationId       = $script:createdLocId
        resourceCode     = "RES$runId"
        resourceName     = "Projector $runId"
        resourceType     = "AV"
        resourceQuantity = 2
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/roomresources" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:createdResId = $r.resourceId
    $r.resourceId -gt 0 -and $r.resourceCode -eq "RES$runId"
}

Test-Endpoint "GET /api/roomresources/{id} - find by ID" {
    if (-not $script:createdResId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/roomresources/$($script:createdResId)" -Headers $headers
    $r.resourceId -eq $script:createdResId
}

Test-Endpoint "GET /api/roomresources/room/{roomId} - get by room" {
    if (-not $script:createdRoomId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/roomresources/room/$($script:createdRoomId)" -Headers $headers
    $r -is [array] -or $r -ne $null
}

# ─── 6. MINIMAL API (/api/minimal/...) ────────────────────────────────────────
Write-Host "`n[6] Minimal API Endpoints" -ForegroundColor Yellow

$script:minLocId = $null
$script:minRoomId = $null
$script:minResId = $null

Test-Endpoint "GET /api/minimal/locations - returns list" {
    $r = Invoke-RestMethod "$baseUrl/api/minimal/locations"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "GET /api/minimal/locations/active - returns active" {
    $r = Invoke-RestMethod "$baseUrl/api/minimal/locations/active"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "POST /api/minimal/locations - creates location" {
    $body = @{
        locationCode = "MIN$runId"
        locationName = "Minimal Location $runId"
        city         = "MinCity"
        country      = "India"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/minimal/locations" -Method POST -Body $body -ContentType "application/json"
    $script:minLocId = $r.locationId
    $r.locationId -gt 0 -and $r.locationCode -eq "MIN$runId"
}

Test-Endpoint "GET /api/minimal/locations/{id} - get by ID" {
    if (-not $script:minLocId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/minimal/locations/$($script:minLocId)"
    $r.locationId -eq $script:minLocId
}

Test-Endpoint "GET /api/minimal/locations/code/MIN$runId - get by code" {
    $r = Invoke-RestMethod "$baseUrl/api/minimal/locations/code/MIN$runId"
    $r.locationCode -eq "MIN$runId"
}

Test-Endpoint "GET /api/minimal/locations/search?searchText=Minimal - search" {
    $r = Invoke-RestMethod "$baseUrl/api/minimal/locations/search?searchText=Minimal"
    $r -is [array]
}

Test-Endpoint "PUT /api/minimal/locations/{id} - updates" {
    if (-not $script:minLocId) { return $false }
    $body = @{ locationName = "Updated Minimal $runId"; city = "UpdatedMin" } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/minimal/locations/$($script:minLocId)" -Method PUT -Body $body -ContentType "application/json"
    $r.locationName -eq "Updated Minimal $runId"
}

Test-Endpoint "GET /api/minimal/locations/999 - 404 for nonexistent" {
    try { Invoke-RestMethod "$baseUrl/api/minimal/locations/999"; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

Test-Endpoint "POST /api/minimal/rooms - creates room" {
    if (-not $script:minLocId) { return $false }
    $body = @{
        locationId   = $script:minLocId
        roomCode     = "MRM$runId"
        roomName     = "Minimal Room $runId"
        roomCapacity = 10
        roomType     = "Meeting"
        floorNumber  = 2
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/minimal/rooms" -Method POST -Body $body -ContentType "application/json"
    $script:minRoomId = $r.roomId
    $r.roomId -gt 0 -and $r.roomCode -eq "MRM$runId"
}

Test-Endpoint "GET /api/minimal/rooms/{id} - get by ID" {
    if (-not $script:minRoomId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/minimal/rooms/$($script:minRoomId)"
    $r.roomId -eq $script:minRoomId
}

Test-Endpoint "GET /api/minimal/rooms/location/{locId} - get by location" {
    if (-not $script:minLocId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/minimal/rooms/location/$($script:minLocId)"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "PUT /api/minimal/rooms/{id} - updates room" {
    if (-not $script:minRoomId) { return $false }
    $body = @{ roomName = "Updated Min Room $runId"; roomCapacity = 15 } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/minimal/rooms/$($script:minRoomId)" -Method PUT -Body $body -ContentType "application/json"
    $r.roomName -eq "Updated Min Room $runId"
}

Test-Endpoint "POST /api/minimal/roomresources - creates resource" {
    if (-not $script:minRoomId -or -not $script:minLocId) { return $false }
    $body = @{
        roomId           = $script:minRoomId
        locationId       = $script:minLocId
        resourceCode     = "MRE$runId"
        resourceName     = "Whiteboard $runId"
        resourceType     = "Furniture"
        resourceQuantity = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/minimal/roomresources" -Method POST -Body $body -ContentType "application/json"
    $script:minResId = $r.resourceId
    $r.resourceId -gt 0 -and $r.resourceCode -eq "MRE$runId"
}

Test-Endpoint "GET /api/minimal/roomresources/{id} - get by ID" {
    if (-not $script:minResId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/minimal/roomresources/$($script:minResId)"
    $r.resourceId -eq $script:minResId
}

Test-Endpoint "GET /api/minimal/roomresources/room/{roomId} - get by room" {
    if (-not $script:minRoomId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/minimal/roomresources/room/$($script:minRoomId)"
    $r -is [array] -or $r -ne $null
}

# ─── 7. GRAPHQL ────────────────────────────────────────────────────────────────
Write-Host "`n[7] GraphQL Endpoints" -ForegroundColor Yellow

$gqlUrl = "$baseUrl/graphql"
$gqlHeaders = @{ "Content-Type" = "application/json" }

function Invoke-GQL($query) {
    $body = (@{ query = $query } | ConvertTo-Json)
    return Invoke-RestMethod $gqlUrl -Method POST -Body $body -Headers $gqlHeaders
}

Test-Endpoint "GraphQL query hello - returns greeting" {
    $r = Invoke-GQL 'query { hello }'
    $r.data.hello -like "*Hello*"
}

Test-Endpoint "GraphQL mutation message - returns ready message" {
    $r = Invoke-GQL 'mutation { message }'
    $r.data.message -ne $null
}

# ─── 8. RABBITMQ ──────────────────────────────────────────────────────────────
Write-Host "`n[8] RabbitMQ" -ForegroundColor Yellow

Test-Endpoint "GET /api/rabbitmq/test - returns status" {
    $r = Invoke-RestMethod "$baseUrl/api/rabbitmq/test"
    $r.service -eq "RabbitMQ" -and ($r.status -eq "Connected" -or $r.status -eq "Disconnected")
}

# ─── CLEANUP ──────────────────────────────────────────────────────────────────
# Delete test resources (best-effort)
if ($script:createdResId) {
    try { Invoke-RestMethod "$baseUrl/api/roomresources/$($script:createdResId)" -Method DELETE -Headers $headers -ErrorAction SilentlyContinue } catch {}
}
if ($script:createdRoomId) {
    try { Invoke-RestMethod "$baseUrl/api/rooms/$($script:createdRoomId)" -Method DELETE -Headers $headers -ErrorAction SilentlyContinue } catch {}
}
if ($script:createdLocId) {
    try { Invoke-RestMethod "$baseUrl/api/locations/$($script:createdLocId)" -Method DELETE -Headers $headers -ErrorAction SilentlyContinue } catch {}
}

# ─── SUMMARY ──────────────────────────────────────────────────────────────────
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  Results: $($script:passed)/$($script:total) passed" -ForegroundColor $(if ($script:failed -eq 0) { "Green" } else { "Yellow" })
if ($script:failed -gt 0) {
    Write-Host "  Failed: $($script:failed)" -ForegroundColor Red
}
Write-Host "========================================`n" -ForegroundColor Cyan
