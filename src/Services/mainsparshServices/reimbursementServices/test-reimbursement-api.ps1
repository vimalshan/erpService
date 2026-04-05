# ReimbursementService.API Test Script
# Port: 5202

$baseUrl = "http://localhost:5202"
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

function Invoke-GQL($query, $headers) {
    $body = @{ query = $query } | ConvertTo-Json
    $params = @{ Uri = "$baseUrl/graphql"; Method = "POST"; Body = $body; ContentType = "application/json"; UseBasicParsing = $true }
    if ($headers) { $params.Headers = $headers }
    $resp = Invoke-WebRequest @params
    if ($resp.Content -is [byte[]]) {
        return [System.Text.Encoding]::UTF8.GetString($resp.Content) | ConvertFrom-Json
    }
    return $resp.Content | ConvertFrom-Json
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  ReimbursementService.API Tests" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# ─── 1. HEALTH CHECK ──────────────────────────────────────────────────────────
Write-Host "`n[1] Health Check" -ForegroundColor Yellow

Test-Endpoint "GET /health - service healthy" {
    $resp = Invoke-WebRequest "$baseUrl/health" -UseBasicParsing
    $content = if ($resp.Content -is [byte[]]) { [System.Text.Encoding]::UTF8.GetString($resp.Content) } else { $resp.Content }
    $content -eq "Healthy"
}

# ─── 2. AUTH TOKEN ─────────────────────────────────────────────────────────────
Write-Host "`n[2] Auth Token" -ForegroundColor Yellow

$script:token = $null

Test-Endpoint "POST /api/auth/token - returns JWT (Admin+Approver+Finance)" {
    $body = @{ userId = 1; userName = "admin"; roles = @("Admin","Approver","Finance") } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/auth/token" -Method POST -Body $body
    $script:token = $r.token
    $r.token -ne $null -and $r.token.Length -gt 20
}

Test-Endpoint "POST /api/auth/token - works without roles" {
    $body = @{ userId = 2; userName = "testuser" } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/auth/token" -Method POST -Body $body
    $r.token -ne $null -and $r.token.Length -gt 20
}

$headers = @{ Authorization = "Bearer $($script:token)" }

# ─── 3. REST CONTROLLER (/api/reimbursements) ─────────────────────────────────
Write-Host "`n[3] REST Controller Endpoints" -ForegroundColor Yellow

$script:reimId = $null

Test-Endpoint "GET /api/reimbursements - returns list" {
    $r = Invoke-Api "$baseUrl/api/reimbursements" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "POST /api/reimbursements - creates reimbursement" {
    $body = @{
        empSysId    = 5001
        reimType    = "Travel"
        amount      = 2500.50
        currency    = "INR"
        reimDate    = (Get-Date -Format "yyyy-MM-dd")
        expenseDate = (Get-Date).AddDays(-2).ToString("yyyy-MM-dd")
        description = "Test travel claim $runId"
        location    = "Delhi"
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/reimbursements" -Method POST -Body $body -Headers $headers
    $script:reimId = $r.reimId
    $r.reimId -gt 0 -and $r.status -eq "DRAFT" -and $r.reimType -eq "TRAVEL"
}

Test-Endpoint "GET /api/reimbursements/{id} - find by ID" {
    if (-not $script:reimId) { return $false }
    $r = Invoke-Api "$baseUrl/api/reimbursements/$($script:reimId)" -Headers $headers
    $r.reimId -eq $script:reimId -and $r.reimRefNo -like "REIM-*"
}

Test-Endpoint "GET /api/reimbursements/employee/5001 - find by employee" {
    $r = Invoke-Api "$baseUrl/api/reimbursements/employee/5001" -Headers $headers
    $r -is [array] -or $r.reimId -gt 0
}

Test-Endpoint "PUT /api/reimbursements/{id} - updates (Draft only)" {
    if (-not $script:reimId) { return $false }
    $body = @{
        reimType    = "Meal"
        amount      = 999.99
        currency    = "INR"
        reimDate    = (Get-Date -Format "yyyy-MM-dd")
        expenseDate = (Get-Date).AddDays(-1).ToString("yyyy-MM-dd")
        description = "Updated to meal $runId"
        location    = "Mumbai"
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/reimbursements/$($script:reimId)" -Method PUT -Body $body -Headers $headers
    $r.reimType -eq "MEAL" -and $r.reimAmount -eq 999.99
}

Test-Endpoint "GET /api/reimbursements/summary?empSysId=5001 - summary" {
    $r = Invoke-Api "$baseUrl/api/reimbursements/summary?empSysId=5001" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "POST /api/reimbursements/{id}/submit - submits claim" {
    if (-not $script:reimId) { return $false }
    $resp = Invoke-ApiRaw "$baseUrl/api/reimbursements/$($script:reimId)/submit" -Method POST -Headers $headers
    $resp.StatusCode -eq 204
}

Test-Endpoint "POST /api/reimbursements/{id}/approve - approves claim" {
    if (-not $script:reimId) { return $false }
    $body = @{ approvedBy = 99; approvalLevel = 1 } | ConvertTo-Json
    $resp = Invoke-ApiRaw "$baseUrl/api/reimbursements/$($script:reimId)/approve" -Method POST -Body $body -Headers $headers
    $resp.StatusCode -eq 204
}

Test-Endpoint "POST /api/reimbursements/{id}/pay - marks paid" {
    if (-not $script:reimId) { return $false }
    $body = @{ paymentDate = (Get-Date -Format "yyyy-MM-dd"); updatedBy = 1 } | ConvertTo-Json
    $resp = Invoke-ApiRaw "$baseUrl/api/reimbursements/$($script:reimId)/pay" -Method POST -Body $body -Headers $headers
    $resp.StatusCode -eq 204
}

Test-Endpoint "GET /api/reimbursements/{id} - verify paid status" {
    if (-not $script:reimId) { return $false }
    $r = Invoke-Api "$baseUrl/api/reimbursements/$($script:reimId)" -Headers $headers
    $r.status -eq "PAID"
}

# Create another to test reject flow
$script:reimId2 = $null

Test-Endpoint "POST /api/reimbursements - create for reject flow" {
    $body = @{
        empSysId    = 5002
        reimType    = "Accommodation"
        amount      = 1500.00
        currency    = "INR"
        reimDate    = (Get-Date -Format "yyyy-MM-dd")
        expenseDate = (Get-Date).AddDays(-3).ToString("yyyy-MM-dd")
        description = "Reject test $runId"
        location    = "Chennai"
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/reimbursements" -Method POST -Body $body -Headers $headers
    $script:reimId2 = $r.reimId
    $r.reimId -gt 0
}

Test-Endpoint "POST /api/reimbursements/{id}/submit - submit for reject" {
    if (-not $script:reimId2) { return $false }
    $resp = Invoke-ApiRaw "$baseUrl/api/reimbursements/$($script:reimId2)/submit" -Method POST -Headers $headers
    $resp.StatusCode -eq 204
}

Test-Endpoint "POST /api/reimbursements/{id}/reject - rejects claim" {
    if (-not $script:reimId2) { return $false }
    $body = @{ rejectedBy = 88; reason = "Insufficient documentation for test $runId" } | ConvertTo-Json
    $resp = Invoke-ApiRaw "$baseUrl/api/reimbursements/$($script:reimId2)/reject" -Method POST -Body $body -Headers $headers
    $resp.StatusCode -eq 204
}

Test-Endpoint "GET /api/reimbursements/{id} - verify rejected" {
    if (-not $script:reimId2) { return $false }
    $r = Invoke-Api "$baseUrl/api/reimbursements/$($script:reimId2)" -Headers $headers
    $r.status -eq "REJECTED" -and $r.rejectionReason -like "*Insufficient*"
}

Test-Endpoint "GET /api/reimbursements/999999 - 404 for nonexistent" {
    try { Invoke-Api "$baseUrl/api/reimbursements/999999" -Headers $headers; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

Test-Endpoint "GET /api/reimbursements - 401 without auth" {
    try { Invoke-Api "$baseUrl/api/reimbursements"; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 401 }
}

# ─── 4. MINIMAL API (/reimbursements) ─────────────────────────────────────────
Write-Host "`n[4] Minimal API Endpoints" -ForegroundColor Yellow

$script:minReimId = $null

Test-Endpoint "GET /reimbursements - returns list" {
    $r = Invoke-Api "$baseUrl/reimbursements" -Headers $headers
    $r -is [array] -or $r -ne $null
}

Test-Endpoint "POST /reimbursements - creates reimbursement" {
    $body = @{
        empSysId    = 6001
        reimType    = "Conference"
        amount      = 8500.00
        currency    = "INR"
        reimDate    = (Get-Date -Format "yyyy-MM-dd")
        expenseDate = (Get-Date).AddDays(-5).ToString("yyyy-MM-dd")
        description = "MinAPI conf test $runId"
        location    = "Bangalore"
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/reimbursements" -Method POST -Body $body -Headers $headers
    $script:minReimId = $r.reimId
    $r.reimId -gt 0 -and $r.reimType -eq "CONFERENCE"
}

Test-Endpoint "GET /reimbursements/{id} - find by ID" {
    if (-not $script:minReimId) { return $false }
    $r = Invoke-Api "$baseUrl/reimbursements/$($script:minReimId)" -Headers $headers
    $r.reimId -eq $script:minReimId
}

Test-Endpoint "GET /reimbursements/employee/6001 - find by employee" {
    $r = Invoke-Api "$baseUrl/reimbursements/employee/6001" -Headers $headers
    $r -is [array] -or $r.reimId -gt 0
}

# ─── 5. GRAPHQL ────────────────────────────────────────────────────────────────
Write-Host "`n[5] GraphQL Endpoints" -ForegroundColor Yellow

Test-Endpoint "GraphQL reimbursementsByStatus (Draft)" {
    $r = Invoke-GQL '{ reimbursementsByStatus(status: "Draft") { reimId reimRefNo status } }'
    $r.data.reimbursementsByStatus -is [array]
}

Test-Endpoint "GraphQL reimbursementsByStatus (Submitted)" {
    $r = Invoke-GQL '{ reimbursementsByStatus(status: "Submitted") { reimId status } }'
    $r.data.reimbursementsByStatus -is [array]
}

Test-Endpoint "GraphQL reimbursementById" {
    if (-not $script:reimId) { return $false }
    $id = $script:reimId
    $r = Invoke-GQL "{ reimbursementById(id: $id) { reimId reimRefNo empSysId reimType reimAmount reimCurrency status } }"
    $r.data.reimbursementById.reimId -eq $id
}

Test-Endpoint "GraphQL reimbursementsByEmployee" {
    $r = Invoke-GQL '{ reimbursementsByEmployee(empSysId: 1001) { reimId empSysId reimType status } }'
    $r.data.reimbursementsByEmployee -is [array]
}

Test-Endpoint "GraphQL mutation submitReimbursement" {
    # Create a fresh draft first
    $body = @{
        empSysId    = 7001
        reimType    = "Misc"
        amount      = 350.00
        currency    = "INR"
        reimDate    = (Get-Date -Format "yyyy-MM-dd")
        expenseDate = (Get-Date).AddDays(-1).ToString("yyyy-MM-dd")
        description = "GQL submit test $runId"
    } | ConvertTo-Json
    $created = Invoke-Api "$baseUrl/api/reimbursements" -Method POST -Body $body -Headers $headers
    $newId = $created.reimId
    if (-not $newId) { return $false }
    $r = Invoke-GQL "mutation { submitReimbursement(reimId: $newId) }"
    $r.data.submitReimbursement -eq $true
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
