$ErrorActionPreference = 'Continue'
$base = 'http://localhost:5147'
$jwt  = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiI5YjViNTZlZC01ZTAxLTQzNGUtOTc3My1kNTExZDQ2OWNjZmUiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc4MjAyNzMsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.xRZqMW6G22XPCUZ85tYhKqcTYQUImVa-T-f_YhUsOVg'
$h = @{ Authorization = "Bearer $jwt" }
$pass = 0; $fail = 0
function Check($name, $ok, $detail='') {
  if ($ok) { Write-Host "  PASS  $name" -ForegroundColor Green; $script:pass++ }
  else     { Write-Host "  FAIL  $name  $detail" -ForegroundColor Red; $script:fail++ }
}

Write-Host "`n== REST tests ==`n"

try { $r = Invoke-WebRequest "$base/health" -UseBasicParsing -Headers $h; Check 'GET /health' ($r.StatusCode -eq 200) } catch { Check 'GET /health' $false $_.Exception.Message }

try {
  $r = Invoke-RestMethod "$base/api/notifications/minimal" -Headers $h
  Check 'GET /api/notifications/minimal' ($r.Count -ge 6)
} catch { Check 'GET /api/notifications/minimal' $false $_.Exception.Message }

try {
  $r = Invoke-RestMethod "$base/api/notifications/minimal/1" -Headers $h
  Check 'GET /api/notifications/minimal/1' ($r.notificationId -eq 1)
} catch { Check 'GET /api/notifications/minimal/1' $false $_.Exception.Message }

try {
  $code = 0
  try { Invoke-RestMethod "$base/api/notifications/minimal/9999" -Headers $h | Out-Null }
  catch { $code = $_.Exception.Response.StatusCode.value__ }
  Check 'GET /api/notifications/minimal/9999 -> 404' ($code -eq 404)
} catch { Check 'GET /api/notifications/minimal/9999 -> 404' $false $_.Exception.Message }

try {
  $r = Invoke-RestMethod "$base/api/notifications/minimal/categories" -Headers $h
  Check 'GET /api/notifications/minimal/categories' ($r.Count -ge 5)
} catch { Check 'GET /api/notifications/minimal/categories' $false $_.Exception.Message }

$createBody = @{
  title = 'REST created notification'
  message = 'Created via REST test harness'
  categoryId = 1
  companyId = 1
  siteId = 1
  serviceId = 1
  priority = 'High'
  targetAudience = 'All'
  expiryDate = (Get-Date).AddDays(7).ToString('o')
  actionRequired = $true
  actionUrl = '/x/1'
  attachmentPath = $null
  relatedEntityType = 'Test'
  relatedEntityId = 1
  createdBy = 1
} | ConvertTo-Json -Compress
$createdId = $null
try {
  $r = Invoke-RestMethod "$base/api/notifications/minimal" -Method Post -ContentType 'application/json' -Body $createBody -Headers $h
  $createdId = $r.notificationId
  Check 'POST /api/notifications/minimal (create)' ($createdId -gt 0)
} catch { Check 'POST /api/notifications/minimal (create)' $false $_.Exception.Message }

if ($createdId) {
  $updBody = @{
    notificationId = $createdId
    title = 'REST updated notification'
    message = 'Updated message'
    categoryId = 2
    companyId = 1
    siteId = 1
    serviceId = 1
    priority = 'Medium'
    status = 'Active'
    expiryDate = (Get-Date).AddDays(14).ToString('o')
    isActive = $true
    targetAudience = 'Auditors'
    actionRequired = $false
    actionUrl = $null
    attachmentPath = $null
    relatedEntityType = $null
    relatedEntityId = $null
    modifiedBy = 1
  } | ConvertTo-Json -Compress
  try {
    $r = Invoke-RestMethod "$base/api/notifications/minimal" -Method Put -ContentType 'application/json' -Body $updBody -Headers $h
    Check 'PUT /api/notifications/minimal' ($r.title -eq 'REST updated notification')
  } catch { Check 'PUT /api/notifications/minimal' $false $_.Exception.Message }

  try {
    $r = Invoke-RestMethod "$base/api/notifications/minimal/$createdId/read?userId=42" -Method Put -Headers $h
    Check 'PUT /api/notifications/minimal/{id}/read' ($r.notificationId -eq $createdId)
  } catch { Check 'PUT /api/notifications/minimal/{id}/read' $false $_.Exception.Message }

  try {
    $r = Invoke-RestMethod "$base/api/notifications/minimal/$createdId/archive?modifiedBy=1" -Method Put -Headers $h
    Check 'PUT /api/notifications/minimal/{id}/archive' ($r.notificationId -eq $createdId)
  } catch { Check 'PUT /api/notifications/minimal/{id}/archive' $false $_.Exception.Message }

  try {
    $r = Invoke-WebRequest "$base/api/notifications/minimal/$createdId" -Method Delete -Headers $h -UseBasicParsing
    Check 'DELETE /api/notifications/minimal/{id}' ($r.StatusCode -eq 204)
  } catch { Check 'DELETE /api/notifications/minimal/{id}' $false $_.Exception.Message }
}

