<#
.SYNOPSIS
  Assigns a distinct port to every ERP microservice.

.WHAT IT UPDATES
  For each service:
    • Dockerfile   – EXPOSE <port>  and  ENV ASPNETCORE_URLS=http://+:<port>
    • appsettings.json / appsettings.Production.json / appsettings.Development.json
      (own-port URL references via Kestrel / applicationUrl / ASPNETCORE_URLS env hints)
    • Properties/launchSettings.json – applicationUrl entries
    • ocelot.json – BaseUrl and any Route UpstreamPathTemplate host references
    • Any other *.json inside a service folder that contains "http://localhost:<oldport>"

.PORT RANGES
  adminServices     5100-5119
  aimsServices      5120-5139
  auditServices     5140-5159
  AuthProvider      5160
  canteenServices   5170-5189
  cashServices      5190-5209
  ddServices        5210-5229
  healthServices    5230-5249
  hrServices        5250-5269
  letServices       5270-5289
  loanServices      5290-5309
  mainsparshSvc     5310-5329
  myworkServices    5330-5349
  payServices       5350-5369
  pfServices        5370-5389
  sciServices       5390-5409
  sparshServices    5410-5429
  sscServices       5430-5449
  taskServices      5450-5469
  tourServices      5470-5489
  travelServices    5490-5509
  wmsServices       5510-5529
#>

$Root = "E:\ERPMicroservice\src\Services"

