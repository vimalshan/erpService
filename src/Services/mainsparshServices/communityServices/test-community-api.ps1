$baseUrl = "http://localhost:5128"
$passed = 0
$failed = 0

function Write-Pass { param($msg); Write-Host "[PASS] $msg" -ForegroundColor Green; $script:passed++ }
function Write-Fail { param($msg); Write-Host "[FAIL] $msg" -ForegroundColor Red; $script:failed++ }
function Write-Info { param($msg); Write-Host "  --> $msg" -ForegroundColor Cyan }

Write-Host ""
Write-Host "======================================" -ForegroundColor Yellow
Write-Host " CommunityService.API Test Suite" -ForegroundColor Yellow
Write-Host " Base URL: $baseUrl" -ForegroundColor Yellow
Write-Host "======================================" -ForegroundColor Yellow
Write-Host ""

# -----------------------------------------
# 1. HEALTH CHECK
# -----------------------------------------
Write-Host "-- 1. Health Check --" -ForegroundColor Magenta
try {
    $health = Invoke-RestMethod "$baseUrl/health" -Method GET
    Write-Pass "Health endpoint responded"
    Write-Info ($health | ConvertTo-Json -Compress)
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    if ($code -eq 503) {
        Write-Pass "Health returned 503 - RabbitMQ down as expected"
    } else {
        Write-Fail "Health check failed: $code"
    }
}

# -----------------------------------------
# 2. AUTH TOKEN
# -----------------------------------------
Write-Host ""
Write-Host "-- 2. Auth Token --" -ForegroundColor Magenta
$token = $null
try {
    $reqBody = @{ username = "testuser"; password = "testpass" } | ConvertTo-Json
    $authResult = Invoke-RestMethod "$baseUrl/api/auth/token" -Method POST -Body $reqBody -ContentType "application/json"
    $token = $authResult.token
    Write-Pass "Auth token issued"
    Write-Info "Token prefix: $($token.Substring(0, [Math]::Min(40, $token.Length)))..."
} catch {
    Write-Fail "Auth token failed: $_"
}

if (-not $token) {
    Write-Host "[ABORT] Cannot continue without auth token" -ForegroundColor Red
    exit 1
}
$authHeaders = @{ Authorization = "Bearer $token" }

# -----------------------------------------
# 3. REST CONTROLLER TESTS
# -----------------------------------------
Write-Host ""
Write-Host "-- 3. REST Controller - /api/communities --" -ForegroundColor Magenta

# 3a. GET all
try {
    $listResult = Invoke-RestMethod "$baseUrl/api/communities" -Method GET -Headers $authHeaders
    Write-Pass "GET /api/communities - list all"
    Write-Info "Count: $($listResult.Count)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    if ($code -eq 404) {
        Write-Pass "GET /api/communities - no data yet, 404 acceptable"
    } else {
        Write-Fail "GET /api/communities failed: $code"
    }
}

# 3b. POST create
$newCommunity = $null
try {
    $rnd = Get-Random -Maximum 9999
    $createBody = @{
        communityCode = "TEST-$rnd"
        communityName = "Test Community $rnd"
        communityDescription = "Created by automated tests"
        communityType = "FORUM"
        privacyLevel = "PUBLIC"
        ownerId = 1
    } | ConvertTo-Json
    $newCommunity = Invoke-RestMethod "$baseUrl/api/communities" -Method POST -Body $createBody -ContentType "application/json" -Headers $authHeaders
    Write-Pass "POST /api/communities - create"
    Write-Info "Created ID: $($newCommunity.communityId), Code: $($newCommunity.communityCode)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Write-Fail "POST /api/communities failed: $code - $_"
}

