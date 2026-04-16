# Fix Dockerfiles with CRLF line endings that were missed by the initial port assignment script
# Applies port assignments using byte-level CRLF normalization

$ServicesRoot = "E:\ERPMicroservice\src\Services"

# Full port mapping table
$PortMap = @{
    # auditServices
    "auditServices/apigateway"              = 5140
    "auditServices/actionapiServices"       = 5141
    "auditServices/auditapiServices"        = 5142
    "auditServices/certificateapiServices"  = 5143
    "auditServices/contractapiServices"     = 5144
    "auditServices/financeapiServices"      = 5145
    "auditServices/findingsapiServices"     = 5146
    "auditServices/notificationapiServices" = 5147
    "auditServices/scheduleapiServices"     = 5148
    "auditServices/settingsapiServices"     = 5149
    # ddServices
    "ddServices/apiGateway"                 = 5210
    "ddServices/appraisalService"           = 5211
    "ddServices/authorizationServices"      = 5212
    "ddServices/compensationServices"       = 5213
    "ddServices/competencyServices"         = 5214
    "ddServices/demandmanagementServices"   = 5215
    "ddServices/documentServices"           = 5216
    "ddServices/employeeServices"           = 5217
    "ddServices/feedbackServices"           = 5218
    "ddServices/learningServices"           = 5219
    "ddServices/objectiveServices"          = 5220
    "ddServices/OtherServices"              = 5221
    "ddServices/promotionServices"          = 5222
    "ddServices/recruitmentServices"        = 5223
    "ddServices/reportingServices"          = 5224
    "ddServices/transactionServices"        = 5225
    # letServices
    "letServices/apiGateway"                = 5270
    "letServices/courseServices"            = 5271
    "letServices/developmentServices"       = 5272
    "letServices/leaveServices"             = 5273
    "letServices/letTransactionServices"    = 5274
    "letServices/masterServices"            = 5275
    "letServices/requestServices"           = 5276
    "letServices/reviewServices"            = 5277
    # loanServices
    "loanServices/apiGateway"               = 5290
    "loanServices/documentServices"         = 5291
    "loanServices/loanaccountServices"      = 5292
    "loanServices/loanapplicationServices"  = 5293
    "loanServices/loandefinitionServices"   = 5294
    "loanServices/loanTransactionServices"  = 5295
    "loanServices/lovServices"              = 5296
    "loanServices/utilityServices"          = 5297
    # pfServices
    "pfServices/apiGateway"                 = 5370
    "pfServices/accountingServices"         = 5371
    "pfServices/bankServices"               = 5372
    "pfServices/contributionServices"       = 5373
    "pfServices/investmentServices"         = 5374
    "pfServices/loanServices"               = 5375
    "pfServices/masterdataServices"         = 5376
    "pfServices/memberServices"             = 5377
    "pfServices/pftransactionalServices"    = 5378
    "pfServices/settlementServices"         = 5379
    "pfServices/trustServices"              = 5380
    # sparshServices
    "sparshServices/apigateway"             = 5410
    "sparshServices/employeepridemanagementServices" = 5411
    "sparshServices/mobileappmanagementServices"     = 5412
    "sparshServices/mobileexpenseServices"           = 5413
    "sparshServices/problemmanagementServices"       = 5414
    "sparshServices/sparshtransactionalServices"     = 5415
    # sscServices
    "sscServices/apigateway"                = 5430
    "sscServices/approvalgroupServices"     = 5431
    "sscServices/batchandenvelopeServices"  = 5432
    "sscServices/categoryandvendorServices" = 5433
    "sscServices/clubmembershipServices"    = 5434
    "sscServices/fillingandarchiveServices" = 5435
    "sscServices/hrdocumentServices"        = 5436
    "sscServices/integrationServices"       = 5437
    "sscServices/invoiceprocessingServices" = 5438
    "sscServices/masterdataServices"        = 5439
    "sscServices/menuandsecurityServices"   = 5440
    "sscServices/menuServices"              = 5441
    "sscServices/ssctransactionalServices"  = 5442
    # taskServices
    "taskServices/apiGateway"               = 5450
    "taskServices/complaintServices"        = 5451
    "taskServices/energyServices"           = 5452
    "taskServices/lookupServices"           = 5453
    "taskServices/taskServices"             = 5454
    "taskServices/taskTransactionalServices"= 5455
    "taskServices/unitServices"             = 5456
    # tourServices
    "tourServices/apiGateway"               = 5470
    "tourServices/adminServices"            = 5471
    "tourServices/bookingServices"          = 5472
    "tourServices/configServices"           = 5473
    "tourServices/tourServices"             = 5474
    "tourServices/tourplanServices"         = 5475
    "tourServices/transactionServices"      = 5476
    "tourServices/travelServices"           = 5477
    # travelServices
    "travelServices/ApiGateway"             = 5490
    "travelServices/adminServices"          = 5491
    "travelServices/agensService"           = 5492
    "travelServices/bookingServices"        = 5493
    "travelServices/expenseServices"        = 5494
    "travelServices/financeServices"        = 5495
    "travelServices/insuranceServices"      = 5496
    "travelServices/masterdataServices"     = 5497
    "travelServices/traveltransactionServices" = 5498
    "travelServices/travelRequestServices"  = 5499
    # wmsServices
    "wmsServices/apiGateway"                = 5510
    "wmsServices/auditlogService"           = 5511
    "wmsServices/customerService"           = 5512
    "wmsServices/emplyeeService"            = 5513
    "wmsServices/fleetManagementService"    = 5514
    "wmsServices/inventoryService"          = 5515
    "wmsServices/orderService"              = 5516
    "wmsServices/productService"            = 5517
    "wmsServices/purchaseorderService"      = 5518
    "wmsServices/rackingsystemService"      = 5519
    "wmsServices/receivingService"          = 5520
    "wmsServices/salesorderService"         = 5521
    "wmsServices/securityService"           = 5522
    "wmsServices/shipmentService"           = 5523
    "wmsServices/supplierService"           = 5524
    "wmsServices/warehousestructureService" = 5525
    "wmsServices/wmtransactionalService"    = 5526
}