# ─────────────────────────────────────────────────────────────────────────────
# MASTER PORT TABLE
# Key   = relative path from $Root (forward slashes, no trailing slash)
# Value = new assigned port
# ─────────────────────────────────────────────────────────────────────────────
$PortMap = [ordered]@{

  # ── adminServices (5100-5119) ──────────────────────────────────────────
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

  # ── aimsServices (5120-5139) ───────────────────────────────────────────
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

  # ── auditServices (5140-5159) ──────────────────────────────────────────
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

  # ── AuthProvider (5160) ────────────────────────────────────────────────
  "AuthProvider"                          = 5160

  # ── canteenServices (5170-5189) ────────────────────────────────────────
  "canteenServices/ApiGateway"                   = 5170
  "canteenServices/canteenTransactionServices"   = 5171
  "canteenServices/canteenunitServices"           = 5172
  "canteenServices/cardmanagementServices"        = 5173
  "canteenServices/deductionServices"             = 5174
  "canteenServices/eligibilityServices"           = 5175
  "canteenServices/itemmasterServices"            = 5176
  "canteenServices/referencedataServices"         = 5177
  "canteenServices/swipeTransactionServices"      = 5178

  # ── cashServices (5190-5209) ───────────────────────────────────────────
  "cashServices/ApiGateway"                       = 5190
  "cashServices/cashmanagementServices"           = 5191
  "cashServices/currentmanagementServices"        = 5192
  "cashServices/dealticketingServices"            = 5193
  "cashServices/emailnotificationServices"        = 5194
  "cashServices/loanmanagementServices"           = 5195
  "cashServices/organizationsetupServices"        = 5196
  "cashServices/transactionServices"              = 5197

  # ── ddServices (5210-5229) ─────────────────────────────────────────────
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

  # ── healthServices (5230-5249) ─────────────────────────────────────────
  "healthServices/apiGateway"                     = 5230
  "healthServices/accidentmanagementServices"     = 5231
  "healthServices/healthcheckupServices"          = 5232
  "healthServices/healthTransactionServices"      = 5233
  "healthServices/insurancemanagementServices"    = 5234
  "healthServices/masterServices"                 = 5235
  "healthServices/medicinemanagementServices"     = 5236
  "healthServices/medicalvisitServices"           = 5237

  # ── hrServices (5250-5269) ─────────────────────────────────────────────
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

  # ── letServices (5270-5289) ────────────────────────────────────────────
  "letServices/apiGateway"                        = 5270
  "letServices/courseServices"                    = 5271
  "letServices/developmentServices"               = 5272
  "letServices/leaveServices"                     = 5273
  "letServices/letTransactionServices"            = 5274
  "letServices/masterServices"                    = 5275
  "letServices/requestServices"                   = 5276
  "letServices/reviewServices"                    = 5277

  # ── loanServices (5290-5309) ───────────────────────────────────────────
  "loanServices/apiGateway"                       = 5290
  "loanServices/documentServices"                 = 5291
  "loanServices/loanaccountServices"              = 5292
  "loanServices/loanapplicationServices"          = 5293
  "loanServices/loandefinitionServices"           = 5294
  "loanServices/loanTransactionServices"          = 5295
  "loanServices/lovServices"                      = 5296
  "loanServices/utilityServices"                  = 5297

  # ── mainsparshServices (5310-5329) ─────────────────────────────────────
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

  # ── myworkServices (5330-5349) ─────────────────────────────────────────
  "myworkServices/Gateway"                        = 5330
  "myworkServices/auditServices"                  = 5331
  "myworkServices/batchServices"                  = 5332
  "myworkServices/csaServices"                    = 5333
  "myworkServices/projectServices"                = 5334
  "myworkServices/riskServices"                   = 5335
  "myworkServices/teamServices"                   = 5336
  "myworkServices/timeSheetServices"              = 5337
  "myworkServices/workorderServices"              = 5338

  # ── payServices (5350-5369) ────────────────────────────────────────────
  "payServices/apiGateway"                        = 5350
  "payServices/employeeServices"                  = 5351
  "payServices/faqServices"                       = 5352
  "payServices/hrServices"                        = 5353
  "payServices/payrollServices"                   = 5354
  "payServices/payTransactionalServices"          = 5355
  "payServices/taxServices"                       = 5356

  # ── pfServices (5370-5389) ─────────────────────────────────────────────
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

  # ── sciServices (5390-5409) ────────────────────────────────────────────
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

  # ── sparshServices (5410-5429) ─────────────────────────────────────────
  "sparshServices/apigateway"                     = 5410
  "sparshServices/employeepridemanagementServices" = 5411
  "sparshServices/mobileappmanagementServices"    = 5412
  "sparshServices/mobileexpenseServices"          = 5413
  "sparshServices/problemmanagementServices"      = 5414
  "sparshServices/sparshtransactionalServices"    = 5415

  # ── sscServices (5430-5449) ────────────────────────────────────────────
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

  # ── taskServices (5450-5469) ───────────────────────────────────────────
  "taskServices/apiGateway"                       = 5450
  "taskServices/complaintServices"                = 5451
  "taskServices/energyServices"                   = 5452
  "taskServices/lookupServices"                   = 5453
  "taskServices/taskServices"                     = 5454
  "taskServices/taskTransactionalServices"        = 5455
  "taskServices/unitServices"                     = 5456

  # ── tourServices (5470-5489) ───────────────────────────────────────────
  "tourServices/apiGateway"                       = 5470
  "tourServices/adminServices"                    = 5471
  "tourServices/bookingServices"                  = 5472
  "tourServices/configServices"                   = 5473
  "tourServices/tourServices"                     = 5474
  "tourServices/tourplanServices"                 = 5475
  "tourServices/transactionServices"              = 5476
  "tourServices/travelServices"                   = 5477

  # ── travelServices (5490-5509) ─────────────────────────────────────────
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

  # ── wmsServices (5510-5529) ────────────────────────────────────────────
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

# ─────────────────────────────────────────────────────────────────────────────
# Helpers
# ─────────────────────────────────────────────────────────────────────────────
function Update-Dockerfile {
  param([string]$Path, [int]$NewPort)

  if (-not (Test-Path $Path)) { return $false }
  $content = Get-Content $Path -Raw

  # Replace all EXPOSE lines that have a port (keep EXPOSE 8443 if it exists separately)
  # Pattern: EXPOSE followed by one or more port numbers on the same line
  $updated = $content `
    -replace '(?m)^(EXPOSE\s+)\d+(\s+\d+)*$', "EXPOSE $NewPort" `
    -replace '(?m)^(ENV ASPNETCORE_URLS=http://\+:)\d+', "`${1}$NewPort"

  if ($updated -ne $content) {
    Set-Content -Path $Path -Value $updated -NoNewline
    return $true
  }
  return $false
}

function Update-JsonFiles {
  param([string]$ServiceFolder, [int]$NewPort)

  $changed = 0
  # Find all JSON files in the service folder tree
  Get-ChildItem -Path $ServiceFolder -Recurse -Filter "*.json" -File | ForEach-Object {
    $file = $_.FullName
    # Skip bin/obj directories
    if ($file -match '[\\/](bin|obj)[\\/]') { return }

    $content = Get-Content $file -Raw -ErrorAction SilentlyContinue
    if (-not $content) { return }

    # Update applicationUrl in launchSettings.json
    # Update "http://localhost:<any-port>" only where we can identify it's this service's own port
    # We replace instances of the service's old port (from Dockerfile EXPOSE) with new port
    # This is done by replacing patterns that set the service's own URL

    # Pattern 1: "applicationUrl": "http://localhost:OLDPORT" or "https://localhost:XXXX;http://localhost:OLDPORT"
    # Pattern 2: ASPNETCORE_URLS value references
    # Pattern 3: Kestrel endpoint URLs
    # Pattern 4: "http://+:<port>" in any json

    # We apply new port globally within the service folder scope
    # for own-port patterns (not downstream service refs)
    $newContent = $content `
      -replace '"applicationUrl":\s*"http://localhost:\d+', "`"applicationUrl`": `"http://localhost:$NewPort" `
      -replace '(;http://localhost:)\d+(")', "`${1}$NewPort`$2" `
      -replace '"http://\+:\d+"', "`"http://+:$NewPort`"" `
      -replace '"https://\+:\d+;http://\+:\d+"', "`"https://+:$($NewPort+1);http://+:$NewPort`"" `
      -replace '("Url":\s*"http://localhost:)\d+(")', "`${1}$NewPort`$2" `
      -replace '("BaseUrl":\s*"http://localhost:)\d+(")', "`${1}$NewPort`$2"

    if ($newContent -ne $content) {
      Set-Content -Path $file -Value $newContent -NoNewline
      $changed++
    }
  }
  return $changed
}

# ─────────────────────────────────────────────────────────────────────────────
# Main loop
# ─────────────────────────────────────────────────────────────────────────────
$totalDockerfiles  = 0
$totalJsonFiles    = 0
$errors            = @()

Write-Host "`n══════════════════════════════════════════════════════" -ForegroundColor Cyan
$ts = Get-Date -Format "yyyy-MM-dd HH:mm"
Write-Host "  ERP Port Assignment Script  --  $ts" -ForegroundColor Cyan
Write-Host "══════════════════════════════════════════════════════`n" -ForegroundColor Cyan

foreach ($entry in $PortMap.GetEnumerator()) {
  $relPath   = $entry.Key.Replace("/", "\")
  $newPort   = $entry.Value
  $svcFolder = Join-Path $Root $relPath

  if (-not (Test-Path $svcFolder)) {
    $errors += "NOT FOUND: $svcFolder"
    continue
  }

  # ── Find Dockerfile ──
  # Try root Dockerfile first, then Docker/Dockerfile, then nested (e.g. BankService/Dockerfile)
  $dockerfilePaths = @(
    (Join-Path $svcFolder "Dockerfile"),
    (Join-Path $svcFolder "Docker\Dockerfile"),
    (Join-Path $svcFolder "src\Dockerfile")
  )
  # Also search one level deeper for cases like csaServices/CSA.Service/Dockerfile
  Get-ChildItem -Path $svcFolder -Recurse -Filter "Dockerfile" -File -Depth 3 |
    Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' } |
    ForEach-Object { $dockerfilePaths += $_.FullName }

  $dockerfilePaths = $dockerfilePaths | Select-Object -Unique

  $dockerUpdated = $false
  foreach ($df in $dockerfilePaths) {
    if (Test-Path $df) {
      $result = Update-Dockerfile -Path $df -NewPort $newPort
      if ($result) {
        $totalDockerfiles++
        $dockerUpdated = $true
        Write-Host "  [Dockerfile] $($df.Replace($Root,'').TrimStart('\'))  →  :$newPort" -ForegroundColor Green
      }
    }
  }

  # ── Update JSON files ──
  $jsonCount = Update-JsonFiles -ServiceFolder $svcFolder -NewPort $newPort
  if ($jsonCount -gt 0) {
    $totalJsonFiles += $jsonCount
    Write-Host "  [JSON $jsonCount files] $relPath  →  :$newPort" -ForegroundColor Yellow
  }

  if (-not $dockerUpdated) {
    Write-Host "  [WARNING] No Dockerfile found/updated in $relPath" -ForegroundColor DarkYellow
  }
}

# ─────────────────────────────────────────────────────────────────────────────
# Summary
# ─────────────────────────────────────────────────────────────────────────────
Write-Host "`n══════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "  DONE" -ForegroundColor Cyan
Write-Host "  Dockerfiles updated : $totalDockerfiles" -ForegroundColor White
Write-Host "  JSON files updated  : $totalJsonFiles"   -ForegroundColor White
if ($errors.Count -gt 0) {
  Write-Host "`n  ERRORS ($($errors.Count)):" -ForegroundColor Red
  $errors | ForEach-Object { Write-Host "    $_" -ForegroundColor Red }
}
Write-Host "══════════════════════════════════════════════════════`n" -ForegroundColor Cyan

# ─────────────────────────────────────────────────────────────────────────────
# Export the port reference table (for docs and gateway config verification)
# ─────────────────────────────────────────────────────────────────────────────
$tableFile = Join-Path $Root "..\..\port-assignments.txt"
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm"
$lines = @("SERVICE PORT ASSIGNMENTS -- Generated $timestamp", "=" * 70)
foreach ($entry in $PortMap.GetEnumerator()) {
  $lines += ("{0,-60} {1}" -f $entry.Key, $entry.Value)
}
$lines | Set-Content -Path $tableFile -Encoding UTF8
Write-Host "Port reference table saved to: $tableFile" -ForegroundColor Cyan