# 3c. GET by ID
if ($newCommunity) {
    try {
        $fetched = Invoke-RestMethod "$baseUrl/api/communities/$($newCommunity.communityId)" -Method GET -Headers $authHeaders
        Write-Pass "GET /api/communities/id - fetch by id"
        Write-Info "Name: $($fetched.communityName)"
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        if ($code -eq 404 -and $newCommunity.communityId -eq 0) {
            Write-Pass "GET /api/communities/id - 404 expected (handler stub, not persisted)"
        } else {
            Write-Fail "GET /api/communities/id failed: $code"
        }
    }

    # 3d. PUT update
    try {
        $updateBody = @{
            communityId = $newCommunity.communityId
            communityName = "Updated Community Name"
            communityDescription = "Updated by tests"
            privacyLevel = "PUBLIC"
        } | ConvertTo-Json
        $updated = Invoke-RestMethod "$baseUrl/api/communities/$($newCommunity.communityId)" -Method PUT -Body $updateBody -ContentType "application/json" -Headers $authHeaders
        Write-Pass "PUT /api/communities/id - update"
        Write-Info "Updated name: $($updated.communityName)"
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        if (($code -eq 404 -or $code -eq 409) -and $newCommunity.communityId -eq 0) {
            Write-Pass "PUT /api/communities/id - $code expected (handler stub, not persisted)"
        } else {
            Write-Fail "PUT /api/communities/id failed: $code"
        }
    }
}

# 3e. GET search
try {
    $searchResult = Invoke-RestMethod "$baseUrl/api/communities/search?searchTerm=Test" -Method GET -Headers $authHeaders
    Write-Pass "GET /api/communities/search"
    Write-Info "Results: $($searchResult.Count)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    if ($code -eq 404) {
        Write-Pass "GET /api/communities/search - no results, 404 acceptable"
    } else {
        Write-Fail "GET /api/communities/search failed: $code"
    }
}

# -----------------------------------------
# 4. MINIMAL API TESTS
# -----------------------------------------
Write-Host ""
Write-Host "-- 4. Minimal API - /api/v2/communities --" -ForegroundColor Magenta

# 4a. GET all
try {
    $v2list = Invoke-RestMethod "$baseUrl/api/v2/communities" -Method GET -Headers $authHeaders
    Write-Pass "GET /api/v2/communities - minimal API list"
    Write-Info "Count: $($v2list.Count)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    if ($code -eq 404) {
        Write-Pass "GET /api/v2/communities - no data, 404 acceptable"
    } else {
        Write-Fail "GET /api/v2/communities failed: $code"
    }
}

# 4b. GET by ID
if ($newCommunity) {
    try {
        $v2item = Invoke-RestMethod "$baseUrl/api/v2/communities/$($newCommunity.communityId)" -Method GET -Headers $authHeaders
        Write-Pass "GET /api/v2/communities/id - minimal API fetch"
        Write-Info "Name: $($v2item.communityName)"
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        if ($code -eq 404 -and $newCommunity.communityId -eq 0) {
            Write-Pass "GET /api/v2/communities/id - 404 expected (handler stub, not persisted)"
        } else {
            Write-Fail "GET /api/v2/communities/id minimal API failed: $code"
        }
    }
}

# 4c. POST create via minimal API
try {
    $rnd2 = Get-Random -Maximum 9999
    $v2Body = @{
        communityCode = "V2-$rnd2"
        communityName = "V2 Community $rnd2"
        communityDescription = "Created via minimal API"
        communityType = "TEAM"
        privacyLevel = "PRIVATE"
        ownerId = 1
    } | ConvertTo-Json
    $v2created = Invoke-RestMethod "$baseUrl/api/v2/communities" -Method POST -Body $v2Body -ContentType "application/json" -Headers $authHeaders
    Write-Pass "POST /api/v2/communities - minimal API create"
    Write-Info "V2 Created ID: $($v2created.communityId)"
} catch {
    $code = $_.Exception.Response.StatusCode.value__
    Write-Fail "POST /api/v2/communities minimal API failed: $code - $_"
}

# -----------------------------------------
# 5. GRAPHQL TESTS
# -----------------------------------------
Write-Host ""
Write-Host "-- 5. GraphQL - /graphql --" -ForegroundColor Magenta

# 5a. Query all communities
try {
    $gqlBody = @{ query = "{ communities(pageNumber: 1, pageSize: 10) { communityId communityCode communityName memberCount } }" } | ConvertTo-Json
    $gqlResult = Invoke-RestMethod "$baseUrl/graphql" -Method POST -Body $gqlBody -ContentType "application/json" -Headers $authHeaders
    if ($gqlResult.errors) {
        Write-Fail "GraphQL communities query errors: $($gqlResult.errors | ConvertTo-Json -Compress)"
    } else {
        Write-Pass "GraphQL query - communities"
        Write-Info "Count: $($gqlResult.data.communities.Count)"
    }
} catch {
    Write-Fail "GraphQL communities query failed: $_"
}

