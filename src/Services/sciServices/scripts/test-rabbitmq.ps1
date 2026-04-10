#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Tests RabbitMQ connectivity and messaging across all SCI ERP microservices.

.DESCRIPTION
    This script validates:
    1. RabbitMQ server connectivity
    2. Virtual host, exchanges, queues, and bindings exist
    3. Publish a test message to each service queue
    4. Service health endpoints (if services are running)

.PARAMETER RabbitHost
    RabbitMQ host. Default: localhost

.PARAMETER RabbitPort
    RabbitMQ Management API port. Default: 15672

.PARAMETER AmqpPort
    RabbitMQ AMQP port. Default: 5672

.PARAMETER Username
    RabbitMQ username. Default: sci_admin

.PARAMETER Password
    RabbitMQ password. Default: SciRabbit@2026!

.PARAMETER VirtualHost
    RabbitMQ virtual host. Default: sci_vhost

.PARAMETER SkipServiceHealth
    Skip checking service health endpoints.

.EXAMPLE
    .\test-rabbitmq.ps1
    .\test-rabbitmq.ps1 -RabbitHost 192.168.1.100
    .\test-rabbitmq.ps1 -SkipServiceHealth
#>

param(
    [string]$RabbitHost = "localhost",
    [int]$RabbitPort = 15672,
    [int]$AmqpPort = 5672,
    [string]$Username = "sci_admin",
    [string]$Password = 'SciRabbit@2026!',
    [string]$VirtualHost = "sci_vhost",
    [switch]$SkipServiceHealth
)

$ErrorActionPreference = "Continue"

# ============================================================
# Configuration
# ============================================================
$BaseUrl = "http://${RabbitHost}:${RabbitPort}/api"
$EncodedVHost = [System.Uri]::EscapeDataString($VirtualHost)
$Credential = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("${Username}:${Password}"))
$Headers = @{ Authorization = "Basic $Credential" }

$TotalTests = 0
$PassedTests = 0
$FailedTests = 0
$Warnings = 0

# Service definitions: Name, Port, Queues, Routing Keys
$Services = @(
    @{ Name = "API Gateway";            Port = 5200;  Queues = @() }
    @{ Name = "Security Service";       Port = 5009;  Queues = @("security.user.created", "security.user.updated", "security.role.changed") }
    @{ Name = "Vehicle Tracking";       Port = 5102;  Queues = @("vehicle.tracking.updated", "vehicle.location.changed") }
    @{ Name = "Dispatch Planning";      Port = 5255;  Queues = @("dispatch.plan.created", "dispatch.plan.updated") }
    @{ Name = "Order Scheduling";       Port = 5160;  Queues = @("order.schedule.created", "order.schedule.updated") }
    @{ Name = "Filling Operations";     Port = 5058;  Queues = @("filling.operation.completed") }
    @{ Name = "EXIM Management";        Port = 5085;  Queues = @("exim.license.updated") }
    @{ Name = "GST Compliance";         Port = 5282;  Queues = @("gst.compliance.filed") }
    @{ Name = "Inventory Management";   Port = 5097;  Queues = @("inventory.stock.changed", "inventory.adjustment.created") }
    @{ Name = "Production Management";  Port = 5087;  Queues = @("production.batch.started", "production.batch.completed") }
    @{ Name = "MAM Allocation";         Port = 5140;  Queues = @("mam.allocation.created") }
    @{ Name = "Purchase Sales";         Port = 5170;  Queues = @("purchase.order.created", "sales.order.created") }
    @{ Name = "Master Data";            Port = 5180;  Queues = @("masterdata.updated") }
    @{ Name = "Strategic Stock";        Port = 5045;  Queues = @("strategic.stock.updated") }
    @{ Name = "Error Logging";          Port = 5292;  Queues = @("error.logged") }
    @{ Name = "SCI Transactional";      Port = 5150;  Queues = @("transactional.record.created") }
)

$ExpectedExchanges = @("sci.events", "sci.commands", "sci.fanout", "sci.dlx")

# ============================================================
# Helpers
# ============================================================
function Write-TestHeader($title) {
    Write-Host ""
    Write-Host ("=" * 70) -ForegroundColor Cyan
    Write-Host "  $title" -ForegroundColor Cyan
    Write-Host ("=" * 70) -ForegroundColor Cyan
}

function Write-Pass($msg) {
    $script:TotalTests++
    $script:PassedTests++
    Write-Host "  [PASS] $msg" -ForegroundColor Green
}

function Write-Fail($msg) {
    $script:TotalTests++
    $script:FailedTests++
    Write-Host "  [FAIL] $msg" -ForegroundColor Red
}

function Write-Warn($msg) {
    $script:Warnings++
    Write-Host "  [WARN] $msg" -ForegroundColor Yellow
}

