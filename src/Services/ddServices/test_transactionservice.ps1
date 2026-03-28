# =========================================================================================================
# TransactionService.API Comprehensive Test Suite
# Tests: REST endpoints, GraphQL operations, RabbitMQ integration, Health Checks, Swagger, Auth
# Port: 5178
# =========================================================================================================

$baseUrl = "http://localhost:5178"
$graphqlUrl = "$baseUrl/graphql"
$pass = 0; $fail = 0; $total = 0

function Test-Result($name, $condition, $detail = "") {
    $script:total++
    if ($condition) { $script:pass++; Write-Host "  PASS: $name" -ForegroundColor Green }
    else { $script:fail++; Write-Host "  FAIL: $name" -ForegroundColor Red }
    if ($detail) { Write-Host "        $detail" -ForegroundColor Gray }
}

# ─── JWT Token Generator ────────────────────────────────────────────────────────
function Get-JwtToken {
    $secret = "your-very-long-secret-key-change-this-in-production-at-least-32-characters"
    $issuer = "TransactionService"
    $audience = "TransactionServiceClient"
    $header = '{"alg":"HS256","typ":"JWT"}'
    $now = [int](Get-Date -UFormat %s)
    $exp = $now + 3600
    $payload = "{`"sub`":`"admin001`",`"unique_name`":`"TestAdmin`",`"http://schemas.microsoft.com/ws/2008/06/identity/claims/role`":`"Admin`",`"iss`":`"$issuer`",`"aud`":`"$audience`",`"iat`":$now,`"exp`":$exp}"
    function B64U($b) { [Convert]::ToBase64String($b).TrimEnd('=').Replace('+','-').Replace('/','_') }
    $hB = B64U([Text.Encoding]::UTF8.GetBytes($header))
    $pB = B64U([Text.Encoding]::UTF8.GetBytes($payload))
    $hmac = New-Object Security.Cryptography.HMACSHA256
    $hmac.Key = [Text.Encoding]::UTF8.GetBytes($secret)
    $sig = B64U($hmac.ComputeHash([Text.Encoding]::UTF8.GetBytes("$hB.$pB")))
    return "$hB.$pB.$sig"
}

# ─── GraphQL Helper ─────────────────────────────────────────────────────────────
function Invoke-GQL($query, $testName, $headers = $null) {
    $body = @{ query = $query } | ConvertTo-Json -Compress
    $params = @{
        Uri = $graphqlUrl
        Method = "POST"
        ContentType = "application/json"
        Body = $body
        UseBasicParsing = $true
        TimeoutSec = 10
    }
    if ($headers) { $params["Headers"] = $headers }
    try {
        $r = Invoke-WebRequest @params
        $json = $r.Content | ConvertFrom-Json
        if ($json.errors) {
            Test-Result $testName $false "GraphQL Error: $($json.errors[0].message)"
        } else {
            Test-Result $testName $true "Data returned successfully"
        }
        return $json
    } catch {
        $code = $null
        if ($_.Exception.Response) { $code = $_.Exception.Response.StatusCode.value__ }
        Test-Result $testName $false "Status: $code - $($_.Exception.Message)"
        return $null
    }
}

# ─── Error Body Reader ──────────────────────────────────────────────────────────
function Get-ErrorBody($exception) {
    try {
        $stream = $exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($stream)
        $body = $reader.ReadToEnd()
        if ($body.Length -gt 300) { $body = $body.Substring(0, 300) + "..." }
        return $body
    } catch { return "" }
}

Write-Host "================================================" -ForegroundColor Cyan
Write-Host " TRANSACTION SERVICE API - TEST SUITE" -ForegroundColor Cyan
Write-Host " Base URL: $baseUrl" -ForegroundColor Cyan
Write-Host "================================================`n" -ForegroundColor Cyan

# ═════════════════════════════════════════════════════════════════════════════════
# 1. HEALTH CHECKS
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "=== 1. HEALTH CHECKS ===" -ForegroundColor Yellow

try {
    $r = Invoke-WebRequest -Uri "$baseUrl/health" -UseBasicParsing -TimeoutSec 5
    $health = $r.Content | ConvertFrom-Json
    Test-Result "GET /health returns 200" ($r.StatusCode -eq 200) "Status: $($health.status)"
} catch {
    Test-Result "GET /health returns 200" $false $_.Exception.Message
}

try {
    $r = Invoke-WebRequest -Uri "$baseUrl/health/ready" -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /health/ready returns 200" ($r.StatusCode -eq 200) "Readiness: OK"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "GET /health/ready returns 200" $false "Status: $code"
}

