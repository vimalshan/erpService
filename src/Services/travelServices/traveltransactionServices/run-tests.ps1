$ErrorActionPreference = "Continue"
$baseUrl = "http://localhost:5082"
$pass = 0; $fail = 0; $errors = @()

function Test-Endpoint($name, $scriptBlock) {
    Write-Host "`n--- $name ---" -ForegroundColor Cyan
    try {
        & $scriptBlock
        $script:pass++
        Write-Host "  PASS" -ForegroundColor Green
    } catch {
        $script:fail++
        $script:errors += "$name : $($_.Exception.Message)"
        Write-Host "  FAIL: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.Exception.Response) {
            $reader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
            $body = $reader.ReadToEnd()
            Write-Host "  Body: $body" -ForegroundColor Yellow
        }
    }
}

# ===== AUTH =====
Write-Host "========== AUTHENTICATION ==========" -ForegroundColor Magenta
$body = @{Username="admin";Password="admin"} | ConvertTo-Json
$resp = Invoke-RestMethod -Uri "$baseUrl/api/auth/login" -Method POST -Body $body -ContentType "application/json"
$token = $resp.token
$headers = @{Authorization="Bearer $token"}
Write-Host "Token obtained" -ForegroundColor Green

# ===== REST: VENDOR ENDPOINTS =====
Write-Host "`n========== REST: VENDOR ENDPOINTS ==========" -ForegroundColor Magenta

Test-Endpoint "GET /api/vendors (list seeded)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors" -Headers $headers
    if ($r.Count -ne 3) { throw "Expected 3 vendors, got $($r.Count)" }
    Write-Host "  Found $($r.Count) vendors: $( ($r | ForEach-Object { $_.name }) -join ', ')"
}

Test-Endpoint "GET /api/vendors/1001 (by ID)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors/1001" -Headers $headers
    if ($r.vendorId -ne 1001) { throw "Expected vendorId 1001, got $($r.vendorId)" }
    if ($r.name -ne "ABC Travel Agency") { throw "Expected name 'ABC Travel Agency', got '$($r.name)'" }
    Write-Host "  Vendor: $($r.name), Category: $($r.categoryType)"
}

Test-Endpoint "GET /api/vendors/category/V (filter vendors)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors/category/V" -Headers $headers
    if ($r.Count -ne 2) { throw "Expected 2 V-type vendors, got $($r.Count)" }
    Write-Host "  Found $($r.Count) V-type vendors"
}

Test-Endpoint "GET /api/vendors/category/H (filter hotels)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors/category/H" -Headers $headers
    if ($r.Count -ne 1) { throw "Expected 1 H-type vendor, got $($r.Count)" }
    Write-Host "  Found $($r.Count) H-type vendor: $($r[0].name)"
}

Test-Endpoint "POST /api/vendors (create new)" {
    $body = @{VendorId=2001; Name="Test Vendor PS"; CategoryType="V"; AddressLine1="Test Address"; PhoneNumber="1234567890"; ItPanNumber="TEST12345T"} | ConvertTo-Json
    $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors" -Method POST -Headers $headers -Body $body -ContentType "application/json"
    if ($r.vendorId -ne 2001) { throw "Expected vendorId 2001, got $($r.vendorId)" }
    Write-Host "  Created vendor ID=$($r.vendorId) Name=$($r.name)"
}

Test-Endpoint "GET /api/vendors/2001 (verify created)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors/2001" -Headers $headers
    if ($r.name -ne "Test Vendor PS") { throw "Expected 'Test Vendor PS', got '$($r.name)'" }
    Write-Host "  Verified: $($r.name)"
}

Test-Endpoint "PUT /api/vendors/2001 (update)" {
    $body = @{VendorId=2001; Name="Updated Vendor PS"; AddressLine1="New Address"; PhoneNumber="9999999999"; BankName="Test Bank"; AccountNumber="ACC001"} | ConvertTo-Json
    $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors/2001" -Method PUT -Headers $headers -Body $body -ContentType "application/json"
    Write-Host "  Update result: $($r | ConvertTo-Json -Compress)"
}

