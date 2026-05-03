$out = 'e:\ERPMicroservice\tmp\test-out.txt'
Remove-Item $out -ErrorAction Ignore
$tok = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiJjOThjMzU0Ny04MmJhLTQ0MmYtYTkyMy03ZGE2MTgzNDAyMTAiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc2NDc5MzUsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.orei8iCDi6aCizYCWeZBKbv8ne34kVXXHBYzyzghvvo'
$h = @{ Authorization = "Bearer $tok" }
function Hit($u, $m = 'GET', $b = $null) {
    try {
        if ($b) { $r = Invoke-WebRequest -Uri $u -Method $m -Headers $h -Body $b -ContentType 'application/json' -UseBasicParsing -TimeoutSec 25 }
        else { $r = Invoke-WebRequest -Uri $u -Method $m -Headers $h -UseBasicParsing -TimeoutSec 25 }
        Add-Content $out "[$m] $u => $($r.StatusCode)"
        $c = if ($r.Content -is [byte[]]) { [System.Text.Encoding]::UTF8.GetString($r.Content) } else { $r.Content }
        Add-Content $out $c
        Add-Content $out '---'
    } catch {
        $resp = $_.Exception.Response
        if ($resp) {
            $sr = New-Object IO.StreamReader($resp.GetResponseStream())
            Add-Content $out "[$m] $u => $([int]$resp.StatusCode)"
            Add-Content $out $sr.ReadToEnd()
            Add-Content $out '---'
        } else { Add-Content $out "[$m] $u => ERR $_"; Add-Content $out '---' }
    }
}
Hit 'http://localhost:5210/health'
Hit 'http://localhost:5210/api/audits'
Hit 'http://localhost:5210/api/audits/minimal'
Hit 'http://localhost:5210/api/audits/minimal/types'
$gq1 = '{"query":"{ viewAudits { isSuccess message data { auditId companyId status sites services } } }"}'
Hit 'http://localhost:5210/graphql' 'POST' $gq1
$gq2 = '{"query":"{ auditDetails(auditId: 1) { isSuccess message data { auditId leadAuditor siteName services auditorTeam } } }"}'
Hit 'http://localhost:5210/graphql' 'POST' $gq2
$gq3 = '{"query":"{ viewSitesForAudit(auditId: 2) { isSuccess data { siteName addressLine city country } } }"}'
Hit 'http://localhost:5210/graphql' 'POST' $gq3
$gq4 = '{"query":"{ viewFindings(auditId: 1) { isSuccess data { findingsId findingNumber title status } } }"}'
Hit 'http://localhost:5210/graphql' 'POST' $gq4
