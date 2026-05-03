$ErrorActionPreference = 'Continue'
$base = "http://localhost:5502"
$token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiI5YjViNTZlZC01ZTAxLTQzNGUtOTc3My1kNTExZDQ2OWNjZmUiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc4MjAyNzMsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.xRZqMW6G22XPCUZ85tYhKqcTYQUImVa-T-f_YhUsOVg"
$h = @{ Authorization = "Bearer $token" }

$ok = 0; $fail = 0
function T($name, $script) {
  try {
    $r = & $script
    Write-Host "[OK]  $name -> $r"
    $script:ok++
  } catch {
    Write-Host "[ERR] $name -> $($_.Exception.Message)"
    $script:fail++
  }
}

T "GET /health"                                  { (Invoke-WebRequest -UseBasicParsing "$base/health").StatusCode }
T "GET /api/invoices"                            { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/invoices?pageNumber=1&pageSize=10").StatusCode }
T "GET /api/invoices?status=Paid"                { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/invoices?status=Paid&pageSize=5").StatusCode }
T "GET /api/invoices/download"                   { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/invoices/download?invoiceNumber=INV-2024-001").StatusCode }
T "PUT /api/invoices/planned-payment-date" {
  $body = @{ InvoiceNumbers = @("INV-2025-002"); PlannedPaymentDate = "2026-07-01" } | ConvertTo-Json
  (Invoke-WebRequest -UseBasicParsing -Method PUT -Headers $h -ContentType "application/json" -Body $body "$base/api/invoices/planned-payment-date").StatusCode
}

# Minimal API endpoints (use FinanceDomainDbContext via MediatR)
T "GET /api/invoices/minimal"                    { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/invoices/minimal").StatusCode }
T "GET /api/invoices/minimal/1"                  { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/invoices/minimal/1").StatusCode }
T "GET /api/invoices/minimal/company/2"          { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/invoices/minimal/company/2").StatusCode }
T "GET /api/financials/minimal/company/2"        { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/financials/minimal/company/2").StatusCode }
T "GET /api/financials/minimal/company/2 (year)" { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/financials/minimal/company/2?year=2025").StatusCode }

T "POST /api/invoices/minimal (create)" {
  $body = @{
    invoiceNumber = "INV-NEW-$(Get-Random)"; companyId = 2; contractId = 1
    invoiceDate = "2026-05-01"; dueDate = "2026-06-01"
    amount = 1000; taxAmount = 150; totalAmount = 1150
    currency = "USD"; createdBy = 1
  } | ConvertTo-Json
  (Invoke-WebRequest -UseBasicParsing -Method POST -Headers $h -ContentType "application/json" -Body $body "$base/api/invoices/minimal").StatusCode
}

T "PUT /api/invoices/minimal/1/pay" {
  (Invoke-WebRequest -UseBasicParsing -Method PUT -Headers $h "$base/api/invoices/minimal/1/pay?paymentMethod=Wire&paymentReference=PMT-X1&paidBy=1").StatusCode
}

# GraphQL
function GQL($name, $query) {
  $body = @{ query = $query } | ConvertTo-Json -Compress
  $tmp = New-TemporaryFile
  Set-Content $tmp $body -Encoding UTF8
  try {
    $resp = curl.exe -s -X POST "$base/graphql" -H "Authorization: Bearer $token" -H "Content-Type: application/json" --data-binary "@$tmp"
    if ($resp -match '"errors"') { Write-Host "[ERR] $name -> $resp"; $script:fail++ }
    else { Write-Host "[OK]  $name -> $resp"; $script:ok++ }
  } finally { Remove-Item $tmp }
}

GQL "GraphQL: InvoiceListPage" '{ InvoiceListPage(pageNumber:1, pageSize:5) { isSuccess message data { items { invoice company status amount } } } }'
GQL "GraphQL: DownloadInvoice (no file)" '{ DownloadInvoice(invoiceNumber:["INV-2024-001"], userId:1) { isSuccess message errorCode } }'
$gqlInv = "INV-GQL-$(Get-Random)"
GQL "GraphQL: createInvoice" "mutation { createInvoice(input:{invoiceNumber:`"$gqlInv`", companyId:2, contractId:1, invoiceDate:`"2026-05-02T00:00:00Z`", dueDate:`"2026-06-02T00:00:00Z`", amount:500, taxAmount:75, totalAmount:575, currency:`"USD`", createdBy:1}) { invoiceId invoiceNumber status totalAmount } }"
GQL "GraphQL: updateInvoice" 'mutation { updateInvoice(input:{invoiceId:2, invoiceNumber:"INV-2024-002", companyId:2, invoiceDate:"2024-05-25T00:00:00Z", dueDate:"2024-06-24T00:00:00Z", amount:16500, taxAmount:2475, totalAmount:18975, currency:"USD", status:"Paid", isActive:true, modifiedBy:1}) { invoiceId amount totalAmount } }'
GQL "GraphQL: changeInvoiceStatus" 'mutation { changeInvoiceStatus(invoiceId:5, newStatus:"Pending", modifiedBy:1) { invoiceId status } }'
GQL "GraphQL: markInvoicePaid" 'mutation { markInvoicePaid(invoiceId:4, paidDate:"2026-05-03T00:00:00Z", paymentMethod:"Wire", paymentReference:"PMT-GQL", modifiedBy:1) { invoiceId status paidDate } }'
GQL "GraphQL: deleteInvoice" 'mutation { deleteInvoice(invoiceId:15) }'
GQL "GraphQL: createFinancial" 'mutation { createFinancial(input:{companyId:2, year:2026, quarter:2, revenue:50000, expenses:10000, profit:40000, paidAmount:50000, currency:"USD", createdBy:1}) { financialId companyId year quarter revenue } }'

Write-Host ""
Write-Host "================================="
Write-Host "PASSED: $ok    FAILED: $fail"
Write-Host "================================="
