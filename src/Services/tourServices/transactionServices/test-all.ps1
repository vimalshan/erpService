$base = "http://localhost:5192"
$pass = 0; $fail = 0; $results = @()

function Test-Endpoint {
    param([string]$Name, [string]$Method, [string]$Url, [string]$Body, [int[]]$ExpectedCodes, [hashtable]$Headers)
    try {
        $params = @{ Uri = $Url; Method = $Method; Headers = $Headers; ErrorAction = 'Stop'; UseBasicParsing = $true }
        if ($Body) { $params.Body = $Body; $params.ContentType = 'application/json' }
        $r = Invoke-WebRequest @params
        $code = [int]$r.StatusCode
        $ok = $ExpectedCodes -contains $code
        $bodySnip = $r.Content.Substring(0, [Math]::Min(120, $r.Content.Length))
    } catch {
        if ($_.Exception.Response) {
            $code = [int]$_.Exception.Response.StatusCode
            try { $sr = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream()); $bodySnip = $sr.ReadToEnd().Substring(0,  [Math]::Min(120, 9999)); $sr.Close() } catch { $bodySnip = "err-read" }
        } else {
            $code = 999
            $bodySnip = $_.Exception.Message.Substring(0, [Math]::Min(80, $_.Exception.Message.Length))
        }
        $ok = $ExpectedCodes -contains $code
    }
    $status = if ($ok) { "PASS" } else { "FAIL" }
    $script:results += [PSCustomObject]@{Status=$status; Test=$Name; Code=$code}
    if ($ok) { $script:pass++ } else { $script:fail++ }
    Write-Host "  $status [$code] $Name"
}

# --- AUTH ---
Write-Host "`n=== AUTHENTICATION ===" -ForegroundColor Cyan
$tokenResp = Invoke-RestMethod -Uri "$base/api/auth/token" -Method POST -ContentType 'application/json' -Body '{"username":"admin","password":"Admin@123!"}'
$token = $tokenResp.token
$h = @{Authorization="Bearer $token"}
Write-Host "Token OK (length=$($token.Length))"

# --- INFRASTRUCTURE ---
Write-Host "`n=== INFRASTRUCTURE ===" -ForegroundColor Cyan
Test-Endpoint "Health" GET "$base/health" $null @(200,503) @{}
Test-Endpoint "Health/Ready" GET "$base/health/ready" $null @(200,503) @{}
Test-Endpoint "Swagger" GET "$base/swagger/v1/swagger.json" $null @(200) @{}
Test-Endpoint "Auth - valid creds" POST "$base/api/auth/token" '{"username":"admin","password":"Admin@123!"}' @(200) @{}
Test-Endpoint "Auth - bad creds" POST "$base/api/auth/token" '{"username":"foo","password":"bar"}' @(401) @{}
Test-Endpoint "Unauthorized access" GET "$base/api/EmployeeJournalVouchers" $null @(401) @{}

# --- EMPLOYEE JV CONTROLLER ---
Write-Host "`n=== EMPLOYEE JV CONTROLLER ===" -ForegroundColor Cyan
Test-Endpoint "GET /EmployeeJournalVouchers" GET "$base/api/EmployeeJournalVouchers" $null @(200) $h
Test-Endpoint "GET /EmployeeJournalVouchers?page=1" GET "$base/api/EmployeeJournalVouchers?page=1&pageSize=5" $null @(200) $h
Test-Endpoint "GET /EmployeeJournalVouchers/99999 (not found)" GET "$base/api/EmployeeJournalVouchers/99999" $null @(404) $h

# --- SUPPLIER JV CONTROLLER ---
Write-Host "`n=== SUPPLIER JV CONTROLLER ===" -ForegroundColor Cyan
Test-Endpoint "GET /SupplierJournalVouchers" GET "$base/api/SupplierJournalVouchers" $null @(200) $h
Test-Endpoint "GET /SupplierJournalVouchers?page=1" GET "$base/api/SupplierJournalVouchers?page=1&pageSize=5" $null @(200) $h
Test-Endpoint "GET /SupplierJournalVouchers/99999 (not found)" GET "$base/api/SupplierJournalVouchers/99999" $null @(404) $h

# --- TRAVEL BATCHES CONTROLLER ---
Write-Host "`n=== TRAVEL BATCHES CONTROLLER ===" -ForegroundColor Cyan
Test-Endpoint "GET /TravelBatches" GET "$base/api/TravelBatches" $null @(200) $h
Test-Endpoint "GET /TravelBatches?status=C" GET "$base/api/TravelBatches?status=C" $null @(200) $h
Test-Endpoint "GET /TravelBatches/NOTEXIST (not found)" GET "$base/api/TravelBatches/NOTEXIST" $null @(404) $h

# --- EMPLOYEE PAYMENTS ---
Write-Host "`n=== EMPLOYEE PAYMENTS CONTROLLER ===" -ForegroundColor Cyan
Test-Endpoint "GET /EmployeePayments/99999 (not found)" GET "$base/api/EmployeePayments/99999" $null @(404) $h
Test-Endpoint "GET /EmployeePayments/by-employee/99999" GET "$base/api/EmployeePayments/by-employee/99999" $null @(200) $h

# --- AIRLINE INVOICES ---
Write-Host "`n=== AIRLINE INVOICES CONTROLLER ===" -ForegroundColor Cyan
Test-Endpoint "GET /AirlineInvoices/NOTEXIST (not found)" GET "$base/api/AirlineInvoices/NOTEXIST" $null @(404) $h
Test-Endpoint "GET /AirlineInvoices/by-booking/NOTEXIST" GET "$base/api/AirlineInvoices/by-booking/NOTEXIST" $null @(200) $h

