$ErrorActionPreference = 'Continue'
$base = 'http://localhost:5149'
$jwt  = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiI5YjViNTZlZC01ZTAxLTQzNGUtOTc3My1kNTExZDQ2OWNjZmUiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc4MjAyNzMsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.xRZqMW6G22XPCUZ85tYhKqcTYQUImVa-T-f_YhUsOVg'
$h    = @{ Authorization = "Bearer $jwt" }
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

$ts = [int](Get-Date -UFormat %s)
$testUsername = "testuser_$ts"
$testEmail    = "testuser_${ts}@test.com"
$testRoleCode = "TEST_ROLE_$ts"
try { $r = Invoke-WebRequest "$base/health" -UseBasicParsing -Headers $h; Check 'GET /health' ($r.StatusCode -eq 200) }
catch { Check 'GET /health' $false $_.Exception.Message }

# Users list
try {
    $r = Invoke-RestMethod "$base/api/users/minimal" -Headers $h
    Check 'GET /api/users/minimal' ($r.Count -ge 5)
} catch { Check 'GET /api/users/minimal' $false $_.Exception.Message }

# Get user by ID
try {
    $r = Invoke-RestMethod "$base/api/users/minimal/1" -Headers $h
    Check 'GET /api/users/minimal/1' ($r.userId -eq 1)
} catch { Check 'GET /api/users/minimal/1' $false $_.Exception.Message }

# 404
try {
    $code = 0
    try { Invoke-RestMethod "$base/api/users/minimal/9999" -Headers $h | Out-Null }
    catch { $code = $_.Exception.Response.StatusCode.value__ }
    Check 'GET /api/users/minimal/9999 -> 404' ($code -eq 404)
} catch { Check 'GET /api/users/minimal/9999 -> 404' $false $_.Exception.Message }

# Create user
$newUser = @{
    username = $testUsername; email = $testEmail
    firstName = 'Test'; lastName = 'User'
    password = 'P@ssw0rd!123'; phone = '+1-555-000-0001'
    position = 'Tester'; department = 'QA'
    timeZone = 'UTC'; language = 'EN'; createdBy = 1
} | ConvertTo-Json
try {
    $r = Invoke-RestMethod "$base/api/users/minimal" -Method POST -Headers ($h + @{'Content-Type'='application/json'}) -Body $newUser
    $script:newUserId = $r.userId
    Check 'POST /api/users/minimal (create)' ($r.userId -gt 0)
} catch { Check 'POST /api/users/minimal (create)' $false $_.Exception.Message }

# Update user
if ($script:newUserId) {
$updateUser = @{
    userId = $script:newUserId; username = $testUsername
    email = $testEmail; firstName = 'Updated'; lastName = 'User'
    isActive = $true; phone = '+1-555-000-0002'; position = 'Senior Tester'
    department = 'QA'; timeZone = 'UTC'; language = 'EN'; modifiedBy = 1
} | ConvertTo-Json
try {
    $r = Invoke-RestMethod "$base/api/users/minimal" -Method PUT -Headers ($h + @{'Content-Type'='application/json'}) -Body $updateUser
    Check 'PUT /api/users/minimal (update)' ($r.userId -gt 0)
} catch { Check 'PUT /api/users/minimal (update)' $false $_.Exception.Message }

# Deactivate user
try {
    $r = Invoke-RestMethod "$base/api/users/minimal/$($script:newUserId)/deactivate?modifiedBy=1" -Method PUT -Headers $h
    Check "PUT /api/users/minimal/{id}/deactivate" ($r -eq $true -or $r -ne $null)
} catch { Check "PUT /api/users/minimal/{id}/deactivate" $false $_.Exception.Message }
} else {
    Check 'PUT /api/users/minimal (update)' $false 'skipped - create failed'
    Check 'PUT /api/users/minimal/{id}/deactivate' $false 'skipped - create failed'
}

# Roles list
try {
    $r = Invoke-RestMethod "$base/api/roles/minimal" -Headers $h
    Check 'GET /api/roles/minimal' ($r.Count -ge 5)
} catch { Check 'GET /api/roles/minimal' $false $_.Exception.Message }

# Create role
$newRole = @{
    roleName = "Test Role $ts"; roleCode = $testRoleCode
    description = 'Test role for testing'; isSystemRole = $false
    permissions = 'READ'; createdBy = 1
} | ConvertTo-Json
try {
    $r = Invoke-RestMethod "$base/api/roles/minimal" -Method POST -Headers ($h + @{'Content-Type'='application/json'}) -Body $newRole
    $script:newRoleId = $r.roleId
    Check 'POST /api/roles/minimal (create)' ($r.roleId -gt 0)
} catch { Check 'POST /api/roles/minimal (create)' $false $_.Exception.Message }

# User preferences
try {
    $r = Invoke-RestMethod "$base/api/users/minimal/1/preferences" -Headers $h
    Check 'GET /api/users/minimal/1/preferences' ($r.Count -ge 1)
} catch { Check 'GET /api/users/minimal/1/preferences' $false $_.Exception.Message }