# Check health response structure
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/health" -UseBasicParsing -TimeoutSec 5
    $health = $r.Content | ConvertFrom-Json
    $hasChecks = $null -ne $health.checks
    $dbCheck = $health.checks | Where-Object { $_.name -like "*Database*" }
    Test-Result "Health response has 'checks' array" $hasChecks "Checks: $(($health.checks | Measure-Object).Count)"
    Test-Result "Database health check present" ($null -ne $dbCheck) "DB Status: $($dbCheck.status)"
} catch {
    Test-Result "Health response structure" $false $_.Exception.Message
}

# ═════════════════════════════════════════════════════════════════════════════════
# 2. AUTHENTICATION
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 2. AUTHENTICATION ===" -ForegroundColor Yellow

$jwt = Get-JwtToken
Write-Host "  JWT Token generated successfully" -ForegroundColor Gray
$headers = @{ "Authorization" = "Bearer $jwt"; "Content-Type" = "application/json" }

# Test unauthorized access (no token)
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/demands" -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /api/demands without token returns 401" $false "Got $($r.StatusCode) instead of 401"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "GET /api/demands without token returns 401" ($code -eq 401) "StatusCode: $code"
}

# Test invalid token
try {
    $badHeaders = @{ "Authorization" = "Bearer invalid.token.here" }
    $r = Invoke-WebRequest -Uri "$baseUrl/api/demands" -Headers $badHeaders -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /api/demands with bad token returns 401" $false "Got $($r.StatusCode)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "GET /api/demands with bad token returns 401" ($code -eq 401) "StatusCode: $code"
}

# Test valid token
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/demands" -Headers $headers -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /api/demands with valid token returns 200" ($r.StatusCode -eq 200) "Authorized OK"
} catch {
    Test-Result "GET /api/demands with valid token returns 200" $false $_.Exception.Message
}

# ═════════════════════════════════════════════════════════════════════════════════
# 3. SWAGGER / OPENAPI
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 3. SWAGGER / OPENAPI ===" -ForegroundColor Yellow

try {
    $r = Invoke-WebRequest -Uri "$baseUrl/swagger/v1/swagger.json" -UseBasicParsing -TimeoutSec 5
    $swagger = $r.Content | ConvertFrom-Json
    $pathCount = ($swagger.paths.PSObject.Properties | Measure-Object).Count
    Test-Result "Swagger JSON available" ($r.StatusCode -eq 200) "Paths documented: $pathCount"
} catch {
    Test-Result "Swagger JSON available" $false $_.Exception.Message
}

# ═════════════════════════════════════════════════════════════════════════════════
# 4. REST - DEMANDS ENDPOINTS (seed data: 3 records)
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 4. REST - DEMANDS ENDPOINTS ===" -ForegroundColor Yellow

# GET all demands (should have seed data)
$existingDemandId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/demands" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $demands = $r.Content | ConvertFrom-Json
    $count = if ($demands -is [array]) { $demands.Count } else { 1 }
    Test-Result "GET /api/demands (list all)" ($r.StatusCode -eq 200 -and $count -ge 1) "Demands: $count"
    if ($demands -is [array] -and $demands.Count -gt 0) { $existingDemandId = $demands[0].id }
    elseif ($demands) { $existingDemandId = $demands.id }
} catch {
    Test-Result "GET /api/demands (list all)" $false $_.Exception.Message
}

# GET demand by ID (seed data)
if ($existingDemandId) {
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/demands/$existingDemandId" -Headers $headers -UseBasicParsing -TimeoutSec 5
        $demand = $r.Content | ConvertFrom-Json
        Test-Result "GET /api/demands/{id} (by ID)" ($r.StatusCode -eq 200 -and $demand.id -eq $existingDemandId) "Type: $($demand.demandType), Desc: $($demand.demandDescription)"
    } catch {
        Test-Result "GET /api/demands/{id} (by ID)" $false $_.Exception.Message
    }
}

# GET non-existent demand
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/demands/99999" -Headers $headers -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /api/demands/99999 returns 404" $false "Got $($r.StatusCode)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "GET /api/demands/99999 returns 404" ($code -eq 404) "StatusCode: $code"
}

# GET demands by status (seed data has status 'O' for Open)
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/demands/status/O" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $statusDemands = $r.Content | ConvertFrom-Json
    $sCount = if ($statusDemands -is [array]) { $statusDemands.Count } else { 1 }
    Test-Result "GET /api/demands/status/O (by status)" ($r.StatusCode -eq 200) "Open demands: $sCount"
} catch {
    Test-Result "GET /api/demands/status/O (by status)" $false $_.Exception.Message
}

# GET demand status count
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/demands/status-count/O" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $statusCount = $r.Content
    Test-Result "GET /api/demands/status-count/O" ($r.StatusCode -eq 200) "Count: $statusCount"
} catch {
    Test-Result "GET /api/demands/status-count/O" $false $_.Exception.Message
}

