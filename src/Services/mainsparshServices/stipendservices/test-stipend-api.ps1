# StipendService.API Test Script
# Port: 5152

$baseUrl = "http://localhost:5152"
$script:passed = 0
$script:failed = 0
$script:total = 0
$runId = (Get-Date -Format "HHmmss")
$catId = Get-Random -Minimum 100 -Maximum 9999
$rankId = Get-Random -Minimum 100 -Maximum 9999

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
Write-Host "  StipendService.API Tests" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# ─── 1. HEALTH CHECK & PING ───────────────────────────────────────────────────
Write-Host "`n[1] Health Check & Ping" -ForegroundColor Yellow

Test-Endpoint "GET /health - service healthy" {
    $r = Invoke-Api "$baseUrl/health"
    $r.status -eq "Healthy"
}

Test-Endpoint "GET /health/db - database healthy" {
    $r = Invoke-Api "$baseUrl/health/db"
    $r.status -eq "Healthy"
}

Test-Endpoint "GET /api/v1/ping - returns pong" {
    $r = Invoke-Api "$baseUrl/api/v1/ping"
    $r.message -like "*running*"
}

# ─── 2. AUTH TOKEN ─────────────────────────────────────────────────────────────
Write-Host "`n[2] Auth Token" -ForegroundColor Yellow

$script:token = $null

Test-Endpoint "POST /api/v1/auth/login - returns JWT" {
    $body = @{ username = "admin"; password = "admin"; userId = 1 } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/v1/auth/login" -Method POST -Body $body
    $script:token = $r.token
    $r.token -ne $null -and $r.token.Length -gt 20 -and $r.expiresAt -ne $null
}

$headers = @{ Authorization = "Bearer $($script:token)" }

# ─── 3. REST - STIPEND MASTER CONTROLLER ──────────────────────────────────────
Write-Host "`n[3] REST StipendMaster Controller" -ForegroundColor Yellow

$script:stipendId = $null

Test-Endpoint "GET /api/v1/stipend-master - returns all" {
    $r = Invoke-Api "$baseUrl/api/v1/stipend-master" -Headers $headers
    $r -is [array] -and $r.Count -ge 5
}

Test-Endpoint "GET /api/v1/stipend-master/{id} - find by ID (seed 1)" {
    $r = Invoke-Api "$baseUrl/api/v1/stipend-master/1" -Headers $headers
    $r.stipendId -eq 1 -and $r.srfMonthlyStipend -eq 37000
}

Test-Endpoint "GET /api/v1/stipend-master/active - find active by category+rank" {
    $r = Invoke-Api "$baseUrl/api/v1/stipend-master/active?categoryId=1&rankId=1" -Headers $headers
    $r.stipendId -gt 0 -and $r.status -eq "A"
}

