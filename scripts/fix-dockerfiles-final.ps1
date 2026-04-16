# Comprehensive Dockerfile port-assignment fix
# - Handles both LF and CRLF files
# - Replaces ANY existing EXPOSE line(s) with exactly one EXPOSE <new_port>
# - Sets/updates ENV ASPNETCORE_URLS=http://+:<new_port>
# - Removes duplicate EXPOSE blocks

$Root = "E:\ERPMicroservice\src\Services"

$PortMap = @{
  "adminServices/ApiGateway"              = 5100
  "adminServices/finyearServices"         = 5101
  "adminServices/locationServices"        = 5102
  "adminServices/lovServices"             = 5103
  "adminServices/scholarshipServices"     = 5104
  "adminServices/stationeryServices"      = 5105
  "adminServices/tdsServices"             = 5106
  "adminServices/transactionServices"     = 5107
  "adminServices/vendorServices"          = 5108
  "adminServices/SharedServices"          = 5109
  "aimsServices/ApiGateway"               = 5120
  "aimsServices/accessServices"           = 5121
  "aimsServices/aimsTransactionServices"  = 5122
  "aimsServices/attendanceServices"       = 5123
  "aimsServices/busServices"              = 5124
  "aimsServices/calendarServices"         = 5125
  "aimsServices/employeeServices"         = 5126
  "aimsServices/groupincentiveServices"   = 5127
  "aimsServices/leaveServices"            = 5128
  "aimsServices/referenceServices"        = 5129
  "aimsServices/visitorServices"          = 5130
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
  "AuthProvider"                          = 5160
  "canteenServices/ApiGateway"                   = 5170
  "canteenServices/canteenTransactionServices"   = 5171
  "canteenServices/canteenunitServices"           = 5172
  "canteenServices/cardmanagementServices"        = 5173
  "canteenServices/deductionServices"             = 5174
  "canteenServices/eligibilityServices"           = 5175
  "canteenServices/itemmasterServices"            = 5176
  "canteenServices/referencedataServices"         = 5177
  "canteenServices/swipeTransactionServices"      = 5178
  "cashServices/ApiGateway"                       = 5190
  "cashServices/cashmanagementServices"           = 5191
  "cashServices/currentmanagementServices"        = 5192
  "cashServices/dealticketingServices"            = 5193
  "cashServices/emailnotificationServices"        = 5194
  "cashServices/loanmanagementServices"           = 5195
  "cashServices/organizationsetupServices"        = 5196
  "cashServices/transactionServices"              = 5197
  "ddServices/apiGateway"                         = 5210
  "ddServices/appraisalService"                   = 5211
  "ddServices/authorizationServices"              = 5212
  "ddServices/compensationServices"               = 5213
  "ddServices/competencyServices"                 = 5214
  "ddServices/demandmanagementServices"           = 5215
  "ddServices/documentServices"                   = 5216
  "ddServices/employeeServices"                   = 5217
  "ddServices/feedbackServices"                   = 5218
  "ddServices/learningServices"                   = 5219
  "ddServices/objectiveServices"                  = 5220
  "ddServices/OtherServices"                      = 5221
  "ddServices/promotionServices"                  = 5222
  "ddServices/recruitmentServices"                = 5223
  "ddServices/reportingServices"                  = 5224
  "ddServices/transactionServices"                = 5225
  "healthServices/apiGateway"                     = 5230
  "healthServices/accidentmanagementServices"     = 5231
  "healthServices/healthcheckupServices"          = 5232
  "healthServices/healthTransactionServices"      = 5233
  "healthServices/insurancemanagementServices"    = 5234
  "healthServices/masterServices"                 = 5235
  "healthServices/medicinemanagementServices"     = 5236
  "healthServices/medicalvisitServices"           = 5237
  "hrServicess/apiGatewayServices"                = 5250
  "hrServicess/alertsnotificationsServices"       = 5251
  "hrServicess/compensationbenefitsServices"      = 5252
  "hrServicess/employeemanagementServices"        = 5253
  "hrServicess/employeerelationsServices"         = 5254
  "hrServicess/employeeTransactionsServices"      = 5255
  "hrServicess/exitmanagementServices"            = 5256
  "hrServicess/organizationstructureServices"     = 5257
  "hrServicess/recruitmentServices"               = 5258
  "hrServicess/timeattendanceServices"            = 5259
  "hrServicess/trainingdevelopmentServices"       = 5260
  "hrServicess/usersecurityServices"              = 5261
  "letServices/apiGateway"                        = 5270
  "letServices/courseServices"                    = 5271
  "letServices/developmentServices"               = 5272
  "letServices/leaveServices"                     = 5273
  "letServices/letTransactionServices"            = 5274
  "letServices/masterServices"                    = 5275
  "letServices/requestServices"                   = 5276
  "letServices/reviewServices"                    = 5277
  "loanServices/apiGateway"                       = 5290
  "loanServices/documentServices"                 = 5291
  "loanServices/loanaccountServices"              = 5292
  "loanServices/loanapplicationServices"          = 5293
  "loanServices/loandefinitionServices"           = 5294
  "loanServices/loanTransactionServices"          = 5295
  "loanServices/lovServices"                      = 5296
  "loanServices/utilityServices"                  = 5297
  "mainsparshServices/apiGateway"                 = 5310
  "mainsparshServices/approvalServices"           = 5311
  "mainsparshServices/bookingServices"            = 5312
  "mainsparshServices/communityServices"          = 5313
  "mainsparshServices/compensationServices"       = 5314
  "mainsparshServices/groupmanagementServices"    = 5315
  "mainsparshServices/locationServices"           = 5316
  "mainsparshServices/meetingServices"            = 5317
  "mainsparshServices/proxyServices"              = 5318
  "mainsparshServices/reimbursementServices"      = 5319
  "mainsparshServices/stipendservices"            = 5320
  "mainsparshServices/timesheetServices"          = 5321
  "mainsparshServices/transactionServices"        = 5322
  "mainsparshServices/usermanagementServices"     = 5323
  "mainsparshServices/websitecontentServices"     = 5324
  "myworkServices/Gateway"                        = 5330
  "myworkServices/auditServices"                  = 5331
  "myworkServices/batchServices"                  = 5332
  "myworkServices/csaServices"                    = 5333
  "myworkServices/projectServices"                = 5334
  "myworkServices/riskServices"                   = 5335
  "myworkServices/teamServices"                   = 5336
  "myworkServices/timeSheetServices"              = 5337
  "myworkServices/workorderServices"              = 5338
  "payServices/apiGateway"                        = 5350
  "payServices/employeeServices"                  = 5351
  "payServices/faqServices"                       = 5352
  "payServices/hrServices"                        = 5353
  "payServices/payrollServices"                   = 5354
  "payServices/payTransactionalServices"          = 5355
  "payServices/taxServices"                       = 5356
  "pfServices/apiGateway"                         = 5370
  "pfServices/accountingServices"                 = 5371
  "pfServices/bankServices"                       = 5372
  "pfServices/contributionServices"               = 5373
  "pfServices/investmentServices"                 = 5374
  "pfServices/loanServices"                       = 5375
  "pfServices/masterdataServices"                 = 5376
  "pfServices/memberServices"                     = 5377
  "pfServices/pftransactionalServices"            = 5378
  "pfServices/settlementServices"                 = 5379
  "pfServices/trustServices"                      = 5380
  "sciServices/ApiGateway"                        = 5390
  "sciServices/dispatchplanningServices"          = 5391
  "sciServices/errorloggingServices"              = 5392
  "sciServices/eximmanagementServices"            = 5393
  "sciServices/fillingoperationServices"          = 5394
  "sciServices/gstcomplianceServices"             = 5395
  "sciServices/inventorymanagementServices"       = 5396
  "sciServices/mamallocationServices"             = 5397
  "sciServices/masterdataServices"                = 5398
  "sciServices/orderscheduleServices"             = 5399
  "sciServices/productionmanagementServices"      = 5400
  "sciServices/purchasesalesService"              = 5401
  "sciServices/scitransactionalServices"          = 5402
  "sciServices/SecurityServices"                  = 5403
  "sciServices/strategicstockServices"            = 5404
  "sciServices/vechicletrackingServices"          = 5405
  "sparshServices/apigateway"                     = 5410
  "sparshServices/employeepridemanagementServices" = 5411
  "sparshServices/mobileappmanagementServices"    = 5412
  "sparshServices/mobileexpenseServices"          = 5413
  "sparshServices/problemmanagementServices"      = 5414
  "sparshServices/sparshtransactionalServices"    = 5415
  "sscServices/apigateway"                        = 5430
  "sscServices/approvalgroupServices"             = 5431
  "sscServices/batchandenvelopeServices"          = 5432
  "sscServices/categoryandvendorServices"         = 5433
  "sscServices/clubmembershipServices"            = 5434
  "sscServices/fillingandarchiveServices"         = 5435
  "sscServices/hrdocumentServices"                = 5436
  "sscServices/integrationServices"               = 5437
  "sscServices/invoiceprocessingServices"         = 5438
  "sscServices/masterdataServices"                = 5439
  "sscServices/menuandsecurityServices"           = 5440
  "sscServices/menuServices"                      = 5441
  "sscServices/ssctransactionalServices"          = 5442
  "taskServices/apiGateway"                       = 5450
  "taskServices/complaintServices"                = 5451
  "taskServices/energyServices"                   = 5452
  "taskServices/lookupServices"                   = 5453
  "taskServices/taskServices"                     = 5454
  "taskServices/taskTransactionalServices"        = 5455
  "taskServices/unitServices"                     = 5456
  "tourServices/apiGateway"                       = 5470
  "tourServices/adminServices"                    = 5471
  "tourServices/bookingServices"                  = 5472
  "tourServices/configServices"                   = 5473
  "tourServices/tourServices"                     = 5474
  "tourServices/tourplanServices"                 = 5475
  "tourServices/transactionServices"              = 5476
  "tourServices/travelServices"                   = 5477
  "travelServices/ApiGateway"                     = 5490
  "travelServices/adminServices"                  = 5491
  "travelServices/agensService"                   = 5492
  "travelServices/bookingServices"                = 5493
  "travelServices/expenseServices"                = 5494
  "travelServices/financeServices"                = 5495
  "travelServices/insuranceServices"              = 5496
  "travelServices/masterdataServices"             = 5497
  "travelServices/traveltransactionServices"      = 5498
  "travelServices/travelRequestServices"          = 5499
  "wmsServices/apiGateway"                        = 5510
  "wmsServices/auditlogService"                   = 5511
  "wmsServices/customerService"                   = 5512
  "wmsServices/emplyeeService"                    = 5513
  "wmsServices/fleetManagementService"            = 5514
  "wmsServices/inventoryService"                  = 5515
  "wmsServices/orderService"                      = 5516
  "wmsServices/productService"                    = 5517
  "wmsServices/purchaseorderService"              = 5518
  "wmsServices/rackingsystemService"              = 5519
  "wmsServices/receivingService"                  = 5520
  "wmsServices/salesorderService"                 = 5521
  "wmsServices/securityService"                   = 5522
  "wmsServices/shipmentService"                   = 5523
  "wmsServices/supplierService"                   = 5524
  "wmsServices/warehousestructureService"         = 5525
  "wmsServices/wmtransactionalService"            = 5526
}

