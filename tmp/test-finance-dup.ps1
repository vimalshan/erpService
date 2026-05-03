$tok = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiI5YjViNTZlZC01ZTAxLTQzNGUtOTc3My1kNTExZDQ2OWNjZmUiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc4MjAyNzMsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.xRZqMW6G22XPCUZ85tYhKqcTYQUImVa-T-f_YhUsOVg"
$body = '{"invoiceNumber":"INV-2024-001","companyId":2,"contractId":1,"invoiceDate":"2024-03-20","dueDate":"2024-04-19","amount":100,"taxAmount":15,"totalAmount":115,"currency":"USD","createdBy":1}'
$tmp = New-TemporaryFile
Set-Content $tmp $body -Encoding UTF8
$out = "$env:TEMP\out.json"
curl.exe -s -o $out -w "HTTP %{http_code}`n" -X POST "http://localhost:5502/api/invoices/minimal" -H "Authorization: Bearer $tok" -H "Content-Type: application/json" --data-binary "@$tmp"
Get-Content $out
Remove-Item $tmp