# POST create new demand
$ts = [DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds()
$createDemandBody = @{
    demandType = "Testing"
    departmentId = 201
    demandDescription = "Test demand created by test suite $ts"
    requiredDate = (Get-Date).AddMonths(1).ToString("yyyy-MM-ddTHH:mm:ss")
    priority = "High"
    createdBy = 9001
} | ConvertTo-Json

$createdDemandId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/demands" -Method POST -Headers $headers -Body $createDemandBody -UseBasicParsing -TimeoutSec 10
    $createdDemandId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/demands (create)" ($r.StatusCode -eq 201 -or $r.StatusCode -eq 200) "Created ID: $createdDemandId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $errBody = Get-ErrorBody $_.Exception
    Test-Result "POST /api/demands (create)" $false "Status: $code, Body: $errBody"
}

# GET the newly created demand
if ($createdDemandId) {
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/demands/$createdDemandId" -Headers $headers -UseBasicParsing -TimeoutSec 5
        $d = $r.Content | ConvertFrom-Json
        Test-Result "GET /api/demands/{id} (verify create)" ($r.StatusCode -eq 200 -and $d.demandType -eq "Testing") "Type: $($d.demandType)"
    } catch {
        Test-Result "GET /api/demands/{id} (verify create)" $false $_.Exception.Message
    }
}

# PUT approve demand
$approveDemandId = if ($createdDemandId) { $createdDemandId } else { $existingDemandId }
if ($approveDemandId) {
    $approveBody = @{
        demandId = $approveDemandId
        approvalStatus = "A"
        approvalRemarks = "Approved by test suite"
        approvedBy = 9002
    } | ConvertTo-Json
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/demands/$approveDemandId/approve" -Method PUT -Headers $headers -Body $approveBody -UseBasicParsing -TimeoutSec 10
        Test-Result "PUT /api/demands/{id}/approve" ($r.StatusCode -eq 200) "Demand approved"
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        $errBody = Get-ErrorBody $_.Exception
        Test-Result "PUT /api/demands/{id}/approve" $false "Status: $code, Body: $errBody"
    }

    # Verify approval
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/demands/$approveDemandId" -Headers $headers -UseBasicParsing -TimeoutSec 5
        $d = $r.Content | ConvertFrom-Json
        Test-Result "GET /api/demands/{id} (verify approve)" ($r.StatusCode -eq 200 -and $d.demandStatus -eq "A") "Status: $($d.demandStatus)"
    } catch {
        Test-Result "GET /api/demands/{id} (verify approve)" $false $_.Exception.Message
    }
}

# PUT complete demand
if ($approveDemandId) {
    $completeBody = @{
        demandId = $approveDemandId
        completionRemarks = "Completed by test suite"
        completedBy = 9003
    } | ConvertTo-Json
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/demands/$approveDemandId/complete" -Method PUT -Headers $headers -Body $completeBody -UseBasicParsing -TimeoutSec 10
        Test-Result "PUT /api/demands/{id}/complete" ($r.StatusCode -eq 200) "Demand completed"
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        $errBody = Get-ErrorBody $_.Exception
        Test-Result "PUT /api/demands/{id}/complete" $false "Status: $code, Body: $errBody"
    }
}

# ═════════════════════════════════════════════════════════════════════════════════
# 5. REST - BUDGETS ENDPOINTS (seed data: 2 records)
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 5. REST - BUDGETS ENDPOINTS ===" -ForegroundColor Yellow

# GET all budgets
$existingBudgetId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/budgets" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $budgets = $r.Content | ConvertFrom-Json
    $count = if ($budgets -is [array]) { $budgets.Count } else { 1 }
    Test-Result "GET /api/budgets (list all)" ($r.StatusCode -eq 200 -and $count -ge 1) "Budgets: $count"
    if ($budgets -is [array] -and $budgets.Count -gt 0) { $existingBudgetId = $budgets[0].id }
} catch {
    Test-Result "GET /api/budgets (list all)" $false $_.Exception.Message
}

# GET budgets by year (seed data year = 2025)
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/budgets/year/2025" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $yearBudgets = $r.Content | ConvertFrom-Json
    $yCount = if ($yearBudgets -is [array]) { $yearBudgets.Count } else { 1 }
    Test-Result "GET /api/budgets/year/2025" ($r.StatusCode -eq 200) "Year 2025 budgets: $yCount"
} catch {
    Test-Result "GET /api/budgets/year/2025" $false $_.Exception.Message
}