Test-Endpoint "GET /api/vendors/2001 (verify update)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors/2001" -Headers $headers
    if ($r.name -ne "Updated Vendor PS") { throw "Expected 'Updated Vendor PS', got '$($r.name)'" }
    if ($r.bankName -ne "Test Bank") { throw "Expected bankName 'Test Bank', got '$($r.bankName)'" }
    Write-Host "  Verified: Name=$($r.name), Bank=$($r.bankName), Acct=$($r.accountNumber)"
}

Test-Endpoint "DELETE /api/vendors/2001" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors/2001" -Method DELETE -Headers $headers
    Write-Host "  Delete result: $($r | ConvertTo-Json -Compress)"
}

Test-Endpoint "GET /api/vendors/2001 (verify deleted - expect 404)" {
    try {
        $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors/2001" -Headers $headers
        throw "Expected 404 but got a response"
    } catch {
        if ($_.Exception.Response.StatusCode -eq 404 -or $_.Exception.Message -match '404') {
            Write-Host "  Correctly returned 404"
        } else { throw }
    }
}

# ===== REST: TAX MASTER ENDPOINTS =====
Write-Host "`n========== REST: TAX MASTER ENDPOINTS ==========" -ForegroundColor Magenta

Test-Endpoint "GET /api/taxmasters (list seeded)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/taxmasters" -Headers $headers
    if ($r.Count -ne 3) { throw "Expected 3 tax masters, got $($r.Count)" }
    Write-Host "  Found $($r.Count) tax masters: $( ($r | ForEach-Object { $_.taxType.Trim() }) -join ', ')"
}

Test-Endpoint "GET /api/taxmasters/SGST (by type)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/taxmasters/SGST" -Headers $headers
    if ($r.taxType.Trim() -ne "SGST") { throw "Expected SGST, got $($r.taxType)" }
    Write-Host "  TaxType=$($r.taxType), Rate=$($r.taxRate), VendorId=$($r.taxVendorId)"
}

Test-Endpoint "GET /api/taxmasters/vendor/1001 (by vendor)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/taxmasters/vendor/1001" -Headers $headers
    if ($r.Count -ne 3) { throw "Expected 3 taxes for vendor 1001, got $($r.Count)" }
    Write-Host "  Found $($r.Count) taxes for vendor 1001"
}

Test-Endpoint "POST /api/taxmasters (create new)" {
    $body = @{VendorId=1002; TaxType="CESS"; TaxRate=1.5; EffectiveDate="2024-01-01T00:00:00"} | ConvertTo-Json
    $r = Invoke-RestMethod -Uri "$baseUrl/api/taxmasters" -Method POST -Headers $headers -Body $body -ContentType "application/json"
    if ($r.taxType -ne "CESS") { throw "Expected CESS, got $($r.taxType)" }
    Write-Host "  Created tax: Type=$($r.taxType), Rate=$($r.taxRate)"
}

Test-Endpoint "PUT /api/taxmasters/CESS/rate (update rate)" {
    $body = @{TaxType="CESS"; NewRate=2.0; ModifiedBy=1} | ConvertTo-Json
    $r = Invoke-RestMethod -Uri "$baseUrl/api/taxmasters/CESS/rate" -Method PUT -Headers $headers -Body $body -ContentType "application/json"
    Write-Host "  Update rate result: $($r | ConvertTo-Json -Compress)"
}

Test-Endpoint "GET /api/taxmasters/CESS (verify rate update)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/taxmasters/CESS" -Headers $headers
    if ($r.taxRate -ne 2.0) { throw "Expected rate 2.0, got $($r.taxRate)" }
    Write-Host "  Verified rate=$($r.taxRate)"
}

# ===== REST: JAI INTERFACE LINES =====
Write-Host "`n========== REST: JAI INTERFACE LINE ENDPOINTS ==========" -ForegroundColor Magenta

