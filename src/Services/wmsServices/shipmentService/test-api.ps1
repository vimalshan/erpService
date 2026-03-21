$ErrorActionPreference = "Continue"
$base = "http://localhost:5148"

function ConvertTo-Base64Url([byte[]]$bytes) {
    [Convert]::ToBase64String($bytes) -replace '\+','-' -replace '/','_' -replace '=',''
}
function Test-Endpoint([string]$label, [string]$method, [string]$url, [hashtable]$headers = @{}, [string]$body = $null) {
    try {
        $params = @{ Uri = $url; Method = $method; UseBasicParsing = $true; TimeoutSec = 10 }
        if ($headers.Count -gt 0) { $params.Headers = $headers }
        if ($body) { $params.Body = $body; $params.ContentType = "application/json" }
        $r = Invoke-WebRequest @params
        $content = if ($r.Content -is [byte[]]) { [System.Text.Encoding]::UTF8.GetString($r.Content) } else { $r.Content }
        $display = try { ($content | ConvertFrom-Json | ConvertTo-Json -Depth 4 -Compress) } catch { $content }
        if ($display.Length -gt 500) { $display = $display.Substring(0,500) + "..." }
        Write-Host "[PASS $($r.StatusCode)] $label" -ForegroundColor Green
        Write-Host "     $display"
    } catch {
        $sc  = try { [int]$_.Exception.Response.StatusCode } catch { "???" }
        $msg = try { $_.ErrorDetails.Message } catch { $_.Exception.Message }
        if ($msg.Length -gt 200) { $msg = $msg.Substring(0,200) }
        Write-Host "[FAIL $sc] $label" -ForegroundColor Red
        Write-Host "     $msg" -ForegroundColor DarkRed
    }
}

Write-Host "--- Generating JWT Token ---"
$keyBytes   = [System.Text.Encoding]::UTF8.GetBytes("ShipmentServiceSuperSecretKeyThatIsAtLeast32CharactersLong!")
$hmac       = New-Object System.Security.Cryptography.HMACSHA256 -ArgumentList @(,$keyBytes)
$now        = [DateTimeOffset]::UtcNow
$payJson    = "{""sub"":""1"",""name"":""Admin"",""role"":""Admin"",""iss"":""ShipmentService"",""aud"":""ShipmentServiceClients"",""iat"":$($now.ToUnixTimeSeconds()),""exp"":$($now.AddHours(1).ToUnixTimeSeconds())}"
$hB64       = ConvertTo-Base64Url ([System.Text.Encoding]::UTF8.GetBytes('{"alg":"HS256","typ":"JWT"}'))
$pB64       = ConvertTo-Base64Url ([System.Text.Encoding]::UTF8.GetBytes($payJson))
$sB64       = ConvertTo-Base64Url ($hmac.ComputeHash([System.Text.Encoding]::UTF8.GetBytes("$hB64.$pB64")))
$JWT        = "$hB64.$pB64.$sB64"
$authHdr    = @{ Authorization = "Bearer $JWT" }
Write-Host "Token OK (len=$($JWT.Length))"

# --- HEALTH ---
Write-Host "`n=== HEALTH ===" -ForegroundColor Magenta
Test-Endpoint "GET /health"       GET "$base/health"
Test-Endpoint "GET /health/ready" GET "$base/health/ready"
Test-Endpoint "GET /health/live"  GET "$base/health/live"

# --- AUTH (expect 401) ---
Write-Host "`n=== AUTH TEST ===" -ForegroundColor Magenta
Test-Endpoint "GET /api/shipments (401 expected)" GET "$base/api/shipments"

# --- REST GET endpoints ---
Write-Host "`n=== REST API (GET) ===" -ForegroundColor Magenta
Test-Endpoint "GET  /api/shipments"               GET "$base/api/shipments?page=1&pageSize=5" $authHdr
Test-Endpoint "GET  /api/shipments/2"              GET "$base/api/shipments/2" $authHdr
Test-Endpoint "GET  /api/shipments/customer/100"   GET "$base/api/shipments/customer/100" $authHdr
Test-Endpoint "GET  /api/shipments/2/tracking"     GET "$base/api/shipments/2/tracking" $authHdr

# --- REST POST create shipment ---
Write-Host "`n=== REST API (POST/PUT) ===" -ForegroundColor Magenta
$rnd = Get-Random -Maximum 99999
$create = @"
{"shipmentNumber":"SHP-TEST-$rnd","warehouseId":1,"customerId":100,"shipmentType":"OUTBOUND","carrier":"FedEx","serviceType":"Ground","trackingNumber":"TRK-$rnd","notes":"Test shipment"}
"@
Test-Endpoint "POST /api/shipments (create)" POST "$base/api/shipments" $authHdr $create.Trim()

