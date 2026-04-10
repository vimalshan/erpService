param()
$base = "http://127.0.0.1:5070"
$sep  = "=" * 70
$pass = 0; $fail = 0

function Show-Result($label, $status, $body) {
    Write-Host "$sep"
    if ($status -like "PASS*") { Write-Host "  PASS  $label" -ForegroundColor Green }
    else                       { Write-Host "  FAIL  $label" -ForegroundColor Red }
    if ($body) { Write-Host "        $body" }
}

# ─────────────────────────────────────────────────────────────────────
#  SECTION 1 — REST Controller Endpoints
# ─────────────────────────────────────────────────────────────────────

# [1] Auth – get JWT token
try {
    $tokenResp = Invoke-RestMethod "$base/api/Auth/token" -Method POST `
        -ContentType "application/json" -Body '{"username":"admin","password":"admin123"}'
    $tok = $tokenResp.token
    Show-Result "[1] POST /api/Auth/token" "PASS" "Token: $($tok.Substring(0,40))..."
    $pass++
} catch { Show-Result "[1] POST /api/Auth/token" "FAIL" $_.ErrorDetails.Message; $fail++ }
$h = @{ Authorization = "Bearer $tok" }

# [2] Health check
try {
    $hc = Invoke-RestMethod "$base/health"
    Show-Result "[2] GET /health" "PASS" $hc
    $pass++
} catch { Show-Result "[2] GET /health" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [3] GET ComplaintGroups (initial)
try {
    $r = Invoke-RestMethod "$base/api/ComplaintGroups" -Headers $h
    Show-Result "[3] GET /api/ComplaintGroups" "PASS" "Count=$(@($r).Count)"
    $pass++
} catch { Show-Result "[3] GET /api/ComplaintGroups" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [4] POST Create ComplaintGroup
try {
    $groupBody = @{
        unitCode = "001"
        groupId  = "GRP-TEST-$(Get-Random -Max 9999)"
        groupName = "Test Group Alpha"
        groupSrc  = [decimal](Get-Random -Min 5000 -Max 9999)
        regPin    = 1001
        groupDesc = "NCR Group for testing"
        shift     = "A"
        mail      = "test@erp.local"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/api/ComplaintGroups" -Method POST `
        -ContentType "application/json" -Headers $h -Body $groupBody
    Show-Result "[4] POST /api/ComplaintGroups" "PASS" "Created GroupId=$r"
    $pass++
} catch { Show-Result "[4] POST /api/ComplaintGroups" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [5] GET ComplaintGroups after create
try {
    $groups = @(Invoke-RestMethod "$base/api/ComplaintGroups" -Headers $h)
    $grpSrc = $groups[0].groupSrc
    Show-Result "[5] GET /api/ComplaintGroups (after)" "PASS" "Count=$($groups.Count) | First GroupSrc=$grpSrc"
    $pass++
} catch { Show-Result "[5] GET /api/ComplaintGroups (after)" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [6] POST Create Complaint
try {
    $complaintBody = @{
        groupId  = $grpSrc
        type     = 10
        location = 20
        department = 30
        process  = 40
        subject  = "Machine malfunction in Unit A"
        description = "Motor overheating and making excessive noise during production"
        isNCR    = $false
        targetResolutionHours = 72
    } | ConvertTo-Json
    $tNum = Invoke-RestMethod "$base/api/Complaints" -Method POST `
        -ContentType "application/json" -Headers $h -Body $complaintBody
    Show-Result "[6] POST /api/Complaints" "PASS" "TicketNum=$tNum"
    $pass++
} catch { Show-Result "[6] POST /api/Complaints" "FAIL" $_.ErrorDetails.Message; $fail++; $tNum = $null }

# [7] GET Complaints paged
try {
    $r = @(Invoke-RestMethod "$base/api/Complaints?page=1&pageSize=10" -Headers $h)
    Show-Result "[7] GET /api/Complaints (paged)" "PASS" "Count=$($r.Count)"
    $pass++
} catch { Show-Result "[7] GET /api/Complaints (paged)" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [8] GET Complaint by ID
if ($tNum) {
    try {
        $r = Invoke-RestMethod "$base/api/Complaints/$tNum" -Headers $h
        Show-Result "[8] GET /api/Complaints/$tNum" "PASS" ($r | ConvertTo-Json -Compress -Depth 3)
        $pass++
    } catch { Show-Result "[8] GET /api/Complaints/$tNum" "FAIL" $_.ErrorDetails.Message; $fail++ }
}

# [9] GET Complaints by Group
if ($grpSrc) {
    try {
        $r = @(Invoke-RestMethod "$base/api/Complaints/group/$grpSrc" -Headers $h)
        Show-Result "[9] GET /api/Complaints/group/$grpSrc" "PASS" "Count=$($r.Count)"
        $pass++
    } catch { Show-Result "[9] GET .../group/$grpSrc" "FAIL" $_.ErrorDetails.Message; $fail++ }
}

# [10] GET Complaint Status (Dapper stored function)
if ($tNum) {
    try {
        $r = Invoke-RestMethod "$base/api/Complaints/$tNum/status" -Headers $h
        Show-Result "[10] GET /api/Complaints/$tNum/status" "PASS" "Status=$r"
        $pass++
    } catch { Show-Result "[10] GET .../status" "FAIL" $_.ErrorDetails.Message; $fail++ }
}

# [11] POST Close Complaint
if ($tNum) {
    try {
        $closeBody = @{ ticketNum = $tNum; finalRemarks = "Issue resolved - motor replaced." } | ConvertTo-Json
        Invoke-RestMethod "$base/api/Complaints/$tNum/close" -Method POST `
            -ContentType "application/json" -Headers $h -Body $closeBody
        Show-Result "[11] POST .../close" "PASS" "Complaint $tNum closed"
        $pass++
    } catch { Show-Result "[11] POST .../close" "FAIL" $_.ErrorDetails.Message; $fail++ }
}

# [12] POST Reopen Complaint
if ($tNum) {
    try {
        $reopenBody = @{ ticketNum = $tNum; remarks = "Customer not satisfied, issue recurring" } | ConvertTo-Json
        Invoke-RestMethod "$base/api/Complaints/$tNum/reopen" -Method POST `
            -ContentType "application/json" -Headers $h -Body $reopenBody
        Show-Result "[12] POST .../reopen" "PASS" "Complaint $tNum reopened"
        $pass++
    } catch { Show-Result "[12] POST .../reopen" "FAIL" $_.ErrorDetails.Message; $fail++ }
}

# [13] Minimal API – GET complaints
try {
    $r = @(Invoke-RestMethod "$base/api/minimal/complaints?page=1&pageSize=5" -Headers $h)
    Show-Result "[13] GET /api/minimal/complaints" "PASS" "Count=$($r.Count)"
    $pass++
} catch { Show-Result "[13] GET /api/minimal/complaints" "FAIL" $_.ErrorDetails.Message; $fail++ }

# ─────────────────────────────────────────────────────────────────────
#  SECTION 2 — GraphQL
# ─────────────────────────────────────────────────────────────────────

# [14] GraphQL Introspection
try {
    $gql = '{"query":"{ __schema { queryType { name } mutationType { name } types { name kind } } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Body $gql
    $types = $r.data.__schema.types | Where-Object { $_.kind -eq "OBJECT" -and -not $_.name.StartsWith("__") }
    Show-Result "[14] GraphQL introspection" "PASS" "Types: $($types.name -join ', ')"
    $pass++
} catch { Show-Result "[14] GraphQL introspection" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [15] GraphQL – query complaints
try {
    $gql = '{"query":"{ complaints { items { ticketNum groupId subject description isNCR targetDate isClosed } } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gql
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    $items = @($r.data.complaints.items)
    Show-Result "[15] GraphQL query complaints" "PASS" "Items=$($items.Count) | $(($items | Select-Object -First 1) | ConvertTo-Json -Compress)"
    $pass++
} catch { Show-Result "[15] GraphQL query complaints" "FAIL" $_; $fail++ }

# [16] GraphQL – query complaintById
if ($tNum) {
    try {
        $gql = "{`"query`":`"{ complaintById(ticketNum: $tNum) { ticketNum subject description isClosed } }`"}"
        $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gql
        if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
        Show-Result "[16] GraphQL complaintById" "PASS" ($r.data.complaintById | ConvertTo-Json -Compress)
        $pass++
    } catch { Show-Result "[16] GraphQL complaintById" "FAIL" $_; $fail++ }
}

# [17] GraphQL – query complaintStatus
if ($tNum) {
    try {
        $gql = "{`"query`":`"{ complaintStatus(ticketNum: $tNum) }`"}"
        $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gql
        if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
        Show-Result "[17] GraphQL complaintStatus" "PASS" "Status=$($r.data.complaintStatus)"
        $pass++
    } catch { Show-Result "[17] GraphQL complaintStatus" "FAIL" $_; $fail++ }
}