$catBody = @{
  categoryName = "RestCat-$([int](Get-Date -UFormat %s))"
  categoryCode = "RC$([int](Get-Date -UFormat %s))"
  description = 'Created by REST test'
  color = '#123456'
  icon = 'flag'
  priority = 6
  displayOrder = 100
  createdBy = 1
} | ConvertTo-Json -Compress
try {
  $r = Invoke-RestMethod "$base/api/notifications/minimal/categories" -Method Post -ContentType 'application/json' -Body $catBody -Headers $h
  Check 'POST /api/notifications/minimal/categories' ($r.categoryId -gt 0)
} catch { Check 'POST /api/notifications/minimal/categories' $false $_.Exception.Message }


Write-Host "`n== GraphQL tests ==`n"

function GQL($name, $query, $check) {
  $tmp = New-TemporaryFile
  $body = @{ query = $query } | ConvertTo-Json -Compress -Depth 20
  Set-Content -Path $tmp -Value $body -Encoding UTF8
  $resp = curl.exe -s -X POST "$base/graphql" -H "Content-Type: application/json" -H "Authorization: Bearer $jwt" --data-binary "@$tmp"
  Remove-Item $tmp -Force
  try {
    $j = $resp | ConvertFrom-Json
    if ($j.errors) { Check $name $false ($j.errors[0].message) ; return $null }
    $ok = & $check $j
    Check $name $ok
    return $j
  } catch { Check $name $false $_.Exception.Message; return $null }
}

GQL 'gql notifications' 'query { notifications(category:[],company:[],service:[],site:[],pageNumber:1,pageSize:10){ data { totalItems items { infoId message } } } }' { param($j) $j.data.notifications -ne $null } | Out-Null
GQL 'gql categoriesFilter' 'query { categoriesFilter { data { id label } } }' { param($j) $j.data.categoriesFilter -ne $null } | Out-Null
GQL 'gql servicesFilter'   'query { servicesFilter   { data { id label } } }' { param($j) $j.data.servicesFilter   -ne $null } | Out-Null
GQL 'gql companiesFilter'  'query { companiesFilter  { data { id label } } }' { param($j) $j.data.companiesFilter  -ne $null } | Out-Null
GQL 'gql sitesFilter'      'query { sitesFilter      { data { id label children { id label } } } }' { param($j) $j.data.sitesFilter -ne $null } | Out-Null

$gqlCreated = $null
$j = GQL 'gql createNotification' 'mutation { createNotification(input:{ title:"GQL created", message:"created via gql", categoryId:1, companyId:1, siteId:1, serviceId:1, priority:"High", targetAudience:"All", actionRequired:true, actionUrl:"/x", relatedEntityType:"Test", relatedEntityId:1, createdBy:1 }) { notificationId title } }' { param($j) $j.data.createNotification.notificationId -gt 0 }
if ($j) { $gqlCreated = $j.data.createNotification.notificationId }

if ($gqlCreated) {
  GQL 'gql updateNotification' "mutation { updateNotification(input:{ notificationId:$gqlCreated, title:`"GQL updated`", message:`"updated`", categoryId:2, priority:`"Medium`", status:`"Active`", isActive:true, actionRequired:false, modifiedBy:1 }) { notificationId title } }" { param($j) $j.data.updateNotification.title -eq 'GQL updated' } | Out-Null
  GQL 'gql markNotificationRead' "mutation { markNotificationRead(notificationId:$gqlCreated, userId:99) { notificationId } }" { param($j) $j.data.markNotificationRead.notificationId -eq $gqlCreated } | Out-Null
  GQL 'gql archiveNotification' "mutation { archiveNotification(notificationId:$gqlCreated, modifiedBy:1) { notificationId status } }" { param($j) $j.data.archiveNotification.notificationId -eq $gqlCreated } | Out-Null
  GQL 'gql deleteNotification' "mutation { deleteNotification(notificationId:$gqlCreated) }" { param($j) $j.data.deleteNotification -eq $true } | Out-Null
}

$ts = [int](Get-Date -UFormat %s)
GQL 'gql createNotificationCategory' "mutation { createNotificationCategory(input:{ categoryName:`"GqlCat-$ts`", categoryCode:`"GC$ts`", description:`"gql cat`", color:`"#abcdef`", icon:`"x`", priority:7, displayOrder:200, createdBy:1 }) { categoryId categoryName } }" { param($j) $j.data.createNotificationCategory.categoryId -gt 0 } | Out-Null

Write-Host "`n========================="
Write-Host "PASS: $pass  FAIL: $fail"