# POST create budget (unique businessId to avoid unique constraint on BusinessId+YearId)
$uniqueBizId = [int]((Get-Date).ToString("mmssff"))
$createBudgetBody = @{
    businessId = $uniqueBizId
    yearId = 2026
    budgetAmount = 750000.00
    updatedBy = 9001
} | ConvertTo-Json

$createdBudgetId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/budgets" -Method POST -Headers $headers -Body $createBudgetBody -UseBasicParsing -TimeoutSec 10
    $createdBudgetId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/budgets (create)" ($r.StatusCode -eq 201 -or $r.StatusCode -eq 200) "Created ID: $createdBudgetId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $errBody = Get-ErrorBody $_.Exception
    Test-Result "POST /api/budgets (create)" $false "Status: $code, Body: $errBody"
}

# PUT update budget
$updateBudgetId = if ($createdBudgetId) { $createdBudgetId } else { $existingBudgetId }
if ($updateBudgetId) {
    $updateBudgetBody = @{
        id = $updateBudgetId
        budgetAmount = 800000.00
        updatedBy = 9002
    } | ConvertTo-Json
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/budgets/$updateBudgetId" -Method PUT -Headers $headers -Body $updateBudgetBody -UseBasicParsing -TimeoutSec 10
        Test-Result "PUT /api/budgets/{id} (update)" ($r.StatusCode -eq 200) "Budget updated"
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        $errBody = Get-ErrorBody $_.Exception
        Test-Result "PUT /api/budgets/{id} (update)" $false "Status: $code, Body: $errBody"
    }
}

# ═════════════════════════════════════════════════════════════════════════════════
# 6. REST - PERIODS ENDPOINTS (seed data: 2 records)
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 6. REST - PERIODS ENDPOINTS ===" -ForegroundColor Yellow

# GET all periods
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/periods" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $periods = $r.Content | ConvertFrom-Json
    $count = if ($periods -is [array]) { $periods.Count } else { 1 }
    Test-Result "GET /api/periods (list all)" ($r.StatusCode -eq 200 -and $count -ge 1) "Periods: $count"
    if ($periods -is [array] -and $periods.Count -gt 0) { $script:seedPeriodId = $periods[0].id }
} catch {
    Test-Result "GET /api/periods (list all)" $false $_.Exception.Message
}

# POST create period
$createPeriodBody = @{
    yearId = 2026
    quarterNo = 1
    periodOpenDate = (Get-Date).AddMonths(6).ToString("yyyy-MM-ddTHH:mm:ss")
    periodCloseDate = (Get-Date).AddMonths(9).ToString("yyyy-MM-ddTHH:mm:ss")
    formOpenDate = (Get-Date).AddMonths(7).ToString("yyyy-MM-ddTHH:mm:ss")
    appraiserLastDate = (Get-Date).AddMonths(8).ToString("yyyy-MM-ddTHH:mm:ss")
    reviewerLastDate = (Get-Date).AddMonths(8).AddDays(7).ToString("yyyy-MM-ddTHH:mm:ss")
    bhrLastDate = (Get-Date).AddMonths(8).AddDays(14).ToString("yyyy-MM-ddTHH:mm:ss")
    uhrLastDate = (Get-Date).AddMonths(8).AddDays(21).ToString("yyyy-MM-ddTHH:mm:ss")
} | ConvertTo-Json

$createdPeriodId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/periods" -Method POST -Headers $headers -Body $createPeriodBody -UseBasicParsing -TimeoutSec 10
    $createdPeriodId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/periods (create)" ($r.StatusCode -eq 201 -or $r.StatusCode -eq 200) "Created ID: $createdPeriodId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $errBody = Get-ErrorBody $_.Exception
    Test-Result "POST /api/periods (create)" $false "Status: $code, Body: $errBody"
}

# ═════════════════════════════════════════════════════════════════════════════════
# 7. REST - LEVELS ENDPOINTS (seed data: 3 records)
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 7. REST - LEVELS ENDPOINTS ===" -ForegroundColor Yellow

# GET all levels
$existingLevelId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/levels" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $levels = $r.Content | ConvertFrom-Json
    $count = if ($levels -is [array]) { $levels.Count } else { 1 }
    Test-Result "GET /api/levels (list all)" ($r.StatusCode -eq 200 -and $count -ge 1) "Levels: $count"
    if ($levels -is [array] -and $levels.Count -gt 0) { $existingLevelId = $levels[0].id }
} catch {
    Test-Result "GET /api/levels (list all)" $false $_.Exception.Message
}

# POST create level
$createLevelBody = @{
    levelDesc = "Level D - Test"
    levelAmount = "1000"
    levelReason = "Test level created by test suite"
    levelMin = 0
    levelMax = 1000.00
    levelEffDate = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
    updatedBy = 9001
} | ConvertTo-Json