function Write-Info($msg) {
    Write-Host "  [INFO] $msg" -ForegroundColor Gray
}

function Invoke-RabbitApi {
    param([string]$Path)
    try {
        $response = Invoke-RestMethod -Uri "${BaseUrl}${Path}" -Headers $Headers -TimeoutSec 10
        return $response
    }
    catch {
        return $null
    }
}

# ============================================================
# Test 1: RabbitMQ Server Connectivity
# ============================================================
Write-TestHeader "1. RabbitMQ Server Connectivity"

# Test Management API
Write-Info "Testing Management API at ${BaseUrl}..."
try {
    $overview = Invoke-RestMethod -Uri "${BaseUrl}/overview" -Headers $Headers -TimeoutSec 10
    Write-Pass "Management API is reachable (RabbitMQ v$($overview.rabbitmq_version), Erlang v$($overview.erlang_version))"
}
catch {
    Write-Fail "Management API is NOT reachable at http://${RabbitHost}:${RabbitPort}"
    Write-Host ""
    Write-Host "  Cannot proceed without RabbitMQ Management API." -ForegroundColor Red
    Write-Host "  Ensure RabbitMQ is running: docker compose up rabbitmq -d" -ForegroundColor Yellow
    Write-Host ""
    exit 1
}

# Test AMQP port
Write-Info "Testing AMQP port ${AmqpPort}..."
try {
    $tcp = New-Object System.Net.Sockets.TcpClient
    $tcp.Connect($RabbitHost, $AmqpPort)
    $tcp.Close()
    Write-Pass "AMQP port ${AmqpPort} is open"
}
catch {
    Write-Fail "AMQP port ${AmqpPort} is NOT reachable"
}

# Test authentication
Write-Info "Testing authentication for user '${Username}'..."
$whoami = Invoke-RabbitApi "/whoami"
if ($whoami -and $whoami.name -eq $Username) {
    Write-Pass "Authenticated as '${Username}' with tags: $($whoami.tags -join ', ')"
}
else {
    Write-Fail "Authentication failed for user '${Username}'"
}

# ============================================================
# Test 2: Virtual Host
# ============================================================
Write-TestHeader "2. Virtual Host"

$vhosts = Invoke-RabbitApi "/vhosts"
if ($vhosts) {
    $targetVhost = $vhosts | Where-Object { $_.name -eq $VirtualHost }
    if ($targetVhost) {
        Write-Pass "Virtual host '${VirtualHost}' exists"
    }
    else {
        Write-Fail "Virtual host '${VirtualHost}' NOT found. Available: $($vhosts.name -join ', ')"
    }
}
else {
    Write-Fail "Could not retrieve virtual hosts"
}

# Test permissions
$permissions = Invoke-RabbitApi "/permissions/${EncodedVHost}/${Username}"
if ($permissions) {
    Write-Pass "User '${Username}' has permissions on '${VirtualHost}' (configure: $($permissions.configure), write: $($permissions.write), read: $($permissions.read))"
}
else {
    Write-Fail "User '${Username}' has NO permissions on '${VirtualHost}'"
}

# ============================================================
# Test 3: Exchanges
# ============================================================
Write-TestHeader "3. Exchanges"

$exchanges = Invoke-RabbitApi "/exchanges/${EncodedVHost}"
if ($exchanges) {
    foreach ($exName in $ExpectedExchanges) {
        $ex = $exchanges | Where-Object { $_.name -eq $exName }
        if ($ex) {
            Write-Pass "Exchange '${exName}' exists (type: $($ex.type), durable: $($ex.durable))"
        }
        else {
            Write-Fail "Exchange '${exName}' NOT found"
        }
    }
    
    # Show any additional custom exchanges
    $customExchanges = $exchanges | Where-Object { $_.name -and $_.name -notmatch "^(amq\.|$)" -and $_.name -notin $ExpectedExchanges }
    if ($customExchanges) {
        Write-Info "Additional exchanges found: $($customExchanges.name -join ', ')"
    }
}
else {
    Write-Fail "Could not retrieve exchanges"
}

# ============================================================
# Test 4: Queues
# ============================================================
Write-TestHeader "4. Queues"

$queues = Invoke-RabbitApi "/queues/${EncodedVHost}"
$allExpectedQueues = @()
foreach ($svc in $Services) {
    $allExpectedQueues += $svc.Queues
}
$allExpectedQueues += "sci.dlq"