# Set user preference
$pref = @{
    userId = 1; preferenceKey = 'DashboardTheme'; preferenceValue = 'dark'
    preferenceType = 'string'; category = 'Display'; modifiedBy = 1
} | ConvertTo-Json
try {
    $r = Invoke-RestMethod "$base/api/users/minimal/preferences" -Method POST -Headers ($h + @{'Content-Type'='application/json'}) -Body $pref
    Check 'POST /api/users/minimal/preferences (set)' ($r.userPreferenceId -gt 0)
} catch { Check 'POST /api/users/minimal/preferences (set)' $false $_.Exception.Message }

Write-Host "`n== GraphQL tests ==`n"

GQL 'gql getUsers (adminList)' `
    'query { adminList(userId: 1) { isSuccess data { name email userStatus } } }' `
    { param($j) $j.data.adminList -ne $null } | Out-Null

GQL 'gql memberList' `
    'query { memberList(userId: 1) { isSuccess data { name email userStatus } } }' `
    { param($j) $j.data.memberList -ne $null } | Out-Null

GQL 'gql getCountries' `
    'query { getCountries { isSuccess data { id countryName countryCode } } }' `
    { param($j) $j.data.getCountries -ne $null } | Out-Null

GQL 'gql userCompanyDetails' `
    'query { userCompanyDetails(userId: 1) { isSuccess message } }' `
    { param($j) $j.data.userCompanyDetails -ne $null } | Out-Null

GQL 'gql preferences' `
    'query { preferences(objectType: "Grid", objectName: "AuditList", pageName: "Audits") { isSuccess message } }' `
    { param($j) $j.data.preferences -ne $null } | Out-Null

# GraphQL mutations — use string concatenation to avoid PS escape issues
GQL 'gql createUser' `
    ('mutation { createUser(input: { username: "gqluser_' + $ts + '", email: "gqluser_' + $ts + '@test.com", firstName: "GQL", lastName: "User", password: "P@ssw0rd!123", timeZone: "UTC", language: "EN", createdBy: 1 }) { userId username email } }') `
    { param($j) $j.data.createUser.userId -gt 0 } | Out-Null

# capture gql user id via REST
$gqlUserId = $null
try {
    $all = Invoke-RestMethod "$base/api/users/minimal" -Headers $h
    $gqlUserId = ($all | Where-Object { $_.username -eq "gqluser_$ts" }).userId
} catch {}

if ($gqlUserId) {
    GQL 'gql updateUser' `
        ('mutation { updateUser(input: { userId: ' + $gqlUserId + ', username: "gqluser_' + $ts + '", email: "gqluser_' + $ts + '@test.com", firstName: "GQL-Updated", lastName: "User", isActive: true, timeZone: "UTC", language: "EN", modifiedBy: 1 }) { userId firstName } }') `
        { param($j) $j.data.updateUser.userId -gt 0 } | Out-Null

    GQL 'gql deactivateUser' `
        ('mutation { deactivateUser(userId: ' + $gqlUserId + ', modifiedBy: 1) }') `
        { param($j) $j.data.deactivateUser -eq $true } | Out-Null
} else {
    Check 'gql updateUser' $false 'could not find gqluser'
    Check 'gql deactivateUser' $false 'could not find gqluser'
}

GQL 'gql createRole' `
    ('mutation { createRole(input: { roleName: "GQL Test Role ' + $ts + '", roleCode: "GQL_TEST_' + $ts + '", description: "GraphQL test role", isSystemRole: false, permissions: "READ", createdBy: 1 }) { roleId roleName } }') `
    { param($j) $j.data.createRole.roleId -gt 0 } | Out-Null

GQL 'gql setUserPreference' `
    'mutation { setUserPreference(input: { userId: 1, preferenceKey: "GqlTheme", preferenceValue: "light", preferenceType: "string", category: "Display", modifiedBy: 1 }) { userPreferenceId preferenceKey } }' `
    { param($j) $j.data.setUserPreference.userPreferenceId -gt 0 } | Out-Null

GQL 'gql updateCompanyDetails' `
    'mutation { updateCompanyDetails(input: { legalEntityId: 1, organizationName: "Test Corp", updatedBy: 1 }) { isSuccess message } }' `
    { param($j) $j.data.updateCompanyDetails -ne $null } | Out-Null

GQL 'gql updateSystemPreferences' `
    'mutation { updateSystemPreferences(input: { generalSettings: { systemName: "ERP Audit System", maintenanceMode: false }, updatedBy: 1 }) { isSuccess message } }' `
    { param($j) $j.data.updateSystemPreferences -ne $null } | Out-Null

Write-Host ""
Write-Host "=========================" -ForegroundColor Cyan
Write-Host "PASS: $pass  FAIL: $fail" -ForegroundColor $(if ($fail -eq 0) { 'Green' } else { 'Red' })
if ($fail -gt 0) { exit 1 }