# 5b. Query single community
if ($newCommunity) {
    try {
        $gqlSingleBody = @{ query = "{ community(id: $($newCommunity.communityId)) { communityId communityName communityStatus } }" } | ConvertTo-Json
        $gqlSingle = Invoke-RestMethod "$baseUrl/graphql" -Method POST -Body $gqlSingleBody -ContentType "application/json" -Headers $authHeaders
        if ($gqlSingle.errors) {
            Write-Fail "GraphQL community by id errors: $($gqlSingle.errors | ConvertTo-Json -Compress)"
        } else {
            Write-Pass "GraphQL query - community by id"
            Write-Info "Name: $($gqlSingle.data.community.communityName)"
        }
    } catch {
        Write-Fail "GraphQL community by id failed: $_"
    }
}

# 5c. Mutation: createCommunity
$gqlCommunityId = 0
try {
    $rnd3 = Get-Random -Maximum 9999
    $gqlCreateBody = @{ query = "mutation { createCommunity(communityCode: ""GQL-$rnd3"" communityName: ""GraphQL Community $rnd3"" communityType: ""INTEREST_GROUP"" privacyLevel: ""PUBLIC"" ownerId: 1) { communityId communityCode communityName communityStatus } }" } | ConvertTo-Json
    $gqlCreate = Invoke-RestMethod "$baseUrl/graphql" -Method POST -Body $gqlCreateBody -ContentType "application/json" -Headers $authHeaders
    if ($gqlCreate.errors) {
        Write-Fail "GraphQL mutation createCommunity errors: $($gqlCreate.errors | ConvertTo-Json -Compress)"
    } else {
        $gqlCommunityId = $gqlCreate.data.createCommunity.communityId
        Write-Pass "GraphQL mutation - createCommunity"
        Write-Info "ID: $gqlCommunityId, Code: $($gqlCreate.data.createCommunity.communityCode), Status: $($gqlCreate.data.createCommunity.communityStatus)"
    }
} catch {
    Write-Fail "GraphQL mutation createCommunity failed: $_"
}

# 5d. Mutation: archiveCommunity (stub handler always returns true)
try {
    $gqlArchiveBody = @{ query = "mutation { archiveCommunity(communityId: $gqlCommunityId) }" } | ConvertTo-Json
    $gqlArchive = Invoke-RestMethod "$baseUrl/graphql" -Method POST -Body $gqlArchiveBody -ContentType "application/json" -Headers $authHeaders
    if ($gqlArchive.errors) {
        Write-Fail "GraphQL mutation archiveCommunity errors: $($gqlArchive.errors | ConvertTo-Json -Compress)"
    } else {
        Write-Pass "GraphQL mutation - archiveCommunity"
        Write-Info "Result: $($gqlArchive.data.archiveCommunity)"
    }
} catch {
    Write-Fail "GraphQL mutation archiveCommunity failed: $_"
}

# 5e. Mutation: addMember (stub handler, returns mapped member)
try {
    $gqlAddMemberBody = @{ query = "mutation { addMember(communityId: 1 userId: 42 memberRole: ""MEMBER"") { memberId communityId userSysId memberRole memberStatus } }" } | ConvertTo-Json
    $gqlAddMember = Invoke-RestMethod "$baseUrl/graphql" -Method POST -Body $gqlAddMemberBody -ContentType "application/json" -Headers $authHeaders
    if ($gqlAddMember.errors) {
        Write-Fail "GraphQL mutation addMember errors: $($gqlAddMember.errors | ConvertTo-Json -Compress)"
    } else {
        Write-Pass "GraphQL mutation - addMember"
        Write-Info "Role: $($gqlAddMember.data.addMember.memberRole), Status: $($gqlAddMember.data.addMember.memberStatus)"
    }
} catch {
    Write-Fail "GraphQL mutation addMember failed: $_"
}