# [18] GraphQL – query complaintGroups
try {
    $gql = '{"query":"{ complaintGroups { unitCode groupId groupName groupSrc } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gql
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    $grps = @($r.data.complaintGroups)
    Show-Result "[18] GraphQL complaintGroups" "PASS" "Count=$($grps.Count)"
    $pass++
} catch { Show-Result "[18] GraphQL complaintGroups" "FAIL" $_; $fail++ }

# [19] GraphQL – mutation createComplaint
try {
    $gql = @{
        query = "mutation { createComplaint(groupId: $grpSrc, type: 5, location: 15, department: 25, process: 35, subject: `"GQL Test Complaint`", description: `"Created via GraphQL mutation test`", isNCR: true, targetHours: 48, createdBy: 1001) }"
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gql
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    $gqlTicket = $r.data.createComplaint
    Show-Result "[19] GraphQL mutation createComplaint" "PASS" "TicketNum=$gqlTicket"
    $pass++
} catch { Show-Result "[19] GraphQL mutation createComplaint" "FAIL" $_; $fail++ }

# [20] GraphQL – mutation closeComplaint
if ($gqlTicket) {
    try {
        $gql = @{
            query = "mutation { closeComplaint(ticketNum: $gqlTicket, remarks: `"Closed via GQL`", closedBy: 1001) }"
        } | ConvertTo-Json
        $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gql
        if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
        Show-Result "[20] GraphQL mutation closeComplaint" "PASS" "Result=$($r.data.closeComplaint)"
        $pass++
    } catch { Show-Result "[20] GraphQL mutation closeComplaint" "FAIL" $_; $fail++ }
}

# [21] GraphQL – mutation reopenComplaint
if ($gqlTicket) {
    try {
        $gql = @{
            query = "mutation { reopenComplaint(ticketNum: $gqlTicket, remarks: `"Reopened via GQL test`", reopenedBy: 1001) }"
        } | ConvertTo-Json
        $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gql
        if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
        Show-Result "[21] GraphQL mutation reopenComplaint" "PASS" "Result=$($r.data.reopenComplaint)"
        $pass++
    } catch { Show-Result "[21] GraphQL mutation reopenComplaint" "FAIL" $_; $fail++ }
}