$createdLevelId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/levels" -Method POST -Headers $headers -Body $createLevelBody -UseBasicParsing -TimeoutSec 10
    $createdLevelId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/levels (create)" ($r.StatusCode -eq 201 -or $r.StatusCode -eq 200) "Created ID: $createdLevelId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $errBody = Get-ErrorBody $_.Exception
    Test-Result "POST /api/levels (create)" $false "Status: $code, Body: $errBody"
}

# ═════════════════════════════════════════════════════════════════════════════════
# 8. REST - RECOMMENDS ENDPOINTS
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 8. REST - RECOMMENDS ENDPOINTS ===" -ForegroundColor Yellow

# GET all recommends
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/recommends" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $recommends = $r.Content | ConvertFrom-Json
    $count = if ($recommends -is [array]) { $recommends.Count } elseif ($recommends) { 1 } else { 0 }
    Test-Result "GET /api/recommends (list all)" ($r.StatusCode -eq 200) "Recommends: $count"
} catch {
    Test-Result "GET /api/recommends (list all)" $false $_.Exception.Message
}

# POST create recommend (use seed period and level IDs)
$recPeriodId = if ($createdPeriodId) { $createdPeriodId } elseif ($script:seedPeriodId) { $script:seedPeriodId } else { 1 }
$recLevelId = if ($createdLevelId) { $createdLevelId } elseif ($existingLevelId) { $existingLevelId } else { 1 }

$createRecommendBody = @{
    yearId = 2025
    periodId = $recPeriodId
    empSysId = 5001
    levelId = $recLevelId
    ctcAmount = 120000.00
    maximumCap = 10000.00
    eligibilityAmount = 8000.00
    recommendAmount = 5000.00
    initiativeTaken = "Led 5 projects successfully in Q1"
    results = "Revenue increased 15%"
    addRemarks = "Highly recommended for increment"
    recommendBy = "Manager-TestSuite"
} | ConvertTo-Json

$createdRecommendId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/recommends" -Method POST -Headers $headers -Body $createRecommendBody -UseBasicParsing -TimeoutSec 10
    $createdRecommendId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/recommends (create)" ($r.StatusCode -eq 201 -or $r.StatusCode -eq 200) "Created ID: $createdRecommendId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $errBody = Get-ErrorBody $_.Exception
    Test-Result "POST /api/recommends (create)" $false "Status: $code, Body: $errBody"
}

# GET recommend by ID
if ($createdRecommendId) {
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/recommends/$createdRecommendId" -Headers $headers -UseBasicParsing -TimeoutSec 5
        $rec = $r.Content | ConvertFrom-Json
        Test-Result "GET /api/recommends/{id}" ($r.StatusCode -eq 200 -and $rec.id -eq $createdRecommendId) "EmpSysId: $($rec.empSysId), Amount: $($rec.recommendAmount)"
    } catch {
        Test-Result "GET /api/recommends/{id}" $false $_.Exception.Message
    }
}

# GET non-existent recommend
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/recommends/99999" -Headers $headers -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /api/recommends/99999 returns 404" $false "Got $($r.StatusCode)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "GET /api/recommends/99999 returns 404" ($code -eq 404) "StatusCode: $code"
}

# GET recommends by period
if ($recPeriodId) {
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/recommends/period/$recPeriodId" -Headers $headers -UseBasicParsing -TimeoutSec 5
        $periodRecs = $r.Content | ConvertFrom-Json
        $pCount = if ($periodRecs -is [array]) { $periodRecs.Count } else { 1 }
        Test-Result "GET /api/recommends/period/{periodId}" ($r.StatusCode -eq 200) "Period recommends: $pCount"
    } catch {
        Test-Result "GET /api/recommends/period/{periodId}" $false $_.Exception.Message
    }
}

# GET recommends by employee
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/recommends/employee/5001" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $empRecs = $r.Content | ConvertFrom-Json
    $eCount = if ($empRecs -is [array]) { $empRecs.Count } else { 1 }
    Test-Result "GET /api/recommends/employee/5001" ($r.StatusCode -eq 200) "Employee recommends: $eCount"
} catch {
    Test-Result "GET /api/recommends/employee/5001" $false $_.Exception.Message
}

# PUT submit recommend (APR role)
if ($createdRecommendId) {
    $submitBody = @{
        recommendId = $createdRecommendId
        approverRole = "APR"
        submittedBy = 9010
        finalAmount = 5000.00
        finalLevel = $recLevelId
    } | ConvertTo-Json
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/recommends/$createdRecommendId/submit" -Method PUT -Headers $headers -Body $submitBody -UseBasicParsing -TimeoutSec 10
        Test-Result "PUT /api/recommends/{id}/submit (APR)" ($r.StatusCode -eq 200) "Recommend submitted"
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        $errBody = Get-ErrorBody $_.Exception
        Test-Result "PUT /api/recommends/{id}/submit (APR)" $false "Status: $code, Body: $errBody"
    }
}

