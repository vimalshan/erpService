###############################################################################
#  EmailNotification Service – Comprehensive Test Suite
#  Base URL : http://localhost:5031
#  Covers   : Auth, EmailTypes CRUD, MailAccess (Recipients), GraphQL, Health
###############################################################################

$base = "http://localhost:5031"
$pass = 0; $fail = 0; $results = @()

function Test($name, $block) {
    try {
        & $block
        $script:pass++; $script:results += "PASS: $name"
        Write-Host "PASS: $name" -F Green
    } catch {
        $script:fail++; $script:results += "FAIL: $name - $_"
        Write-Host "FAIL: $name - $_" -F Red
    }
}

function GQL($query, $variables = $null, $token = $null) {
    $body = @{ query = $query }
    if ($variables) { $body.variables = $variables }
    $json = $body | ConvertTo-Json -Depth 10 -Compress
    $headers = @{ "Content-Type" = "application/json" }
    if ($token) { $headers["Authorization"] = "Bearer $token" }
    $r = Invoke-RestMethod "$base/graphql" -Method POST -Headers $headers -Body $json -ErrorAction Stop
    if ($r.errors) { throw ($r.errors | ConvertTo-Json -Depth 5) }
    return $r.data
}

###############################################################################
Write-Host "`n--- Authentication ---" -F Cyan
###############################################################################

$adminToken = $null

Test "POST /auth/login (admin)" {
    $r = Invoke-RestMethod "$base/api/auth/login" -Method POST -ContentType "application/json" `
         -Body '{"username":"admin","password":"Admin@123"}'
    if (-not $r.accessToken) { throw "No accessToken" }
    $script:adminToken = $r.accessToken
    Write-Host "  Token obtained (${($r.accessToken.Length)} chars)"
}

Test "POST /auth/login (invalid creds → still returns token)" {
    # AuthController is a placeholder – accepts any non-empty creds
    $r = Invoke-RestMethod "$base/api/auth/login" -Method POST -ContentType "application/json" `
         -Body '{"username":"testuser","password":"testpass"}'
    if (-not $r.accessToken) { throw "No accessToken" }
    Write-Host "  Placeholder auth: returns token for any credentials"
}

$authHeader = @{ "Authorization" = "Bearer $adminToken"; "Content-Type" = "application/json" }

###############################################################################
Write-Host "`n--- Email Types (GET) ---" -F Cyan
###############################################################################

$allTypes = $null

Test "GET /emailtypes (get all)" {
    $r = Invoke-RestMethod "$base/api/emailtypes" -ErrorAction Stop
    if ($r.Count -lt 2) { throw "Expected at least 2 email types, got $($r.Count)" }
    $script:allTypes = $r
    Write-Host "  Found $($r.Count) email types"
}

$firstId = $null

Test "GET /emailtypes/{id} (get by ID)" {
    $id = $allTypes[0].id
    $script:firstId = $id
    $r = Invoke-RestMethod "$base/api/emailtypes/$id" -ErrorAction Stop
    if ($r.id -ne $id) { throw "ID mismatch: expected $id got $($r.id)" }
    Write-Host "  Email type ${id}: $($r.emailName) ($($r.emailType))"
}

Test "GET /emailtypes/bytype/Daily" {
    $r = Invoke-RestMethod "$base/api/emailtypes/bytype/Daily" -ErrorAction Stop
    if ($r.Count -lt 1) { throw "Expected at least 1 Daily type" }
    Write-Host "  Found $($r.Count) Daily email types"
}

Test "GET /emailtypes/bytype/Event" {
    $r = Invoke-RestMethod "$base/api/emailtypes/bytype/Event" -ErrorAction Stop
    if ($r.Count -lt 1) { throw "Expected at least 1 Event type" }
    Write-Host "  Found $($r.Count) Event email types"
}

Test "GET /emailtypes/{id} (not found → 404)" {
    try {
        Invoke-WebRequest "$base/api/emailtypes/999999" -UseBasicParsing -ErrorAction Stop | Out-Null
        throw "Expected 404"
    } catch {
        if ($_.Exception.Response.StatusCode.value__ -ne 404) { throw "Expected 404, got $($_.Exception.Response.StatusCode.value__)" }
        Write-Host "  Correctly returned 404"
    }
}

###############################################################################
Write-Host "`n--- Email Types (Create / Update) ---" -F Cyan
###############################################################################

$createdTypeId = $null