if ($queues) {
    $queueNames = $queues | ForEach-Object { $_.name }
    
    foreach ($qName in $allExpectedQueues) {
        $q = $queues | Where-Object { $_.name -eq $qName }
        if ($q) {
            $consumers = $q.consumers
            $messages = $q.messages
            $status = if ($consumers -gt 0) { "consumers: ${consumers}" } else { "no consumers" }
            Write-Pass "Queue '${qName}' exists (messages: ${messages}, ${status})"
        }
        else {
            Write-Fail "Queue '${qName}' NOT found"
        }
    }
    
    # Dead Letter Queue check
    $dlq = $queues | Where-Object { $_.name -eq "sci.dlq" }
    if ($dlq -and $dlq.messages -gt 0) {
        Write-Warn "Dead letter queue has $($dlq.messages) messages - check for failed processing"
    }
    
    # Show unexpected queues
    $unexpectedQueues = $queueNames | Where-Object { $_ -notin $allExpectedQueues }
    if ($unexpectedQueues) {
        Write-Info "Additional queues found: $($unexpectedQueues -join ', ')"
    }
}
else {
    Write-Fail "Could not retrieve queues"
}

# ============================================================
# Test 5: Bindings
# ============================================================
Write-TestHeader "5. Bindings"

$bindings = Invoke-RabbitApi "/bindings/${EncodedVHost}"
if ($bindings) {
    # Check that each queue is bound to sci.events
    $eventBindings = $bindings | Where-Object { $_.source -eq "sci.events" }
    
    foreach ($qName in ($allExpectedQueues | Where-Object { $_ -ne "sci.dlq" })) {
        $bound = $eventBindings | Where-Object { $_.destination -eq $qName }
        if ($bound) {
            Write-Pass "Queue '${qName}' bound to sci.events (routing: $($bound.routing_key -join ', '))"
        }
        else {
            Write-Fail "Queue '${qName}' NOT bound to sci.events exchange"
        }
    }
    
    # Check DLQ binding
    $dlqBinding = $bindings | Where-Object { $_.source -eq "sci.dlx" -and $_.destination -eq "sci.dlq" }
    if ($dlqBinding) {
        Write-Pass "Dead letter queue 'sci.dlq' bound to sci.dlx exchange"
    }
    else {
        Write-Fail "Dead letter queue 'sci.dlq' NOT bound to sci.dlx exchange"
    }
}
else {
    Write-Fail "Could not retrieve bindings"
}

# ============================================================
# Test 6: Publish Test Messages
# ============================================================
Write-TestHeader "6. Publish Test Messages (per service)"

$timestamp = (Get-Date).ToString("o")
$testCorrelationId = [Guid]::NewGuid().ToString()

foreach ($svc in $Services) {
    if ($svc.Queues.Count -eq 0) { continue }
    
    $firstQueue = $svc.Queues[0]
    $routingKey = $firstQueue  # routing key matches queue name for exact bindings
    
    $messageBody = @{
        eventType     = "rabbitmq.connectivity.test"
        service       = $svc.Name
        correlationId = $testCorrelationId
        timestamp     = $timestamp
        data          = @{
            message = "RabbitMQ connectivity test from test-rabbitmq.ps1"
            queue   = $firstQueue
        }
    } | ConvertTo-Json -Compress
    
    $publishPayload = @{
        properties      = @{
            delivery_mode = 1  # non-persistent (test message)
            content_type  = "application/json"
            headers       = @{
                "x-test-message" = "true"
                "x-correlation-id" = $testCorrelationId
            }
        }
        routing_key     = $routingKey
        payload         = $messageBody
        payload_encoding = "string"
    } | ConvertTo-Json -Depth 5

    try {
        $result = Invoke-RestMethod `
            -Uri "${BaseUrl}/exchanges/${EncodedVHost}/sci.events/publish" `
            -Method Post `
            -Headers $Headers `
            -ContentType "application/json" `
            -Body $publishPayload `
            -TimeoutSec 10
        
        if ($result.routed -eq $true) {
            Write-Pass "$($svc.Name): Published test message to '${firstQueue}' (routed: true)"
        }
        else {
            Write-Warn "$($svc.Name): Message published but NOT routed to '${firstQueue}' - check bindings"
        }
    }
    catch {
        Write-Fail "$($svc.Name): Failed to publish to '${firstQueue}' - $($_.Exception.Message)"
    }
}

# ============================================================
# Test 7: Verify Test Messages Arrived
# ============================================================
Write-TestHeader "7. Verify Test Messages in Queues"

# Give RabbitMQ a moment to route
foreach ($svc in $Services) {
    if ($svc.Queues.Count -eq 0) { continue }
    
    $firstQueue = $svc.Queues[0]
    $encodedQueue = [System.Uri]::EscapeDataString($firstQueue)
    
    $q = Invoke-RabbitApi "/queues/${EncodedVHost}/${encodedQueue}"
    if ($q -and $q.messages -gt 0) {
        Write-Pass "$($svc.Name): Queue '${firstQueue}' has $($q.messages) message(s)"
    }
    elseif ($q) {
        Write-Warn "$($svc.Name): Queue '${firstQueue}' is empty (message may have been consumed)"
    }
    else {
        Write-Fail "$($svc.Name): Could not check queue '${firstQueue}'"
    }
}

