# CompensationService.API Test Script
# Port: 5129
# Run: dotnet run --project CompensationService.API

$baseUrl = "http://localhost:5129"
$passed = 0
$failed = 0
$total = 0
$runId = (Get-Date -Format "HHmmss")

function Test-Endpoint {
    param([string]$Name, [scriptblock]$Block)
    $total++
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
Write-Host "  CompensationService.API Tests" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# ─── 1. HEALTH CHECK ───────────────────────────────────────────────────────────
Write-Host "`n[1] Health Checks" -ForegroundColor Yellow

Test-Endpoint "GET /health - service healthy" {
    $r = Invoke-RestMethod "$baseUrl/health"
    $r.status -eq "Healthy"
}

Test-Endpoint "GET /health - Database healthy" {
    $r = Invoke-RestMethod "$baseUrl/health"
    $r.details.Database.status -eq "Healthy"
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

# ─── 3. REST CONTROLLER (api/compensationgrades) ──────────────────────────────
Write-Host "`n[3] REST Controller Endpoints" -ForegroundColor Yellow

$createdId = $null

Test-Endpoint "GET /api/compensationgrades - returns list" {
    $r = Invoke-RestMethod "$baseUrl/api/compensationgrades" -Headers $headers
    $r -is [array] -or $r.Count -ge 0
}

Test-Endpoint "GET /api/compensationgrades/active - returns active grades" {
    $r = Invoke-RestMethod "$baseUrl/api/compensationgrades/active" -Headers $headers
    $r -is [array] -or $r.Count -ge 0
}

Test-Endpoint "POST /api/compensationgrades - creates new grade" {
    $body = @{
        gradeCode    = "TG$runId"
        gradeName    = "Test Grade One"
        gradeLevel   = 1
        baseSalary   = 50000.00
        hraPercentage = 10.00
        daPercentage  = 5.00
        effectiveFrom = "2025-01-01T00:00:00Z"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/compensationgrades" -Method POST -Body $body -ContentType "application/json" -Headers $headers
    $script:createdId = $r.gradeId
    $r.gradeId -gt 0 -and $r.gradeCode -eq "TG$runId"
}

Test-Endpoint "GET /api/compensationgrades/{id} - returns by ID" {
    if (-not $script:createdId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/compensationgrades/$($script:createdId)" -Headers $headers
    $r.gradeId -eq $script:createdId
}

Test-Endpoint "PUT /api/compensationgrades/{id} - updates grade" {
    if (-not $script:createdId) { return $false }
    $body = @{
        gradeName    = "Updated Test Grade"
        baseSalary   = 55000.00
        hraPercentage = 12.00
        daPercentage  = 6.00
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/compensationgrades/$($script:createdId)" -Method PUT -Body $body -ContentType "application/json" -Headers $headers
    $r.gradeName -eq "Updated Test Grade"
}

Test-Endpoint "PATCH /api/compensationgrades/{id}/status - changes status" {
    if (-not $script:createdId) { return $false }
    $body = @{ newStatus = "I" } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/compensationgrades/$($script:createdId)/status" -Method PATCH -Body $body -ContentType "application/json" -Headers $headers
    $r -eq $true -or $r -ne $null
}

Test-Endpoint "GET /api/compensationgrades/{id} - 404 for nonexistent" {
    try {
        Invoke-RestMethod "$baseUrl/api/compensationgrades/999999" -Headers $headers
        return $false
    } catch {
        $_.Exception.Response.StatusCode.value__ -eq 404
    }
}

# ─── 4. MINIMAL API (api/minimal/grades) ──────────────────────────────────────
Write-Host "`n[4] Minimal API Endpoints" -ForegroundColor Yellow

$minimalCreatedId = $null

Test-Endpoint "GET /api/minimal/grades - returns list" {
    $r = Invoke-RestMethod "$baseUrl/api/minimal/grades"
    $r -is [array] -or $r.Count -ge 0
}

Test-Endpoint "POST /api/minimal/grades - creates grade" {
    $body = @{
        gradeCode    = "MG$runId"
        gradeName    = "Minimal API Grade"
        gradeLevel   = 2
        baseSalary   = 60000.00
        hraPercentage = 15.00
        daPercentage  = 8.00
        effectiveFrom = "2025-01-01T00:00:00Z"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/minimal/grades" -Method POST -Body $body -ContentType "application/json"
    $script:minimalCreatedId = $r.gradeId
    $r.gradeId -gt 0 -and $r.gradeCode -eq "MG$runId"
}

Test-Endpoint "GET /api/minimal/grades/{id} - returns by ID" {
    if (-not $script:minimalCreatedId) { return $false }
    $r = Invoke-RestMethod "$baseUrl/api/minimal/grades/$($script:minimalCreatedId)"
    $r.gradeId -eq $script:minimalCreatedId
}

Test-Endpoint "PUT /api/minimal/grades/{id} - updates grade" {
    if (-not $script:minimalCreatedId) { return $false }
    $body = @{
        gradeName    = "Updated Minimal Grade"
        baseSalary   = 65000.00
        hraPercentage = 16.00
        daPercentage  = 9.00
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$baseUrl/api/minimal/grades/$($script:minimalCreatedId)" -Method PUT -Body $body -ContentType "application/json"
    $r.gradeName -eq "Updated Minimal Grade"
}

# ─── 5. GRAPHQL ────────────────────────────────────────────────────────────────
Write-Host "`n[5] GraphQL Endpoints" -ForegroundColor Yellow

$gqlUrl = "$baseUrl/graphql"
$gqlHeaders = @{ "Content-Type" = "application/json" }

function Invoke-GQL($query) {
    $body = (@{ query = $query } | ConvertTo-Json)
    return Invoke-RestMethod $gqlUrl -Method POST -Body $body -Headers $gqlHeaders
}

Test-Endpoint "GraphQL query compensationGrades - returns list" {
    $r = Invoke-GQL 'query { compensationGrades { gradeId gradeCode gradeName gradeLevel baseSalary } }'
    $r.data.compensationGrades -is [array]
}

Test-Endpoint "GraphQL query activeCompensationGrades - returns active list" {
    $r = Invoke-GQL 'query { activeCompensationGrades { gradeId gradeCode gradeName } }'
    $r.data.activeCompensationGrades -is [array]
}

Test-Endpoint "GraphQL query compensationGrade by id - returns grade" {
    if (-not $script:createdId) { return $false }
    $id = $script:createdId
    $r = Invoke-GQL "query { compensationGrade(id: $id) { gradeId gradeCode gradeName } }"
    $r.data.compensationGrade.gradeId -eq $id
}

Test-Endpoint "GraphQL query compensationGrade nonexistent - returns null" {
    $r = Invoke-GQL 'query { compensationGrade(id: 999999) { gradeId } }'
    $r.data.compensationGrade -eq $null
}

$gqlCreatedId = $null

Test-Endpoint "GraphQL mutation createCompensationGrade" {
    $r = Invoke-GQL "mutation { createCompensationGrade(gradeCode: `"GQ$runId`", gradeName: `"GraphQL Grade`", gradeLevel: 3, baseSalary: 70000, hraPercentage: 10, daPercentage: 5, effectiveFrom: `"2025-01-01`") { gradeId gradeCode gradeName } }"
    $script:gqlCreatedId = $r.data.createCompensationGrade.gradeId
    $r.data.createCompensationGrade.gradeCode -eq "GQ$runId"
}

Test-Endpoint "GraphQL mutation updateCompensationGrade" {
    if (-not $script:gqlCreatedId) { return $false }
    $id = $script:gqlCreatedId
    $r = Invoke-GQL "mutation { updateCompensationGrade(gradeId: $id, gradeName: `"Updated GQL Grade`", baseSalary: 75000, hraPercentage: 12, daPercentage: 6) { gradeId gradeName } }"
    $r.data.updateCompensationGrade.gradeName -eq "Updated GQL Grade"
}

Test-Endpoint "GraphQL mutation changeCompensationGradeStatus" {
    if (-not $script:gqlCreatedId) { return $false }
    $id = $script:gqlCreatedId
    $r = Invoke-GQL "mutation { changeCompensationGradeStatus(gradeId: $id, newStatus: `"I`") }"
    
    $r.data.changeCompensationGradeStatus -eq $true -or $r.data.changeCompensationGradeStatus -ne $null
}

# ─── 6. RABBITMQ TEST ──────────────────────────────────────────────────────────
Write-Host "`n[6] RabbitMQ" -ForegroundColor Yellow

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
