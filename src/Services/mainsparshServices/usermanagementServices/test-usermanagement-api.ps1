# UserManagement.API Test Script
# Port: 5243

$baseUrl = "http://localhost:5243"
$script:passed = 0
$script:failed = 0
$script:total = 0
$runId = (Get-Date -Format "HHmmss")

function Test-Endpoint {
    param([string]$Name, [scriptblock]$Block)
    $script:total++
    try {
        $result = & $Block
        if ($result) {
            Write-Host "  [PASS] $Name" -ForegroundColor Green
            $script:passed++
        } else {
            Write-Host "  [FAIL] $Name" -ForegroundColor Red
            $script:failed++
        }
    } catch {
        Write-Host "  [FAIL] $Name - $_" -ForegroundColor Red
        $script:failed++
    }
}

function Invoke-Api {
    param([string]$Url, [string]$Method = "GET", $Body, $Headers)
    $params = @{ Uri = $Url; Method = $Method; UseBasicParsing = $true }
    if ($Headers) { $params.Headers = $Headers }
    if ($Body) { $params.Body = $Body; $params.ContentType = "application/json" }
    $resp = Invoke-WebRequest @params
    $text = if ($resp.Content -is [byte[]]) { [System.Text.Encoding]::UTF8.GetString($resp.Content) } else { $resp.Content }
    return $text | ConvertFrom-Json
}

function Invoke-ApiRaw {
    param([string]$Url, [string]$Method = "GET", $Body, $Headers)
    $params = @{ Uri = $Url; Method = $Method; UseBasicParsing = $true }
    if ($Headers) { $params.Headers = $Headers }
    if ($Body) { $params.Body = $Body; $params.ContentType = "application/json" }
    return Invoke-WebRequest @params
}

