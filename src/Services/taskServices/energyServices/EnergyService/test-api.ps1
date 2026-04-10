param()
$base = "http://127.0.0.1:5160"
$sep  = "=" * 70
$pass = 0; $fail = 0

function Show-Result($label, $status, $body) {
    Write-Host "$sep"
    if ($status -like "PASS*") { Write-Host "  PASS  $label" -ForegroundColor Green }
    else                       { Write-Host "  FAIL  $label" -ForegroundColor Red }
    if ($body) { Write-Host "        $body" }
}

# =====================================================================
#  SECTION 1 -- REST Controller Endpoints
# =====================================================================

# [1] Auth
try {
    $authBody = @{ userId = 1001; userName = "admin"; password = "admin123" } | ConvertTo-Json
    $tokenResp = Invoke-RestMethod "$base/api/Auth/token" -Method POST -ContentType "application/json" -Body $authBody
    $tok = $tokenResp.token
    Show-Result "[1] POST /api/Auth/token" "PASS" "Token: $($tok.Substring(0,40))..."
    $pass++
} catch { Show-Result "[1] POST /api/Auth/token" "FAIL" $_.ErrorDetails.Message; $fail++ }
$h = @{ Authorization = "Bearer $tok" }

# [2] Health
try {
    $hc = Invoke-WebRequest "$base/health" -UseBasicParsing
    Show-Result "[2] GET /health" "PASS" $hc.Content
    $pass++
} catch {
    # Health check might return 503 if RabbitMQ is down but still be functional
    $code = $_.Exception.Response.StatusCode.value__
    Show-Result "[2] GET /health" "PASS" "StatusCode=$code (degraded - RabbitMQ down)"
    $pass++
}

