$base = "http://localhost:5081/api"
$gql  = "http://localhost:5081/graphql"
$pass = 0; $fail = 0; $results = @()
$ts = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds()

function Test($name, $block) {
    try {
        $r = & $block
        $script:pass++
        $script:results += "PASS: $name"
        Write-Host "PASS: $name" -F Green
        return $r
    } catch {
        $script:fail++
        $script:results += "FAIL: $name - $_"
        Write-Host "FAIL: $name - $_" -F Red
        return $null
    }
}

function GQL($query) {
    $body = @{ query = $query } | ConvertTo-Json -Depth 10
    $r = Invoke-RestMethod $gql -Method Post -ContentType "application/json" -Headers $ah -Body $body
    if ($r.errors) { throw ($r.errors | ConvertTo-Json -Compress) }
    return $r.data
}

$h = @{ "Content-Type" = "application/json" }

# ─── 1. AUTH ───
Write-Host "`n--- Authentication ---" -F Yellow

$token = Test "POST /auth/login (admin)" {
    $r = Invoke-RestMethod "$base/auth/login" -Method Post -ContentType "application/json" -Body '{"username":"admin","password":"Admin@123"}'
    if (-not $r.token) { throw "No token" }
    $r.token
}
$ah = @{ Authorization = "Bearer $token"; "Content-Type" = "application/json" }

$approverToken = Test "POST /auth/login (approver)" {
    $r = Invoke-RestMethod "$base/auth/login" -Method Post -ContentType "application/json" -Body '{"username":"approver","password":"Approver@123"}'
    if (-not $r.token) { throw "No token" }
    $r.token
}
$approverH = @{ Authorization = "Bearer $approverToken"; "Content-Type" = "application/json" }

Test "POST /auth/login (invalid creds)" {
    try {
        Invoke-RestMethod "$base/auth/login" -Method Post -ContentType "application/json" -Body '{"username":"bad","password":"bad"}'
        throw "Should have returned 401"
    } catch {
        if ($_.Exception.Response.StatusCode -ne 401 -and $_ -notmatch "401") { throw "Expected 401, got: $_" }
        Write-Host "  Correctly returned 401" -F Cyan
    }
}

# ─── 2. DEAL BATCH CRUD ───
Write-Host "`n--- Deal Batches ---" -F Yellow