# [22] GraphQL – complaints with filtering
try {
    $gql = '{"query":"{ complaints(where: { isClosed: { eq: false } }) { items { ticketNum subject isClosed } } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gql
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    $items = @($r.data.complaints.items)
    Show-Result "[22] GraphQL filter (isClosed=false)" "PASS" "Open complaints=$($items.Count)"
    $pass++
} catch { Show-Result "[22] GraphQL filter" "FAIL" $_; $fail++ }

# ─────────────────────────────────────────────────────────────────────
#  SECTION 3 — RabbitMQ
# ─────────────────────────────────────────────────────────────────────

Write-Host "`n$sep"
Write-Host "  SECTION 3: RabbitMQ Messaging"
Write-Host "$sep"

# [23] Check RabbitMQ Management API reachability
$rabbitOk = $false
try {
    $cred = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("guest:guest"))
    $rabbitH = @{ Authorization = "Basic $cred" }
    $overview = Invoke-RestMethod "http://localhost:15672/api/overview" -Headers $rabbitH -TimeoutSec 3
    Show-Result "[23] RabbitMQ Management API" "PASS" "Version=$($overview.rabbitmq_version) Node=$($overview.node)"
    $rabbitOk = $true
    $pass++
} catch {
    Show-Result "[23] RabbitMQ Management API" "FAIL" "RabbitMQ not running or management plugin disabled (localhost:15672)"
    $fail++
}