$totalFixed = 0
$totalSkipped = 0

foreach ($entry in $PortMap.GetEnumerator()) {
    $relKey   = $entry.Key          # e.g. "adminServices/ApiGateway"
    $newPort  = $entry.Value        # e.g. 5100

    # Build the service folder path (key uses / but we need OS-native)
    $svcFolder = Join-Path $Root ($relKey -replace '/', '\')

    if (-not (Test-Path $svcFolder)) {
        Write-Host "  [MISSING FOLDER] $relKey" -ForegroundColor DarkGray
        continue
    }

    # Find all Dockerfiles in this service folder (any depth)
    $dockerfiles = Get-ChildItem -Path $svcFolder -Recurse -Filter "Dockerfile" -File |
        Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' }

    if ($dockerfiles.Count -eq 0) {
        Write-Host "  [NO DOCKERFILE] $relKey" -ForegroundColor DarkYellow
        continue
    }

    foreach ($df in $dockerfiles) {
        $relPath = $df.FullName.Substring($Root.Length + 1)

        # Read raw bytes, convert to string, normalize CRLF -> LF
        $rawBytes = [System.IO.File]::ReadAllBytes($df.FullName)
        $content  = [System.Text.Encoding]::UTF8.GetString($rawBytes)
        $norm     = $content.Replace("`r`n", "`n")

        # Step 1: Remove ALL lines that start with EXPOSE (handles 8080, 8443, 80, 5000, etc.)
        $fixed = [regex]::Replace($norm, '(?m)^EXPOSE\s+\S[^\n]*\n?', '')

        # Step 2: Remove all ENV ASPNETCORE_URLS lines (we'll re-add correct one)
        $fixed = [regex]::Replace($fixed, '(?m)^ENV ASPNETCORE_URLS=[^\n]*\n?', '')

        # Step 3: Remove any blank lines that appeared at start of "base" stage after WORKDIR /app
        # to keep file clean (optional but nice)

        # Step 4: Insert EXPOSE + ENV after the FIRST "WORKDIR /app" in the base stage
        $reWorkdir = [regex]'(?m)^(WORKDIR /app)\n'
        if ($reWorkdir.IsMatch($fixed)) {
            # Replace only the FIRST occurrence (count=1)
            $fixed = $reWorkdir.Replace($fixed, "`$1`nEXPOSE $newPort`nENV ASPNETCORE_URLS=http://+:$newPort`n", 1)
        } else {
            # Fallback: insert before the first FROM ... AS build line
            $reFromBuild = [regex]'(?m)^(FROM\s+\S+\s+AS\s+build)'
            $fixed = $reFromBuild.Replace($fixed, "EXPOSE $newPort`nENV ASPNETCORE_URLS=http://+:$newPort`n`n`$1", 1)
        }

        # Only write if content actually changed
        if ($fixed -ne $norm) {
            $utf8NoBom = New-Object System.Text.UTF8Encoding($false)
            [System.IO.File]::WriteAllText($df.FullName, $fixed, $utf8NoBom)
            Write-Host "  [FIXED] $relPath  ->  :$newPort" -ForegroundColor Green
            $totalFixed++
        } else {
            Write-Host "  [OK]    $relPath  (already :$newPort)" -ForegroundColor DarkGray
            $totalSkipped++
        }
    }
}

Write-Host ""
Write-Host ("=" * 52)
Write-Host "  Fixed  : $totalFixed"
Write-Host "  Already OK: $totalSkipped"
Write-Host ("=" * 52)
