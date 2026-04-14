$ErrorActionPreference = "Continue"
$base = "http://localhost:5080"

# 1. Get token
Write-Host "=== STEP 1: Get Token ===" -ForegroundColor Cyan
$loginResp = Invoke-RestMethod -Uri "$base/api/Auth/token" -Method Post -Body '{"Username":"admin","Password":"Admin@123"}' -ContentType "application/json"
$token = $loginResp.token
$h = @{ Authorization = "Bearer $token" }
Write-Host "Token acquired." -ForegroundColor Green

# 2. Create Purchase Order
Write-Host "`n=== STEP 2: Create Purchase Order ===" -ForegroundColor Cyan
$ts = Get-Date -Format "HHmmss"
$poBody = @{
    poNumber = "RMQ-PO-$ts"
    supplierId = 1
    expectedDate = "2025-02-15T00:00:00Z"
    notes = "RabbitMQ Test PO"
    lines = @(@{ productId = 1; quantityOrdered = 100; unitPrice = 25.50; notes = "Test line 1" })
} | ConvertTo-Json -Depth 3
try {
    $po = Invoke-RestMethod -Uri "$base/api/PurchaseOrder" -Method Post -Body $poBody -ContentType "application/json" -Headers $h
    $poId = $po.poId
    Write-Host "PO Created: poId=$poId, poNumber=$($po.poNumber), status=$($po.status)" -ForegroundColor Green
} catch {
    Write-Host "PO Create FAILED: $($_.Exception.Message)" -ForegroundColor Red
    $sr = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($sr)
    Write-Host $reader.ReadToEnd() -ForegroundColor Red
}

# 3. Confirm Purchase Order
Write-Host "`n=== STEP 3: Confirm Purchase Order ===" -ForegroundColor Cyan
try {
    $poConfirm = Invoke-RestMethod -Uri "$base/api/PurchaseOrder/$poId/confirm" -Method Put -Headers $h -ContentType "application/json"
    Write-Host "PO Confirmed: id=$($poConfirm.id), status=$($poConfirm.status)" -ForegroundColor Green
} catch {
    Write-Host "PO Confirm FAILED: $($_.Exception.Message)" -ForegroundColor Red
    $sr = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($sr)
    Write-Host $reader.ReadToEnd() -ForegroundColor Red
}

# 4. Create Sales Order
Write-Host "`n=== STEP 4: Create Sales Order ===" -ForegroundColor Cyan
$soBody = @{
    soNumber = "RMQ-SO-$ts"
    customerId = 1
    requestedDate = "2025-03-15T00:00:00Z"
    notes = "RabbitMQ Test SO"
    lines = @(@{ productId = 1; quantityOrdered = 50; unitPrice = 35.00; notes = "Test SO line 1" })
} | ConvertTo-Json -Depth 3
try {
    $so = Invoke-RestMethod -Uri "$base/api/SalesOrder" -Method Post -Body $soBody -ContentType "application/json" -Headers $h
    $soId = $so.soId
    Write-Host "SO Created: soId=$soId, soNumber=$($so.soNumber), status=$($so.status)" -ForegroundColor Green
} catch {
    Write-Host "SO Create FAILED: $($_.Exception.Message)" -ForegroundColor Red
    $sr = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($sr)
    Write-Host $reader.ReadToEnd() -ForegroundColor Red
}

# 5. Confirm Sales Order
Write-Host "`n=== STEP 5: Confirm Sales Order ===" -ForegroundColor Cyan
try {
    $soConfirm = Invoke-RestMethod -Uri "$base/api/SalesOrder/$soId/confirm" -Method Put -Headers $h -ContentType "application/json"
    Write-Host "SO Confirmed: id=$($soConfirm.id), status=$($soConfirm.status)" -ForegroundColor Green
} catch {
    Write-Host "SO Confirm FAILED: $($_.Exception.Message)" -ForegroundColor Red
    $sr = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($sr)
    Write-Host $reader.ReadToEnd() -ForegroundColor Red
}

