$ErrorActionPreference = 'Continue'
$base = 'http://localhost:5146'
$token = (Get-Content e:\ERPMicroservice\tmp\jwt.txt -Raw).Trim()
$auth = @{ Authorization = "Bearer $token" }
$pass = 0; $fail = 0; $failures = @()

function T($name, $block) {
    try {
        & $block
        Write-Host "  PASS  $name" -ForegroundColor Green
        $script:pass++
    } catch {
        Write-Host "  FAIL  $name -> $($_.Exception.Message)" -ForegroundColor Red
        $script:fail++
        $script:failures += $name
    }
}

function GraphQL($q, $vars=$null) {
    $body = @{ query = $q }
    if ($vars) { $body.variables = $vars }
    $tmp = New-TemporaryFile
    ($body | ConvertTo-Json -Depth 10 -Compress) | Set-Content -Path $tmp -Encoding utf8
    $resp = curl.exe -s -X POST "$base/graphql" -H "Authorization: Bearer $token" -H "Content-Type: application/json" --data-binary "@$tmp"
    Remove-Item $tmp -Force
    if (-not $resp) { throw 'empty response' }
    $obj = $resp | ConvertFrom-Json
    if ($obj.errors) {
        $msg = ($obj.errors | ForEach-Object { $_.message }) -join '; '
        throw "GraphQL: $msg"
    }
    return $obj.data
}

Write-Host "== REST tests ==" -ForegroundColor Cyan
T 'GET /health' { Invoke-RestMethod "$base/health" -Headers $auth | Out-Null }
T 'GET /api/findings' {
    $r = Invoke-RestMethod "$base/api/findings" -Headers $auth
    if ($r.Count -lt 1) { throw "expected findings, got $($r.Count)" }
}
T 'GET /api/findings/1' {
    $r = Invoke-RestMethod "$base/api/findings/1" -Headers $auth
    if (-not $r.findingNumber) { throw 'no findingNumber' }
}
T 'GET /api/findings/9999 -> 404' {
    try { Invoke-RestMethod "$base/api/findings/9999" -Headers $auth | Out-Null; throw 'expected 404' }
    catch { if ($_.Exception.Response.StatusCode.value__ -ne 404) { throw $_ } }
}
T 'GET /api/findings/search?term=PPE' {
    $r = Invoke-RestMethod "$base/api/findings/search?term=PPE" -Headers $auth
    if ($r.Count -lt 1) { throw "expected results, got $($r.Count)" }
}
T 'POST /api/findings (create)' {
    $body = @{
        findingNumber = "FND-TEST-$([Guid]::NewGuid().ToString().Substring(0,8))"
        auditId = 1001; siteId = 1; title = 'Test finding'; description = 'created via API test'
        findingType = 'Quality'; severity = 'Low'; findingStatusId = 1; findingCategoryId = 2
        identifiedDate = (Get-Date).ToString('o')
    } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/api/findings" -Method Post -Headers $auth -ContentType 'application/json' -Body $body
    if (-not $r.findingId) { throw 'no findingId' }
    $script:newId = $r.findingId
}
T 'PUT /api/findings/{id}' {
    if (-not $script:newId) { throw 'no newId from create' }
    $body = @{ findingId = $script:newId; title = 'Updated title'; description = 'updated'; findingType = 'Quality'; severity = 'Low'; findingStatusId = 2; findingCategoryId = 2 } | ConvertTo-Json
    Invoke-RestMethod "$base/api/findings/$($script:newId)" -Method Put -Headers $auth -ContentType 'application/json' -Body $body | Out-Null
}
T 'POST /api/findings/{id}/close' {
    if (-not $script:newId) { throw 'no newId' }
    $body = @{ findingId = $script:newId; closureNotes = 'verified'; closedBy = 1 } | ConvertTo-Json
    Invoke-RestMethod "$base/api/findings/$($script:newId)/close" -Method Post -Headers $auth -ContentType 'application/json' -Body $body | Out-Null
}
T 'POST /api/findings/bulk-status' {
    $body = @{ findingIds = @(1,2); newStatusId = 2; reason = 'test' } | ConvertTo-Json
    Invoke-RestMethod "$base/api/findings/bulk-status" -Method Post -Headers $auth -ContentType 'application/json' -Body $body | Out-Null
}
T 'GET /api/companies' {
    $r = Invoke-RestMethod "$base/api/companies" -Headers $auth
    if ($r.Count -lt 1) { throw "expected companies, got $($r.Count)" }
}
T 'GET /api/companies/1' {
    $r = Invoke-RestMethod "$base/api/companies/1" -Headers $auth
    if (-not $r.companyName) { throw 'no companyName' }
}
T 'GET /api/companies/1/sites' {
    $r = Invoke-RestMethod "$base/api/companies/1/sites" -Headers $auth
    if ($r.Count -lt 1) { throw "expected sites, got $($r.Count)" }
}