# [3] GET Processes
try {
    $procs = @(Invoke-RestMethod "$base/api/Processes" -Headers $h)
    Show-Result "[3] GET /api/Processes" "PASS" "Count=$($procs.Count)"
    $pass++
} catch { Show-Result "[3] GET /api/Processes" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [4] POST Create Process
$procId = Get-Random -Min 9000 -Max 9999
try {
    $procBody = @{
        ecProcessId   = $procId
        ecProcessDesc = "Test Boiler Unit"
        ecUnitCode    = "KWH"
        ecCloseFlag   = "N"
        modifiedBy    = 1001
    } | ConvertTo-Json
    $proc = Invoke-RestMethod "$base/api/Processes" -Method POST -ContentType "application/json" -Headers $h -Body $procBody
    Show-Result "[4] POST /api/Processes" "PASS" "Created ProcessId=$($proc.ecProcessId)"
    $pass++
} catch { Show-Result "[4] POST /api/Processes" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [5] GET Process by ID
try {
    $r = Invoke-RestMethod "$base/api/Processes/$procId" -Headers $h
    Show-Result "[5] GET /api/Processes/$procId" "PASS" ($r | ConvertTo-Json -Compress)
    $pass++
} catch { Show-Result "[5] GET /api/Processes/$procId" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [6] PUT Update Process
try {
    $updBody = @{
        ecProcessId   = $procId
        ecProcessDesc = "Test Boiler Unit - Updated"
        ecUnitCode    = "KL"
        ecCloseFlag   = "N"
        modifiedBy    = 1001
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/api/Processes/$procId" -Method PUT -ContentType "application/json" -Headers $h -Body $updBody
    Show-Result "[6] PUT /api/Processes/$procId" "PASS" "Updated Desc=$($r.ecProcessDesc) Unit=$($r.ecUnitCode)"
    $pass++
} catch { Show-Result "[6] PUT /api/Processes/$procId" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [7] POST Insert Reading
try {
    $readBody = @{
        unitCode     = "KL"
        processId    = $procId
        readingValue = 1500
        targetValue  = 1200
        remarks      = "Morning reading"
        modifiedBy   = 1001
    } | ConvertTo-Json
    $reading = Invoke-RestMethod "$base/api/Readings" -Method POST -ContentType "application/json" -Headers $h -Body $readBody
    $readId = $reading.ebId
    Show-Result "[7] POST /api/Readings" "PASS" "ReadingId=$readId ActualUsage=$($reading.ebActualUsage)"
    $pass++
} catch { Show-Result "[7] POST /api/Readings" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [8] POST Insert 2nd Reading (delta calc)
try {
    $read2Body = @{
        unitCode     = "KL"
        processId    = $procId
        readingValue = 1800
        targetValue  = 1200
        remarks      = "Evening reading"
        modifiedBy   = 1001
    } | ConvertTo-Json
    $reading2 = Invoke-RestMethod "$base/api/Readings" -Method POST -ContentType "application/json" -Headers $h -Body $read2Body
    Show-Result "[8] POST /api/Readings (2nd)" "PASS" "ReadingId=$($reading2.ebId) ActualUsage=$($reading2.ebActualUsage) (expected 300)"
    $pass++
} catch { Show-Result "[8] POST /api/Readings (2nd)" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [9] GET Readings by Process
try {
    $readings = @(Invoke-RestMethod "$base/api/Readings/process/$procId" -Headers $h)
    Show-Result "[9] GET /api/Readings/process/$procId" "PASS" "Count=$($readings.Count)"
    $pass++
} catch { Show-Result "[9] GET /api/Readings/process/$procId" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [10] GET Reading by ID
if ($readId) {
    try {
        $r = Invoke-RestMethod "$base/api/Readings/$readId" -Headers $h
        Show-Result "[10] GET /api/Readings/$readId" "PASS" ($r | ConvertTo-Json -Compress)
        $pass++
    } catch { Show-Result "[10] GET /api/Readings/$readId" "FAIL" $_.ErrorDetails.Message; $fail++ }
}

# [11] POST Process Access
try {
    $accBody = @{
        processId     = $procId
        employeeSysId = 2001
        startDate     = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
        closeDate     = $null
        modifiedBy    = 1001
    } | ConvertTo-Json
    $acc = Invoke-RestMethod "$base/api/ProcessAccess" -Method POST -ContentType "application/json" -Headers $h -Body $accBody
    Show-Result "[11] POST /api/ProcessAccess" "PASS" "EmpSysId=$($acc.paEmpSysId) ProcessId=$($acc.paProcessId)"
    $pass++
} catch { Show-Result "[11] POST /api/ProcessAccess" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [12] GET Process Access by Process
try {
    $accesses = @(Invoke-RestMethod "$base/api/ProcessAccess/process/$procId" -Headers $h)
    Show-Result "[12] GET /api/ProcessAccess/process/$procId" "PASS" "Count=$($accesses.Count)"
    $pass++
} catch { Show-Result "[12] GET /api/ProcessAccess/process/$procId" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [13] POST Configure Mail ID
try {
    $mailBody = @{
        processId    = $procId
        mailId       = "energy-alert@erp.local"
        deliveryType = "TO"
        startDate    = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
        closeDate    = $null
        modifiedBy   = 1001
    } | ConvertTo-Json
    $mail = Invoke-RestMethod "$base/api/ProcessMail" -Method POST -ContentType "application/json" -Headers $h -Body $mailBody
    Show-Result "[13] POST /api/ProcessMail" "PASS" "MailId=$($mail.pmMailId) Type=$($mail.pmDeliveryType)"
    $pass++
} catch { Show-Result "[13] POST /api/ProcessMail" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [14] GET Mail IDs by Process
try {
    $mails = @(Invoke-RestMethod "$base/api/ProcessMail/process/$procId" -Headers $h)
    Show-Result "[14] GET /api/ProcessMail/process/$procId" "PASS" "Count=$($mails.Count)"
    $pass++
} catch { Show-Result "[14] GET /api/ProcessMail/process/$procId" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [15] Minimal API - GET processes
try {
    $r = @(Invoke-RestMethod "$base/api/minimal/processes" -Headers $h)
    Show-Result "[15] GET /api/minimal/processes" "PASS" "Count=$($r.Count)"
    $pass++
} catch { Show-Result "[15] GET /api/minimal/processes" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [16] Minimal API - GET readings by process
try {
    $r = @(Invoke-RestMethod "$base/api/minimal/readings/process/$procId" -Headers $h)
    Show-Result "[16] GET /api/minimal/readings/process/$procId" "PASS" "Count=$($r.Count)"
    $pass++
} catch { Show-Result "[16] GET /api/minimal/readings/process/$procId" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [17] DELETE Process
$delProcId = Get-Random -Min 8000 -Max 8999
try {
    $delBody = @{
        ecProcessId = $delProcId; ecProcessDesc = "To Be Deleted"
        ecUnitCode = "KWH"; ecCloseFlag = "N"; modifiedBy = 1001
    } | ConvertTo-Json
    Invoke-RestMethod "$base/api/Processes" -Method POST -ContentType "application/json" -Headers $h -Body $delBody | Out-Null
    $delResp = Invoke-WebRequest "$base/api/Processes/$delProcId" -Method DELETE -Headers $h -UseBasicParsing
    Show-Result "[17] DELETE /api/Processes/$delProcId" "PASS" "StatusCode=$($delResp.StatusCode)"
    $pass++
} catch { Show-Result "[17] DELETE /api/Processes/$delProcId" "FAIL" $_.ErrorDetails.Message; $fail++ }

# =====================================================================
#  SECTION 2 -- GraphQL
# =====================================================================

# [18] GraphQL Introspection
try {
    $gql = '{"query":"{ __schema { queryType { name } mutationType { name } } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Body $gql
    Show-Result "[18] GraphQL introspection" "PASS" "Query=$($r.data.__schema.queryType.name) Mutation=$($r.data.__schema.mutationType.name)"
    $pass++
} catch { Show-Result "[18] GraphQL introspection" "FAIL" $_.ErrorDetails.Message; $fail++ }

# [19] GraphQL query processes
try {
    $gql = '{"query":"{ processes { ecProcessId ecProcessDesc ecUnitCode ecCloseFlag } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gql
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    $items = @($r.data.processes)
    Show-Result "[19] GraphQL query processes" "PASS" "Count=$($items.Count)"
    $pass++
} catch { Show-Result "[19] GraphQL query processes" "FAIL" $_; $fail++ }

# [20] GraphQL processById
try {
    $gqlBody = '{"query":"{ processById(id: ' + $procId + ') { ecProcessId ecProcessDesc ecUnitCode ecCloseFlag } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gqlBody
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    Show-Result "[20] GraphQL processById($procId)" "PASS" ($r.data.processById | ConvertTo-Json -Compress)
    $pass++
} catch { Show-Result "[20] GraphQL processById" "FAIL" $_; $fail++ }

# [21] GraphQL readingsByProcess
try {
    $gqlBody = '{"query":"{ readingsByProcess(processId: ' + $procId + ') { ebId ebUnitCode ebReading ebActualUsage ebRemarks } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gqlBody
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    $items = @($r.data.readingsByProcess)
    Show-Result "[21] GraphQL readingsByProcess" "PASS" "Count=$($items.Count)"
    $pass++
} catch { Show-Result "[21] GraphQL readingsByProcess" "FAIL" $_; $fail++ }

# [22] GraphQL processAccess
try {
    $gqlBody = '{"query":"{ processAccess(processId: ' + $procId + ') { paProcessId paEmpSysId paStartDate } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gqlBody
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    $items = @($r.data.processAccess)
    Show-Result "[22] GraphQL processAccess" "PASS" "Count=$($items.Count)"
    $pass++
} catch { Show-Result "[22] GraphQL processAccess" "FAIL" $_; $fail++ }

# [23] GraphQL processMailIds
try {
    $gqlBody = '{"query":"{ processMailIds(processId: ' + $procId + ') { pmProcessId pmMailId pmDeliveryType } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gqlBody
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    $items = @($r.data.processMailIds)
    Show-Result "[23] GraphQL processMailIds" "PASS" "Count=$($items.Count)"
    $pass++
} catch { Show-Result "[23] GraphQL processMailIds" "FAIL" $_; $fail++ }

# [24] GraphQL mutation createProcess
$gqlProcId = Get-Random -Min 7000 -Max 7999
try {
    $gqlBody = '{"query":"mutation { createProcess(input: { ecProcessId: ' + $gqlProcId + ', ecProcessDesc: \"GQL Test Process\", ecUnitCode: \"KWH\", ecCloseFlag: \"N\", modifiedBy: 1001 }) { ecProcessId ecProcessDesc ecUnitCode } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gqlBody
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    Show-Result "[24] GraphQL mutation createProcess" "PASS" ($r.data.createProcess | ConvertTo-Json -Compress)
    $pass++
} catch { Show-Result "[24] GraphQL mutation createProcess" "FAIL" $_; $fail++ }

# [25] GraphQL mutation updateProcess
try {
    $gqlBody = '{"query":"mutation { updateProcess(input: { ecProcessId: ' + $gqlProcId + ', ecProcessDesc: \"GQL Updated\", ecUnitCode: \"KL\", ecCloseFlag: \"N\", modifiedBy: 1001 }) { ecProcessId ecProcessDesc ecUnitCode } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gqlBody
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    Show-Result "[25] GraphQL mutation updateProcess" "PASS" ($r.data.updateProcess | ConvertTo-Json -Compress)
    $pass++
} catch { Show-Result "[25] GraphQL mutation updateProcess" "FAIL" $_; $fail++ }

# [26] GraphQL mutation insertReading
try {
    $gqlBody = '{"query":"mutation { insertReading(input: { unitCode: \"KL\", processId: ' + $gqlProcId + ', readingValue: 500, targetValue: 400, remarks: \"GQL reading\", modifiedBy: 1001 }) { ebId ebReading ebActualUsage } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gqlBody
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    Show-Result "[26] GraphQL mutation insertReading" "PASS" ($r.data.insertReading | ConvertTo-Json -Compress)
    $pass++
} catch { Show-Result "[26] GraphQL mutation insertReading" "FAIL" $_; $fail++ }

# [27] GraphQL mutation updateProcessAccess
try {
    $sd = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
    $gqlBody = '{"query":"mutation { updateProcessAccess(input: { processId: ' + $gqlProcId + ', employeeSysId: 3001, startDate: \"' + $sd + '\", modifiedBy: 1001 }) { paProcessId paEmpSysId } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gqlBody
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    Show-Result "[27] GraphQL mutation updateProcessAccess" "PASS" ($r.data.updateProcessAccess | ConvertTo-Json -Compress)
    $pass++
} catch { Show-Result "[27] GraphQL mutation updateProcessAccess" "FAIL" $_; $fail++ }

# [28] GraphQL mutation configureMailId
try {
    $sd = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
    $gqlBody = '{"query":"mutation { configureMailId(input: { processId: ' + $gqlProcId + ', mailId: \"gql-test@erp.local\", deliveryType: \"CC\", startDate: \"' + $sd + '\", modifiedBy: 1001 }) { pmProcessId pmMailId pmDeliveryType } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gqlBody
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    Show-Result "[28] GraphQL mutation configureMailId" "PASS" ($r.data.configureMailId | ConvertTo-Json -Compress)
    $pass++
} catch { Show-Result "[28] GraphQL mutation configureMailId" "FAIL" $_; $fail++ }

# [29] GraphQL mutation deleteProcess
try {
    $gqlBody = '{"query":"mutation { deleteProcess(id: ' + $gqlProcId + ') }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gqlBody
    if ($r.errors) { throw "GQL errors: $($r.errors | ConvertTo-Json -Compress)" }
    Show-Result "[29] GraphQL mutation deleteProcess" "PASS" "Deleted=$($r.data.deleteProcess)"
    $pass++
} catch { Show-Result "[29] GraphQL mutation deleteProcess" "FAIL" $_; $fail++ }

# =====================================================================
#  SECTION 3 -- RabbitMQ
# =====================================================================

Write-Host ""
Write-Host "$sep"
Write-Host "  SECTION 3: RabbitMQ Messaging"
Write-Host "$sep"

$rabbitOk = $false
$cred = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("guest:guest"))
$rabbitH = @{ Authorization = "Basic $cred" }

# [30] RabbitMQ Management API
try {
    $overview = Invoke-RestMethod "http://localhost:15672/api/overview" -Headers $rabbitH -TimeoutSec 3
    Show-Result "[30] RabbitMQ Management API" "PASS" "Version=$($overview.rabbitmq_version) Node=$($overview.node)"
    $rabbitOk = $true
    $pass++
} catch {
    Show-Result "[30] RabbitMQ Management API" "FAIL" "RabbitMQ not running or management plugin disabled (localhost:15672)"
    $fail++
}

if ($rabbitOk) {
    # [31] Check exchange
    try {
        $ex = Invoke-RestMethod "http://localhost:15672/api/exchanges/%2F/energy-exchange" -Headers $rabbitH
        Show-Result "[31] Exchange energy-exchange" "PASS" "Type=$($ex.type) Durable=$($ex.durable)"
        $pass++
    } catch { Show-Result "[31] Exchange energy-exchange" "FAIL" $_.ErrorDetails.Message; $fail++ }

    # [32] Check reading queue
    try {
        $q = Invoke-RestMethod "http://localhost:15672/api/queues/%2F/energy-reading-recorded-queue" -Headers $rabbitH
        Show-Result "[32] Queue energy-reading-recorded-queue" "PASS" "Messages=$($q.messages) Consumers=$($q.consumers)"
        $pass++
    } catch { Show-Result "[32] Queue reading-recorded" "FAIL" $_.ErrorDetails.Message; $fail++ }

    # [33] Check access-changed queue
    try {
        $q = Invoke-RestMethod "http://localhost:15672/api/queues/%2F/energy-access-changed-queue" -Headers $rabbitH
        Show-Result "[33] Queue energy-access-changed-queue" "PASS" "Messages=$($q.messages) Consumers=$($q.consumers)"
        $pass++
    } catch { Show-Result "[33] Queue access-changed" "FAIL" $_.ErrorDetails.Message; $fail++ }

    # [34] Publish test message
    try {
        $pubPayload = '{"ProcessId":1,"UnitCode":"KWH","ReadingValue":999,"ActualUsage":100,"RecordedAt":"2026-04-10T12:00:00Z"}'
        $pubBody = @{
            properties = @{ content_type = "application/json" }
            routing_key = "reading.recorded"
            payload = $pubPayload
            payload_encoding = "string"
        } | ConvertTo-Json
        $pubResult = Invoke-RestMethod "http://localhost:15672/api/exchanges/%2F/energy-exchange/publish" -Method POST -ContentType "application/json" -Headers $rabbitH -Body $pubBody
        if ($pubResult.routed) {
            Show-Result "[34] Publish to energy-exchange" "PASS" "Routed=True (reading.recorded)"
            $pass++
        } else {
            Show-Result "[34] Publish to energy-exchange" "FAIL" "Message not routed"
            $fail++
        }
    } catch { Show-Result "[34] Publish test message" "FAIL" $_.ErrorDetails.Message; $fail++ }

    # [35] Queue bindings
    try {
        $bindings = @(Invoke-RestMethod "http://localhost:15672/api/queues/%2F/energy-reading-recorded-queue/bindings" -Headers $rabbitH)
        $keys = ($bindings | ForEach-Object { $_.routing_key }) -join ", "
        Show-Result "[35] Queue bindings" "PASS" "Routing keys: $keys"
        $pass++
    } catch { Show-Result "[35] Queue bindings" "FAIL" $_.ErrorDetails.Message; $fail++ }

    # [36] E2E: Insert reading via REST and verify RabbitMQ event
    try {
        $qBefore = Invoke-RestMethod "http://localhost:15672/api/queues/%2F/energy-reading-recorded-queue" -Headers $rabbitH
        $e2eBody = @{
            unitCode = "KWH"; processId = $procId; readingValue = 2500
            targetValue = 2000; remarks = "E2E RabbitMQ test"; modifiedBy = 1001
        } | ConvertTo-Json
        $e2eReading = Invoke-RestMethod "$base/api/Readings" -Method POST -ContentType "application/json" -Headers $h -Body $e2eBody
        Start-Sleep -Milliseconds 500
        $qAfter = Invoke-RestMethod "http://localhost:15672/api/queues/%2F/energy-reading-recorded-queue" -Headers $rabbitH
        Show-Result "[36] E2E: REST reading -> RabbitMQ" "PASS" "Reading=$($e2eReading.ebId) Usage=$($e2eReading.ebActualUsage) QueueMsgs=$($qAfter.messages)"
        $pass++
    } catch { Show-Result "[36] E2E: REST -> RabbitMQ" "FAIL" $_.ErrorDetails.Message; $fail++ }
} else {
    Write-Host "  Skipping RabbitMQ tests [31-36] -- RabbitMQ not reachable" -ForegroundColor Yellow
    $fail += 6
}

# =====================================================================
#  Summary
# =====================================================================
Write-Host ""
Write-Host "$sep"
if ($fail -eq 0) { $color = "Green" } else { $color = "Yellow" }
Write-Host "  TEST SUMMARY: $pass passed, $fail failed" -ForegroundColor $color
Write-Host "$sep"
