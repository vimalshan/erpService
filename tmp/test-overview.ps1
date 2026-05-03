$ErrorActionPreference = 'Continue'
$base = 'http://localhost:5150'
$jwt  = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiI5YjViNTZlZC01ZTAxLTQzNGUtOTc3My1kNTExZDQ2OWNjZmUiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc4MjAyNzMsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.xRZqMW6G22XPCUZ85tYhKqcTYQUImVa-T-f_YhUsOVg'
$h = @{ Authorization = "Bearer $jwt" }
$pass = 0; $fail = 0

function Check($name, $ok, $detail = '') {
    if ($ok) { Write-Host "  PASS  $name" -ForegroundColor Green; $script:pass++ }
    else      { Write-Host "  FAIL  $name  $detail" -ForegroundColor Red; $script:fail++ }
}

function GQL($name, $query, $assert) {
    try {
        $tmp = [System.IO.Path]::GetTempFileName()
        @{ query = $query } | ConvertTo-Json -Compress | Set-Content $tmp -Encoding utf8
        $resp = curl.exe -s -X POST "$base/graphql" -H "Content-Type: application/json" -H "Authorization: Bearer $jwt" --data-binary "@$tmp" | ConvertFrom-Json
        Remove-Item $tmp -ErrorAction SilentlyContinue
        if ($resp.errors) { Check $name $false ($resp.errors[0].message); return }
        $ok = & $assert $resp
        Check $name $ok
    } catch { Check $name $false $_.Exception.Message }
}

Write-Host "`n== REST tests ==`n"

try { $r = Invoke-WebRequest "$base/health" -UseBasicParsing -Headers $h; Check 'GET /health' ($r.StatusCode -eq 200) } catch { Check 'GET /health' $false $_.Exception.Message }

Write-Host "`n== GraphQL tests ==`n"

GQL 'gql viewCertificationQuicklinkCard' `
    'query { viewCertificationQuicklinkCard { currentPage totalItems totalPages data { serviceId serviceName } } }' `
    { param($j) $j.data.viewCertificationQuicklinkCard -ne $null } | Out-Null

GQL 'gql widgetForFinancials' `
    'query { widgetForFinancials { financialStatus financialCount financialPercentage } }' `
    { param($j) $j.data.widgetForFinancials -ne $null } | Out-Null

GQL 'gql widgetForUpcomingAudit' `
    'query { widgetForUpcomingAudit { confirmed toBeConfirmed toBeConfirmedBySuaadhya } }' `
    { param($j) $j.data.widgetForUpcomingAudit -ne $null } | Out-Null

GQL 'gql widgetForTrainingStatus' `
    'query { widgetForTrainingStatus { completed pending inProgress } }' `
    { param($j) $j.data.widgetForTrainingStatus -ne $null } | Out-Null

Write-Host ""
Write-Host "=========================" -ForegroundColor Cyan
Write-Host "PASS: $pass  FAIL: $fail" -ForegroundColor $(if ($fail -eq 0) { 'Green' } else { 'Red' })
if ($fail -gt 0) { exit 1 }
