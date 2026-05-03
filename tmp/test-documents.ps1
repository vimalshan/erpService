$ErrorActionPreference = 'Continue'
$base = "http://localhost:5501"
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

T "GET /health"                  { (Invoke-WebRequest -UseBasicParsing "$base/health").StatusCode }
T "GET /api/documents"           { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/documents").StatusCode }
T "GET /api/documents (audit=1)" { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/documents?auditId=1").StatusCode }
T "GET /api/documents/ContractList" { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/documents/ContractList").StatusCode }
T "GET /api/documents/download (seeded)" {
  (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/documents/download?documentId=11111111-1111-1111-1111-111111111111").StatusCode
}
T "GET /api/documents/download (missing)" {
  try { (Invoke-WebRequest -UseBasicParsing -Headers $h "$base/api/documents/download?documentId=99999999-9999-9999-9999-999999999999").StatusCode }
  catch { if ($_.Exception.Response.StatusCode.value__ -eq 404) { "404 (expected)" } else { throw } }
}

# Bulkdownload
T "POST /api/documents/Bulkdownload" {
  $body = '["11111111-1111-1111-1111-111111111111","22222222-2222-2222-2222-222222222222"]'
  (Invoke-WebRequest -UseBasicParsing -Method POST -Headers $h -ContentType "application/json" -Body $body "$base/api/documents/Bulkdownload?docType=audit").StatusCode
}
T "POST /api/documents/ExportContract" {
  (Invoke-WebRequest -UseBasicParsing -Method POST -Headers $h -ContentType "application/json" -Body '{}' "$base/api/documents/ExportContract").StatusCode
}

# Upload (multipart)
T "POST /api/documents/upload" {
  $tmp = New-TemporaryFile
  Set-Content $tmp "hello upload test"
  $r = curl.exe -s -o NUL -w "%{http_code}" -X POST "$base/api/documents/upload?category=Test&auditId=1" -H "Authorization: Bearer $token" -F "file=@$tmp"
  Remove-Item $tmp
  $r
}

# Delete (use a seeded id)
T "DELETE /api/documents/DeleteDocument" {
  (Invoke-WebRequest -UseBasicParsing -Method DELETE -Headers $h "$base/api/documents/DeleteDocument?documentId=55555555-5555-5555-5555-555555555555").StatusCode
}

# GraphQL tests
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

GQL "GraphQL: documentCount"   "{ documentCount }"
GQL "GraphQL: documents"       "{ documents { documentId fileName fileSize category } }"
GQL "GraphQL: documentById"    '{ documentById(documentId:"11111111-1111-1111-1111-111111111111") { fileName category } }'
GQL "GraphQL: createDocument"  'mutation { createDocument(input:{fileName:"gql-test.pdf", contentType:"application/pdf", fileSize:1234, category:"GQL"}) { documentId fileName } }'
GQL "GraphQL: deleteDocument"  'mutation { deleteDocument(documentId:"22222222-2222-2222-2222-222222222222") }'
GQL "GraphQL: documents (filter category)" '{ documents(where:{category:{eq:"Audit"}}) { fileName category } }'

Write-Host ""
Write-Host "================================="
Write-Host "PASSED: $ok    FAILED: $fail"
Write-Host "================================="
