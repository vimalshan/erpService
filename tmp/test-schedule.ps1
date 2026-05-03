$ErrorActionPreference = 'Continue'
$base = 'http://localhost:5148'
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

try {
    $r = Invoke-RestMethod "$base/api/schedules/minimal" -Headers $h
    Check 'GET /api/schedules/minimal' ($r.Count -ge 5)
} catch { Check 'GET /api/schedules/minimal' $false $_.Exception.Message }

try {
    $r = Invoke-RestMethod "$base/api/schedules/minimal/1" -Headers $h
    Check 'GET /api/schedules/minimal/1' ($r.auditSiteAuditId -eq 1)
} catch { Check 'GET /api/schedules/minimal/1' $false $_.Exception.Message }

try {
    $code = 0
    try { Invoke-RestMethod "$base/api/schedules/minimal/9999" -Headers $h | Out-Null }
    catch { $code = $_.Exception.Response.StatusCode.value__ }
    Check 'GET /api/schedules/minimal/9999 -> 404' ($code -eq 404)
} catch { Check 'GET /api/schedules/minimal/9999 -> 404' $false $_.Exception.Message }

try {
    $r = Invoke-RestMethod "$base/api/schedules/minimal/audit/1001" -Headers $h
    Check 'GET /api/schedules/minimal/audit/1001' ($r.Count -ge 1)
} catch { Check 'GET /api/schedules/minimal/audit/1001' $false $_.Exception.Message }

try {
    $r = Invoke-RestMethod "$base/api/schedules/minimal/site/1" -Headers $h
    Check 'GET /api/schedules/minimal/site/1' ($r.Count -ge 1)
} catch { Check 'GET /api/schedules/minimal/site/1' $false $_.Exception.Message }

# POST create
$newSchedule = @{
    auditId = 1003; siteId = 2; auditTypeId = 1
    auditNumber = 'AUD-2026-TEST'; scheduledDate = '2026-08-01T00:00:00'
    leadAuditorId = 1; notes = 'Test schedule'; createdBy = 1
} | ConvertTo-Json
try {
    $r = Invoke-RestMethod "$base/api/schedules/minimal" -Method POST -Headers ($h + @{'Content-Type'='application/json'}) -Body $newSchedule
    $script:newId = $r.auditSiteAuditId
    Check 'POST /api/schedules/minimal (create)' ($r.auditSiteAuditId -gt 0)
} catch { Check 'POST /api/schedules/minimal (create)' $false $_.Exception.Message }

# PUT update
$updateSchedule = @{
    auditSiteAuditId = $script:newId; auditId = 1003; siteId = 2; auditTypeId = 1
    auditNumber = 'AUD-2026-TEST'; scheduledDate = '2026-08-05T00:00:00'
    status = 'scheduled'; isActive = $true; certificateIssued = $false
    modifiedBy = 1
} | ConvertTo-Json
try {
    $r = Invoke-RestMethod "$base/api/schedules/minimal" -Method PUT -Headers ($h + @{'Content-Type'='application/json'}) -Body $updateSchedule
    Check 'PUT /api/schedules/minimal (update)' ($r.auditSiteAuditId -gt 0)
} catch { Check 'PUT /api/schedules/minimal (update)' $false $_.Exception.Message }

# Start audit
try {
    $r = Invoke-RestMethod "$base/api/schedules/minimal/$($script:newId)/start?startedBy=1" -Method PUT -Headers $h
    Check "PUT /api/schedules/minimal/{id}/start" ($r.status -ne $null)
} catch { Check "PUT /api/schedules/minimal/{id}/start" $false $_.Exception.Message }

# Complete audit
try {
    $r = Invoke-RestMethod "$base/api/schedules/minimal/$($script:newId)/complete?completedBy=1" -Method PUT -Headers $h
    Check "PUT /api/schedules/minimal/{id}/complete" ($r.status -ne $null)
} catch { Check "PUT /api/schedules/minimal/{id}/complete" $false $_.Exception.Message }

# DELETE
try {
    $code = 0
    try { Invoke-RestMethod "$base/api/schedules/minimal/$($script:newId)" -Method DELETE -Headers $h | Out-Null; $code = 204 }
    catch { $code = $_.Exception.Response.StatusCode.value__ }
    Check 'DELETE /api/schedules/minimal/{id}' ($code -eq 204)
} catch { Check 'DELETE /api/schedules/minimal/{id}' $false $_.Exception.Message }