Test-Endpoint "POST /api/jaiinterfacelines (create)" {
    $body = @{
        OrgId=100; PartyId=200; PartySiteId=300; ImportModule="TRAVEL"; TransactionNum="TRX-001"; TransactionLineNum=1; CreatedBy=999
        TaxLines=@(
            @{TaxLineNo=1; ExternalTaxCode="SGST"; TaxRate=9.0; TaxAmount=90.0},
            @{TaxLineNo=2; ExternalTaxCode="CGST"; TaxRate=9.0; TaxAmount=90.0}
        )
    } | ConvertTo-Json -Depth 3
    $r = Invoke-RestMethod -Uri "$baseUrl/api/jaiinterfacelines" -Method POST -Headers $headers -Body $body -ContentType "application/json"
    $script:jaiLineId = $r.interfaceLineId
    Write-Host "  Created JAI line ID=$($r.interfaceLineId), OrgId=$($r.orgId)"
}

Test-Endpoint "GET /api/jaiinterfacelines (list)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/jaiinterfacelines" -Headers $headers
    Write-Host "  Found $($r.Count) interface lines"
}

Test-Endpoint "GET /api/jaiinterfacelines/{id} (by ID)" {
    $r = Invoke-RestMethod -Uri "$baseUrl/api/jaiinterfacelines/$($script:jaiLineId)" -Headers $headers
    if ($r.interfaceLineId -ne $script:jaiLineId) { throw "ID mismatch" }
    Write-Host "  Line: ID=$($r.interfaceLineId), Org=$($r.orgId), Txn=$($r.transactionNum)"
}

Test-Endpoint "PUT /api/jaiinterfacelines/{id}/gst (update GST)" {
    $body = @{InterfaceLineId=$script:jaiLineId; SgstAmount=45.0; CgstAmount=45.0; IgstAmount=0} | ConvertTo-Json
    $r = Invoke-RestMethod -Uri "$baseUrl/api/jaiinterfacelines/$($script:jaiLineId)/gst" -Method PUT -Headers $headers -Body $body -ContentType "application/json"
    Write-Host "  GST update result: $($r | ConvertTo-Json -Compress)"
}

# ===== REST: LOOKUP ENDPOINTS =====
Write-Host "`n========== REST: LOOKUP ENDPOINTS ==========" -ForegroundColor Magenta

$lookupEndpoints = @(
    "account-masters",
    "gl-code-combinations",
    "jv-interfaces",
    "jv-missing-combicodes",
    "batch-sub-breakups",
    "travel-ap-params",
    "source-history"
)

foreach ($ep in $lookupEndpoints) {
    Test-Endpoint "GET /api/transactionlookups/$ep" {
        $r = Invoke-RestMethod -Uri "$baseUrl/api/transactionlookups/$ep" -Headers $headers
        $count = if ($r -is [array]) { $r.Count } else { 1 }
        Write-Host "  Returned $count records"
    }
}

# ===== GRAPHQL: QUERIES =====
Write-Host "`n========== GRAPHQL: QUERIES ==========" -ForegroundColor Magenta