# 6. Create Receiving for the PO
Write-Host "`n=== STEP 6: Create Receiving ===" -ForegroundColor Cyan
$poLineId = $po.lines[0].poLineId
$rcvBody = @{
    receivingNumber = "RMQ-RCV-$ts"
    poId = $poId
    notes = "RabbitMQ Test Receiving"
    lines = @(@{ poLineId = $poLineId; productId = 1; binId = 1; quantityReceived = 100; notes = "Recv all" })
} | ConvertTo-Json -Depth 3
try {
    $rcv = Invoke-RestMethod -Uri "$base/api/PurchaseOrder/$poId/receivings" -Method Post -Body $rcvBody -ContentType "application/json" -Headers $h
    $rcvId = $rcv.receivingId
    Write-Host "Receiving Created: receivingId=$rcvId, receivingNumber=$($rcv.receivingNumber), status=$($rcv.status)" -ForegroundColor Green
} catch {
    Write-Host "Receiving Create FAILED: $($_.Exception.Message)" -ForegroundColor Red
    $sr = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($sr)
    Write-Host $reader.ReadToEnd() -ForegroundColor Red
}

# 7. Close Receiving
Write-Host "`n=== STEP 7: Close Receiving ===" -ForegroundColor Cyan
try {
    $rcvClose = Invoke-RestMethod -Uri "$base/api/PurchaseOrder/receivings/$rcvId/close" -Method Put -Headers $h -ContentType "application/json"
    Write-Host "Receiving Closed: id=$($rcvClose.id), status=$($rcvClose.status)" -ForegroundColor Green
} catch {
    Write-Host "Receiving Close FAILED: $($_.Exception.Message)" -ForegroundColor Red
    $sr = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($sr)
    Write-Host $reader.ReadToEnd() -ForegroundColor Red
}

# 8. Create Shipment for the SO
Write-Host "`n=== STEP 8: Create Shipment ===" -ForegroundColor Cyan
$soLineId = $so.lines[0].soLineId
$shipBody = @{
    shipmentNumber = "RMQ-SHP-$ts"
    soId = $soId
    carrier = "FedEx"
    notes = "RabbitMQ Test Shipment"
    lines = @(@{ soLineId = $soLineId; productId = 1; binId = 1; quantityShipped = 50; notes = "Ship all" })
} | ConvertTo-Json -Depth 3
try {
    $ship = Invoke-RestMethod -Uri "$base/api/SalesOrder/$soId/shipments" -Method Post -Body $shipBody -ContentType "application/json" -Headers $h
    $shipId = $ship.shipmentId
    Write-Host "Shipment Created: shipmentId=$shipId, shipmentNumber=$($ship.shipmentNumber), status=$($ship.status)" -ForegroundColor Green
} catch {
    Write-Host "Shipment Create FAILED: $($_.Exception.Message)" -ForegroundColor Red
    $sr = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($sr)
    Write-Host $reader.ReadToEnd() -ForegroundColor Red
}

# 9. Ship Shipment
Write-Host "`n=== STEP 9: Ship Shipment ===" -ForegroundColor Cyan
try {
    $shipShip = Invoke-RestMethod -Uri "$base/api/SalesOrder/shipments/$shipId/ship" -Method Put -Headers $h -ContentType "application/json"
    Write-Host "Shipment Shipped: id=$($shipShip.id), status=$($shipShip.status)" -ForegroundColor Green
} catch {
    Write-Host "Shipment Ship FAILED: $($_.Exception.Message)" -ForegroundColor Red
    $sr = $_.Exception.Response.GetResponseStream()
    $reader = New-Object System.IO.StreamReader($sr)
    Write-Host $reader.ReadToEnd() -ForegroundColor Red
}

Write-Host "`n=== ALL TESTS COMPLETE ===" -ForegroundColor Cyan
Write-Host "Check API server logs for consumer messages." -ForegroundColor Yellow