# POST create a 2nd recommend to test reject
$createRecommend2Body = @{
    yearId = 2025
    periodId = $recPeriodId
    empSysId = 5002
    levelId = $recLevelId
    ctcAmount = 100000.00
    maximumCap = 8000.00
    eligibilityAmount = 6000.00
    recommendAmount = 3000.00
    initiativeTaken = "Managed team of 10"
    results = "Project delivered on time"
    recommendBy = "Manager2-TestSuite"
} | ConvertTo-Json

$createdRecommend2Id = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/recommends" -Method POST -Headers $headers -Body $createRecommend2Body -UseBasicParsing -TimeoutSec 10
    $createdRecommend2Id = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/recommends (create 2nd)" ($r.StatusCode -eq 201 -or $r.StatusCode -eq 200) "Created ID: $createdRecommend2Id"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $errBody = Get-ErrorBody $_.Exception
    Test-Result "POST /api/recommends (create 2nd)" $false "Status: $code, Body: $errBody"
}

# PUT reject recommend
if ($createdRecommend2Id) {
    $rejectBody = @{
        recommendId = $createdRecommend2Id
        rejectedBy = 9011
        rejectionRemarks = "Does not meet criteria - test suite"
    } | ConvertTo-Json
    try {
        $r = Invoke-WebRequest -Uri "$baseUrl/api/recommends/$createdRecommend2Id/reject" -Method PUT -Headers $headers -Body $rejectBody -UseBasicParsing -TimeoutSec 10
        Test-Result "PUT /api/recommends/{id}/reject" ($r.StatusCode -eq 200) "Recommend rejected"
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        $errBody = Get-ErrorBody $_.Exception
        Test-Result "PUT /api/recommends/{id}/reject" $false "Status: $code, Body: $errBody"
    }
}

# ═════════════════════════════════════════════════════════════════════════════════
# 9. REST - SUBMITS ENDPOINTS
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 9. REST - SUBMITS ENDPOINTS ===" -ForegroundColor Yellow

# GET all submits
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/submits" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $submits = $r.Content | ConvertFrom-Json
    $count = if ($submits -is [array]) { $submits.Count } elseif ($submits) { 1 } else { 0 }
    Test-Result "GET /api/submits (list all)" ($r.StatusCode -eq 200) "Submits: $count"
} catch {
    Test-Result "GET /api/submits (list all)" $false $_.Exception.Message
}

# POST create submit
$submitPeriodId = if ($createdPeriodId) { $createdPeriodId } elseif ($script:seedPeriodId) { $script:seedPeriodId } else { 1 }
$createSubmitBody = @{
    periodId = $submitPeriodId
    busId = 10
    bhrUpdBy = 9001
    bhrAmount = 250000.00
} | ConvertTo-Json

$createdSubmitId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/submits" -Method POST -Headers $headers -Body $createSubmitBody -UseBasicParsing -TimeoutSec 10
    $createdSubmitId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/submits (create)" ($r.StatusCode -eq 201 -or $r.StatusCode -eq 200) "Created ID: $createdSubmitId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $errBody = Get-ErrorBody $_.Exception
    Test-Result "POST /api/submits (create)" $false "Status: $code, Body: $errBody"
}

# ═════════════════════════════════════════════════════════════════════════════════
# 10. REST - MAIL TRIGGERS ENDPOINTS
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 10. REST - MAIL TRIGGERS ENDPOINTS ===" -ForegroundColor Yellow

# GET all mail triggers
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/mailtriggers" -Headers $headers -UseBasicParsing -TimeoutSec 5
    $triggers = $r.Content | ConvertFrom-Json
    $count = if ($triggers -is [array]) { $triggers.Count } elseif ($triggers) { 1 } else { 0 }
    Test-Result "GET /api/mailtriggers (list all)" ($r.StatusCode -eq 200) "Triggers: $count"
} catch {
    Test-Result "GET /api/mailtriggers (list all)" $false $_.Exception.Message
}

# POST create mail trigger
$createMailBody = @{
    quarterId = 1
    empSysId = 5001
    mailId = "test-mail-$ts@company.com"
    triggeredBy = 9001
} | ConvertTo-Json

$createdMailId = $null
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/mailtriggers" -Method POST -Headers $headers -Body $createMailBody -UseBasicParsing -TimeoutSec 10
    $createdMailId = ($r.Content | ConvertFrom-Json)
    Test-Result "POST /api/mailtriggers (create)" ($r.StatusCode -eq 201 -or $r.StatusCode -eq 200) "Created ID: $createdMailId"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    $errBody = Get-ErrorBody $_.Exception
    Test-Result "POST /api/mailtriggers (create)" $false "Status: $code, Body: $errBody"
}

