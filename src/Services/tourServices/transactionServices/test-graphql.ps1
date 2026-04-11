$token = (Invoke-RestMethod -Uri http://localhost:5192/api/auth/token -Method POST -ContentType 'application/json' -Body '{"username":"admin","password":"Admin@123!"}').token
$h = @{ Authorization = "Bearer $token"; "Content-Type" = "application/json" }

$pass = 0; $fail = 0; $results = @()

function Test-GQL($name, $query, $expectNotFound=$false, $expectDomainError=$false) {
    $body = @{ query = $query } | ConvertTo-Json -Depth 5 -Compress
    try {
        $r = Invoke-WebRequest -Uri http://localhost:5192/graphql -Method POST -Headers $h -Body $body -UseBasicParsing
        $json = [System.Text.Encoding]::UTF8.GetString($r.Content) | ConvertFrom-Json
        if ($json.errors) {
            if ($expectNotFound -and ($json.errors[0].extensions.code -eq "NOT_FOUND" -or $json.errors[0].message -match "not found")) {
                $script:pass++
                Write-Host "  PASS $name (not found - expected)"
                $script:results += [PSCustomObject]@{Status="PASS";Test=$name;Error=""}
            } elseif ($expectDomainError) {
                $script:pass++
                Write-Host "  PASS $name (domain error - expected: $($json.errors[0].message))"
                $script:results += [PSCustomObject]@{Status="PASS";Test=$name;Error=""}
            } else {
                $script:fail++
                $msg = $json.errors[0].message
                Write-Host "  FAIL $name - $msg"
                $script:results += [PSCustomObject]@{Status="FAIL";Test=$name;Error=$msg}
            }
        } else {
            $script:pass++
            Write-Host "  PASS $name"
            $script:results += [PSCustomObject]@{Status="PASS";Test=$name;Error=""}
        }
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        try {
            $sr = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
            $errBody = $sr.ReadToEnd(); $sr.Close()
            $errJson = $errBody | ConvertFrom-Json
            if ($expectNotFound -and ($errJson.errors[0].extensions.code -eq "NOT_FOUND" -or $errJson.errors[0].message -match "not found")) {
                $script:pass++
                Write-Host "  PASS $name (not found - expected)"
                $script:results += [PSCustomObject]@{Status="PASS";Test=$name;Error=""}
                return
            }
            $msg = "HTTP $code - $($errJson.errors[0].message)"
        } catch { $msg = "HTTP $code - $($_.Exception.Message)" }
        $script:fail++
        Write-Host "  FAIL $name - $msg"
        $script:results += [PSCustomObject]@{Status="FAIL";Test=$name;Error=$msg}
    }
}

Write-Host "`n=== GRAPHQL QUERIES ==="

# List queries
Test-GQL "employeeJVs (list)" '{ employeeJVs(page:1,pageSize:5) { jvBatchId jvType jvDate jvNetAmt } }'
Test-GQL "supplierJVs (list)" '{ supplierJVs(page:1,pageSize:5) { jvId jvType jvDate jvNetAmt } }'
Test-GQL "travelBatches (list)" '{ travelBatches(page:1,pageSize:5) { batchId adminId batchDate status } }'
Test-GQL "employeePaymentsByEmployee" '{ employeePaymentsByEmployee(empSysId:99999) { payId payAmount } }'
Test-GQL "airlineInvoicesByBooking" '{ airlineInvoicesByBooking(bookCnfId:"NOTEXIST") { airTicketId ticketNumber } }'

# By-ID queries (expect not-found error)
Test-GQL "employeeJV (not found)" '{ employeeJV(jvBatchId:99999) { jvBatchId jvType } }' $true
Test-GQL "supplierJV (not found)" '{ supplierJV(jvId:99999) { jvId jvType } }' $true
Test-GQL "travelBatch (not found)" '{ travelBatch(batchId:"NOTEXIST") { batchId status } }' $true
Test-GQL "employeePayment (not found)" '{ employeePayment(payId:99999) { payId payAmount } }' $true
Test-GQL "airlineInvoice (not found)" '{ airlineInvoice(airTicketId:"NOTEXIST") { airTicketId ticketNumber } }' $true

Write-Host "`n=== GRAPHQL MUTATIONS ==="

$ts = [DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds()

# Create Employee JV
$q = 'mutation { createEmployeeJV(input: { jvBatchId: ' + $ts + ', jvTpId: 1, jvType: "INV", jvDate: "2026-04-11T00:00:00Z", jvEmpSysId: 1001, jvTrnType: "JV", jvNetAmt: 500.00, jvPayUnitId: 1, createdBy: 1 }) { jvBatchId jvType jvNetAmt } }'
Test-GQL "createEmployeeJV" $q

# Create Supplier JV
$q = 'mutation { createSupplierJV(input: { jvId: ' + ($ts+1) + ', jvType: "INV", jvDate: "2026-04-11T00:00:00Z", jvVendorId: 2001, jvPayUnitId: 1, jvRefInvNo: "REFINV' + $ts + '", jvNetAmt: 1500.00, jvTrnType: "JV", jvOraVendorId: 3001, jvAdminId: 1, jvInvBatchId: 0, jvOraSiteId: 0, jvCenvatApplicable: "N", jvDocKeyNo: "DOC' + $ts + '", createdBy: 1 }) { jvId jvType jvNetAmt } }'
Test-GQL "createSupplierJV" $q

# Create Travel Batch
$q = 'mutation { createTravelBatch(input: { batchId: "GQL' + $ts + '", adminId: "ADM1", payUnitId: "PU1", vendorId: "V1", batchType: "N", createdBy: "1" }) { batchId status } }'
Test-GQL "createTravelBatch" $q

# Create Employee Payment
$q = 'mutation { createEmployeePayment(input: { payId: ' + ($ts+2) + ', payTpId: 1, payTrnType: "PAY", payEmpSysId: 1001, payUnitId: 1, payMode: "CHQ", payType: "SAL", payAmount: 2500.00, payRefId: 0, payBatchId: 0, payJvId: 0, createdBy: 1 }) { payId payAmount } }'
Test-GQL "createEmployeePayment" $q

# Create Airline Invoice
$q = 'mutation { createAirlineInvoice(input: { airTicketId: "GQL' + $ts + '", bookCnfId: "BK' + $ts + '", ticketNumber: "TKT' + $ts + '", airlineVendorId: "AV01", invoiceNumber: "INV' + $ts + '", invoiceDate: "2026-04-11T00:00:00Z", invoiceCost: "25000", enteredBy: "TESTER" }) { airTicketId ticketNumber invoiceCost } }'
Test-GQL "createAirlineInvoice" $q

# Workflow mutations - use data just created above
$q = 'mutation { postEmployeeJV(jvBatchId: ' + $ts + ', postedBy: 1) }'
Test-GQL "postEmployeeJV (no lines=expected)" $q $false $true

$q = 'mutation { adminApproveTravelBatch(batchId: "GQL' + $ts + '", approvedBy: "ADM1", remarks: "Test approved") }'
Test-GQL "adminApproveTravelBatch" $q

$q = 'mutation { postSupplierJV(jvId: ' + ($ts+1) + ', postedBy: 1) }'
Test-GQL "postSupplierJV (no lines=expected)" $q $false $true

Write-Host "`n========================================"
Write-Host "  GRAPHQL TEST RESULTS"
Write-Host "========================================"
$results | Format-Table -AutoSize
Write-Host "TOTAL: $($pass+$fail) | PASS: $pass | FAIL: $fail"