function Invoke-GQL($query) {
    $body = @{ query = $query } | ConvertTo-Json
    $resp = Invoke-WebRequest "$baseUrl/graphql" -Method POST -Body $body -ContentType "application/json" -UseBasicParsing
    $text = if ($resp.Content -is [byte[]]) { [System.Text.Encoding]::UTF8.GetString($resp.Content) } else { $resp.Content }
    return $text | ConvertFrom-Json
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  UserManagement.API Tests" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# ─── 1. HEALTH CHECKS ─────────────────────────────────────────────────────────
Write-Host "`n[1] Health Checks" -ForegroundColor Yellow

Test-Endpoint "GET /health - service healthy" {
    $r = Invoke-Api "$baseUrl/health"
    $r.status -eq "Healthy"
}

Test-Endpoint "GET /health/ready - database ready" {
    $r = Invoke-Api "$baseUrl/health/ready"
    $r.status -eq "Healthy"
}

Test-Endpoint "GET /health/live - liveness probe" {
    $resp = Invoke-ApiRaw "$baseUrl/health/live"
    $resp.StatusCode -eq 200
}

# ─── 2. AUTH TOKEN ─────────────────────────────────────────────────────────────
Write-Host "`n[2] Auth Token" -ForegroundColor Yellow

$script:token = $null

Test-Endpoint "POST /api/v1/auth/login - returns JWT" {
    $body = @{ username = "admin"; password = "admin" } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/v1/auth/login" -Method POST -Body $body
    $script:token = $r.token
    $r.token -ne $null -and $r.token.Length -gt 20 -and $r.expiresAt -ne $null
}

$headers = @{ Authorization = "Bearer $($script:token)" }

# ─── 3. REST - USER POLICIES CONTROLLER ───────────────────────────────────────
Write-Host "`n[3] REST UserPolicies Controller" -ForegroundColor Yellow

$script:policyId = $null
$userSysId = Get-Random -Minimum 5000 -Maximum 99999

Test-Endpoint "GET /api/UserPolicies - returns all policies" {
    $r = Invoke-Api "$baseUrl/api/UserPolicies" -Headers $headers
    $r -is [array] -and $r.Count -ge 4
}

Test-Endpoint "GET /api/UserPolicies?policyType=SECURITY - filter by type" {
    $r = Invoke-Api "$baseUrl/api/UserPolicies?policyType=SECURITY" -Headers $headers
    $r -is [array] -and $r.Count -ge 1
}

Test-Endpoint "GET /api/UserPolicies/{id} - find by ID (seed -1)" {
    $r = Invoke-Api "$baseUrl/api/UserPolicies/-1" -Headers $headers
    $r.policyId -eq -1 -and $r.policyCode -eq "SECURITY_DEFAULT" -and $r.policyStatus -eq "A"
}

Test-Endpoint "POST /api/UserPolicies - create policy" {
    $body = @{
        userSysId          = $userSysId
        policyCode         = "TEST-POLICY-$runId"
        policyType         = "SECURITY"
        effectiveFrom      = "2026-01-01"
        createdBy          = 1
        dataRetentionDays  = 90
        sessionTimeoutMins = 30
        maxLoginAttempts   = 5
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/UserPolicies" -Method POST -Body $body -Headers $headers
    $script:policyId = $r.policyId
    $r.policyId -gt 0 -and $r.policyCode -eq "TEST-POLICY-$runId" -and $r.policyStatus -eq "A"
}

Test-Endpoint "GET /api/UserPolicies/{id} - verify created" {
    if (-not $script:policyId) { return $false }
    $r = Invoke-Api "$baseUrl/api/UserPolicies/$($script:policyId)" -Headers $headers
    $r.policyId -eq $script:policyId -and $r.userSysId -eq $userSysId
}

Test-Endpoint "PUT /api/UserPolicies/{id} - update policy" {
    if (-not $script:policyId) { return $false }
    $body = @{
        policyId           = $script:policyId
        policyType         = "ACCESS_CONTROL"
        dataRetentionDays  = 180
        sessionTimeoutMins = 60
        maxLoginAttempts   = 3
        updatedBy          = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/UserPolicies/$($script:policyId)" -Method PUT -Body $body -Headers $headers
    $r.dataRetentionDays -eq 180 -and $r.sessionTimeoutMins -eq 60
}

Test-Endpoint "DELETE /api/UserPolicies/{id} - deactivate policy" {
    if (-not $script:policyId) { return $false }
    $resp = Invoke-ApiRaw "$baseUrl/api/UserPolicies/$($script:policyId)?deletedBy=1" -Method DELETE -Headers $headers
    $resp.StatusCode -eq 204
}

Test-Endpoint "GET /api/UserPolicies/{id} - verify deactivated" {
    if (-not $script:policyId) { return $false }
    $r = Invoke-Api "$baseUrl/api/UserPolicies/$($script:policyId)" -Headers $headers
    $r.policyStatus -eq "I"
}

Test-Endpoint "GET /api/UserPolicies/999999 - 404 for nonexistent" {
    try { Invoke-Api "$baseUrl/api/UserPolicies/999999" -Headers $headers; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 404 }
}

Test-Endpoint "GET /api/UserPolicies - 401 without auth" {
    try { Invoke-Api "$baseUrl/api/UserPolicies"; return $false }
    catch { $_.Exception.Response.StatusCode.value__ -eq 401 }
}

# ─── 4. REST - WEBSITE CONTACTS CONTROLLER ────────────────────────────────────
Write-Host "`n[4] REST WebsiteContacts Controller" -ForegroundColor Yellow

$script:contactId = $null
$contactUserSysId = Get-Random -Minimum 6000 -Maximum 99999

Test-Endpoint "GET /api/WebsiteContacts/{id} - find by ID (seed -1)" {
    $r = Invoke-Api "$baseUrl/api/WebsiteContacts/-1" -Headers $headers
    $r.contactId -eq -1 -and $r.primaryEmail -eq "admin@sparsh.local"
}

Test-Endpoint "GET /api/WebsiteContacts/user/{userSysId} - by user (seed 1001)" {
    $r = Invoke-Api "$baseUrl/api/WebsiteContacts/user/1001" -Headers $headers
    $r -is [array] -and $r.Count -ge 1
}

Test-Endpoint "POST /api/WebsiteContacts - create contact" {
    $body = @{
        userSysId      = $contactUserSysId
        primaryEmail   = "test-$runId@sparsh.local"
        createdBy      = 1
        phone          = "+91-22-12345678"
        mobile         = "+91-9000000099"
        newsletterOptIn = $true
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/WebsiteContacts" -Method POST -Body $body -Headers $headers
    $script:contactId = $r.contactId
    $r.contactId -gt 0 -and $r.primaryEmail -eq "test-$runId@sparsh.local" -and $r.contactStatus -eq "A"
}

Test-Endpoint "GET /api/WebsiteContacts/{id} - verify created contact" {
    if (-not $script:contactId) { return $false }
    $r = Invoke-Api "$baseUrl/api/WebsiteContacts/$($script:contactId)" -Headers $headers
    $r.contactId -eq $script:contactId -and $r.userSysId -eq $contactUserSysId
}

Test-Endpoint "PUT /api/WebsiteContacts/{id} - update contact" {
    if (-not $script:contactId) { return $false }
    $body = @{
        contactId       = $script:contactId
        secondaryEmail  = "secondary-$runId@sparsh.local"
        phone           = "+91-22-99999999"
        mobile          = "+91-9000000088"
        newsletterOptIn = $false
        updatedBy       = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/WebsiteContacts/$($script:contactId)" -Method PUT -Body $body -Headers $headers
    $r.secondaryEmail -eq "secondary-$runId@sparsh.local" -and $r.newsletterOptIn -eq "N"
}

Test-Endpoint "GET /api/WebsiteContacts/user/{userSysId} - verify contact by user" {
    if (-not $script:contactId) { return $false }
    $r = Invoke-Api "$baseUrl/api/WebsiteContacts/user/$contactUserSysId" -Headers $headers
    $r -is [array] -and $r.Count -ge 1
}

# ─── 5. REST - PROFILE HISTORY CONTROLLER ─────────────────────────────────────
Write-Host "`n[5] REST ProfileHistory Controller" -ForegroundColor Yellow

Test-Endpoint "GET /api/ProfileHistory/user/{userSysId} - by user" {
    $r = Invoke-Api "$baseUrl/api/ProfileHistory/user/$userSysId" -Headers $headers
    $r -is [array]
}

Test-Endpoint "GET /api/ProfileHistory/policy/{policyId} - by policy" {
    if (-not $script:policyId) { return $false }
    $r = Invoke-Api "$baseUrl/api/ProfileHistory/policy/$($script:policyId)" -Headers $headers
    $r -is [array]
}

# ─── 6. MINIMAL API v2 - POLICIES ─────────────────────────────────────────────
Write-Host "`n[6] Minimal API v2 - Policies" -ForegroundColor Yellow

$script:minPolicyId = $null
$minUserSysId = Get-Random -Minimum 7000 -Maximum 99999

Test-Endpoint "GET /api/v2/policies - list all" {
    $r = Invoke-Api "$baseUrl/api/v2/policies" -Headers $headers
    $r -is [array] -and $r.Count -ge 4
}

Test-Endpoint "GET /api/v2/policies/{id} - find by ID" {
    $r = Invoke-Api "$baseUrl/api/v2/policies/-2" -Headers $headers
    $r.policyId -eq -2 -and $r.policyCode -eq "NOTIFICATION_EMAIL"
}

Test-Endpoint "POST /api/v2/policies - create via minimal API" {
    $body = @{
        userSysId     = $minUserSysId
        policyCode    = "MINAPI-$runId"
        policyType    = "NOTIFICATION"
        effectiveFrom = "2026-01-01"
        createdBy     = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/v2/policies" -Method POST -Body $body -Headers $headers
    $script:minPolicyId = $r.policyId
    $r.policyId -gt 0 -and $r.policyCode -eq "MINAPI-$runId"
}

Test-Endpoint "PUT /api/v2/policies/{id} - update via minimal API" {
    if (-not $script:minPolicyId) { return $false }
    $body = @{
        policyId      = $script:minPolicyId
        policyType    = "PREFERENCES"
        updatedBy     = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/v2/policies/$($script:minPolicyId)" -Method PUT -Body $body -Headers $headers
    $r.policyType -eq "PREFERENCES"
}

Test-Endpoint "DELETE /api/v2/policies/{id} - deactivate via minimal API" {
    if (-not $script:minPolicyId) { return $false }
    $resp = Invoke-ApiRaw "$baseUrl/api/v2/policies/$($script:minPolicyId)?deletedBy=1" -Method DELETE -Headers $headers
    $resp.StatusCode -eq 204
}

# ─── 7. MINIMAL API v2 - CONTACTS ─────────────────────────────────────────────
Write-Host "`n[7] Minimal API v2 - Contacts" -ForegroundColor Yellow

$minContactUserSysId = Get-Random -Minimum 8000 -Maximum 99999

Test-Endpoint "POST /api/v2/contacts - create via minimal API" {
    $body = @{
        userSysId    = $minContactUserSysId
        primaryEmail = "minapi-$runId@sparsh.local"
        createdBy    = 1
    } | ConvertTo-Json
    $r = Invoke-Api "$baseUrl/api/v2/contacts" -Method POST -Body $body -Headers $headers
    $r.contactId -gt 0 -and $r.primaryEmail -eq "minapi-$runId@sparsh.local"
}

Test-Endpoint "GET /api/v2/contacts/user/{userSysId} - by user via minimal API" {
    $r = Invoke-Api "$baseUrl/api/v2/contacts/user/$minContactUserSysId" -Headers $headers
    $r -is [array] -and $r.Count -ge 1
}

# ─── 8. GRAPHQL QUERIES ───────────────────────────────────────────────────────
Write-Host "`n[8] GraphQL Queries" -ForegroundColor Yellow

Test-Endpoint "GraphQL query userPolicies" {
    $r = Invoke-GQL '{ userPolicies(policyType: null) { policyId policyCode policyType policyStatus } }'
    $r.data.userPolicies -is [array] -and $r.data.userPolicies.Count -ge 4
}

Test-Endpoint "GraphQL query userPolicies with policyType filter" {
    $r = Invoke-GQL '{ userPolicies(policyType: "SECURITY") { policyId policyCode policyType } }'
    $r.data.userPolicies -is [array] -and $r.data.userPolicies.Count -ge 1
}

Test-Endpoint "GraphQL query userPolicy by ID" {
    $r = Invoke-GQL '{ userPolicy(policyId: -1) { policyId policyCode userSysId policyStatus dataRetentionDays sessionTimeoutMins maxLoginAttempts } }'
    $r.data.userPolicy.policyId -eq -1 -and $r.data.userPolicy.policyCode -eq "SECURITY_DEFAULT"
}

Test-Endpoint "GraphQL query websiteContact by ID" {
    $r = Invoke-GQL '{ websiteContact(contactId: -1) { contactId primaryEmail userSysId contactStatus newsletterOptIn } }'
    $r.data.websiteContact.contactId -eq -1 -and $r.data.websiteContact.primaryEmail -eq "admin@sparsh.local"
}

Test-Endpoint "GraphQL query userContacts by userSysId" {
    $r = Invoke-GQL '{ userContacts(userSysId: 1001) { contactId primaryEmail contactStatus } }'
    $r.data.userContacts -is [array] -and $r.data.userContacts.Count -ge 1
}

Test-Endpoint "GraphQL query profileHistory by user" {
    $r = Invoke-GQL "{ profileHistory(userSysId: $userSysId) { histId profileField oldValue newValue changedBy } }"
    $r.data.profileHistory -is [array]
}

# ─── 9. GRAPHQL MUTATIONS ─────────────────────────────────────────────────────
Write-Host "`n[9] GraphQL Mutations" -ForegroundColor Yellow

$gqlUserSysId = Get-Random -Minimum 9000 -Maximum 99999
$script:gqlPolicyId = $null

Test-Endpoint "GraphQL mutation createUserPolicy" {
    $r = Invoke-GQL "mutation { createUserPolicy(input: { userSysId: $gqlUserSysId, policyCode: `"GQL-POLICY-$runId`", policyType: `"NOTIFICATION`", effectiveFrom: `"2026-01-01`", createdBy: 1, dataRetentionDays: 120, sessionTimeoutMins: 45, maxLoginAttempts: 3 }) { policyId policyCode policyStatus } }"
    $script:gqlPolicyId = $r.data.createUserPolicy.policyId
    $r.data.createUserPolicy.policyId -gt 0 -and $r.data.createUserPolicy.policyCode -eq "GQL-POLICY-$runId"
}

Test-Endpoint "GraphQL mutation updateUserPolicy" {
    if (-not $script:gqlPolicyId) { return $false }
    $r = Invoke-GQL "mutation { updateUserPolicy(input: { policyId: $($script:gqlPolicyId), policyType: `"PREFERENCES`", dataRetentionDays: 365, updatedBy: 1 }) { policyId policyType dataRetentionDays } }"
    $r.data.updateUserPolicy.policyType -eq "PREFERENCES" -and $r.data.updateUserPolicy.dataRetentionDays -eq 365
}

Test-Endpoint "GraphQL mutation deleteUserPolicy" {
    if (-not $script:gqlPolicyId) { return $false }
    $r = Invoke-GQL "mutation { deleteUserPolicy(policyId: $($script:gqlPolicyId), deletedBy: 1) }"
    $r.data.deleteUserPolicy -eq $true
}

$gqlContactUserSysId = Get-Random -Minimum 10000 -Maximum 99999

Test-Endpoint "GraphQL mutation createWebsiteContact" {
    $r = Invoke-GQL "mutation { createWebsiteContact(input: { userSysId: $gqlContactUserSysId, primaryEmail: `"gql-$runId@sparsh.local`", createdBy: 1, newsletterOptIn: true }) { contactId primaryEmail contactStatus newsletterOptIn } }"
    $r.data.createWebsiteContact.contactId -gt 0 -and $r.data.createWebsiteContact.primaryEmail -eq "gql-$runId@sparsh.local"
}

Test-Endpoint "GraphQL mutation updateWebsiteContact" {
    $q = "{ userContacts(userSysId: $gqlContactUserSysId) { contactId } }"
    $r = Invoke-GQL $q
    $cid = $r.data.userContacts[0].contactId
    $r2 = Invoke-GQL "mutation { updateWebsiteContact(input: { contactId: $cid, phone: `"+91-22-55555555`", newsletterOptIn: false, updatedBy: 1 }) { contactId phone newsletterOptIn } }"
    $r2.data.updateWebsiteContact.phone -eq "+91-22-55555555" -and $r2.data.updateWebsiteContact.newsletterOptIn -eq "N"
}

# ─── 10. RABBITMQ ─────────────────────────────────────────────────────────────
Write-Host "`n[10] RabbitMQ" -ForegroundColor Yellow

Test-Endpoint "GET /api/rabbitmq/test - returns status" {
    $r = Invoke-Api "$baseUrl/api/rabbitmq/test"
    $r.service -eq "RabbitMQ" -and ($r.status -eq "Available" -or $r.status -eq "Disconnected")
}

# ─── SUMMARY ──────────────────────────────────────────────────────────────────
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  Results: $($script:passed)/$($script:total) passed" -ForegroundColor $(if ($script:failed -eq 0) { "Green" } else { "Yellow" })
if ($script:failed -gt 0) {
    Write-Host "  Failed: $($script:failed)" -ForegroundColor Red
}
Write-Host "========================================`n" -ForegroundColor Cyan