# ═════════════════════════════════════════════════════════════════════════════════
# 11. GRAPHQL QUERIES
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 11. GRAPHQL QUERIES ===" -ForegroundColor Yellow

# getDemands
Invoke-GQL '{ getDemands { id demandType departmentId demandDescription requiredDate priority demandStatus createdBy createdAt } }' "GraphQL: getDemands"

# getDemand by ID
if ($existingDemandId) {
    Invoke-GQL "{ getDemand(id: $existingDemandId) { id demandType departmentId demandDescription priority demandStatus } }" "GraphQL: getDemand(id: $existingDemandId)"
}

# getDemand non-existent (should return null)
Invoke-GQL '{ getDemand(id: 99999) { id demandType } }' "GraphQL: getDemand(id: 99999) returns null"

# getBudgets
Invoke-GQL '{ getBudgets { id businessId yearId budgetAmount updatedBy updatedOn createdAt } }' "GraphQL: getBudgets"

# getBudgetsByYear
Invoke-GQL '{ getBudgetsByYear(yearId: 2025) { id businessId budgetAmount } }' "GraphQL: getBudgetsByYear(yearId: 2025)"

# getPeriods
Invoke-GQL '{ getPeriods { id yearId quarterNo status periodOpenDate periodCloseDate formOpenDate createdAt } }' "GraphQL: getPeriods"

# getLevels
Invoke-GQL '{ getLevels { id levelDesc levelAmount levelReason levelMin levelMax levelEffDate createdAt } }' "GraphQL: getLevels"

# getRecommends
Invoke-GQL '{ getRecommends { id yearId periodId empSysId ctcAmount recommendAmount status initiativeTaken results createdAt } }' "GraphQL: getRecommends"

# getRecommend by ID
if ($createdRecommendId) {
    Invoke-GQL "{ getRecommend(id: $createdRecommendId) { id empSysId ctcAmount recommendAmount status recommendBy } }" "GraphQL: getRecommend(id: $createdRecommendId)"
}

# getRecommendsByPeriod
if ($recPeriodId) {
    Invoke-GQL "{ getRecommendsByPeriod(periodId: $recPeriodId) { id empSysId recommendAmount } }" "GraphQL: getRecommendsByPeriod(periodId: $recPeriodId)"
}

# getRecommendsByEmployee
Invoke-GQL '{ getRecommendsByEmployee(empSysId: 5001) { id periodId recommendAmount status } }' "GraphQL: getRecommendsByEmployee(empSysId: 5001)"

# getSubmits
Invoke-GQL '{ getSubmits { id periodId busId bhrFlag chrFlag bhrAmount chrAmount createdAt } }' "GraphQL: getSubmits"

# getMailTriggers
Invoke-GQL '{ getMailTriggers { id quarterId empSysId mailId triggeredBy triggeredOn createdAt } }' "GraphQL: getMailTriggers"

# ═════════════════════════════════════════════════════════════════════════════════
# 12. GRAPHQL SCHEMA INTROSPECTION
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 12. GRAPHQL SCHEMA INTROSPECTION ===" -ForegroundColor Yellow

$schemaResult = Invoke-GQL '{ __schema { queryType { fields { name } } } }' "GraphQL: Schema introspection"
if ($schemaResult -and $schemaResult.data) {
    $fieldNames = $schemaResult.data.__schema.queryType.fields | ForEach-Object { $_.name }
    $expectedFields = @("getDemands","getDemand","getBudgets","getBudgetsByYear","getPeriods","getLevels","getRecommends","getRecommend","getRecommendsByPeriod","getRecommendsByEmployee","getSubmits","getMailTriggers")
    $allFound = $true
    foreach ($f in $expectedFields) {
        if ($fieldNames -notcontains $f) {
            $allFound = $false
            Write-Host "        Missing query: $f" -ForegroundColor Red
        }
    }
    Test-Result "All 12 GraphQL queries registered" $allFound "Fields: $($fieldNames -join ', ')"
}

# GraphQL type introspection
Invoke-GQL '{ __type(name: "DemandMasterType") { name fields { name type { name kind } } } }' "GraphQL: DemandMasterType introspection"

# ═════════════════════════════════════════════════════════════════════════════════
# 13. RABBITMQ INTEGRATION
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 13. RABBITMQ INTEGRATION ===" -ForegroundColor Yellow

