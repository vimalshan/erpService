# MeetingModule.API Test Script
# Port: 5225
# Run: dotnet run --project src/MeetingModule.API --launch-profile http

$baseUrl = "http://localhost:5225"
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
Write-Host "  MeetingModule.API Tests" -ForegroundColor Cyan
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
    $body = @{ username = "testuser"; userId = 1; role = "Admin" } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/auth/token" -Method POST -Body $body -ContentType "application/json"
    $script:token = $r.token
    $r.token -ne $null -and $r.token.Length -gt 20
}

Test-Endpoint "POST /api/auth/token - has expiration" {
    $body = @{ username = "testuser"; userId = 1; role = "Admin" } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/auth/token" -Method POST -Body $body -ContentType "application/json"
    $r.expiration -ne $null
}

$headers = @{ Authorization = "Bearer $($script:token)" }

# ─── 3. REST - MEETING TYPES ──────────────────────────────────────────────────
Write-Host "`n[3] REST Meeting Types Controller" -ForegroundColor Yellow

$script:meetTypeId = $null

Test-Endpoint "GET /api/meetingtypes - returns list" {
    $r = Invoke-RestMethod "$baseUrl/api/meetingtypes"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "GET /api/meetingtypes/active - returns active types" {
    $r = Invoke-RestMethod "$baseUrl/api/meetingtypes/active"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "POST /api/meetingtypes - creates meeting type" {
    $body = @{
        meetTypeCode = "MT$runId"
        meetTypeName = "Test Meeting Type $runId"
        meetTypeDesc = "Created by test script"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/meetingtypes" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:meetTypeId = $r.meetTypeId
    $r.meetTypeId -gt 0 -and $r.meetTypeCode -eq "MT$runId"
}

Test-Endpoint "GET /api/meetingtypes/{id} - find newly created type" {
    if (-not $script:meetTypeId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/meetingtypes/$($script:meetTypeId)"
    $r.meetTypeId -eq $script:meetTypeId
}

Test-Endpoint "GET /api/meetingtypes/code/MT$runId - get by code" {
    $r = Invoke-RestMethod "$baseUrl/api/meetingtypes/code/MT$runId"
    $r.meetTypeCode -eq "MT$runId"
}

Test-Endpoint "PUT /api/meetingtypes/{id} - updates meeting type" {
    if (-not $script:meetTypeId) { return $false }
    $body = @{
        meetTypeName = "Updated Type $runId"
        meetTypeDesc = "Updated by test"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/meetingtypes/$($script:meetTypeId)" -Method PUT -Body $body -ContentType "application/json" -Headers $headers
    $r.meetTypeName -eq "Updated Type $runId"
}

Test-Endpoint "GET /api/meetingtypes/999 - 404 for nonexistent" {
    try { Invoke-RestMethod "$baseUrl/api/meetingtypes/999"; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

# ─── 4. REST - MEETINGS ───────────────────────────────────────────────────────
Write-Host "`n[4] REST Meetings Controller" -ForegroundColor Yellow

$script:meetingId = $null

Test-Endpoint "GET /api/meetings - returns list" {
    $r = Invoke-RestMethod "$baseUrl/api/meetings"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "POST /api/meetings - creates meeting" {
    if (-not $script:meetTypeId) { return $false }
    $body = @{
        meetTypeId       = $script:meetTypeId
        meetingTitle     = "Test Meeting $runId"
        meetingDate      = (Get-Date).AddDays(7).ToString("o")
        meetingLocation  = "Conference Room A"
        meetingDuration  = 60
        organizerId      = 1
        notes            = "Created by test script"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/meetings" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:meetingId = $r.meetingId
    $r.meetingId -gt 0 -and $r.meetingTitle -eq "Test Meeting $runId"
}

Test-Endpoint "GET /api/meetings/{id} - find newly created" {
    if (-not $script:meetingId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/meetings/$($script:meetingId)"
    $r.meetingId -eq $script:meetingId
}

Test-Endpoint "GET /api/meetings/status/Scheduled - get by status" {
    $r = Invoke-RestMethod "$baseUrl/api/meetings/status/Scheduled"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "GET /api/meetings/organizer/1 - get by organizer" {
    $r = Invoke-RestMethod "$baseUrl/api/meetings/organizer/1"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "PUT /api/meetings/{id} - updates meeting" {
    if (-not $script:meetingId) { return $false }
    $body = @{
        meetingTitle    = "Updated Meeting $runId"
        meetingDate     = (Get-Date).AddDays(14).ToString("o")
        meetingLocation = "Conference Room B"
        meetingDuration = 90
        notes           = "Updated by test"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/meetings/$($script:meetingId)" -Method PUT -Body $body -ContentType "application/json" -Headers $headers
    $r.meetingTitle -eq "Updated Meeting $runId"
}

Test-Endpoint "PUT /api/meetings/{id}/start - starts meeting" {
    if (-not $script:meetingId) { return $false }
    Invoke-RestMethod "$baseUrl/api/meetings/$($script:meetingId)/start" -Method PUT -Headers $headers -ErrorAction SilentlyContinue
    $r = Invoke-RestMethod "$baseUrl/api/meetings/$($script:meetingId)"
    $r.meetingStatus -eq "ONGOING"
}

Test-Endpoint "PUT /api/meetings/{id}/complete - completes meeting" {
    if (-not $script:meetingId) { return $false }
    Invoke-RestMethod "$baseUrl/api/meetings/$($script:meetingId)/complete" -Method PUT -Headers $headers -ErrorAction SilentlyContinue
    $r = Invoke-RestMethod "$baseUrl/api/meetings/$($script:meetingId)"
    $r.meetingStatus -eq "COMPLETED"
}

Test-Endpoint "GET /api/meetings/999 - 404 for nonexistent" {
    try { Invoke-RestMethod "$baseUrl/api/meetings/999"; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

# ─── 5. REST - POLLS ──────────────────────────────────────────────────────────
Write-Host "`n[5] REST Polls Controller" -ForegroundColor Yellow

$script:pollId = $null

Test-Endpoint "POST /api/polls - creates poll" {
    if (-not $script:meetingId) { return $false }
    $body = @{
        meetingId    = $script:meetingId
        pollQuestion = "Test poll: $($runId)?"
        pollType     = "YES_NO"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/polls" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:pollId = $r.pollId
    $r.pollQuestion -like "*$($runId)*"
}

Test-Endpoint "GET /api/polls/{id} - find by ID" {
    if (-not $script:pollId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/polls/$($script:pollId)"
    $r.pollId -eq $script:pollId
}

Test-Endpoint "GET /api/polls/meeting/{meetingId} - get by meeting" {
    if (-not $script:meetingId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/polls/meeting/$($script:meetingId)"
    $r -is [array]
}

Test-Endpoint "PUT /api/polls/{id} - updates poll" {
    if (-not $script:pollId) { return $false }
    $body = @{
        pollQuestion = "Updated poll: $($runId)?"
        pollType     = "MULTIPLE_CHOICE"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/polls/$($script:pollId)" -Method PUT -Body $body -ContentType "application/json" -Headers $headers
    $r.pollQuestion -like "*Updated poll*"
}

# ─── 6. MINIMAL API (/api/v2/meetings) ────────────────────────────────────────
Write-Host "`n[6] Minimal API Endpoints" -ForegroundColor Yellow

Test-Endpoint "GET /api/v2/meetings/types - returns meeting types with counts" {
    $r = Invoke-RestMethod "$baseUrl/api/v2/meetings/types"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "GET /api/v2/meetings/upcoming - returns upcoming meetings" {
    $r = Invoke-RestMethod "$baseUrl/api/v2/meetings/upcoming"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "GET /api/v2/meetings/upcoming?top=5 - returns limited results" {
    $r = Invoke-RestMethod "$baseUrl/api/v2/meetings/upcoming?top=5"
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "GET /api/v2/meetings/{id}/detail - returns meeting detail" {
    if (-not $script:meetingId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/v2/meetings/$($script:meetingId)/detail"
    $r -ne $null
}

Test-Endpoint "GET /api/v2/meetings/999/detail - 404 for nonexistent" {
    try { Invoke-RestMethod "$baseUrl/api/v2/meetings/999/detail"; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

# ─── 7. GRAPHQL ────────────────────────────────────────────────────────────────
Write-Host "`n[7] GraphQL Endpoints" -ForegroundColor Yellow

$gqlUrl = "$baseUrl/graphql"
$gqlHeaders = @{ "Content-Type" = "application/json" }

function Invoke-GQL($query) {
    $body = (@{ query = $query } | ConvertTo-Json)
    return Invoke-RestMethod $gqlUrl -Method POST -Body $body -Headers $gqlHeaders
}

Test-Endpoint "GraphQL query getMeetingTypes - returns list" {
    $r = Invoke-GQL 'query { meetingTypes { meetTypeId meetTypeCode meetTypeName meetTypeStatus } }'
    $r.data.meetingTypes -is [array]
}

Test-Endpoint "GraphQL query getMeetings - returns list" {
    $r = Invoke-GQL 'query { meetings { meetingId meetingTitle meetingStatus } }'
    $r.data.meetings -is [array]
}

Test-Endpoint "GraphQL query getMeetingById - returns meeting" {
    if (-not $script:meetingId) { return $false }
    $id = $script:meetingId
    $r = Invoke-GQL "query { meetingById(id: $id) { meetingId meetingTitle meetingStatus } }"
    $r.data.meetingById.meetingId -eq $id
}

Test-Endpoint "GraphQL query getMeetingTypeById - returns type" {
    if (-not $script:meetTypeId) { return $false }
    $id = $script:meetTypeId
    $r = Invoke-GQL "query { meetingTypeById(id: $id) { meetTypeId meetTypeCode meetTypeName } }"
    $r.data.meetingTypeById.meetTypeId -eq $id
}

Test-Endpoint "GraphQL query getMeetingsByStatus - returns filtered list" {
    $r = Invoke-GQL 'query { meetingsByStatus(status: "Completed") { meetingId meetingTitle meetingStatus } }'
    $r.data.meetingsByStatus -is [array]
}

Test-Endpoint "GraphQL mutation createMeetingType" {
    $code = "GQL$runId"
    $r = Invoke-GQL "mutation { createMeetingType(input: { meetTypeCode: `"$code`", meetTypeName: `"GQL Type $runId`" }, userId: 1) { meetTypeId meetTypeCode } }"
    $r.data.createMeetingType.meetTypeCode -eq $code
}

# ─── 8. RABBITMQ ──────────────────────────────────────────────────────────────
Write-Host "`n[8] RabbitMQ" -ForegroundColor Yellow

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