# Capture the created shipment ID for subsequent tests
try {
    $createParams = @{ Uri = "$base/api/shipments"; Method = "POST"; UseBasicParsing = $true; TimeoutSec = 10; Body = $create.Trim(); ContentType = "application/json"; Headers = $authHdr }
    $rnd2 = Get-Random -Maximum 99999
    $create2Body = "{""shipmentNumber"":""SHP-CHAIN-$rnd2"",""warehouseId"":1,""customerId"":100,""shipmentType"":""OUTBOUND"",""carrier"":""UPS""}"
    $createParams.Body = $create2Body
    $cr = Invoke-WebRequest @createParams
    $crContent = if ($cr.Content -is [byte[]]) { [System.Text.Encoding]::UTF8.GetString($cr.Content) } else { $cr.Content }
    $newId = ($crContent | ConvertFrom-Json).shipmentId
    Write-Host "  (Created shipment $newId for chained tests)" -ForegroundColor Cyan
} catch { $newId = $null; Write-Host "  (Could not create chained shipment)" -ForegroundColor DarkYellow }

# --- REST PUT status (Pending -> Open, valid transition) ---
if ($newId) {
    $statusBody = '{"newStatus":"OPEN","location":"Warehouse A","description":"Opened for processing","updatedBy":"admin"}'
    Test-Endpoint "PUT  /api/shipments/$newId/status (Pending->Open)" PUT "$base/api/shipments/$newId/status" $authHdr $statusBody
}

# --- REST POST add package ---
if ($newId) {
    $pkgBody = '{"packageNumber":"PKG-TEST-001","weight":3.5,"volume":1.2,"dimensions":"25x15x10","trackingNumber":"PKGTRK-001","contentsDescription":"Test contents"}'
    Test-Endpoint "POST /api/shipments/$newId/packages" POST "$base/api/shipments/$newId/packages" $authHdr $pkgBody
}

# --- MINIMAL API v2 ---
Write-Host "`n=== MINIMAL API v2 ===" -ForegroundColor Magenta
Test-Endpoint "GET /api/v2/shipments/2" GET "$base/api/v2/shipments/2" $authHdr

# --- GRAPHQL ---
Write-Host "`n=== GRAPHQL QUERIES ===" -ForegroundColor Magenta

# Introspection
Test-Endpoint "GraphQL: introspection" POST "$base/graphql" $authHdr '{"query":"{ __schema { queryType { name } mutationType { name } } }"}'

# shipments (paginated)
Test-Endpoint "GraphQL: shipments(page,pageSize)" POST "$base/graphql" $authHdr '{"query":"{ shipments(page: 1, pageSize: 3) { items { shipmentId shipmentNumber status carrier } totalCount } }"}'

# shipmentById
Test-Endpoint "GraphQL: shipmentById(2)" POST "$base/graphql" $authHdr '{"query":"{ shipmentById(id: 2) { shipmentId shipmentNumber status carrier customerId } }"}'

# shipmentsByCustomer
Test-Endpoint "GraphQL: shipmentsByCustomer(100)" POST "$base/graphql" $authHdr '{"query":"{ shipmentsByCustomer(customerId: 100) { shipmentId shipmentNumber status } }"}'

# trackingHistory
Test-Endpoint "GraphQL: trackingHistory(2)" POST "$base/graphql" $authHdr '{"query":"{ trackingHistory(shipmentId: 2) { trackingId status location description eventDatetime } }"}'

Write-Host "`n=== GRAPHQL MUTATIONS ===" -ForegroundColor Magenta

# createShipment
$gqlRnd = Get-Random -Maximum 99999
$createMut = "{""query"":""mutation { createShipment(input: { shipmentNumber: \""SHP-GQL-$gqlRnd\"", warehouseId: 1, customerId: 200, shipmentType: \""INBOUND\"", carrier: \""DHL\"" }) { shipmentId shipmentNumber status } }""}"
Test-Endpoint "GraphQL: createShipment" POST "$base/graphql" $authHdr $createMut

# updateShipmentStatus (use newly created shipment which is Pending -> Open)
if ($newId) {
    $updateMut = "{""query"":""mutation { updateShipmentStatus(shipmentId: $newId, newStatus: \""PICKED_UP\"", location: \""Loading Dock\"", description: \""Picked up by carrier\"") { shipmentId status } }""}"
    Test-Endpoint "GraphQL: updateShipmentStatus (Open->PickedUp)" POST "$base/graphql" $authHdr $updateMut
}

# addPackage
if ($newId) {
    $addPkgMut = "{""query"":""mutation { addPackage(input: { shipmentId: $newId, packageNumber: \""PKG-GQL-$gqlRnd\"", weight: 7.2, volume: 3.0, dimensions: \""40x30x20\"", trackingNumber: \""GQLTRK-$gqlRnd\"", contentsDescription: \""GraphQL test package\"" }) { packageId packageNumber weight trackingNumber } }""}"
    Test-Endpoint "GraphQL: addPackage" POST "$base/graphql" $authHdr $addPkgMut
}

Write-Host "`n=== DONE ===" -ForegroundColor Yellow
Write-Host "Valid state transitions: Pending->Open, Open->PickedUp, Open->Cancelled, PickedUp->InTransit, InTransit->Shipped, InTransit->Exception, Shipped->Delivered, Shipped->Exception, Exception->InTransit, Exception->Cancelled" -ForegroundColor DarkGray