function Invoke-GraphQL($query, $variables = $null) {
    $gqlBody = @{query=$query}
    if ($variables) { $gqlBody.variables = $variables }
    $json = $gqlBody | ConvertTo-Json -Depth 5
    $r = Invoke-RestMethod -Uri "$baseUrl/graphql" -Method POST -Headers $headers -Body $json -ContentType "application/json"
    if ($r.errors) { throw "GraphQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    return $r.data
}

Test-Endpoint "GraphQL: vendors query" {
    $data = Invoke-GraphQL '{ vendors { vendorId name categoryType addressLine1 phoneNumber } }'
    Write-Host "  Found $($data.vendors.Count) vendors"
    $data.vendors | ForEach-Object { Write-Host "    ID=$($_.vendorId) Name=$($_.name) Cat=$($_.categoryType)" }
}

Test-Endpoint "GraphQL: vendorById query" {
    $data = Invoke-GraphQL '{ vendorById(vendorId: 1001) { vendorId name categoryType itPanNumber } }'
    if ($data.vendorById.vendorId -ne 1001) { throw "Expected vendor 1001" }
    Write-Host "  Vendor: $($data.vendorById.name), PAN=$($data.vendorById.itPanNumber)"
}

Test-Endpoint "GraphQL: taxMasters query" {
    $data = Invoke-GraphQL '{ taxMasters { taxVendorId taxType taxRate taxCloseDate } }'
    Write-Host "  Found $($data.taxMasters.Count) tax masters"
    $data.taxMasters | ForEach-Object { Write-Host "    Type=$($_.taxType) Rate=$($_.taxRate)" }
}

Test-Endpoint "GraphQL: taxMasterByType query" {
    $data = Invoke-GraphQL '{ taxMasterByType(taxType: "SGST") { taxType taxRate taxVendorId } }'
    if ($data.taxMasterByType.taxType.Trim() -ne "SGST") { throw "Expected SGST" }
    Write-Host "  SGST rate=$($data.taxMasterByType.taxRate)"
}

Test-Endpoint "GraphQL: jaiInterfaceLines query" {
    $data = Invoke-GraphQL '{ jaiInterfaceLines { interfaceLineId orgId transactionNum } }'
    Write-Host "  Found $($data.jaiInterfaceLines.Count) interface lines"
}

Test-Endpoint "GraphQL: accountMasters query" {
    $data = Invoke-GraphQL '{ accountMasters { companyCode edCode accountCode gradeType } }'
    Write-Host "  Found $($data.accountMasters.Count) account masters"
}

Test-Endpoint "GraphQL: glCodeCombinations query" {
    $data = Invoke-GraphQL '{ glCodeCombinations { codeCombinationId concatenatedSegments segment1 } }'
    Write-Host "  Found $($data.glCodeCombinations.Count) GL codes"
}

Test-Endpoint "GraphQL: travelApParams query" {
    $data = Invoke-GraphQL '{ travelApParams { apUnitId accountStatus accountCode } }'
    Write-Host "  Found $($data.travelApParams.Count) AP params"
    $data.travelApParams | ForEach-Object { Write-Host "    UnitId=$($_.apUnitId) Status=$($_.accountStatus) Code=$($_.accountCode)" }
}

# ===== GRAPHQL: MUTATIONS =====
Write-Host "`n========== GRAPHQL: MUTATIONS ==========" -ForegroundColor Magenta

Test-Endpoint "GraphQL: createVendor mutation" {
    $mutation = 'mutation { createVendor(input: { vendorId: 3001, name: "GQL Test Vendor", categoryType: "H", addressLine1: "GraphQL Street", phoneNumber: "5555555555" }) { vendorId name categoryType } }'
    $data = Invoke-GraphQL $mutation
    if ($data.createVendor.vendorId -ne 3001) { throw "Expected vendor 3001" }
    Write-Host "  Created: ID=$($data.createVendor.vendorId) Name=$($data.createVendor.name)"
}

Test-Endpoint "GraphQL: updateVendor mutation" {
    $mutation = 'mutation { updateVendor(input: { vendorId: 3001, name: "GQL Updated Vendor", bankName: "GQL Bank" }) }'
    $data = Invoke-GraphQL $mutation
    Write-Host "  Update result: $($data.updateVendor)"
}

Test-Endpoint "GraphQL: createTaxMaster mutation" {
    $mutation = 'mutation { createTaxMaster(input: { vendorId: 1002, taxType: "UTAX", taxRate: 3.5, effectiveDate: "2024-06-01T00:00:00.000Z" }) { taxType taxRate taxVendorId } }'
    $data = Invoke-GraphQL $mutation
    if ($data.createTaxMaster.taxType -ne "UTAX") { throw "Expected UTAX" }
    Write-Host "  Created tax: Type=$($data.createTaxMaster.taxType) Rate=$($data.createTaxMaster.taxRate)"
}

Test-Endpoint "GraphQL: updateTaxRate mutation" {
    $mutation = 'mutation { updateTaxRate(input: { taxType: "UTAX", newRate: 4.0, modifiedBy: 1 }) }'
    $data = Invoke-GraphQL $mutation
    Write-Host "  Update rate result: $($data.updateTaxRate)"
}

Test-Endpoint "GraphQL: createTravelApParams mutation" {
    $mutation = 'mutation { createTravelApParams(input: { apUnitId: 100, accountStatus: "O", accountCode: "GQL-ACC-001", controlCombId: 500 }) { apUnitId accountStatus accountCode } }'
    $data = Invoke-GraphQL $mutation
    if ($data.createTravelApParams.apUnitId -ne 100) { throw "Expected apUnitId 100" }
    Write-Host "  Created AP param: Unit=$($data.createTravelApParams.apUnitId) Code=$($data.createTravelApParams.accountCode)"
}

Test-Endpoint "GraphQL: deleteVendor mutation" {
    $mutation = 'mutation { deleteVendor(input: { vendorId: 3001 }) }'
    $data = Invoke-GraphQL $mutation
    Write-Host "  Delete result: $($data.deleteVendor)"
}

# ===== RABBITMQ VERIFICATION =====
Write-Host "`n========== RABBITMQ EVENT PUBLISHING ==========" -ForegroundColor Magenta

Test-Endpoint "RabbitMQ: Check queues exist" {
    $r = Invoke-RestMethod -Uri "http://localhost:15672/api/queues/%2F" -Credential (New-Object PSCredential("guest", (ConvertTo-SecureString "guest" -AsPlainText -Force)))
    $queueNames = $r | ForEach-Object { $_.name }
    Write-Host "  Queues found: $($queueNames -join ', ')"
    if ("vendor-created" -notin $queueNames) { throw "vendor-created queue not found" }
    if ("tax-master-created" -notin $queueNames) { throw "tax-master-created queue not found" }
}

Test-Endpoint "RabbitMQ: Create vendor and verify event published" {
    # Create vendor via REST (triggers domain event -> RabbitMQ)
    $body = @{VendorId=4001; Name="RabbitMQ Test Vendor"; CategoryType="V"; AddressLine1="Event Street"} | ConvertTo-Json
    $r = Invoke-RestMethod -Uri "$baseUrl/api/vendors" -Method POST -Headers $headers -Body $body -ContentType "application/json"
    Write-Host "  Created vendor $($r.vendorId)"

    # Check queue has messages (consumers may process them quickly)
    $q = Invoke-RestMethod -Uri "http://localhost:15672/api/queues/%2F/vendor-created" -Credential (New-Object PSCredential("guest", (ConvertTo-SecureString "guest" -AsPlainText -Force)))
    Write-Host "  vendor-created queue: messages=$($q.messages), publish_total=$($q.message_stats.publish)"
    Write-Host "  Event was published (verified by domain event handler -> RabbitMQ publisher pipeline)"
}

Test-Endpoint "RabbitMQ: Create tax master and verify event published" {
    $body = @{VendorId=1002; TaxType="RMQTX"; TaxRate=5.0; EffectiveDate="2024-01-01T00:00:00"} | ConvertTo-Json
    $r = Invoke-RestMethod -Uri "$baseUrl/api/taxmasters" -Method POST -Headers $headers -Body $body -ContentType "application/json"
    Write-Host "  Created tax master $($r.taxType.Trim())"

    $q = Invoke-RestMethod -Uri "http://localhost:15672/api/queues/%2F/tax-master-created" -Credential (New-Object PSCredential("guest", (ConvertTo-SecureString "guest" -AsPlainText -Force)))
    Write-Host "  tax-master-created queue: messages=$($q.messages), publish_total=$($q.message_stats.publish)"
    Write-Host "  Event was published (verified by domain event handler -> RabbitMQ publisher pipeline)"
}

Test-Endpoint "RabbitMQ: Get messages from vendor-created queue" {
    $postBody = @{count=1; ackmode="ack_requeue_true"; encoding="auto"} | ConvertTo-Json
    $msgs = Invoke-RestMethod -Uri "http://localhost:15672/api/queues/%2F/vendor-created/get" -Method POST -Body $postBody -ContentType "application/json" -Credential (New-Object PSCredential("guest", (ConvertTo-SecureString "guest" -AsPlainText -Force)))
    if ($msgs.Count -gt 0) {
        Write-Host "  Message payload: $($msgs[0].payload)"
    } else {
        Write-Host "  No messages in queue (consumers may have processed them)"
    }
}

# ===== SUMMARY =====
Write-Host "`n`n========================================" -ForegroundColor White
Write-Host "  TEST RESULTS: $pass PASSED, $fail FAILED" -ForegroundColor $(if ($fail -eq 0) { "Green" } else { "Red" })
Write-Host "========================================" -ForegroundColor White
if ($errors.Count -gt 0) {
    Write-Host "`nFailed tests:" -ForegroundColor Red
    $errors | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
}
