$token='eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiJjYTkyZTAxNC0zZjhiLTQ4MWUtOGViNS1jYmQ0ZDJhNzUxOWEiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc4MTQ4MTIsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.aohLXrUK6iQqJcD2D7ItVTSoKPrGz2B7YsGyFLRUujI'
$base = 'http://localhost:5210/graphql'

function GQL($name, $body) {
    Write-Host ""
    Write-Host "=== $name ===" -ForegroundColor Magenta
    $h = @{ Authorization = "Bearer $token"; Accept = 'application/json' }
    try {
        $r = Invoke-WebRequest -Uri $base -Method POST -Body $body -ContentType 'application/json' -Headers $h -UseBasicParsing -TimeoutSec 15
        Write-Host "Status: $($r.StatusCode)" -ForegroundColor Green
        $c = $r.Content; if ($c.Length -gt 2000) { $c.Substring(0,2000)+"...[truncated]" } else { $c }
    } catch {
        Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.ErrorDetails) { Write-Host "Body: $($_.ErrorDetails.Message)" -ForegroundColor Yellow }
        else {
            try {
                $resp = $_.Exception.Response
                if ($resp) {
                    $sr = New-Object System.IO.StreamReader($resp.GetResponseStream())
                    Write-Host "Body: $($sr.ReadToEnd())" -ForegroundColor Yellow
                }
            } catch {}
        }
    }
}

GQL 'viewAudits' '{"query":"{ viewAudits { isSuccess message data { auditId leadAuditor type status } } }"}'
GQL 'auditDetails(1)' '{"query":"query($id:Int!){ auditDetails(auditId:$id){ isSuccess message data { auditId leadAuditor startDate endDate status services } } }","variables":{"id":1}}'
GQL 'viewFindings(1)' '{"query":"query($id:Int!){ viewFindings(auditId:$id){ isSuccess message data { findingsId findingNumber title category status } } }","variables":{"id":1}}'
GQL 'viewSitesForAudit(1)' '{"query":"query($id:Int!){ viewSitesForAudit(auditId:$id){ isSuccess message data { siteName addressLine } } }","variables":{"id":1}}'
GQL 'viewSubAudits(1)' '{"query":"query($id:Int!){ viewSubAudits(auditId:$id){ isSuccess message data { auditId status startDate endDate } } }","variables":{"id":1}}'
GQL 'getAuditDaysPerSite' '{"query":"query{ getAuditDaysPerSite(startDate:\"2024-01-01\", endDate:\"2025-12-31\", companies:null, services:null, sites:null){ isSuccess message } }"}'

Write-Host ""
Write-Host "=== Unauthenticated GraphQL (no token) ===" -ForegroundColor Yellow
try {
    $r = Invoke-WebRequest -Uri $base -Method POST -Body '{"query":"{ viewAudits { isSuccess } }"}' -ContentType 'application/json' -UseBasicParsing -TimeoutSec 10
    Write-Host "Status: $($r.StatusCode)"
    $r.Content
} catch {
    Write-Host "Error: $($_.Exception.Message)"
    if ($_.ErrorDetails) { $_.ErrorDetails.Message }
}