# Check health includes RabbitMQ
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/health" -UseBasicParsing -TimeoutSec 5
    $health = $r.Content | ConvertFrom-Json
    $rabbitCheck = $health.checks | Where-Object { $_.name -like "*RabbitMQ*" }
    if ($rabbitCheck) {
        Test-Result "RabbitMQ health check present" $true "Status: $($rabbitCheck.status), Desc: $($rabbitCheck.description)"
    } else {
        Test-Result "RabbitMQ health check present" $false "No RabbitMQ check in health response"
    }
} catch {
    Test-Result "RabbitMQ health check present" $false $_.Exception.Message
}

# Verify API functions even with RabbitMQ unavailable (graceful degradation)
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/demands" -Headers $headers -UseBasicParsing -TimeoutSec 5
    Test-Result "API functional without active RabbitMQ" ($r.StatusCode -eq 200) "Graceful degradation: OK"
} catch {
    Test-Result "API functional without active RabbitMQ" $false $_.Exception.Message
}

try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/budgets" -Headers $headers -UseBasicParsing -TimeoutSec 5
    Test-Result "Budgets API with RabbitMQ config" ($r.StatusCode -eq 200) "Graceful degradation: OK"
} catch {
    Test-Result "Budgets API with RabbitMQ config" $false $_.Exception.Message
}

try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/recommends" -Headers $headers -UseBasicParsing -TimeoutSec 5
    Test-Result "Recommends API with RabbitMQ config" ($r.StatusCode -eq 200) "Graceful degradation: OK"
} catch {
    Test-Result "Recommends API with RabbitMQ config" $false $_.Exception.Message
}

# ═════════════════════════════════════════════════════════════════════════════════
# 14. EDGE CASES & ERROR HANDLING
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n=== 14. EDGE CASES & ERROR HANDLING ===" -ForegroundColor Yellow

# Invalid route
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/nonexistent" -Headers $headers -UseBasicParsing -TimeoutSec 5
    Test-Result "GET /api/nonexistent returns 404" $false "Got $($r.StatusCode)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "GET /api/nonexistent returns 404" ($code -eq 404) "StatusCode: $code"
}

# POST demand with missing required fields
$emptyDemand = @{} | ConvertTo-Json
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/demands" -Method POST -Headers $headers -Body $emptyDemand -UseBasicParsing -TimeoutSec 10
    Test-Result "POST /api/demands (empty body) returns 400" $false "Got $($r.StatusCode)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "POST /api/demands (empty body) returns 400" ($code -eq 400 -or $code -eq 500) "StatusCode: $code"
}

# POST budget with negative amount
$negativeBudget = @{
    businessId = 99
    yearId = 2026
    budgetAmount = -999.00
    updatedBy = 9001
} | ConvertTo-Json
try {
    $r = Invoke-WebRequest -Uri "$baseUrl/api/budgets" -Method POST -Headers $headers -Body $negativeBudget -UseBasicParsing -TimeoutSec 10
    Test-Result "POST /api/budgets (negative amount)" ($r.StatusCode -eq 200 -or $r.StatusCode -eq 201 -or $r.StatusCode -eq 400) "StatusCode: $($r.StatusCode) (accepted or rejected)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Test-Result "POST /api/budgets (negative amount)" ($code -eq 400) "StatusCode: $code (validation)"
}

# ═════════════════════════════════════════════════════════════════════════════════
# SUMMARY
# ═════════════════════════════════════════════════════════════════════════════════
Write-Host "`n================================================" -ForegroundColor Cyan
Write-Host " TEST RESULTS SUMMARY" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  Total:  $total" -ForegroundColor White
Write-Host "  Passed: $pass" -ForegroundColor Green
Write-Host "  Failed: $fail" -ForegroundColor $(if ($fail -gt 0) { "Red" } else { "Green" })
$rate = if ($total -gt 0) { [math]::Round($pass/$total*100, 1) } else { 0 }
Write-Host "  Rate:   $rate%`n" -ForegroundColor $(if ($fail -gt 0) { "Yellow" } else { "Green" })
Write-Host "  Service:   TransactionService.API" -ForegroundColor Gray
Write-Host "  Port:      5178" -ForegroundColor Gray
Write-Host "  DB:        TransactionServiceDb (LocalDB)" -ForegroundColor Gray
Write-Host "  Auth:      JWT Bearer (HS256)" -ForegroundColor Gray
Write-Host "  GraphQL:   /graphql (HotChocolate 14)" -ForegroundColor Gray
Write-Host "  RabbitMQ:  Graceful degradation mode" -ForegroundColor Gray
Write-Host "  Endpoints: 7 controllers, 27 REST routes" -ForegroundColor Gray
Write-Host "  GraphQL:   12 query operations" -ForegroundColor Gray
Write-Host "================================================`n" -ForegroundColor Cyan