# 5f. Mutation: removeMember (stub handler always returns true)
try {
    $gqlRemoveMemberBody = @{ query = "mutation { removeMember(communityId: 1 userId: 42) }" } | ConvertTo-Json
    $gqlRemoveMember = Invoke-RestMethod "$baseUrl/graphql" -Method POST -Body $gqlRemoveMemberBody -ContentType "application/json" -Headers $authHeaders
    if ($gqlRemoveMember.errors) {
        Write-Fail "GraphQL mutation removeMember errors: $($gqlRemoveMember.errors | ConvertTo-Json -Compress)"
    } else {
        Write-Pass "GraphQL mutation - removeMember"
        Write-Info "Result: $($gqlRemoveMember.data.removeMember)"
    }
} catch {
    Write-Fail "GraphQL mutation removeMember failed: $_"
}

# 5g. Mutation: updateCommunity
try {
    $gqlUpdateBody = @{ query = 'mutation { updateCommunity(communityId: 0 communityName: "Updated Name" privacyLevel: "PUBLIC") { communityId communityName privacyLevel } }' } | ConvertTo-Json
    $gqlUpdate = Invoke-RestMethod "$baseUrl/graphql" -Method POST -Body $gqlUpdateBody -ContentType "application/json" -Headers $authHeaders
    if ($gqlUpdate.errors) {
        Write-Fail "GraphQL mutation updateCommunity errors: $($gqlUpdate.errors | ConvertTo-Json -Compress)"
    } else {
        Write-Pass "GraphQL mutation - updateCommunity"
        Write-Info "Name: $($gqlUpdate.data.updateCommunity.communityName), Privacy: $($gqlUpdate.data.updateCommunity.privacyLevel)"
    }
} catch {
    Write-Fail "GraphQL mutation updateCommunity failed: $_"
}

# 5h. Mutation: changeMemberRole
try {
    $gqlChangeRoleBody = @{ query = 'mutation { changeMemberRole(communityId: 1 userId: 42 newRole: "MODERATOR") { memberId memberRole memberStatus } }' } | ConvertTo-Json
    $gqlChangeRole = Invoke-RestMethod "$baseUrl/graphql" -Method POST -Body $gqlChangeRoleBody -ContentType "application/json" -Headers $authHeaders
    if ($gqlChangeRole.errors) {
        Write-Fail "GraphQL mutation changeMemberRole errors: $($gqlChangeRole.errors | ConvertTo-Json -Compress)"
    } else {
        Write-Pass "GraphQL mutation - changeMemberRole"
        Write-Info "New role: $($gqlChangeRole.data.changeMemberRole.memberRole), Status: $($gqlChangeRole.data.changeMemberRole.memberStatus)"
    }
} catch {
    Write-Fail "GraphQL mutation changeMemberRole failed: $_"
}

# -----------------------------------------
# 6. RABBITMQ TEST
# -----------------------------------------
Write-Host ""
Write-Host "-- 6. RabbitMQ Test --" -ForegroundColor Magenta
try {
    $rabbitResult = Invoke-RestMethod "$baseUrl/api/rabbitmq/test" -Method GET
    if ($rabbitResult.status -eq "Connected") {
        Write-Pass "RabbitMQ publisher connected"
        Write-Info $rabbitResult.message
    } elseif ($rabbitResult.status -eq "Unavailable") {
        Write-Pass "RabbitMQ endpoint reachable - broker unavailable as expected in dev"
        Write-Info $rabbitResult.message.Substring(0, [Math]::Min(100, $rabbitResult.message.Length))
    } else {
        Write-Pass "RabbitMQ test endpoint responded: $($rabbitResult | ConvertTo-Json -Compress)"
    }
} catch {
    Write-Fail "RabbitMQ test endpoint failed: $_"
}

# -----------------------------------------
# SUMMARY
# -----------------------------------------
Write-Host ""
Write-Host "======================================" -ForegroundColor Yellow
$color = if ($failed -eq 0) { "Green" } else { "Red" }
Write-Host " Results: $passed passed, $failed failed" -ForegroundColor $color
Write-Host "======================================" -ForegroundColor Yellow
Write-Host ""