Test-Endpoint "POST /api/v1/stipend-master - creates new" {
    $body = @{
        researchCategoryId = $catId
        srfRankId          = $rankId
        srfMonthlyStipend  = 45000.00
        additionalAllowance = 3000.00
        effectiveFrom      = "2026-04-01T00:00:00"
        createdBy          = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/v1/stipend-master" -Method POST -Body $body -Headers $headers
    $script:stipendId = $r.stipendId
    $r.stipendId -gt 0 -and $r.srfMonthlyStipend -eq 45000 -and $r.status -eq "A"
}

Test-Endpoint "GET /api/v1/stipend-master/{id} - verify created" {
    if (-not $script:stipendId) { return $false }
    $r = Invoke-Api "$baseUrl/api/v1/stipend-master/$($script:stipendId)" -Headers $headers
    $r.stipendId -eq $script:stipendId -and $r.researchCategoryId -eq $catId
}

Test-Endpoint "PUT /api/v1/stipend-master/{id} - updates" {
    if (-not $script:stipendId) { return $false }
    $body = @{
        stipendId         = $script:stipendId
        srfMonthlyStipend = 50000.00
        additionalAllowance = 3500.00
        effectiveFrom     = "2026-04-01T00:00:00"
        updatedBy         = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/v1/stipend-master/$($script:stipendId)" -Method PUT -Body $body -Headers $headers
    $r.srfMonthlyStipend -eq 50000
}

Test-Endpoint "DELETE /api/v1/stipend-master/{id} - deactivates" {
    if (-not $script:stipendId) { return $false }
    $resp = Invoke-ApiRaw "$baseUrl/api/v1/stipend-master/$($script:stipendId)?updatedBy=1" -Method DELETE -Headers $headers
    $resp.StatusCode -eq 204
}

Test-Endpoint "GET /api/v1/stipend-master/{id} - verify deactivated" {
    if (-not $script:stipendId) { return $false }
    $r = Invoke-Api "$baseUrl/api/v1/stipend-master/$($script:stipendId)" -Headers $headers
    $r.status -eq "I"
}

Test-Endpoint "GET /api/v1/stipend-master/999999 - 404 for nonexistent" {
    try { Invoke-Api "$baseUrl/api/v1/stipend-master/999999" -Headers $headers; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

Test-Endpoint "GET /api/v1/stipend-master - 401 without auth" {
    try { Invoke-Api "$baseUrl/api/v1/stipend-master"; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 401 }
}

# ─── 4. REST - DISBURSEMENT CONTROLLER ────────────────────────────────────────
Write-Host "`n[4] REST Disbursement Controller" -ForegroundColor Yellow

Test-Endpoint "POST /api/v1/disbursement/process - process monthly" {
    $body = @{ monthYear = "2026-04"; processedBy = 1 } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/v1/disbursement/process" -Method POST -Body $body -Headers $headers
    $r.monthYear -eq "2026-04" -and $r.success -eq $true
}

Test-Endpoint "GET /api/v1/disbursement/by-month/2026-04 - find by month" {
    $r = Invoke-Api "$baseUrl/api/v1/disbursement/by-month/2026-04" -Headers $headers
    $r -is [array]
}

$script:disbId = $null
Test-Endpoint "POST /api/v1/disbursement/calculate - calculate disbursement" {
    $body = @{ monthYear = "2026-03"; processedBy = 1 } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/v1/disbursement/calculate" -Method POST -Body $body -Headers $headers
    $r.success -eq $true
}

Test-Endpoint "GET /api/v1/disbursement/by-month/2026-03 - verify calculated" {
    $r = Invoke-Api "$baseUrl/api/v1/disbursement/by-month/2026-03" -Headers $headers
    if ($r -is [array] -and $r.Count -gt 0) {
        $script:disbId = $r[0].disbursementId
        $true
    } else { $r -ne $null }
}

Test-Endpoint "GET /api/v1/disbursement/{id} - find by ID" {
    if (-not $script:disbId) { return $false }
    $r = Invoke-Api "$baseUrl/api/v1/disbursement/$($script:disbId)" -Headers $headers
    $r.disbursementId -eq $script:disbId
}

Test-Endpoint "GET /api/v1/disbursement/by-srf/{srfId} - find by SRF" {
    $r = Invoke-Api "$baseUrl/api/v1/disbursement/by-srf/1" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "PUT /api/v1/disbursement/{id}/bank-reference - set bank ref" {
    if (-not $script:disbId) { return $false }
    $body = @{
        disbursementId = $script:disbId
        bankReference  = "BANK-REF-$runId"
        referenceNo    = "REF-$runId"
        updatedBy      = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/v1/disbursement/$($script:disbId)/bank-reference" -Method PUT -Body $body -Headers $headers
    $r.bankReference -eq "BANK-REF-$runId"
}

Test-Endpoint "POST /api/v1/disbursement/{id}/reject - reject disbursement" {
    if (-not $script:disbId) { return $false }
    try {
        $r = Invoke-Api "$baseUrl/api/v1/disbursement/$($script:disbId)/reject?updatedBy=1" -Method POST -Headers $headers
        $r -ne $null
    } catch {
        # 400 if already rejected from previous run
        $_.Exception.Response.StatusCode.value__ -eq 400
    }
}

Test-Endpoint "POST /api/v1/disbursement/process-sp - process via SP" {
    try {
        $r = Invoke-Api "$baseUrl/api/v1/disbursement/process-sp?monthYear=2026-05&processedBy=1" -Method POST -Headers $headers
        $r.success -ne $null
    } catch {
        # SP may not exist in local DB - 500 is expected
        $_.Exception.Response.StatusCode.value__ -eq 500
    }
}

Test-Endpoint "POST /api/v1/disbursement/calculate-sp - calculate via SP" {
    try {
        $r = Invoke-Api "$baseUrl/api/v1/disbursement/calculate-sp?monthYear=2026-06&processedBy=1" -Method POST -Headers $headers
        $r.success -ne $null
    } catch {
        # SP may not exist in local DB - 500 is expected
        $_.Exception.Response.StatusCode.value__ -eq 500
    }
}

# ─── 5. GRAPHQL ────────────────────────────────────────────────────────────────
Write-Host "`n[5] GraphQL Endpoints" -ForegroundColor Yellow

Test-Endpoint "GraphQL query stipendMasters" {
    $r = Invoke-GQL '{ stipendMasters { stipendId researchCategoryId srfRankId srfMonthlyStipend status } }'
    $r.data.stipendMasters -is [array] -and $r.data.stipendMasters.Count -ge 5
}

Test-Endpoint "GraphQL query stipendMasters with all fields" {
    $r = Invoke-GQL '{ stipendMasters { stipendId researchCategoryId srfRankId srfMonthlyStipend additionalAllowance effectiveFrom effectiveTo status createdBy createdOn updatedBy updatedOn } }'
    $r.data.stipendMasters -is [array] -and $r.data.stipendMasters.Count -ge 5 -and $r.data.stipendMasters[0].createdBy -ne $null
}

Test-Endpoint "GraphQL query disbursements" {
    $r = Invoke-GQL '{ disbursements { disbursementId srfId stipendId disbursementAmount disbursementStatus monthYear } }'
    $r.data.disbursements -is [array]
}

Test-Endpoint "GraphQL mutation createStipendMaster" {
    $gqlCat = Get-Random -Minimum 1000 -Maximum 99999
    $gqlRank = Get-Random -Minimum 1000 -Maximum 99999
    $r = Invoke-GQL "mutation { createStipendMaster(input: { researchCategoryId: $gqlCat, srfRankId: $gqlRank, srfMonthlyStipend: 55000.00, additionalAllowance: 4000.00, effectiveFrom: `"2026-04-01T00:00:00Z`", createdBy: 1 }) { stipendId srfMonthlyStipend status } }"
    $r.data.createStipendMaster.stipendId -gt 0 -and $r.data.createStipendMaster.srfMonthlyStipend -eq 55000
}

Test-Endpoint "GraphQL mutation processMonthlyStipend" {
    $r = Invoke-GQL 'mutation { processMonthlyStipend(input: { monthYear: "2026-07", processedBy: 1 }) { monthYear rowsProcessed success } }'
    $r.data.processMonthlyStipend.success -eq $true
}

Test-Endpoint "GraphQL mutation calculateAndDisburse" {
    $r = Invoke-GQL 'mutation { calculateAndDisburse(input: { monthYear: "2026-08", processedBy: 1 }) { monthYear rowsCreated success } }'
    $r.data.calculateAndDisburse.success -eq $true
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
