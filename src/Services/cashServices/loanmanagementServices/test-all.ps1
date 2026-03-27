$base = "http://localhost:5268"

# ── 1. Get JWT Token ──
Write-Host "=== 1. AUTH: Get JWT Token ==="
$loginBody = '{"username":"admin","password":"Admin@123"}'
try {
    $authResult = Invoke-RestMethod -Uri "$base/api/auth/token" -Method POST -Body $loginBody -ContentType "application/json"
    $token = $authResult.accessToken
    Write-Host "Token obtained. ExpiresIn: $($authResult.expiresIn)s"
    Write-Host "Token (first 50): $($token.Substring(0,50))..."
} catch {
    Write-Host "AUTH FAILED: $($_.Exception.Message)"
    exit 1
}

$headers = @{ "Content-Type" = "application/json"; "Authorization" = "Bearer $token" }

# ── 2. GET All Loans ──
Write-Host "`n=== 2. GET /api/loans ==="
try {
    $loans = Invoke-RestMethod -Uri "$base/api/loans" -Headers $headers
    Write-Host "Loans count: $($loans.Count)"
    $loans | ForEach-Object { Write-Host "  LoanId=$($_.loanId) Key=$($_.loanKey) Amount=$($_.loanAmount) Status=$($_.loanStatus)" }
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
}

# ── 3. GET Loan by ID (use first loan) ──
Write-Host "`n=== 3. GET /api/loans/{id} ==="
if ($loans -and $loans.Count -gt 0) {
    $firstLoanId = $loans[0].loanId
    try {
        $loan = Invoke-RestMethod -Uri "$base/api/loans/$firstLoanId" -Headers $headers
        Write-Host "Loan: $($loan | ConvertTo-Json -Depth 3 -Compress)"
    } catch {
        Write-Host "ERROR: $($_.Exception.Message)"
    }
} else {
    Write-Host "No loans to query"
}

# ── 4. GET Loan not found ──
Write-Host "`n=== 4. GET /api/loans/99999 (not found) ==="
try {
    $r = Invoke-WebRequest -Uri "$base/api/loans/99999" -Headers $headers -UseBasicParsing
    Write-Host "Status: $($r.StatusCode) Body: $($r.Content)"
} catch {
    Write-Host "Status: $($_.Exception.Response.StatusCode)"
}

# ── 5. POST Create Loan ──
Write-Host "`n=== 5. POST /api/loans (create) ==="
$newLoanBody = @{
    loanId = 9999
    loanKey = "TEST-RMQ-001"
    orgId = 1
    loanAmount = 500000
    loanTypeId = 1
    bankId = 1
    createdBy = 1
    loanDate = "2026-03-27T00:00:00"
    orgCurr = 1
    loanCurr = 1
} | ConvertTo-Json
try {
    $created = Invoke-RestMethod -Uri "$base/api/loans" -Method POST -Body $newLoanBody -Headers $headers
    Write-Host "Created: LoanId=$($created.loanId) Key=$($created.loanKey) Status=$($created.loanStatus)"
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
    try { $sr = New-Object IO.StreamReader($_.Exception.Response.GetResponseStream()); Write-Host "Body: $($sr.ReadToEnd())" } catch {}
}

# ── 6. POST Add Disbursement ──
Write-Host "`n=== 6. POST /api/loans/{id}/disbursements ==="
$disbBody = @{
    disbDate = "2026-04-01T00:00:00"
    amount = 100000
    excRate = 1.0
} | ConvertTo-Json
$loanIdForTest = if ($created) { $created.loanId } elseif ($loans -and $loans.Count -gt 0) { $loans[0].loanId } else { 1 }
try {
    $disb = Invoke-RestMethod -Uri "$base/api/loans/$loanIdForTest/disbursements" -Method POST -Body $disbBody -Headers $headers
    Write-Host "Disbursement: $($disb | ConvertTo-Json -Depth 3 -Compress)"
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
    try { $sr = New-Object IO.StreamReader($_.Exception.Response.GetResponseStream()); Write-Host "Body: $($sr.ReadToEnd())" } catch {}
}

# ── 7. POST Add Interest ──
Write-Host "`n=== 7. POST /api/loans/{id}/interests ==="
$intBody = @{
    rateType = "FX"
    percentage = 5.5
    floatTypeId = $null
    effectiveDate = "2026-03-27T00:00:00"
} | ConvertTo-Json
try {
    $interest = Invoke-RestMethod -Uri "$base/api/loans/$loanIdForTest/interests" -Method POST -Body $intBody -Headers $headers
    Write-Host "Interest: $($interest | ConvertTo-Json -Depth 3 -Compress)"
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
    try { $sr = New-Object IO.StreamReader($_.Exception.Response.GetResponseStream()); Write-Host "Body: $($sr.ReadToEnd())" } catch {}
}

