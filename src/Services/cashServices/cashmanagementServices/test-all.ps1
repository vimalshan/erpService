$base = "http://localhost:5249/api/v1"
$gql  = "http://localhost:5249/graphql"
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

# ─── 1. AUTH ───
$token = Test "POST /auth/token" {
    $r = Invoke-RestMethod "$base/auth/token" -Method Post -ContentType "application/json" -Body '{"username":"admin","password":"admin123"}'
    if (-not $r.accessToken) { throw "No token" }
    $r.accessToken
}
$h = @{ Authorization = "Bearer $token"; "Content-Type" = "application/json" }

# ─── 2. CASH UNITS ───
Test "GET /cashunits (seed data)" {
    $r = Invoke-RestMethod "$base/cashunits" -Headers $h
    if ($r.Count -lt 2) { throw "Expected at least 2 cash units, got $($r.Count)" }
    Write-Host "  Found $($r.Count) cash units" -F Cyan
}

Test "GET /cashunits/1" {
    $r = Invoke-RestMethod "$base/cashunits/1" -Headers $h
    if ($r.cashUnitId -ne 1) { throw "Wrong ID" }
    Write-Host "  Unit: $($r.name) [$($r.code)]" -F Cyan
}

Test "GET /cashunits/1/balance" {
    $r = Invoke-RestMethod "$base/cashunits/1/balance" -Headers $h
    Write-Host "  Balance: $($r.balance)" -F Cyan
}