$batchId = 1000 + ($ts % 9000)
$batch = Test "POST /dealbatch (create FX Forward)" {
    $body = @{
        dealBatchId = $batchId
        dealDate = "2026-03-27T00:00:00"
        dealDerType = 1
        dealBankId = 1
        dealBookedBy = 100
        dealBankTrader = "John Smith"
        dealBusinessId = 1001
        dealModifiedBy = 1
        dealUnitId = 10
        dealOptionType = $null
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/dealbatch" -Method Post -Headers $ah -Body $body
    if ($r.dealBatchId -ne $batchId) { throw "Expected batch ID $batchId, got $($r.dealBatchId)" }
    Write-Host "  Created batch $($r.dealBatchId) on $($r.dealDate)" -F Cyan
    $r
}

Test "GET /dealbatch/$batchId" {
    $r = Invoke-RestMethod "$base/dealbatch/$batchId" -Headers $ah
    if ($r.dealBatchId -ne $batchId) { throw "Wrong batch ID" }
    Write-Host "  Batch $($r.dealBatchId): DerType=$($r.dealDerType), Bank=$($r.bankName)" -F Cyan
}

Test "GET /dealbatch/by-date/2026-03-27" {
    $r = Invoke-RestMethod "$base/dealbatch/by-date/2026-03-27" -Headers $ah
    if ($r.Count -lt 1) { throw "Expected at least 1 batch, got $($r.Count)" }
    Write-Host "  Found $($r.Count) batches for 2026-03-27" -F Cyan
}

Test "PUT /dealbatch/$batchId/screenshot" {
    $body = @{ screenshot = "reuters_screenshot_base64_data_$ts"; modifiedBy = 1 } | ConvertTo-Json
    Invoke-RestMethod "$base/dealbatch/$batchId/screenshot" -Method Put -Headers $ah -Body $body
    Write-Host "  Screenshot updated on batch $batchId" -F Cyan
}

# ─── 3. DEAL DETAILS ───
Write-Host "`n--- Deal Details ---" -F Yellow

$dealId = 2000 + ($ts % 8000)
$deal = Test "POST /dealdetail (create FX Forward deal)" {
    $body = @{
        dealId = $dealId
        dealNo = 1
        dealVersionId = 1
        dealBatchId = $batchId
        dealTranType = "B"
        dealPosition = "Long"
        dealAmount = 1000000
        dealBankId = 1
        dealCurrency1 = 1
        dealCurrency2 = 2
        dealSpotRate = 1.0850
        dealForPoints = 0.0025
        dealBankMargin = 0.0002
        dealBookRate = 1.0877
        dealMatDate = "2026-06-27T00:00:00"
        dealDealType = 1
        dealBusiness = 1001
        dealCategory = 1
        dealRemarks = "Test FX Forward - Buy EUR/USD"
        modifiedBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/dealdetail" -Method Post -Headers $ah -Body $body
    if ($r.dealId -ne $dealId) { throw "Expected deal ID $dealId, got $($r.dealId)" }
    Write-Host "  Created deal $($r.dealId): $($r.dealTranType) $($r.dealAmount) at $($r.dealBookRate)" -F Cyan
    $r
}

$dealId2 = $dealId + 1
$deal2 = Test "POST /dealdetail (create FX Option deal)" {
    $body = @{
        dealId = $dealId2
        dealNo = 2
        dealVersionId = 1
        dealBatchId = $batchId
        dealTranType = "B"
        dealPosition = "Long"
        dealAmount = 500000
        dealBankId = 2
        dealCurrency1 = 3
        dealCurrency2 = 1
        dealSpotRate = 1.2650
        dealForPoints = 0
        dealBankMargin = 0.015
        dealBookRate = 1.2800
        dealMatDate = "2026-09-27T00:00:00"
        dealDealType = 2
        dealBusiness = 1001
        dealCategory = 2
        dealRemarks = "Test FX Option - Buy GBP/USD Call"
        modifiedBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/dealdetail" -Method Post -Headers $ah -Body $body
    Write-Host "  Created deal $($r.dealId): Option $($r.dealAmount) at $($r.dealBookRate)" -F Cyan
    $r
}

Test "GET /dealdetail/$dealId" {
    $r = Invoke-RestMethod "$base/dealdetail/$dealId" -Headers $ah
    if ($r.dealId -ne $dealId) { throw "Wrong deal ID" }
    Write-Host "  Deal $($r.dealId): Batch=$($r.dealBatchId), Amount=$($r.dealAmount), Status=$($r.dealAppStatus)" -F Cyan
}

Test "GET /dealdetail/by-batch/$batchId" {
    $r = Invoke-RestMethod "$base/dealdetail/by-batch/$batchId" -Headers $ah
    if ($r.Count -lt 2) { throw "Expected at least 2 deals, got $($r.Count)" }
    Write-Host "  Batch $batchId has $($r.Count) deals" -F Cyan
}

Test "GET /dealdetail/pending-approvals" {
    $r = Invoke-RestMethod "$base/dealdetail/pending-approvals" -Headers $ah
    if ($r.Count -lt 1) { throw "Expected at least 1 pending deal" }
    Write-Host "  $($r.Count) deals pending approval" -F Cyan
}

# ─── 4. DEAL APPROVAL & REJECTION ───
Write-Host "`n--- Approval Workflow ---" -F Yellow

Test "POST /dealdetail/$dealId/approve" {
    $body = @{ appBusiness = 2001; remarks = "Approved - within risk limits"; modifiedBy = 2 } | ConvertTo-Json
    Invoke-RestMethod "$base/dealdetail/$dealId/approve" -Method Post -Headers $ah -Body $body
    Write-Host "  Deal $dealId approved" -F Cyan
}

Test "GET /dealdetail/$dealId (verify approved)" {
    $r = Invoke-RestMethod "$base/dealdetail/$dealId" -Headers $ah
    if ($r.dealAppStatus -ne 'Y') { throw "Expected status Y, got $($r.dealAppStatus)" }
    Write-Host "  Deal $dealId status: $($r.dealAppStatus) (approved)" -F Cyan
}

Test "POST /dealdetail/$dealId2/reject" {
    $body = @{ remarks = "Rejected - exceeds daily limit"; modifiedBy = 2 } | ConvertTo-Json
    Invoke-RestMethod "$base/dealdetail/$dealId2/reject" -Method Post -Headers $ah -Body $body
    Write-Host "  Deal $dealId2 rejected" -F Cyan
}

Test "GET /dealdetail/$dealId2 (verify rejected)" {
    $r = Invoke-RestMethod "$base/dealdetail/$dealId2" -Headers $ah
    if ($r.dealAppStatus -ne 'R') { throw "Expected status R, got $($r.dealAppStatus)" }
    Write-Host "  Deal $dealId2 status: $($r.dealAppStatus) (rejected)" -F Cyan
}

# ─── 5. DEAL BATCH REJECTION ───
Write-Host "`n--- Batch Rejection ---" -F Yellow

$batchId2 = $batchId + 100
Test "POST /dealbatch (create batch for rejection)" {
    $body = @{
        dealBatchId = $batchId2
        dealDate = "2026-03-27T00:00:00"
        dealDerType = 3
        dealBankId = 3
        dealBookedBy = 100
        dealBankTrader = "Jane Doe"
        dealBusinessId = 1002
        dealModifiedBy = 1
        dealUnitId = 20
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/dealbatch" -Method Post -Headers $ah -Body $body
    Write-Host "  Created batch $batchId2 for rejection test" -F Cyan
}

Test "POST /dealbatch/$batchId2/reject" {
    $body = @{ reason = "Duplicate batch entry"; modifiedBy = 2 } | ConvertTo-Json
    Invoke-RestMethod "$base/dealbatch/$batchId2/reject" -Method Post -Headers $ah -Body $body
    Write-Host "  Batch $batchId2 rejected" -F Cyan
}

# ─── 6. SETTLEMENTS ───
Write-Host "`n--- Settlements ---" -F Yellow

$setId = 3000 + ($ts % 7000)
Test "POST /dealdetail/$dealId/settlements (create)" {
    $body = @{
        setId = $setId
        dealId = $dealId
        gainLossAmt = 15250.75
        setType = "C"
        spotRate = 1.0920
        exchangeRate = 1.0877
        modifiedBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/dealdetail/$dealId/settlements" -Method Post -Headers $ah -Body $body
    Write-Host "  Settlement $($r.setId): GainLoss=$($r.setGainLossAmt)" -F Cyan
}

Test "GET /dealdetail/$dealId/settlements" {
    $r = Invoke-RestMethod "$base/dealdetail/$dealId/settlements" -Headers $ah
    if ($r.Count -lt 1) { throw "Expected at least 1 settlement" }
    Write-Host "  Deal $dealId has $($r.Count) settlement(s)" -F Cyan
}

# ─── 7. REPORTS ───
Write-Host "`n--- Reports ---" -F Yellow

Test "GET /reports/deal-summary" {
    $r = Invoke-RestMethod "$base/reports/deal-summary?fromDate=2026-01-01&toDate=2026-12-31" -Headers $ah
    Write-Host "  Deal summary: $($r.Count) record(s)" -F Cyan
}

Test "GET /reports/pnl" {
    $r = Invoke-RestMethod "$base/reports/pnl?fromDate=2026-01-01" -Headers $ah
    Write-Host "  P&L report: $($r.Count) record(s)" -F Cyan
}

Test "GET /reports/pending-approvals" {
    $r = Invoke-RestMethod "$base/reports/pending-approvals" -Headers $ah
    Write-Host "  Pending approvals report: $($r.Count) record(s)" -F Cyan
}

# ─── 8. GRAPHQL QUERIES ───
Write-Host "`n--- GraphQL Queries ---" -F Yellow

Test "GQL: dealBatch" {
    $d = GQL "{ dealBatch(id: $batchId) { dealBatchId dealDate dealDerType bankName dealBusinessId } }"
    if ($d.dealBatch.dealBatchId -ne $batchId) { throw "Wrong batch" }
    Write-Host "  Batch $($d.dealBatch.dealBatchId): Bank=$($d.dealBatch.bankName)" -F Cyan
}

Test "GQL: dealBatches" {
    $d = GQL '{ dealBatches(date: "2026-03-27T00:00:00.000Z") { dealBatchId dealDate } }'
    if ($d.dealBatches.Count -lt 1) { throw "Expected at least 1 batch" }
    Write-Host "  Found $($d.dealBatches.Count) batches" -F Cyan
}

Test "GQL: dealDetail" {
    $d = GQL "{ dealDetail(id: $dealId) { dealId dealBatchId dealAmount dealAppStatus dealBookRate } }"
    if ($d.dealDetail.dealId -ne $dealId) { throw "Wrong deal" }
    Write-Host "  Deal $($d.dealDetail.dealId): Amount=$($d.dealDetail.dealAmount), Status=$($d.dealDetail.dealAppStatus)" -F Cyan
}

Test "GQL: dealDetails (by batch)" {
    $d = GQL "{ dealDetails(batchId: $batchId) { dealId dealNo dealAmount } }"
    if ($d.dealDetails.Count -lt 1) { throw "Expected deals in batch" }
    Write-Host "  Batch ${batchId}: $($d.dealDetails.Count) deal(s)" -F Cyan
}

Test "GQL: pendingApprovals" {
    $d = GQL "{ pendingApprovals { dealId dealAppStatus dealAmount } }"
    Write-Host "  $($d.pendingApprovals.Count) pending approval(s)" -F Cyan
}

Test "GQL: settlements" {
    $d = GQL "{ settlements(dealId: $dealId) { setId setGainLossAmt setType } }"
    if ($d.settlements.Count -lt 1) { throw "Expected at least 1 settlement" }
    Write-Host "  Deal ${dealId}: $($d.settlements.Count) settlement(s)" -F Cyan
}

Test "GQL: banks" {
    $d = GQL "{ banks { bankId bankName } }"
    if ($d.banks.Count -lt 5) { throw "Expected at least 5 banks, got $($d.banks.Count)" }
    Write-Host "  $($d.banks.Count) banks: $($d.banks[0..2].bankName -join ', ')..." -F Cyan
}

# ─── 9. GRAPHQL MUTATIONS ───
Write-Host "`n--- GraphQL Mutations ---" -F Yellow

$gqlBatchId = 50000 + ($ts % 5000)
Test "GQL Mutation: createDealBatch" {
    $d = GQL "mutation { createDealBatch(input: { dealBatchId: $gqlBatchId, dealDate: ""2026-03-27T00:00:00Z"", dealDerType: 2, dealBankId: 4, dealBookedBy: 200, dealBankTrader: ""GQL Trader"", dealBusinessId: 2001, dealModifiedBy: 1, dealUnitId: 30 }) { dealBatchId dealDate bankName } }"
    if ($d.createDealBatch.dealBatchId -ne $gqlBatchId) { throw "Wrong batch ID" }
    Write-Host "  Created GQL batch $($d.createDealBatch.dealBatchId): Bank=$($d.createDealBatch.bankName)" -F Cyan
}

$gqlDealId = 60000 + ($ts % 4000)
Test "POST /dealdetail (deal for GQL approve)" {
    $body = @{
        dealId = $gqlDealId
        dealNo = 1
        dealVersionId = 1
        dealBatchId = $gqlBatchId
        dealTranType = "S"
        dealPosition = "Short"
        dealAmount = 250000
        dealBankId = 4
        dealCurrency1 = 2
        dealCurrency2 = 1
        dealSpotRate = 1.0900
        dealBookRate = 1.0925
        dealMatDate = "2026-12-27T00:00:00"
        dealDealType = 1
        dealBusiness = 2001
        dealCategory = 1
        dealRemarks = "GQL test deal"
        modifiedBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/dealdetail" -Method Post -Headers $ah -Body $body
    Write-Host "  Created deal $gqlDealId for GQL mutation tests" -F Cyan
}

Test "GQL Mutation: approveDeal" {
    $d = GQL "mutation { approveDeal(dealId: $gqlDealId, appBusiness: 3001, remarks: ""GQL approved"", modifiedBy: 2) }"
    if ($d.approveDeal -ne $true) { throw "Expected true" }
    Write-Host "  GQL approved deal $gqlDealId" -F Cyan
}

$gqlDealId2 = $gqlDealId + 1
Test "POST /dealdetail (deal for GQL reject)" {
    $body = @{
        dealId = $gqlDealId2
        dealNo = 2
        dealVersionId = 1
        dealBatchId = $gqlBatchId
        dealTranType = "B"
        dealAmount = 750000
        dealBankId = 4
        dealCurrency1 = 1
        dealCurrency2 = 3
        dealSpotRate = 0.7900
        dealBookRate = 0.7950
        dealMatDate = "2026-12-27T00:00:00"
        dealDealType = 2
        dealBusiness = 2001
        dealCategory = 2
        dealRemarks = "GQL reject test deal"
        modifiedBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/dealdetail" -Method Post -Headers $ah -Body $body
    Write-Host "  Created deal $gqlDealId2 for GQL reject test" -F Cyan
}

Test "GQL Mutation: rejectDeal" {
    $d = GQL "mutation { rejectDeal(dealId: $gqlDealId2, remarks: ""GQL rejected - risk too high"", modifiedBy: 2) }"
    if ($d.rejectDeal -ne $true) { throw "Expected true" }
    Write-Host "  GQL rejected deal $gqlDealId2" -F Cyan
}

# ─── 10. MINIMAL API V2 ENDPOINTS ───
Write-Host "`n--- Minimal API v2 ---" -F Yellow

Test "GET /v2/deals/pending-approvals" {
    $r = Invoke-RestMethod "http://localhost:5081/api/v2/deals/pending-approvals" -Headers $ah
    Write-Host "  V2 pending approvals: $($r.Count) deal(s)" -F Cyan
}

Test "GET /v2/deals/$dealId/settlements" {
    $r = Invoke-RestMethod "http://localhost:5081/api/v2/deals/$dealId/settlements" -Headers $ah
    Write-Host "  V2 settlements for deal $dealId`: $($r.Count)" -F Cyan
}

Test "GET /v2/deals/summary" {
    $r = Invoke-RestMethod "http://localhost:5081/api/v2/deals/summary?fromDate=2026-01-01&toDate=2026-12-31" -Headers $ah
    Write-Host "  V2 deal summary: $($r.Count) record(s)" -F Cyan
}

# ─── 11. HEALTH CHECK ───
Write-Host "`n--- Health ---" -F Yellow

Test "GET /health" {
    $r = Invoke-RestMethod "http://localhost:5081/health"
    Write-Host "  Health: $r" -F Cyan
}

# ─── 12. DOMAIN EVENTS & MESSAGING ───
Write-Host "`n--- Domain Events & MassTransit ---" -F Yellow
Write-Host "MassTransit consumers registered (loopback transport - no RabbitMQ required):" -F Cyan
Write-Host "  - DealBatchCreatedConsumer" -F Cyan
Write-Host "  - DealApprovedConsumer" -F Cyan
Write-Host "  - DealRejectedConsumer" -F Cyan
Write-Host "  - DealSettledConsumer" -F Cyan
Write-Host "Domain events dispatched via MediatR:" -F Cyan
Write-Host "  - DealBatchCreatedEvent, DealCreatedEvent" -F Cyan
Write-Host "  - DealApprovedEvent, DealRejectedEvent, DealSettledEvent" -F Cyan

# ─── SUMMARY ───
Write-Host "`n========================================" -F White
Write-Host " RESULTS: $pass PASSED, $fail FAILED" -F $(if ($fail -eq 0) { "Green" } else { "Red" })
Write-Host "========================================" -F White
$results | ForEach-Object { Write-Host $_ -F $(if ($_ -match "^PASS") { "Green" } else { "Red" }) }