# --- V2 MINIMAL API ENDPOINTS ---
Write-Host "`n=== V2 MINIMAL API ===" -ForegroundColor Cyan
Test-Endpoint "GET /v2/transactions/employee-jvs" GET "$base/api/v2/transactions/employee-jvs" $null @(200) $h
Test-Endpoint "GET /v2/transactions/employee-jvs/99999 (not found)" GET "$base/api/v2/transactions/employee-jvs/99999" $null @(404) $h
Test-Endpoint "GET /v2/transactions/supplier-jvs" GET "$base/api/v2/transactions/supplier-jvs" $null @(200) $h
Test-Endpoint "GET /v2/transactions/supplier-jvs/99999 (not found)" GET "$base/api/v2/transactions/supplier-jvs/99999" $null @(404) $h
Test-Endpoint "GET /v2/transactions/travel-batches" GET "$base/api/v2/transactions/travel-batches" $null @(200) $h
Test-Endpoint "GET /v2/transactions/travel-batches/NOTEXIST (not found)" GET "$base/api/v2/transactions/travel-batches/NOTEXIST" $null @(404) $h
Test-Endpoint "GET /v2/transactions/employee-payments/99999 (not found)" GET "$base/api/v2/transactions/employee-payments/99999" $null @(404) $h
Test-Endpoint "GET /v2/transactions/airline-invoices/NOTEXIST (not found)" GET "$base/api/v2/transactions/airline-invoices/NOTEXIST" $null @(404) $h

# --- CREATE OPERATIONS (POST) ---
Write-Host "`n=== CREATE OPERATIONS ===" -ForegroundColor Cyan
$ts = [DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds()

$empJvBody = @{
    jvBatchId = $ts
    jvTpId = 1
    jvType = "INV"
    jvDate = "2026-04-11"
    jvEmpSysId = 9001
    jvTrnType = "ADV"
    jvNetAmt = 10000
    jvPayUnitId = 1
    createdBy = 1
    lines = @(@{ jvSubId = ($ts + 1); jvBu = "BU01"; jvAcCode = "AC01"; jvSubAcc = "SA01"; jvCcCode = "CC01"; jvProduct = "P01"; jvDcFlag = "D"; jvTrnAmt = "10000"; jvIutaBu = "IB01"; jvLoc = "L01"; jvRemarks = "Test"; jvLineFlag = "Y"; jvSubType = "ADV" })
} | ConvertTo-Json -Depth 3
Test-Endpoint "POST /EmployeeJournalVouchers (create)" POST "$base/api/EmployeeJournalVouchers" $empJvBody @(200,201) $h

$supJvBody = @{
    jvId = ($ts + 2)
    jvType = "INV"
    jvDate = "2026-04-11"
    jvVendorId = 1001
    jvPayUnitId = 1
    jvRefInvNo = "REF001"
    jvNetAmt = 20000
    jvTrnType = "EXP"
    jvOraVendorId = 1
    jvAdminId = 1
    jvInvBatchId = 1
    jvOraSiteId = 1
    jvCenvatApplicable = "N"
    jvDocKeyNo = "DOC001"
    createdBy = 1
    lines = @(@{ jvSubId = ($ts + 3); jvBu = "BU01"; jvAcCode = "AC01"; jvSubAcc = "SA01"; jvCcCode = "CC01"; jvProduct = "P01"; jvDcFlag = "C"; jvTrnAmt = 20000; jvLoc = "L01"; jvRemarks = "Test"; jvLineFlag = "Y"; jvCombinationId = "COMB01"; jvSubType = "EXP"; jvIutaBu = "IB01"; jvTpId = 1; jvBatchSubId = 1 })
} | ConvertTo-Json -Depth 3
Test-Endpoint "POST /SupplierJournalVouchers (create)" POST "$base/api/SupplierJournalVouchers" $supJvBody @(200,201) $h

$batchBody = @{
    batchId = "BATCH$ts"
    adminId = "ADM001"
    payUnitId = "PU001"
    vendorId = "V001"
    batchType = "T"
    createdBy = "TESTER"
    subItems = @(@{ batchSubId = "SUB$ts"; creditType = "D"; bookCnfId = "BK001"; basAmt = "5000"; totAmt = "5000"; appAmt = "5000" })
} | ConvertTo-Json -Depth 3
Test-Endpoint "POST /TravelBatches (create)" POST "$base/api/TravelBatches" $batchBody @(200,201) $h

$payBody = @{
    payId = ($ts + 4)
    payTpId = 1
    payTrnType = "ADV"
    payEmpSysId = 9001
    payUnitId = 1
    payMode = "CHQ"
    payType = "ADV"
    payAmount = 5000
    payRefId = 1
    payBatchId = 1
    payJvId = 1
    createdBy = 1
} | ConvertTo-Json
Test-Endpoint "POST /EmployeePayments (create)" POST "$base/api/EmployeePayments" $payBody @(200,201) $h

$invBody = @{
    airTicketId = "AIR$ts"
    bookCnfId = "BK001"
    ticketNumber = "TKT$ts"
    airlineVendorId = "AIRV001"
    invoiceNumber = "INV$ts"
    invoiceDate = "2026-04-11"
    invoiceCost = "15000"
    enteredBy = "TESTER"
} | ConvertTo-Json
Test-Endpoint "POST /AirlineInvoices (create)" POST "$base/api/AirlineInvoices" $invBody @(200,201) $h

# --- RESULTS ---
Write-Host "`n========================================" -ForegroundColor Yellow
Write-Host "  CONTROLLER TEST RESULTS" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow
$results | Format-Table -AutoSize -Property Status, Code, Test -Wrap
Write-Host "TOTAL: $($pass + $fail) | PASS: $pass | FAIL: $fail" -ForegroundColor $(if ($fail -eq 0) { "Green" } else { "Red" })