$newCashUnitId = 9000 + ($ts % 1000)
$newCashUnit = Test "POST /cashunits (create new)" {
    $body = @{
        cashUnitId = $newCashUnitId
        name = "Test Cash Register $ts"
        code = "TCR-$ts"
        location = "Test Location"
        inChargeEmployeeId = 1
        openingBalance = 5000.00
        createdBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/cashunits" -Method Post -Headers $h -Body $body
    if ($r.cashUnitId -ne $newCashUnitId) { throw "Expected ID $newCashUnitId, got $($r.cashUnitId)" }
    Write-Host "  Created: $($r.name) ID=$($r.cashUnitId)" -F Cyan
    $r
}

Test "PUT /cashunits/$newCashUnitId/status (deactivate)" {
    $body = @{ cashUnitId = $newCashUnitId; isActive = $false; updatedBy = 1 } | ConvertTo-Json
    Invoke-RestMethod "$base/cashunits/$newCashUnitId/status" -Method Put -Headers $h -Body $body
    Write-Host "  Status updated" -F Cyan
}

# ─── 3. CASH TRANSACTIONS ───
$cashReceipt = Test "POST /cashtransactions/receipt" {
    $body = @{
        cashUnitId = 1
        amount = 1500.00
        source = "Customer Payment"
        refNo = "RCPT-$ts"
        remarks = "Test receipt"
        createdBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/cashtransactions/receipt" -Method Post -Headers $h -Body $body
    Write-Host "  Receipt TxnId=$($r.cashTxnId) Amount=$($r.amount)" -F Cyan
    $r
}

$cashDisb = Test "POST /cashtransactions/disbursement" {
    $body = @{
        cashUnitId = 1
        amount = 500.00
        source = "Office Supplies"
        payeeId = 2
        refNo = "DISB-$ts"
        remarks = "Test disbursement"
        createdBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/cashtransactions/disbursement" -Method Post -Headers $h -Body $body
    Write-Host "  Disbursement TxnId=$($r.cashTxnId) Amount=$($r.amount)" -F Cyan
    $r
}

Test "GET /cashtransactions/by-unit/1" {
    $from = (Get-Date).AddMonths(-1).ToString("yyyy-MM-dd")
    $to   = (Get-Date).AddDays(1).ToString("yyyy-MM-dd")
    $r = Invoke-RestMethod "$base/cashtransactions/by-unit/1?from=$from&to=$to" -Headers $h
    Write-Host "  Found $($r.Count) transactions" -F Cyan
}

# ─── 4. BANK ACCOUNTS ───
Test "GET /bankaccounts (seed data)" {
    $r = Invoke-RestMethod "$base/bankaccounts" -Headers $h
    if ($r.Count -lt 2) { throw "Expected at least 2 bank accounts, got $($r.Count)" }
    Write-Host "  Found $($r.Count) bank accounts" -F Cyan
}

Test "GET /bankaccounts/1" {
    $r = Invoke-RestMethod "$base/bankaccounts/1" -Headers $h
    if ($r.bankAccountId -ne 1) { throw "Wrong ID" }
    Write-Host "  Bank: $($r.bankName) [$($r.accountNo)]" -F Cyan
}

Test "GET /bankaccounts/1/balance" {
    $r = Invoke-RestMethod "$base/bankaccounts/1/balance" -Headers $h
    Write-Host "  Balance: $($r.balance)" -F Cyan
}

$newBankId = 8000 + ($ts % 1000)
$newBank = Test "POST /bankaccounts (create new)" {
    $body = @{
        bankAccountId = $newBankId
        bankName = "Test Bank $ts"
        accountNo = "TST-$ts"
        branch = "Test Branch"
        accountType = "Savings"
        createdBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/bankaccounts" -Method Post -Headers $h -Body $body
    if ($r.bankAccountId -ne $newBankId) { throw "Expected ID $newBankId, got $($r.bankAccountId)" }
    Write-Host "  Created: $($r.bankName) ID=$($r.bankAccountId)" -F Cyan
    $r
}

Test "PUT /bankaccounts/$newBankId/status (deactivate)" {
    $body = @{ bankAccountId = $newBankId; isActive = $false; updatedBy = 1 } | ConvertTo-Json
    Invoke-RestMethod "$base/bankaccounts/$newBankId/status" -Method Put -Headers $h -Body $body
    Write-Host "  Status updated" -F Cyan
}

# ─── 5. BANK TRANSACTIONS ───
$bankTxn = Test "POST /bank-transactions (deposit)" {
    $body = @{
        bankAccountId = 1
        txnType = "Deposit"
        amount = 10000.00
        reference = "DEP-$ts"
        remarks = "Test deposit"
        createdBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/bank-transactions" -Method Post -Headers $h -Body $body
    Write-Host "  Deposit TxnId=$($r.bankTxnId) Amount=$($r.amount)" -F Cyan
    $r
}

Test "POST /bank-transactions (withdrawal)" {
    $body = @{
        bankAccountId = 1
        txnType = "Withdrawal"
        amount = 2000.00
        reference = "WDR-$ts"
        remarks = "Test withdrawal"
        createdBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/bank-transactions" -Method Post -Headers $h -Body $body
    Write-Host "  Withdrawal TxnId=$($r.bankTxnId) Amount=$($r.amount)" -F Cyan
}

Test "GET /bank-transactions/by-account/1" {
    $from = (Get-Date).AddMonths(-1).ToString("yyyy-MM-dd")
    $to   = (Get-Date).AddDays(1).ToString("yyyy-MM-dd")
    $r = Invoke-RestMethod "$base/bank-transactions/by-account/1?from=$from&to=$to" -Headers $h
    Write-Host "  Found $($r.Count) bank transactions" -F Cyan
}

# ─── 6. CHEQUES ───
$chqNum = "CHQ-$ts-001"
$cheque = Test "POST /cheques (issue)" {
    $body = @{
        bankAccountId = 1
        chequeNumber = $chqNum
        payeeName = "John Doe"
        amount = 3000.00
        chequeDate = "2026-04-15"
        reference = "INV-$ts"
        issuedBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/cheques" -Method Post -Headers $h -Body $body
    Write-Host "  Issued ChequeId=$($r.chequeId) Number=$($r.chequeNumber)" -F Cyan
    $r
}

$chequeId = $cheque.chequeId

Test "GET /cheques/by-account/1" {
    $r = Invoke-RestMethod "$base/cheques/by-account/1" -Headers $h
    Write-Host "  Found $($r.Count) cheques" -F Cyan
}

if ($chequeId) {
    Test "GET /cheques/$chequeId" {
        $r = Invoke-RestMethod "$base/cheques/$chequeId" -Headers $h
        Write-Host "  Cheque: $($r.chequeNumber) Status=$($r.status) Amount=$($r.chequeAmount)" -F Cyan
    }

    Test "PUT /cheques/$chequeId/clear" {
        $body = @{ chequeId = $chequeId; processedBy = 1 } | ConvertTo-Json
        Invoke-RestMethod "$base/cheques/$chequeId/clear" -Method Put -Headers $h -Body $body
        Write-Host "  Cheque cleared" -F Cyan
    }
}

# Issue 2nd cheque to test bounce
$chqNum2 = "CHQ-$ts-002"
$cheque2 = Test "POST /cheques (issue 2nd)" {
    $body = @{
        bankAccountId = 1
        chequeNumber = $chqNum2
        payeeName = "Jane Smith"
        amount = 1500.00
        chequeDate = "2026-04-20"
        reference = "INV2-$ts"
        issuedBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/cheques" -Method Post -Headers $h -Body $body
    Write-Host "  Issued ChequeId=$($r.chequeId)" -F Cyan
    $r
}

$cheque2Id = $cheque2.chequeId
if ($cheque2Id) {
    Test "PUT /cheques/$cheque2Id/bounce" {
        $body = @{ chequeId = $cheque2Id; bounceReason = "Insufficient funds"; processedBy = 1 } | ConvertTo-Json
        Invoke-RestMethod "$base/cheques/$cheque2Id/bounce" -Method Put -Headers $h -Body $body
        Write-Host "  Cheque bounced" -F Cyan
    }
}

# Issue 3rd cheque to test cancel
$chqNum3 = "CHQ-$ts-003"
$cheque3 = Test "POST /cheques (issue 3rd)" {
    $body = @{
        bankAccountId = 1
        chequeNumber = $chqNum3
        payeeName = "Bob Builder"
        amount = 750.00
        chequeDate = "2026-04-25"
        reference = "INV3-$ts"
        issuedBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/cheques" -Method Post -Headers $h -Body $body
    Write-Host "  Issued ChequeId=$($r.chequeId)" -F Cyan
    $r
}

$cheque3Id = $cheque3.chequeId
if ($cheque3Id) {
    Test "PUT /cheques/$cheque3Id/cancel" {
        $body = @{ chequeId = $cheque3Id; processedBy = 1 } | ConvertTo-Json
        Invoke-RestMethod "$base/cheques/$cheque3Id/cancel" -Method Put -Headers $h -Body $body
        Write-Host "  Cheque cancelled" -F Cyan
    }
}

# ─── 7. RECONCILIATION ───
$recon = Test "POST /reconciliation" {
    $body = @{
        bankAccountId = 1
        bankStatementBalance = 8000.00
        reconciliationDate = "2026-03-27"
        createdBy = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/reconciliation" -Method Post -Headers $h -Body $body
    Write-Host "  ReconId=$($r.reconId) Status=$($r.status) Diff=$($r.differenceAmount)" -F Cyan
    $r
}

Test "GET /reconciliation/by-account/1" {
    $r = Invoke-RestMethod "$base/reconciliation/by-account/1" -Headers $h
    Write-Host "  Found $($r.Count) reconciliations" -F Cyan
}

if ($recon) {
    Test "GET /reconciliation/$($recon.reconId)" {
        $r = Invoke-RestMethod "$base/reconciliation/$($recon.reconId)" -Headers $h
        Write-Host "  Recon: Status=$($r.status) LedgerBal=$($r.ledgerBalance) StmtBal=$($r.bankStatementBalance)" -F Cyan
    }
}

# ─── 8. MINIMAL API ENDPOINTS ───
Test "GET /minimal/cash-units" {
    $r = Invoke-RestMethod "$base/minimal/cash-units" -Headers $h
    Write-Host "  Minimal cash units: $($r.Count)" -F Cyan
}

Test "GET /minimal/cash-units/1/balance" {
    $r = Invoke-RestMethod "$base/minimal/cash-units/1/balance" -Headers $h
    Write-Host "  Minimal balance: $($r.balance)" -F Cyan
}

Test "GET /minimal/bank-accounts" {
    $r = Invoke-RestMethod "$base/minimal/bank-accounts" -Headers $h
    Write-Host "  Minimal bank accounts: $($r.Count)" -F Cyan
}

Test "GET /minimal/bank-accounts/1/balance" {
    $r = Invoke-RestMethod "$base/minimal/bank-accounts/1/balance" -Headers $h
    Write-Host "  Minimal bank balance: $($r.balance)" -F Cyan
}

# ─── 9. GRAPHQL QUERIES ───
Write-Host "`n--- GraphQL Queries ---" -F Yellow

function GQL($query) {
    $body = @{ query = $query } | ConvertTo-Json -Depth 5
    $r = Invoke-RestMethod $gql -Method Post -Headers $h -Body $body
    if ($r.errors) { throw ($r.errors | ConvertTo-Json -Compress) }
    $r.data
}

Test "GQL: cashUnits" {
    $d = GQL '{ cashUnits { cashUnitId name code status currentBalance } }'
    Write-Host "  Cash Units: $($d.cashUnits.Count)" -F Cyan
}

Test "GQL: cashUnit(id:1)" {
    $d = GQL '{ cashUnit(id: 1) { cashUnitId name code location status } }'
    Write-Host "  Unit: $($d.cashUnit.name)" -F Cyan
}

Test "GQL: cashInHand" {
    $d = GQL '{ cashInHand(cashUnitId: 1) { cashUnitId unitName balance } }'
    Write-Host "  Cash in hand: $($d.cashInHand.balance)" -F Cyan
}

Test "GQL: cashTransactions" {
    $d = GQL '{ cashTransactions(cashUnitId: 1, from: "2025-01-01T00:00:00.000Z", to: "2027-01-01T00:00:00.000Z") { cashTxnId txnType amount } }'
    Write-Host "  Transactions: $($d.cashTransactions.Count)" -F Cyan
}

Test "GQL: bankAccounts" {
    $d = GQL '{ bankAccounts { bankAccountId bankName accountNo status } }'
    Write-Host "  Bank accounts: $($d.bankAccounts.Count)" -F Cyan
}

Test "GQL: bankAccount(id:1)" {
    $d = GQL '{ bankAccount(id: 1) { bankAccountId bankName accountNo branch } }'
    Write-Host "  Bank: $($d.bankAccount.bankName)" -F Cyan
}

Test "GQL: bankBalance" {
    $d = GQL '{ bankBalance(bankAccountId: 1) { bankAccountId bankName balance } }'
    Write-Host "  Bank balance: $($d.bankBalance.balance)" -F Cyan
}

Test "GQL: bankTransactions" {
    $d = GQL '{ bankTransactions(bankAccountId: 1, from: "2025-01-01T00:00:00.000Z", to: "2027-01-01T00:00:00.000Z") { bankTxnId txnType amount } }'
    Write-Host "  Bank transactions: $($d.bankTransactions.Count)" -F Cyan
}

Test "GQL: cheques" {
    $d = GQL '{ cheques(bankAccountId: 1) { chequeId chequeNumber payeeName chequeAmount status } }'
    Write-Host "  Cheques: $($d.cheques.Count)" -F Cyan
}

if ($chequeId) {
    Test "GQL: cheque(id)" {
        $d = GQL "{ cheque(id: $chequeId) { chequeId chequeNumber payeeName status chequeAmount } }"
        Write-Host "  Cheque: $($d.cheque.chequeNumber) Status=$($d.cheque.status)" -F Cyan
    }
}

Test "GQL: reconciliations" {
    $d = GQL '{ reconciliations(bankAccountId: 1) { reconId bankStatementBalance ledgerBalance status } }'
    Write-Host "  Reconciliations: $($d.reconciliations.Count)" -F Cyan
}

if ($recon) {
    Test "GQL: reconciliation(id)" {
        $d = GQL "{ reconciliation(id: $($recon.reconId)) { reconId bankStatementBalance ledgerBalance differenceAmount status } }"
        Write-Host "  Recon: ID=$($d.reconciliation.reconId) Status=$($d.reconciliation.status)" -F Cyan
    }
}

# ─── 10. GRAPHQL MUTATIONS ───
Write-Host "`n--- GraphQL Mutations ---" -F Yellow

$gqlCashUnitId = 7000 + ($ts % 1000)
Test "GQL Mutation: createCashUnit" {
    $d = GQL "mutation { createCashUnit(input: { cashUnitId: $gqlCashUnitId, name: ""GQL Cash Unit"", code: ""GQL-$ts"", location: ""GQL Test"", openingBalance: 2000, createdBy: 1 }) { cashUnitId name code status } }"
    Write-Host "  Created: $($d.createCashUnit.name) ID=$($d.createCashUnit.cashUnitId)" -F Cyan
}

Test "GQL Mutation: recordCashReceipt" {
    $d = GQL 'mutation { recordCashReceipt(input: { cashUnitId: 1, amount: 800, source: "GQL Receipt", refNo: "GQL-R-001", remarks: "GraphQL test", createdBy: 1 }) { cashTxnId txnType amount } }'
    Write-Host "  Receipt: TxnId=$($d.recordCashReceipt.cashTxnId) Amount=$($d.recordCashReceipt.amount)" -F Cyan
}

Test "GQL Mutation: recordCashDisbursement" {
    $d = GQL 'mutation { recordCashDisbursement(input: { cashUnitId: 1, amount: 200, source: "GQL Disbursement", refNo: "GQL-D-001", remarks: "GraphQL test", createdBy: 1 }) { cashTxnId txnType amount } }'
    Write-Host "  Disbursement: TxnId=$($d.recordCashDisbursement.cashTxnId) Amount=$($d.recordCashDisbursement.amount)" -F Cyan
}

$gqlBankId = 6000 + ($ts % 1000)
Test "GQL Mutation: createBankAccount" {
    $d = GQL "mutation { createBankAccount(input: { bankAccountId: $gqlBankId, bankName: ""GQL Bank"", accountNo: ""GQL-$ts"", branch: ""GQL Branch"", accountType: ""Checking"", createdBy: 1 }) { bankAccountId bankName accountNo status } }"
    Write-Host "  Created: $($d.createBankAccount.bankName) ID=$($d.createBankAccount.bankAccountId)" -F Cyan
}

Test "GQL Mutation: recordBankTransaction" {
    $d = GQL 'mutation { recordBankTransaction(input: { bankAccountId: 1, txnType: DEPOSIT, amount: 5000, reference: "GQL-DEP", remarks: "GraphQL test", createdBy: 1 }) { bankTxnId txnType amount } }'
    Write-Host "  Deposit: TxnId=$($d.recordBankTransaction.bankTxnId) Amount=$($d.recordBankTransaction.amount)" -F Cyan
}

$gqlChqNum = "GQL-CHQ-$ts"
Test "GQL Mutation: issueCheque" {
    $d = GQL "mutation { issueCheque(input: { bankAccountId: 1, chequeNumber: ""$gqlChqNum"", payeeName: ""GQL Payee"", amount: 1200, chequeDate: ""2026-05-01"", reference: ""GQL-INV"", issuedBy: 1 }) { chequeId chequeNumber payeeName chequeAmount status } }"
    Write-Host "  Issued: $($d.issueCheque.chequeNumber) ID=$($d.issueCheque.chequeId) Status=$($d.issueCheque.status)" -F Cyan
    $script:gqlChequeId = $d.issueCheque.chequeId
}

if ($gqlChequeId) {
    Test "GQL Mutation: markChequeCleared" {
        $d = GQL "mutation { markChequeCleared(chequeId: $gqlChequeId, processedBy: 1) }"
        Write-Host "  Cleared: $($d.markChequeCleared)" -F Cyan
    }
}

Test "GQL Mutation: performReconciliation" {
    $d = GQL 'mutation { performReconciliation(input: { bankAccountId: 1, bankStatementBalance: 15000, reconciliationDate: "2026-03-27", createdBy: 1 }) { reconId bankStatementBalance ledgerBalance differenceAmount status } }'
    Write-Host "  Recon: ID=$($d.performReconciliation.reconId) Status=$($d.performReconciliation.status)" -F Cyan
}

# Issue a cheque for GQL bounce test
$gqlChqBounce = "GQL-BNC-$ts"
$gqlBounceCheque = Test "GQL Mutation: issueCheque (for bounce)" {
    $d = GQL "mutation { issueCheque(input: { bankAccountId: 1, chequeNumber: ""$gqlChqBounce"", payeeName: ""GQL Bounce Test"", amount: 600, chequeDate: ""2026-06-01"", reference: ""GQL-BNC"", issuedBy: 1 }) { chequeId chequeNumber status } }"
    Write-Host "  Issued for bounce: $($d.issueCheque.chequeNumber) ID=$($d.issueCheque.chequeId)" -F Cyan
    $d.issueCheque
}

if ($gqlBounceCheque) {
    Test "GQL Mutation: markChequeBounced" {
        $d = GQL "mutation { markChequeBounced(chequeId: $($gqlBounceCheque.chequeId), bounceReason: ""Insufficient funds"", processedBy: 1) }"
        Write-Host "  Bounced: $($d.markChequeBounced)" -F Cyan
    }
}

# Issue a cheque for GQL cancel test
$gqlChqCancel = "GQL-CXL-$ts"
$gqlCancelCheque = Test "GQL Mutation: issueCheque (for cancel)" {
    $d = GQL "mutation { issueCheque(input: { bankAccountId: 1, chequeNumber: ""$gqlChqCancel"", payeeName: ""GQL Cancel Test"", amount: 400, chequeDate: ""2026-06-15"", reference: ""GQL-CXL"", issuedBy: 1 }) { chequeId chequeNumber status } }"
    Write-Host "  Issued for cancel: $($d.issueCheque.chequeNumber) ID=$($d.issueCheque.chequeId)" -F Cyan
    $d.issueCheque
}

if ($gqlCancelCheque) {
    Test "GQL Mutation: cancelCheque" {
        $d = GQL "mutation { cancelCheque(chequeId: $($gqlCancelCheque.chequeId), processedBy: 1) }"
        Write-Host "  Cancelled: $($d.cancelCheque)" -F Cyan
    }
}

Test "GQL Mutation: updateCashUnitStatus" {
    $d = GQL "mutation { updateCashUnitStatus(cashUnitId: $gqlCashUnitId, isActive: false, updatedBy: 1) }"
    Write-Host "  Updated cash unit status: $($d.updateCashUnitStatus)" -F Cyan
}

Test "GQL Mutation: updateBankAccountStatus" {
    $d = GQL "mutation { updateBankAccountStatus(bankAccountId: $gqlBankId, isActive: false, updatedBy: 1) }"
    Write-Host "  Updated bank account status: $($d.updateBankAccountStatus)" -F Cyan
}

# ─── 11. HEALTH CHECKS ───
Test "GET /health" {
    $r = Invoke-RestMethod "http://localhost:5249/health"
    Write-Host "  Health: $($r.status)" -F Cyan
}

# ─── 12. DOMAIN EVENTS + RABBITMQ ───
Write-Host "`n--- Domain Events & RabbitMQ ---" -F Yellow
Write-Host "Domain events dispatched via MediatR -> Event Handlers log + publish to RabbitMQ (graceful degradation)." -F Cyan
Write-Host "Events published: cashunit.created, cash.receipt.recorded, cash.disbursement.recorded," -F Cyan
Write-Host "  bankaccount.created, bank.transaction.recorded, cheque.issued, cheque.bounced, bank.reconciled" -F Cyan

# ─── SUMMARY ───
Write-Host "`n========================================" -F White
Write-Host " RESULTS: $pass PASSED, $fail FAILED" -F $(if ($fail -eq 0) { "Green" } else { "Red" })
Write-Host "========================================" -F White
$results | ForEach-Object { Write-Host $_ -F $(if ($_ -match "^PASS") { "Green" } else { "Red" }) }