Write-Host "`n== GraphQL tests ==`n"

GQL 'gql viewAuditSchedules' `
    'query { viewAuditSchedules(calendarScheduleFilter: { companyIds: [], serviceIds: [], siteIds: [], statuses: [] }) { isSuccess data { siteAuditId status siteId } } }' `
    { param($j) $j.data.viewAuditSchedules -ne $null } | Out-Null

GQL 'gql addToCalender' `
    'query { addToCalender(isAddToCalender: false, siteAuditId: 1) { isSuccess } }' `
    { param($j) $j.data.addToCalender -ne $null } | Out-Null

# Create via GraphQL mutation
GQL 'gql scheduleAudit' `
    'mutation { scheduleAudit(input: { auditId: 1001, siteId: 3, auditTypeId: 1, auditNumber: "AUD-GQL-001", scheduledDate: "2026-09-01T00:00:00Z", createdBy: 1 }) { auditSiteAuditId auditNumber status } }' `
    { param($j) $j.data.scheduleAudit.auditSiteAuditId -gt 0 } | Out-Null

# Capture created ID for further mutations
$gqlId = $null
try {
    $tmp = [System.IO.Path]::GetTempFileName()
    @{ query = 'query { getAll: viewAuditSchedules(calendarScheduleFilter: { companyIds: [], serviceIds: [], siteIds: [], statuses: [] }) { data { siteAuditId auditNumber } } }' } | ConvertTo-Json -Compress | Set-Content $tmp -Encoding utf8
    $r = curl.exe -s -X POST "$base/graphql" -H "Content-Type: application/json" -H "Authorization: Bearer $jwt" --data-binary "@$tmp" | ConvertFrom-Json
    Remove-Item $tmp -ErrorAction SilentlyContinue
    # find created record
    $gqlId = ($r.data.getAll.data | Where-Object { $_.auditNumber -eq 'AUD-GQL-001' }).siteAuditId
} catch {}

if (-not $gqlId) {
    # fallback: get id from DB directly via minimal API
    try {
        $all = Invoke-RestMethod "$base/api/schedules/minimal" -Headers $h
        $gqlId = ($all | Where-Object { $_.auditNumber -eq 'AUD-GQL-001' }).auditSiteAuditId
    } catch {}
}

GQL 'gql updateSchedule' `
    ('mutation { updateSchedule(input: { auditSiteAuditId: ' + $gqlId + ', auditId: 1001, siteId: 3, auditTypeId: 1, auditNumber: "AUD-GQL-001", status: "scheduled", isActive: true, certificateIssued: false, modifiedBy: 1 }) { auditSiteAuditId status } }') `
    { param($j) $j.data.updateSchedule.auditSiteAuditId -gt 0 } | Out-Null

GQL 'gql rescheduleAudit' `
    ('mutation { rescheduleAudit(auditSiteAuditId: ' + $gqlId + ', newDate: "2026-09-15T00:00:00Z", modifiedBy: 1) { auditSiteAuditId scheduledDate } }') `
    { param($j) $j.data.rescheduleAudit.auditSiteAuditId -gt 0 } | Out-Null

GQL 'gql startAudit' `
    ('mutation { startAudit(auditSiteAuditId: ' + $gqlId + ', startDate: "2026-09-15T00:00:00Z", modifiedBy: 1) { auditSiteAuditId status } }') `
    { param($j) $j.data.startAudit.status -ne $null } | Out-Null

GQL 'gql completeAudit' `
    ('mutation { completeAudit(auditSiteAuditId: ' + $gqlId + ', completedDate: "2026-09-20T00:00:00Z", reportPath: "/reports/gql-001.pdf", modifiedBy: 1) { auditSiteAuditId status } }') `
    { param($j) $j.data.completeAudit.status -ne $null } | Out-Null

GQL 'gql deleteSchedule' `
    ('mutation { deleteSchedule(auditSiteAuditId: ' + $gqlId + ') }') `
    { param($j) $j.data.deleteSchedule -eq $true } | Out-Null

Write-Host ""
Write-Host "=========================" -ForegroundColor Cyan
Write-Host "PASS: $pass  FAIL: $fail" -ForegroundColor $(if ($fail -eq 0) { 'Green' } else { 'Red' })
if ($fail -gt 0) { exit 1 }