# ── 8. POST Add Repayment Schedule ──
Write-Host "`n=== 8. POST /api/loans/{id}/repayments ==="
$repayBody = @{
    lines = @(
        @{ repayDate = "2026-06-01T00:00:00"; amount = 50000 },
        @{ repayDate = "2026-07-01T00:00:00"; amount = 50000 }
    )
} | ConvertTo-Json -Depth 3
try {
    $repay = Invoke-RestMethod -Uri "$base/api/loans/$loanIdForTest/repayments" -Method POST -Body $repayBody -Headers $headers
    Write-Host "Repayments created: $($repay.Count)"
    $repay | ForEach-Object { Write-Host "  RepayId=$($_.repayId) Date=$($_.repayDate) Amt=$($_.repayAmt) Flag=$($_.repayFlag)" }
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
    try { $sr = New-Object IO.StreamReader($_.Exception.Response.GetResponseStream()); Write-Host "Body: $($sr.ReadToEnd())" } catch {}
}

# ── 9. GET Repayment Schedule ──
Write-Host "`n=== 9. GET /api/loans/{id}/repayments ==="
try {
    $schedule = Invoke-RestMethod -Uri "$base/api/loans/$loanIdForTest/repayments" -Headers $headers
    Write-Host "Repayments: $($schedule.Count)"
    $schedule | ForEach-Object { Write-Host "  RepayId=$($_.repayId) Date=$($_.repayDate) Amt=$($_.repayAmt) Flag=$($_.repayFlag)" }
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
}

# ── 10. POST Close Loan ──
Write-Host "`n=== 10. POST /api/loans/{id}/close ==="
$closeBody = '{"modifiedBy":1}'
try {
    $r = Invoke-WebRequest -Uri "$base/api/loans/$loanIdForTest/close" -Method POST -Body $closeBody -Headers $headers -UseBasicParsing
    Write-Host "Status: $($r.StatusCode)"
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
    try { $sr = New-Object IO.StreamReader($_.Exception.Response.GetResponseStream()); Write-Host "Body: $($sr.ReadToEnd())" } catch {}
}

# ── 11. GraphQL: Get All Loans ──
Write-Host "`n=== 11. GraphQL: loans query ==="
$gqlBody = '{"query":"{ loans { loanId loanKey loanAmount loanStatus loanDate } }"}'
try {
    $gql = Invoke-RestMethod -Uri "$base/graphql" -Method POST -Body $gqlBody -ContentType "application/json"
    Write-Host ($gql | ConvertTo-Json -Depth 5 -Compress)
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
}

# ── 12. GraphQL: Get Loan by ID ──
Write-Host "`n=== 12. GraphQL: loanById query ==="
$gqlBody = "{`"query`":`"{ loanById(loanId: $firstLoanId) { loanId loanKey loanAmount loanStatus disbursements { disbId disbAmount disbDate } interests { intId intRateType intPer } repayments { repayId repayAmt repayDate repayFlag } } }`"}"
try {
    $gql = Invoke-RestMethod -Uri "$base/graphql" -Method POST -Body $gqlBody -ContentType "application/json"
    Write-Host ($gql | ConvertTo-Json -Depth 6 -Compress)
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
}

# ── 13. GraphQL: Repayment Schedule ──
Write-Host "`n=== 13. GraphQL: repaymentSchedule query ==="
$gqlBody = "{`"query`":`"{ repaymentSchedule(loanId: $firstLoanId) { repayId repayAmt repayDate repayFlag } }`"}"
try {
    $gql = Invoke-RestMethod -Uri "$base/graphql" -Method POST -Body $gqlBody -ContentType "application/json"
    Write-Host ($gql | ConvertTo-Json -Depth 5 -Compress)
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
}

# ── 14. GraphQL: Create Loan Mutation ──
Write-Host "`n=== 14. GraphQL: createLoan mutation ==="
$gqlBody = '{"query":"mutation { createLoan(input: { loanKey: \"GQL-TEST-001\", orgId: 2, loanAmount: 250000, loanTypeId: 1, bankId: 1, createdBy: 1, loanDate: \"2026-03-27T00:00:00.000Z\", orgCurr: 1, loanCurr: 1 }) { loanId loanKey loanAmount loanStatus } }"}'
try {
    $gql = Invoke-RestMethod -Uri "$base/graphql" -Method POST -Body $gqlBody -ContentType "application/json"
    Write-Host ($gql | ConvertTo-Json -Depth 5 -Compress)
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
}

# ── 15. GraphQL: Close Loan Mutation (close the loan created in test 14) ──
Write-Host "`n=== 15. GraphQL: closeLoan mutation ==="
$gqlLoanId = if ($gql.data.createLoan) { $gql.data.createLoan.loanId } else { 5 }
$gqlBody = "{`"query`":`"mutation { closeLoan(loanId: $gqlLoanId, modifiedBy: 1) }`"}"
try {
    $gql = Invoke-RestMethod -Uri "$base/graphql" -Method POST -Body $gqlBody -ContentType "application/json"
    Write-Host ($gql | ConvertTo-Json -Depth 5 -Compress)
} catch {
    Write-Host "ERROR: $($_.Exception.Message)"
}

# ── SUMMARY ──
Write-Host "`n========================================="
Write-Host "TEST COMPLETE"
Write-Host "========================================="