Test "POST /emailtypes (create new type)" {
    $body = @{
        emailName     = "Test Notification Type"
        emailType     = "E"
        emailProcName = "usp_TestNotification"
        createdBy     = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/api/emailtypes" -Method POST -Headers $authHeader -Body $body -ErrorAction Stop
    if ($r -lt 1) { throw "Expected created ID > 0, got $r" }
    $script:createdTypeId = $r
    Write-Host "  Created email type ID: $r"
}

Test "GET /emailtypes/{id} (verify created)" {
    $r = Invoke-RestMethod "$base/api/emailtypes/$createdTypeId" -ErrorAction Stop
    if ($r.emailName -ne "Test Notification Type") { throw "Name mismatch" }
    if ($r.emailProcName -ne "usp_TestNotification") { throw "ProcName mismatch" }
    Write-Host "  Verified: $($r.emailName), Proc=$($r.emailProcName)"
}

Test "PUT /emailtypes/{id} (update)" {
    $body = @{
        id            = $createdTypeId
        emailName     = "Updated Test Notification"
        emailProcName = "usp_UpdatedTestNotification"
        modifiedBy    = 2
    } | ConvertTo-Json
    $status = (Invoke-WebRequest "$base/api/emailtypes/$createdTypeId" -Method PUT -Headers $authHeader -Body $body -UseBasicParsing -ErrorAction Stop).StatusCode
    if ($status -ne 204) { throw "Expected 204, got $status" }
    Write-Host "  Updated email type $createdTypeId"
}

Test "GET /emailtypes/{id} (verify updated)" {
    $r = Invoke-RestMethod "$base/api/emailtypes/$createdTypeId" -ErrorAction Stop
    if ($r.emailName -ne "Updated Test Notification") { throw "Name not updated" }
    if ($r.emailProcName -ne "usp_UpdatedTestNotification") { throw "ProcName not updated" }
    Write-Host "  Verified: $($r.emailName), Proc=$($r.emailProcName)"
}

###############################################################################
Write-Host "`n--- Recipients (Mail Access) ---" -F Cyan
###############################################################################

$recipientId = $null

Test "POST /emailtypes/{id}/recipients (add recipient)" {
    $body = @{
        emailTypeId   = $createdTypeId
        emailAddress  = "test.recipient@bankxyz.com"
        orgId         = 1
        businessId    = 1
        recipientName = "Test Recipient"
        createdBy     = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/api/emailtypes/$createdTypeId/recipients" -Method POST -Headers $authHeader -Body $body -ErrorAction Stop
    if ($r -lt 1) { throw "Expected recipient ID > 0, got $r" }
    $script:recipientId = $r
    Write-Host "  Added recipient ID: $r"
}

Test "POST /emailtypes/{id}/recipients (add 2nd recipient)" {
    $body = @{
        emailTypeId   = $createdTypeId
        emailAddress  = "another.recipient@bankxyz.com"
        orgId         = 2
        businessId    = $null
        recipientName = "Another Recipient"
        createdBy     = 1
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/api/emailtypes/$createdTypeId/recipients" -Method POST -Headers $authHeader -Body $body -ErrorAction Stop
    if ($r -lt 1) { throw "Expected recipient ID > 0" }
    Write-Host "  Added 2nd recipient ID: $r"
}

Test "GET /emailtypes/{id}/recipients/byorg (org filter)" {
    $r = Invoke-RestMethod "$base/api/emailtypes/$createdTypeId/recipients/byorg?orgId=1&businessId=1" -ErrorAction Stop
    if ($r.Count -lt 1) { throw "Expected at least 1 recipient for org=1,biz=1" }
    Write-Host "  Found $($r.Count) recipient(s) for org=1, business=1"
}

Test "GET /emailtypes/{id}/recipients/byorg (org 2 filter)" {
    $r = Invoke-RestMethod "$base/api/emailtypes/$createdTypeId/recipients/byorg?orgId=2" -ErrorAction Stop
    if ($r.Count -lt 1) { throw "Expected at least 1 recipient for org=2" }
    Write-Host "  Found $($r.Count) recipient(s) for org=2"
}

# Test recipients for seed data (Daily Treasury Report - first email type)
Test "GET /emailtypes/{id}/recipients/byorg (seed data - org 1)" {
    $r = Invoke-RestMethod "$base/api/emailtypes/$firstId/recipients/byorg?orgId=1" -ErrorAction Stop
    Write-Host "  Seed data: $($r.Count) recipient(s) for org=1 on email type $firstId"
}

Test "DELETE /emailtypes/{id}/recipients/{rid} (remove recipient)" {
    $body = @{
        mailAccessId = $recipientId
        modifiedBy   = 1
    } | ConvertTo-Json
    $status = (Invoke-WebRequest "$base/api/emailtypes/$createdTypeId/recipients/$recipientId" -Method DELETE -Headers $authHeader -Body $body -UseBasicParsing -ErrorAction Stop).StatusCode
    if ($status -ne 204) { throw "Expected 204, got $status" }
    Write-Host "  Removed recipient $recipientId"
}

###############################################################################
Write-Host "`n--- GraphQL Queries ---" -F Cyan
###############################################################################

Test "GQL: emailTypes (all)" {
    $d = GQL '{ emailTypes { id emailName emailType emailProcName } }'
    if ($d.emailTypes.Count -lt 2) { throw "Expected at least 2 email types" }
    Write-Host "  Found $($d.emailTypes.Count) email types"
}

Test "GQL: emailType (by ID)" {
    $d = GQL "{ emailType(id: $firstId) { id emailName emailType recipients { id mailEmail } } }"
    if ($d.emailType.id -ne $firstId) { throw "ID mismatch" }
    Write-Host "  Email type ${firstId}: $($d.emailType.emailName) with $($d.emailType.recipients.Count) recipient(s)"
}

Test "GQL: emailTypesByType (Daily)" {
    $d = GQL '{ emailTypesByType(emailType: "Daily") { id emailName } }'
    if ($d.emailTypesByType.Count -lt 1) { throw "Expected at least 1 Daily type" }
    Write-Host "  Found $($d.emailTypesByType.Count) Daily email type(s)"
}

Test "GQL: emailTypesByType (Event)" {
    $d = GQL '{ emailTypesByType(emailType: "Event") { id emailName } }'
    if ($d.emailTypesByType.Count -lt 1) { throw "Expected at least 1 Event type" }
    Write-Host "  Found $($d.emailTypesByType.Count) Event email type(s)"
}

Test "GQL: recipients (by org/business)" {
    $d = GQL "{ recipients(emailTypeId: $firstId, orgId: 1, businessId: 1) { id mailEmail mailName } }"
    Write-Host "  Found $($d.recipients.Count) recipient(s) for org=1, biz=1"
}

###############################################################################
Write-Host "`n--- GraphQL Mutations ---" -F Cyan
###############################################################################

$gqlTypeId = $null

Test "GQL Mutation: createEmailType" {
    $q = 'mutation($input: CreateEmailTypeInput!) { createEmailType(input: $input) }'
    $v = @{
        input = @{
            emailName     = "GQL Test Email"
            emailType     = "D"
            emailProcName = "usp_GqlTestEmail"
            createdBy     = 1
        }
    }
    $d = GQL $q $v $adminToken
    if ($d.createEmailType -lt 1) { throw "Expected ID > 0" }
    $script:gqlTypeId = $d.createEmailType
    Write-Host "  Created GQL email type ID: $($d.createEmailType)"
}

Test "GQL Mutation: updateEmailType" {
    $q = 'mutation($input: UpdateEmailTypeInput!) { updateEmailType(input: $input) }'
    $v = @{
        input = @{
            id            = $gqlTypeId
            emailName     = "GQL Updated Email"
            emailProcName = "usp_GqlUpdatedEmail"
            modifiedBy    = 2
        }
    }
    $d = GQL $q $v $adminToken
    if ($d.updateEmailType -ne $true) { throw "Expected true" }
    Write-Host "  Updated GQL email type $gqlTypeId"
}

Test "GQL: verify updateEmailType" {
    $d = GQL "{ emailType(id: $gqlTypeId) { id emailName emailProcName } }"
    if ($d.emailType.emailName -ne "GQL Updated Email") { throw "Name not updated" }
    Write-Host "  Verified: $($d.emailType.emailName)"
}

$gqlRecipientId = $null

Test "GQL Mutation: addRecipient" {
    $q = 'mutation($input: AddRecipientInput!) { addRecipient(input: $input) }'
    $v = @{
        input = @{
            emailTypeId   = $gqlTypeId
            emailAddress  = "gql.recipient@bankxyz.com"
            orgId         = 1
            businessId    = 1
            recipientName = "GQL Test Recipient"
            createdBy     = 1
        }
    }
    $d = GQL $q $v $adminToken
    if ($d.addRecipient -lt 1) { throw "Expected recipient ID > 0" }
    $script:gqlRecipientId = $d.addRecipient
    Write-Host "  Added GQL recipient ID: $($d.addRecipient)"
}

Test "GQL Mutation: removeRecipient" {
    $q = "mutation { removeRecipient(mailAccessId: $gqlRecipientId, modifiedBy: 1) }"
    $d = GQL $q $null $adminToken
    if ($d.removeRecipient -ne $true) { throw "Expected true" }
    Write-Host "  Removed GQL recipient $gqlRecipientId"
}

###############################################################################
Write-Host "`n--- Health ---" -F Cyan
###############################################################################

Test "GET /health" {
    $r = Invoke-RestMethod "$base/health" -ErrorAction Stop
    if ($r -ne "Healthy") { throw "Expected Healthy, got $r" }
    Write-Host "  Health: $r"
}

###############################################################################
Write-Host "`n========================================"
Write-Host " RESULTS: $pass PASSED, $fail FAILED"
Write-Host "========================================" 
$results | ForEach-Object { 
    if ($_ -match "^PASS") { Write-Host $_ -F Green } else { Write-Host $_ -F Red }
}
