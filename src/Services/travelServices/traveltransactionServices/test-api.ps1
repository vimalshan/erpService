$base = "http://localhost:5082"
$headers = @{Authorization="Bearer $token"; "Content-Type"="application/json"}

function Test($label, $scriptBlock) {
    Write-Host "`n=== $label ===" -ForegroundColor Cyan
    try { & $scriptBlock } catch { Write-Host "ERROR: $_" -ForegroundColor Red }
}

# ---- VENDORS ----
Test "POST /api/vendors - Create Vendor 1" {
    $body = @{VendorId=0; Name="ABC Travel Agency"; CategoryType="V"; AddressLine1="123 Main Street"; PhoneNumber="9876543210"; ItPanNumber="ABCDE1234F"} | ConvertTo-Json
    $script:vendor1 = Invoke-RestMethod -Uri "$base/api/vendors" -Method POST -Body $body -Headers $headers
    $script:vendor1 | ConvertTo-Json -Depth 2
}

Test "POST /api/vendors - Create Vendor 2" {
    $body = @{VendorId=0; Name="XYZ Hotels"; CategoryType="H"; AddressLine1="456 Park Avenue"; PhoneNumber="9876543211"; ItPanNumber="XYZAB5678G"} | ConvertTo-Json
    $script:vendor2 = Invoke-RestMethod -Uri "$base/api/vendors" -Method POST -Body $body -Headers $headers
    $script:vendor2 | ConvertTo-Json -Depth 2
}

Test "GET /api/vendors - List All" {
    $resp = Invoke-RestMethod -Uri "$base/api/vendors" -Headers $headers
    Write-Host "Count: $($resp.Count)"
    $resp | ConvertTo-Json -Depth 3
}

Test "GET /api/vendors/{id} - Get By Id" {
    $id = $script:vendor1.vendorId
    $resp = Invoke-RestMethod -Uri "$base/api/vendors/$id" -Headers $headers
    $resp | ConvertTo-Json -Depth 2
}

Test "GET /api/vendors/category/V - Get By Category" {
    $resp = Invoke-RestMethod -Uri "$base/api/vendors/category/V" -Headers $headers
    $resp | ConvertTo-Json -Depth 3
}

Test "PUT /api/vendors/{id} - Update Vendor" {
    $id = $script:vendor1.vendorId
    $body = @{VendorId=$id; Name="ABC Travel Agency Updated"; AddressLine1="999 Updated Street"; PhoneNumber="1111111111"; ItPanNumber="ABCDE1234F"; BankName="SBI"; AccountNumber="1234567890"} | ConvertTo-Json
    $resp = Invoke-RestMethod -Uri "$base/api/vendors/$id" -Method PUT -Body $body -Headers $headers
    Write-Host "Update result: $resp"
}

Test "GET /api/vendors/{id} - Verify Update" {
    $id = $script:vendor1.vendorId
    $resp = Invoke-RestMethod -Uri "$base/api/vendors/$id" -Headers $headers
    $resp | ConvertTo-Json -Depth 2
}

# ---- TAX MASTERS ----
Test "POST /api/taxmasters - Create SGST" {
    $id = $script:vendor1.vendorId
    $body = @{VendorId=$id; TaxType="SGST"; TaxRate=9; EffectiveDate="2024-01-01"} | ConvertTo-Json
    $resp = Invoke-RestMethod -Uri "$base/api/taxmasters" -Method POST -Body $body -Headers $headers
    $resp | ConvertTo-Json -Depth 2
}

Test "POST /api/taxmasters - Create CGST" {
    $id = $script:vendor1.vendorId
    $body = @{VendorId=$id; TaxType="CGST"; TaxRate=9; EffectiveDate="2024-01-01"} | ConvertTo-Json
    $resp = Invoke-RestMethod -Uri "$base/api/taxmasters" -Method POST -Body $body -Headers $headers
    $resp | ConvertTo-Json -Depth 2
}

Test "GET /api/taxmasters - List All" {
    $resp = Invoke-RestMethod -Uri "$base/api/taxmasters" -Headers $headers
    $resp | ConvertTo-Json -Depth 3
}

Test "GET /api/taxmasters/SGST - Get By Type" {
    $resp = Invoke-RestMethod -Uri "$base/api/taxmasters/SGST" -Headers $headers
    $resp | ConvertTo-Json -Depth 2
}

Test "GET /api/taxmasters/vendor/{id} - Get By Vendor" {
    $id = $script:vendor1.vendorId
    $resp = Invoke-RestMethod -Uri "$base/api/taxmasters/vendor/$id" -Headers $headers
    $resp | ConvertTo-Json -Depth 3
}

Test "PUT /api/taxmasters/SGST/rate - Update Tax Rate" {
    $body = @{TaxType="SGST"; NewRate=12; ModifiedBy="admin"} | ConvertTo-Json
    $resp = Invoke-RestMethod -Uri "$base/api/taxmasters/SGST/rate" -Method PUT -Body $body -Headers $headers
    Write-Host "Update result: $resp"
}

# ---- JAI INTERFACE LINES ----
Test "POST /api/jaiinterfacelines - Create Interface Line" {
    $body = @{OrgId=100; PartyId=200; PartySiteId=300; ImportModule="AP"; TransactionNum="TRX001"; TransactionLineNum=1; CreatedBy="admin"; TaxLines=@()} | ConvertTo-Json
    $resp = Invoke-RestMethod -Uri "$base/api/jaiinterfacelines" -Method POST -Body $body -Headers $headers
    $script:jaiLine = $resp
    $resp | ConvertTo-Json -Depth 2
}

Test "GET /api/jaiinterfacelines - List All" {
    $resp = Invoke-RestMethod -Uri "$base/api/jaiinterfacelines" -Headers $headers
    $resp | ConvertTo-Json -Depth 3
}

# ---- LOOKUP ENDPOINTS ----
Test "GET /api/transactionlookups/account-masters" {
    $resp = Invoke-RestMethod -Uri "$base/api/transactionlookups/account-masters" -Headers $headers
    Write-Host "Count: $($resp.Count)"
}

Test "GET /api/transactionlookups/gl-code-combinations" {
    $resp = Invoke-RestMethod -Uri "$base/api/transactionlookups/gl-code-combinations" -Headers $headers
    Write-Host "Count: $($resp.Count)"
}

Test "GET /api/transactionlookups/travel-ap-params" {
    $resp = Invoke-RestMethod -Uri "$base/api/transactionlookups/travel-ap-params" -Headers $headers
    Write-Host "Count: $($resp.Count)"
}

# ---- DELETE ----
Test "DELETE /api/vendors/{id} - Delete Vendor 2" {
    $id = $script:vendor2.vendorId
    $resp = Invoke-RestMethod -Uri "$base/api/vendors/$id" -Method DELETE -Headers $headers
    Write-Host "Delete result: $resp"
}

Test "GET /api/vendors - After Delete" {
    $resp = Invoke-RestMethod -Uri "$base/api/vendors" -Headers $headers
    Write-Host "Count: $($resp.Count)"
    $resp | ConvertTo-Json -Depth 3
}

Write-Host "`n=== ALL REST TESTS COMPLETE ===" -ForegroundColor Green
