# TimesheetService.API Test Script
# Port: 5272
# Seed: 14 timesheets (emp 1001: 5 APPROVED + 3 SUBMITTED, emp 1002: 3 DRAFT, emp 1003: 1 REJECTED + 1 APPROVED)

$baseUrl = "http://localhost:5272"
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

function Invoke-Api {
    param([string]$Url, [string]$Method = "GET", $Body, $Headers)
    $params = @{ Uri = $Url; Method = $Method; UseBasicParsing = $true }
    if ($Headers) { $params.Headers = $Headers }
    if ($Body) { $params.Body = $Body; $params.ContentType = "application/json" }
    $resp = Invoke-WebRequest @params
    if ($resp.Content -is [byte[]]) {
        return [System.Text.Encoding]::UTF8.GetString($resp.Content) | ConvertFrom-Json
    }
    return $resp.Content | ConvertFrom-Json
}

function Invoke-ApiRaw {
    param([string]$Url, [string]$Method = "GET", $Body, $Headers)
    $params = @{ Uri = $Url; Method = $Method; UseBasicParsing = $true }
    if ($Headers) { $params.Headers = $Headers }
    if ($Body) { $params.Body = $Body; $params.ContentType = "application/json" }
    return Invoke-WebRequest @params
}

function Invoke-GQL($query) {
    $body = @{ query = $query } | ConvertTo-Json
    $resp = Invoke-WebRequest "$baseUrl/graphql" -Method POST -Body $body -ContentType "application/json" -UseBasicParsing
    if ($resp.Content -is [byte[]]) {
        return [System.Text.Encoding]::UTF8.GetString($resp.Content) | ConvertFrom-Json
    }
    return $resp.Content | ConvertFrom-Json
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  TimesheetService.API Tests" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# ─── 1. HEALTH CHECK ──────────────────────────────────────────────────────────
Write-Host "`n[1] Health Check" -ForegroundColor Yellow

Test-Endpoint "GET /health - service healthy" {
    $raw = Invoke-ApiRaw "$baseUrl/health"
    $text = if ($raw.Content -is [byte[]]) { [System.Text.Encoding]::UTF8.GetString($raw.Content) } else { $raw.Content }
    $text -match "Healthy"
}

Test-Endpoint "GET /health/db - database healthy" {
    $raw = Invoke-ApiRaw "$baseUrl/health/db"
    $text = if ($raw.Content -is [byte[]]) { [System.Text.Encoding]::UTF8.GetString($raw.Content) } else { $raw.Content }
    $text -match "Healthy"
}

# ─── 2. AUTH TOKEN ─────────────────────────────────────────────────────────────
Write-Host "`n[2] Auth Token" -ForegroundColor Yellow

$script:token = $null

Test-Endpoint "POST /api/v1/auth/login - returns JWT" {
    $body = '{}' 
    $r = Invoke-Api "$baseUrl/api/v1/auth/login" -Method POST -Body $body
    $script:token = $r.token
    $r.token -ne $null -and $r.token.Length -gt 20 -and $r.expiresAt -ne $null
}

$headers = @{ Authorization = "Bearer $($script:token)" }

# ─── 3. REST - TIMESHEETS CONTROLLER ──────────────────────────────────────────
Write-Host "`n[3] REST Timesheets Controller" -ForegroundColor Yellow

Test-Endpoint "GET /api/timesheets/1 - get seed timesheet by ID" {
    $r = Invoke-Api "$baseUrl/api/timesheets/1" -Headers $headers
    $r.timesheetId -eq 1 -and $r.employeeId -eq 1001 -and $r.status -eq "APPROVED"
}

Test-Endpoint "GET /api/timesheets/employee/1001 - get by employee" {
    $r = Invoke-Api "$baseUrl/api/timesheets/employee/1001" -Headers $headers
    $r -is [array] -and $r.Count -ge 8
}

Test-Endpoint "GET /api/timesheets/employee/1002 - get DRAFT by employee" {
    $r = Invoke-Api "$baseUrl/api/timesheets/employee/1002" -Headers $headers
    $r -is [array] -and $r.Count -ge 3 -and ($r | Where-Object { $_.status -eq "DRAFT" }).Count -ge 3
}

Test-Endpoint "GET /api/timesheets/pending - get pending (Manager role)" {
    $r = Invoke-Api "$baseUrl/api/timesheets/pending" -Headers $headers
    $r -is [array] -and $r.Count -ge 3
}

Test-Endpoint "GET /api/timesheets/999999 - 404 for nonexistent" {
    try { Invoke-Api "$baseUrl/api/timesheets/999999" -Headers $headers; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

Test-Endpoint "GET /api/timesheets/1 - 401 without auth" {
    try { Invoke-Api "$baseUrl/api/timesheets/1"; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 401 }
}

# Create a new timesheet
$script:newId = $null

Test-Endpoint "POST /api/timesheets - create new timesheet" {
    $body = @{
        employeeId      = 5000 + $runId
        timesheetDate   = "2026-04-04"
        workDate        = "2026-04-03"
        startTime       = "09:00:00"
        endTime         = "17:00:00"
        totalHours      = 8.0
        projectId       = 500
        taskId          = 600
        workDescription = "Test timesheet $runId"
        createdBy       = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/timesheets" -Method POST -Body $body -Headers $headers
    $script:newId = $r.timesheetId
    $r.timesheetId -gt 0 -and $r.status -eq "DRAFT" -and $r.approvalStatus -eq "PENDING"
}

Test-Endpoint "GET /api/timesheets/{id} - verify created" {
    if (-not $script:newId) { return $false }
    $r = Invoke-Api "$baseUrl/api/timesheets/$($script:newId)" -Headers $headers
    $r.timesheetId -eq $script:newId -and $r.totalHours -eq 8.0
}

# Submit the new timesheet
Test-Endpoint "POST /api/timesheets/{id}/submit - submit timesheet" {
    if (-not $script:newId) { return $false }
    $body = @{ actorId = 1 } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/timesheets/$($script:newId)/submit" -Method POST -Body $body -Headers $headers
    $r.status -eq "SUBMITTED" -and $r.approvalStatus -eq "PENDING"
}

# Approve the submitted timesheet
Test-Endpoint "POST /api/timesheets/{id}/approve - approve timesheet" {
    if (-not $script:newId) { return $false }
    $body = @{ actorId = 9001 } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/timesheets/$($script:newId)/approve" -Method POST -Body $body -Headers $headers
    $r.status -eq "APPROVED" -and $r.approvalStatus -eq "APPROVED" -and $r.approvedBy -eq 9001
}

# Create another timesheet for reject flow
$script:rejectId = $null

Test-Endpoint "POST /api/timesheets - create for reject flow" {
    $body = @{
        employeeId      = 6000 + $runId
        timesheetDate   = "2026-04-04"
        workDate        = "2026-04-02"
        startTime       = "09:00:00"
        endTime         = "13:00:00"
        totalHours      = 4.0
        projectId       = 500
        createdBy       = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/timesheets" -Method POST -Body $body -Headers $headers
    $script:rejectId = $r.timesheetId
    $r.timesheetId -gt 0 -and $r.status -eq "DRAFT"
}

Test-Endpoint "POST /api/timesheets/{id}/submit - submit for reject" {
    if (-not $script:rejectId) { return $false }
    $body = @{ actorId = 1 } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/timesheets/$($script:rejectId)/submit" -Method POST -Body $body -Headers $headers
    $r.status -eq "SUBMITTED"
}

Test-Endpoint "POST /api/timesheets/{id}/reject - reject timesheet" {
    if (-not $script:rejectId) { return $false }
    $body = @{ actorId = 9001; rejectionReason = "Hours mismatch - test $runId" } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/timesheets/$($script:rejectId)/reject" -Method POST -Body $body -Headers $headers
    $r.status -eq "REJECTED" -and $r.approvalStatus -eq "REJECTED" -and $r.rejectionReason -like "*mismatch*"
}

# ─── 4. MINIMAL API ───────────────────────────────────────────────────────────
Write-Host "`n[4] Minimal API" -ForegroundColor Yellow

Test-Endpoint "GET /api/min/timesheets/1 - get by ID" {
    $r = Invoke-Api "$baseUrl/api/min/timesheets/1" -Headers $headers
    $r.timesheetId -eq 1 -and $r.employeeId -eq 1001
}

Test-Endpoint "GET /api/min/timesheets/employee/1001 - get by employee" {
    $r = Invoke-Api "$baseUrl/api/min/timesheets/employee/1001" -Headers $headers
    $r -is [array] -and $r.Count -ge 8
}

Test-Endpoint "GET /api/min/timesheets/999999 - 404 for nonexistent" {
    try { Invoke-Api "$baseUrl/api/min/timesheets/999999" -Headers $headers; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

$script:minId = $null

Test-Endpoint "POST /api/min/timesheets - create via minimal API" {
    $body = @{
        employeeId      = 7000 + $runId
        timesheetDate   = "2026-04-04"
        workDate        = "2026-04-01"
        startTime       = "08:30:00"
        endTime         = "16:30:00"
        totalHours      = 8.0
        projectId       = 700
        taskId          = 800
        workDescription = "Minimal API test $runId"
        createdBy       = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/min/timesheets" -Method POST -Body $body -Headers $headers
    $script:minId = $r.timesheetId
    $r.timesheetId -gt 0 -and $r.status -eq "DRAFT"
}

Test-Endpoint "POST /api/min/timesheets/{id}/submit - submit via minimal" {
    if (-not $script:minId) { return $false }
    $r = Invoke-Api "$baseUrl/api/min/timesheets/$($script:minId)/submit" -Method POST -Body "1" -Headers $headers
    $r.status -eq "SUBMITTED"
}

# ─── 5. GRAPHQL ────────────────────────────────────────────────────────────────
Write-Host "`n[5] GraphQL Endpoints" -ForegroundColor Yellow

Test-Endpoint "GraphQL query timesheetById" {
    $r = Invoke-GQL '{ timesheetById(id: 1) { timesheetId employeeId workDate totalHours status approvalStatus } }'
    $r.data.timesheetById.timesheetId -eq 1 -and $r.data.timesheetById.status -eq "APPROVED"
}

Test-Endpoint "GraphQL query timesheetsByEmployee" {
    $r = Invoke-GQL '{ timesheetsByEmployee(employeeId: 1001) { timesheetId employeeId workDate totalHours status approvalStatus } }'
    $r.data.timesheetsByEmployee -is [array] -and $r.data.timesheetsByEmployee.Count -ge 8
}

Test-Endpoint "GraphQL query timesheetsByEmployee with date range" {
    $r = Invoke-GQL '{ timesheetsByEmployee(employeeId: 1001, from: "2026-03-01", to: "2026-03-15") { timesheetId status } }'
    $r.data.timesheetsByEmployee -is [array]
}

Test-Endpoint "GraphQL query pendingTimesheets" {
    $r = Invoke-GQL '{ pendingTimesheets { timesheetId employeeId status approvalStatus } }'
    $r.data.pendingTimesheets -is [array]
}

$script:gqlId = $null

Test-Endpoint "GraphQL mutation createTimesheet" {
    $gqlEmp = 8000 + (Get-Random -Minimum 100 -Maximum 9999)
    $r = Invoke-GQL "mutation { createTimesheet(employeeId: $gqlEmp, timesheetDate: `"2026-04-04`", workDate: `"2026-04-03`", startTime: `"09:00:00`", endTime: `"17:00:00`", totalHours: 8.0, projectId: 900, createdBy: 1) { timesheetId status approvalStatus } }"
    if ($r.data.createTimesheet) {
        $script:gqlId = $r.data.createTimesheet.timesheetId
        $r.data.createTimesheet.status -eq "DRAFT"
    } else { $false }
}

Test-Endpoint "GraphQL mutation submitTimesheet" {
    if (-not $script:gqlId) { return $false }
    $r = Invoke-GQL "mutation { submitTimesheet(timesheetId: $($script:gqlId), updatedBy: 1) { timesheetId status } }"
    $r.data.submitTimesheet.status -eq "SUBMITTED"
}

Test-Endpoint "GraphQL mutation approveTimesheet" {
    if (-not $script:gqlId) { return $false }
    $r = Invoke-GQL "mutation { approveTimesheet(timesheetId: $($script:gqlId), approverId: 9001) { timesheetId status approvalStatus approvedBy } }"
    $r.data.approveTimesheet.status -eq "APPROVED" -and $r.data.approveTimesheet.approvedBy -eq 9001
}

# Create another for reject via GraphQL
$script:gqlRejectId = $null

Test-Endpoint "GraphQL mutation createTimesheet for reject" {
    $gqlEmp2 = 9000 + (Get-Random -Minimum 100 -Maximum 9999)
    $r = Invoke-GQL "mutation { createTimesheet(employeeId: $gqlEmp2, timesheetDate: `"2026-04-04`", workDate: `"2026-04-02`", totalHours: 4.0, createdBy: 1) { timesheetId status } }"
    if ($r.data.createTimesheet) {
        $script:gqlRejectId = $r.data.createTimesheet.timesheetId
        $r.data.createTimesheet.status -eq "DRAFT"
    } else { $false }
}

Test-Endpoint "GraphQL mutation submitTimesheet for reject" {
    if (-not $script:gqlRejectId) { return $false }
    $r = Invoke-GQL "mutation { submitTimesheet(timesheetId: $($script:gqlRejectId), updatedBy: 1) { timesheetId status } }"
    $r.data.submitTimesheet.status -eq "SUBMITTED"
}

Test-Endpoint "GraphQL mutation rejectTimesheet" {
    if (-not $script:gqlRejectId) { return $false }
    $r = Invoke-GQL "mutation { rejectTimesheet(timesheetId: $($script:gqlRejectId), approverId: 9001, rejectionReason: `"GraphQL test rejection $runId`") { timesheetId status rejectionReason } }"
    $r.data.rejectTimesheet.status -eq "REJECTED" -and $r.data.rejectTimesheet.rejectionReason -like "*GraphQL*"
}

# ─── 6. RABBITMQ ──────────────────────────────────────────────────────────────
Write-Host "`n[6] RabbitMQ" -ForegroundColor Yellow

Test-Endpoint "GET /api/rabbitmq/test - returns status" {
    $r = Invoke-Api "$baseUrl/api/rabbitmq/test"
    $r.service -eq "RabbitMQ" -and ($r.status -eq "Available" -or $r.status -eq "Disconnected")
}

# ─── SUMMARY ──────────────────────────────────────────────────────────────────
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  Results: $($script:passed)/$($script:total) passed" -ForegroundColor $(if ($script:failed -eq 0) { "Green" } else { "Yellow" })
if ($script:failed -gt 0) {
    Write-Host "  Failed: $($script:failed)" -ForegroundColor Red
}
Write-Host "========================================`n" -ForegroundColor Cyan