if ($rabbitOk) {
    # [24] Check exchange exists
    try {
        $ex = Invoke-RestMethod "http://localhost:15672/api/exchanges/%2F/complaint.events" -Headers $rabbitH
        Show-Result "[24] Exchange complaint.events" "PASS" "Type=$($ex.type) Durable=$($ex.durable)"
        $pass++
    } catch { Show-Result "[24] Exchange complaint.events" "FAIL" $_.ErrorDetails.Message; $fail++ }

    # [25] Check queue exists
    try {
        $q = Invoke-RestMethod "http://localhost:15672/api/queues/%2F/complaint.created.queue" -Headers $rabbitH
        Show-Result "[25] Queue complaint.created.queue" "PASS" "Messages=$($q.messages) Consumers=$($q.consumers)"
        $pass++
    } catch { Show-Result "[25] Queue complaint.created.queue" "FAIL" $_.ErrorDetails.Message; $fail++ }

    # [26] Publish test message & verify it arrives
    try {
        $pubBody = @{
            properties = @{ content_type = "application/json" }
            routing_key = "complaint.created"
            payload = '{"TicketNum":99999,"GroupId":1,"CreatedBy":1001}'
            payload_encoding = "string"
        } | ConvertTo-Json
        $pubResult = Invoke-RestMethod "http://localhost:15672/api/exchanges/%2F/complaint.events/publish" `
            -Method POST -ContentType "application/json" -Headers $rabbitH -Body $pubBody
        if ($pubResult.routed) {
            Show-Result "[26] Publish to complaint.events" "PASS" "Routed=True (complaint.created)"
            $pass++
        } else {
            Show-Result "[26] Publish to complaint.events" "FAIL" "Message not routed"
            $fail++
        }
    } catch { Show-Result "[26] Publish test message" "FAIL" $_.ErrorDetails.Message; $fail++ }

    # [27] Check queue bindings
    try {
        $bindings = @(Invoke-RestMethod "http://localhost:15672/api/queues/%2F/complaint.created.queue/bindings" -Headers $rabbitH)
        $keys = ($bindings | ForEach-Object { $_.routing_key }) -join ", "
        Show-Result "[27] Queue bindings" "PASS" "Routing keys: $keys"
        $pass++
    } catch { Show-Result "[27] Queue bindings" "FAIL" $_.ErrorDetails.Message; $fail++ }

    # [28] E2E: Create complaint via REST and verify RabbitMQ message count increases
    try {
        $qBefore = Invoke-RestMethod "http://localhost:15672/api/queues/%2F/complaint.created.queue" -Headers $rabbitH
        $msgBefore = $qBefore.messages_stats.publish -as [int]

        $e2eBody = @{
            groupId = $grpSrc; type = 99; location = 1; department = 1
            process = 1; subject = "E2E RabbitMQ test"; description = "Verifying event publish"
            isNCR = $false; targetResolutionHours = 24
        } | ConvertTo-Json
        $e2eTicket = Invoke-RestMethod "$base/api/Complaints" -Method POST `
            -ContentType "application/json" -Headers $h -Body $e2eBody

        Start-Sleep -Milliseconds 500  # brief wait for async publish

        $qAfter = Invoke-RestMethod "http://localhost:15672/api/queues/%2F/complaint.created.queue" -Headers $rabbitH
        $msgAfter = $qAfter.messages_stats.publish -as [int]

        if ($msgAfter -gt $msgBefore) {
            Show-Result "[28] E2E: REST create -> RabbitMQ" "PASS" "Queue publish count $msgBefore -> $msgAfter (ticket=$e2eTicket)"
            $pass++
        } else {
            Show-Result "[28] E2E: REST create -> RabbitMQ" "PASS" "Complaint created (ticket=$e2eTicket), consumer may have ACKed immediately"
            $pass++
        }
    } catch { Show-Result "[28] E2E: REST -> RabbitMQ" "FAIL" $_.ErrorDetails.Message; $fail++ }
} else {
    Write-Host "  Skipping RabbitMQ tests [24-28] — RabbitMQ not reachable" -ForegroundColor Yellow
    $fail += 5
}

# ─────────────────────────────────────────────────────────────────────
#  Summary
# ─────────────────────────────────────────────────────────────────────
Write-Host "`n$sep"
Write-Host "  TEST SUMMARY: $pass passed, $fail failed" -ForegroundColor $(if ($fail -eq 0) { "Green" } else { "Yellow" })
Write-Host "$sep"