$updated = 0
$skipped = 0
$notFound = 0

# Find all stale Dockerfiles
$staleFiles = Get-ChildItem -Path $ServicesRoot -Recurse -Filter "Dockerfile" | Where-Object {
    $c = [System.IO.File]::ReadAllText($_.FullName, [System.Text.Encoding]::UTF8)
    $c -match "EXPOSE 8080|EXPOSE 8443"
}

Write-Host "Stale Dockerfiles to fix: $($staleFiles.Count)"
Write-Host ""

foreach ($df in $staleFiles) {
    # Determine the service folder key: <module>/<service>
    # Dockerfile may be nested like module/service/SubProject/Dockerfile
    # We extract the path relative to ServicesRoot, then take first two segments
    $rel = $df.FullName.Substring($ServicesRoot.Length + 1)
    $parts = $rel -split '[\\/]'
    $module  = $parts[0]
    $svc     = $parts[1]
    $key     = "$module/$svc"

    if (-not $PortMap.ContainsKey($key)) {
        Write-Host "  [SKIP] No port mapping for: $key" -ForegroundColor DarkYellow
        $skipped++
        continue
    }

    $port = $PortMap[$key]

    # Read with explicit UTF-8, normalize CRLF -> LF
    $content = [System.IO.File]::ReadAllText($df.FullName, [System.Text.Encoding]::UTF8)
    $norm = $content.Replace("`r`n", "`n")

    # Replace EXPOSE 8080 and EXPOSE 8443 lines
    # Handle both "EXPOSE 8080" alone and with extra ports on same line
    $fixed = $norm -replace '(?m)^EXPOSE\s+\d+(\s+\d+)*$', "EXPOSE $port"

    # Update or add ENV ASPNETCORE_URLS
    if ($fixed -match "ENV\s+ASPNETCORE_URLS") {
        $fixed = $fixed -replace '(?m)^(ENV\s+ASPNETCORE_URLS\s*=\s*)http://\+:\d+', "`${1}http://+:$port"
    } else {
        # Insert after the last EXPOSE line
        $fixed = $fixed -replace "(?m)^(EXPOSE $port)\s*$", "`$1`nENV ASPNETCORE_URLS=http://+:$port"
    }

    # Only write if changed
    if ($fixed -ne $norm) {
        [System.IO.File]::WriteAllText($df.FullName, $fixed, (New-Object System.Text.UTF8Encoding($false)))
        Write-Host "  [FIXED] $rel  ->  :$port" -ForegroundColor Green
        $updated++
    } else {
        Write-Host "  [UNCHANGED] $rel" -ForegroundColor DarkYellow
        $skipped++
    }
}

Write-Host ""
Write-Host ("=" * 50)
Write-Host "  Fixed  : $updated"
Write-Host "  Skipped: $skipped"
Write-Host ("=" * 50)