Write-Host "`n== GraphQL tests ==" -ForegroundColor Cyan
T 'gql findings (paged)' {
    $d = GraphQL '{ findings(first:5) { totalCount nodes { findingId findingNumber title } } }'
    if (-not $d.findings.nodes -or $d.findings.nodes.Count -lt 1) { throw 'no findings' }
}
T 'gql finding(id:1)' {
    $d = GraphQL '{ finding(id:1) { findingId findingNumber title } }'
    if (-not $d.finding) { throw 'no finding' }
}
T 'gql findingsStatistics' {
    $d = GraphQL '{ findingsStatistics { totalCount openCount closedCount } }'
    if ($null -eq $d.findingsStatistics) { throw 'no stats' }
}
T 'gql searchFindings (paged)' {
    $d = GraphQL 'query($t:String!,$s:SearchField!){ searchFindings(searchTerm:$t,searchIn:$s,first:5) { nodes { findingId title } } }' @{ t='PPE'; s='ALL' }
    if (-not $d.searchFindings.nodes) { throw 'no search nodes' }
}
T 'gql companies (paged)' {
    $d = GraphQL '{ companies(first:5) { nodes { companyId companyName } } }'
    if (-not $d.companies.nodes -or $d.companies.nodes.Count -lt 1) { throw 'no companies' }
}
T 'gql sites (paged)' {
    $d = GraphQL '{ sites(companyId:1,first:5) { nodes { siteId siteName } } }'
    if (-not $d.sites.nodes) { throw 'no sites' }
}
T 'gql createFinding' {
    $q = 'mutation($i: CreateFindingInput!) { createFinding(input:$i) { finding { findingId findingNumber } message } }'
    $vars = @{ i = @{ title='GQL test'; description='gql desc'; category='Minor'; companyId=1; siteId=2; severity='Low' } }
    $d = GraphQL $q $vars
    if (-not $d.createFinding.finding.findingId) { throw 'no id' }
    $script:gqlId = $d.createFinding.finding.findingId
}
T 'gql updateFinding' {
    if (-not $script:gqlId) { throw 'no gqlId' }
    $q = 'mutation($i: UpdateFindingInput!) { updateFinding(input:$i) { finding { findingId } message } }'
    $vars = @{ i = @{ findingId = $script:gqlId; status='Open'; response='ok' } }
    GraphQL $q $vars | Out-Null
}
T 'gql closeFinding' {
    if (-not $script:gqlId) { throw 'no gqlId' }
    $q = 'mutation($i: CloseFindingInput!) { closeFinding(input:$i) { finding { findingId } message } }'
    $vars = @{ i = @{ findingId = $script:gqlId; closureNotes='gql close' } }
    GraphQL $q $vars | Out-Null
}
T 'gql bulkUpdateFindingsStatus' {
    $q = 'mutation($i: BulkUpdateFindingsStatusInput!) { bulkUpdateFindingsStatus(input:$i) { bulkUpdatePayload { updatedCount failedCount } } }'
    $vars = @{ i = @{ findingIds = @(1,2); newStatus='Open'; reason='test' } }
    $d = GraphQL $q $vars
    if ($null -eq $d.bulkUpdateFindingsStatus.bulkUpdatePayload) { throw 'no bulk payload' }
}
T 'gql findingStatuses' {
    $d = GraphQL 'mutation { findingStatuses { findingStatusDto { findingStatusId statusName } } }'
    if (-not $d.findingStatuses.findingStatusDto -or $d.findingStatuses.findingStatusDto.Count -lt 5) { throw "expected 5 statuses" }
}
T 'gql findingCategories' {
    $d = GraphQL 'mutation { findingCategories { findingCategoryDto { findingCategoryId categoryName } } }'
    if (-not $d.findingCategories.findingCategoryDto -or $d.findingCategories.findingCategoryDto.Count -lt 5) { throw 'expected 5 categories' }
}

Write-Host "`n=========================" -ForegroundColor Cyan
$color = if ($fail -eq 0) { 'Green' } else { 'Yellow' }
Write-Host "PASS: $pass  FAIL: $fail" -ForegroundColor $color
if ($failures.Count) { Write-Host ("Failures:`n  " + ($failures -join "`n  ")) -ForegroundColor Red }