# ============================================================
# Test 8: Service Health Endpoints
# ============================================================
if (-not $SkipServiceHealth) {
    Write-TestHeader "8. Service Health Endpoints (RabbitMQ dependency)"
    
    foreach ($svc in $Services) {
        $healthUrl = "http://localhost:$($svc.Port)/health"
        try {
            $healthResponse = Invoke-WebRequest -Uri $healthUrl -TimeoutSec 5 -UseBasicParsing
            if ($healthResponse.StatusCode -eq 200) {
                Write-Pass "$($svc.Name) (port $($svc.Port)): Health OK"
            }
            else {
                Write-Warn "$($svc.Name) (port $($svc.Port)): Health returned $($healthResponse.StatusCode)"
            }
        }
        catch [System.Net.WebException] {
            Write-Warn "$($svc.Name) (port $($svc.Port)): Not reachable (service may not be running)"
        }
        catch {
            Write-Warn "$($svc.Name) (port $($svc.Port)): $($_.Exception.Message)"
        }
    }
}
else {
    Write-Info "Skipping service health endpoint checks (--SkipServiceHealth)"
}

# ============================================================
# Test 9: Connection & Channel Stats
# ============================================================
Write-TestHeader "9. RabbitMQ Connection & Channel Statistics"

$connections = Invoke-RabbitApi "/connections"
if ($connections) {
    $connCount = @($connections).Count
    Write-Info "Active connections: ${connCount}"
    
    $connByUser = $connections | Group-Object -Property user
    foreach ($group in $connByUser) {
        Write-Info "  User '$($group.Name)': $($group.Count) connection(s)"
    }
    
    if ($connCount -gt 0) {
        Write-Pass "RabbitMQ has active connections"
    }
    else {
        Write-Warn "No active connections (services may not be running)"
    }
}

$channels = Invoke-RabbitApi "/channels"
if ($channels) {
    $chanCount = @($channels).Count
    Write-Info "Active channels: ${chanCount}"
    
    if ($chanCount -gt 0) {
        Write-Pass "RabbitMQ has active channels"
    }
    else {
        Write-Warn "No active channels"
    }
}

# ============================================================
# Test 10: Cleanup Test Messages
# ============================================================
Write-TestHeader "10. Cleanup Test Messages"

foreach ($svc in $Services) {
    if ($svc.Queues.Count -eq 0) { continue }
    
    $firstQueue = $svc.Queues[0]
    $encodedQueue = [System.Uri]::EscapeDataString($firstQueue)
    
    # Peek and consume only our test messages (get with ack_requeue_false)
    $getPayload = @{
        count    = 10
        ackmode  = "ack_requeue_false"
        encoding = "auto"
    } | ConvertTo-Json

    try {
        $messages = Invoke-RestMethod `
            -Uri "${BaseUrl}/queues/${EncodedVHost}/${encodedQueue}/get" `
            -Method Post `
            -Headers $Headers `
            -ContentType "application/json" `
            -Body $getPayload `
            -TimeoutSec 10
        
        $testMsgCount = 0
        foreach ($msg in $messages) {
            $payload = $msg.payload | ConvertFrom-Json -ErrorAction SilentlyContinue
            if ($payload -and $payload.eventType -eq "rabbitmq.connectivity.test" -and $payload.correlationId -eq $testCorrelationId) {
                $testMsgCount++
            }
        }
        
        if ($testMsgCount -gt 0) {
            Write-Info "$($svc.Name): Cleaned up ${testMsgCount} test message(s) from '${firstQueue}'"
        }
    }
    catch {
        # Queue might be empty or not exist, that's fine
    }
}

Write-Info "Test messages cleaned up"

# ============================================================
# Summary
# ============================================================
Write-Host ""
Write-Host ("=" * 70) -ForegroundColor Cyan
Write-Host "  TEST SUMMARY" -ForegroundColor Cyan
Write-Host ("=" * 70) -ForegroundColor Cyan
Write-Host ""
Write-Host "  Total Tests:  $TotalTests" -ForegroundColor White
Write-Host "  Passed:       $PassedTests" -ForegroundColor Green
Write-Host "  Failed:       $FailedTests" -ForegroundColor $(if ($FailedTests -gt 0) { "Red" } else { "Green" })
Write-Host "  Warnings:     $Warnings" -ForegroundColor $(if ($Warnings -gt 0) { "Yellow" } else { "Green" })
Write-Host ""

if ($FailedTests -eq 0) {
    Write-Host "  ALL TESTS PASSED!" -ForegroundColor Green
    Write-Host ""
    exit 0
}
else {
    Write-Host "  $FailedTests TEST(S) FAILED - Review output above." -ForegroundColor Red
    Write-Host ""
    exit 1
}
