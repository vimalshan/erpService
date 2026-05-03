$token='eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiJjYTkyZTAxNC0zZjhiLTQ4MWUtOGViNS1jYmQ0ZDJhNzUxOWEiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc4MTQ4MTIsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.aohLXrUK6iQqJcD2D7ItVTSoKPrGz2B7YsGyFLRUujI'
$h = @{ Authorization = "Bearer $token" }
$base = 'http://localhost:5210'

function Test-Endpoint($name, $method, $url, $body) {
    Write-Host ""
    Write-Host "=== $name : $method $url ===" -ForegroundColor Cyan
    try {
        if ($body) {
            $r = Invoke-WebRequest -Uri $url -Headers $h -Method $method -Body $body -ContentType 'application/json' -UseBasicParsing -TimeoutSec 15
        } else {
            $r = Invoke-WebRequest -Uri $url -Headers $h -Method $method -UseBasicParsing -TimeoutSec 15
        }
        Write-Host "Status: $($r.StatusCode)" -ForegroundColor Green
        $c = $r.Content; if ($c.Length -gt 1500) { $c.Substring(0,1500) + "...[truncated]" } else { $c }
    } catch {
        Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.ErrorDetails) { $_.ErrorDetails.Message }
    }
}

Test-Endpoint 'Health'           'GET'  "$base/health"
Test-Endpoint 'AuditList'        'GET'  "$base/api/audits"
Test-Endpoint 'AuditDetails'     'GET'  "$base/api/audits/1"
Test-Endpoint 'AuditFindings'    'GET'  "$base/api/audits/1/findings"
Test-Endpoint 'AuditSites'       'GET'  "$base/api/audits/1/sites"
Test-Endpoint 'SubAudits'        'GET'  "$base/api/audits/1/subaudits"
Test-Endpoint 'MinimalList'      'GET'  "$base/api/audits/minimal"
Test-Endpoint 'MinimalTypes'     'GET'  "$base/api/audits/minimal/types"

Write-Host ""
Write-Host "=== GraphQL viewAudits ===" -ForegroundColor Magenta
$gql1 = @{ query = '{ viewAudits { success message data { auditId auditName auditType } } }' } | ConvertTo-Json -Compress
Test-Endpoint 'GQL viewAudits' 'POST' "$base/graphql" $gql1

Write-Host ""
Write-Host "=== GraphQL auditDetails ===" -ForegroundColor Magenta
$gql2 = @{ query = 'query($id:Int!){ auditDetails(auditId:$id){ success message } }'; variables=@{ id=1 } } | ConvertTo-Json -Compress
Test-Endpoint 'GQL auditDetails' 'POST' "$base/graphql" $gql2

Write-Host ""
Write-Host "=== GraphQL viewFindings ===" -ForegroundColor Magenta
$gql3 = @{ query = 'query($id:Int!){ viewFindings(auditId:$id){ success message } }'; variables=@{ id=1 } } | ConvertTo-Json -Compress
Test-Endpoint 'GQL viewFindings' 'POST' "$base/graphql" $gql3

Write-Host ""
Write-Host "=== GraphQL Unauthenticated check ===" -ForegroundColor Magenta
try {
    $r = Invoke-WebRequest -Uri "$base/graphql" -Method POST -Body $gql1 -ContentType 'application/json' -UseBasicParsing -TimeoutSec 10
    Write-Host "Status (no auth): $($r.StatusCode)" -ForegroundColor Yellow
    if ($r.Content.Length -gt 500) { $r.Content.Substring(0,500) } else { $r.Content }
} catch { Write-Host "Status (no auth): $($_.Exception.Message)" -ForegroundColor Yellow }
