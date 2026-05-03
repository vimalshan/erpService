# Cross-check test of all auditServices queries and mutations
param(
    [string]$Token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiJhMjYzM2I4Zi1mMTc5LTQyMmQtYjE1My0yYzQ1ZmNkODQxMzgiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc2Mzc0MTUsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.Q95pZQJoaiLzvLZ4mXz0T48I92obneCf2vZJA46enB4'
)

$ErrorActionPreference = 'Continue'
$headers = @{ Authorization = "Bearer $Token"; 'Content-Type' = 'application/json' }

$services = [ordered]@{
    action       = 5001
    audit        = 5002
    certificate  = 5003
    contract     = 5004
    finance      = 5005
    findings     = 5006
    notification = 5007
    schedule     = 5008
    settings     = 5009
    overview     = 5011
}

function Get-SdlBlock([string]$sdl, [string]$typeName) {
    $m = [regex]::Match($sdl, "(?s)^type $typeName \{(.*?)\n\}", 'Multiline')
    if ($m.Success) { return $m.Groups[1].Value }
    return $null
}

function Get-FieldList([string]$block) {
    if (-not $block) { return @() }
    # Match each top-level field entry: "  fieldName(args): ReturnType ..."
    $rx = [regex]'(?m)^\s{2}([a-zA-Z_][a-zA-Z0-9_]*)(\s*\(([^)]*)\))?\s*:\s*([^\n@]+?)(\s*@.*)?$'
    $items = @()
    foreach ($m in $rx.Matches($block)) {
        $items += [pscustomobject]@{
            Name = $m.Groups[1].Value
            Args = $m.Groups[3].Value.Trim()
            Return = $m.Groups[4].Value.Trim()
        }
    }
    return $items
}

# Build a "smart" sample value for an input scalar/list signature
function New-SampleArg([string]$argDecl) {
    # argDecl example: "pageNumber: Int!" or "ids: [Int!]!" or "filter: SomeInput!"
    $parts = $argDecl -split ':\s*', 2
    if ($parts.Count -lt 2) { return $null }
    $name = $parts[0].Trim()
    $type = $parts[1].Trim().TrimEnd('!')
    $isList = $type.StartsWith('[')
    $inner = $type.Trim('[', ']').TrimEnd('!')
    $value = switch -Wildcard ($inner) {
        'Int'      { 1 }
        'Long'     { 1 }
        'Float'    { 1.0 }
        'Decimal'  { 1.0 }
        'Boolean'  { '$false-token$' }
        'String'   { '""' }
        'ID'       { '"1"' }
        'DateTime' { '"2024-01-01T00:00:00Z"' }
        'Date'     { '"2024-01-01"' }
        'UUID'     { '"00000000-0000-0000-0000-000000000000"' }
        default    {
            # Input object - send empty {}
            '{}'
        }
    }
    if ($null -eq $value) { return $null }
    if ($isList) { $value = "[]" }
    if ($value -eq '$false-token$') { $value = 'false' }
    return "$name`: $value"
}

function Build-FieldArgList([string]$argsBlock) {
    if (-not $argsBlock) { return '' }
    # Split by spaces between argument declarations is unreliable;
    # arguments are separated by spaces in SDL. But values inside types may include "[Int!]".
    # Use regex to split top-level args.
    $args = @()
    $depth = 0
    $current = ''
    foreach ($ch in $argsBlock.ToCharArray()) {
        if ($ch -eq '[' -or $ch -eq '(') { $depth++ }
        if ($ch -eq ']' -or $ch -eq ')') { $depth-- }
        if ($ch -eq ' ' -and $depth -eq 0 -and $current -match ':') {
            # Look ahead — only split if current looks like complete "name: Type"
            # Simpler: split on regex of "([a-zA-Z_]\w*\s*:)"
        }
        $current += $ch
    }
    # Simpler approach: split using regex lookahead for "name:" patterns.
    $rx = [regex]'(?<=^|\s)([a-zA-Z_]\w*)\s*:\s*([^\s][^\s]*(?:\s*\[[^\]]*\][!]?)?)'
    $matches = [regex]::Matches($argsBlock, '([a-zA-Z_]\w*)\s*:\s*((?:\[[^\]]+\][!]?)|(?:[A-Za-z_]\w*[!]?))')
    foreach ($m in $matches) {
        $argDecl = "$($m.Groups[1].Value): $($m.Groups[2].Value)"
        $sample = New-SampleArg $argDecl
        if ($sample) { $args += $sample }
    }
    if ($args.Count -eq 0) { return '' }
    return '(' + ($args -join ', ') + ')'
}

function Invoke-Op($port, $opType, $name, $argsBlock, $returnsScalarish) {
    $argList = Build-FieldArgList $argsBlock
    # For selection set: try common shape, fall back to __typename
    $selection = ' { __typename }'
    # Special-case scalar return types: no selection set
    $returnTrim = ($returnsScalarish -replace '!|\[|\]', '').Trim()
    if ($returnTrim -in @('Boolean','String','Int','Long','Float','Decimal','ID','DateTime','Date','UUID')) {
        $selection = ''
    }
    $opPrefix = if ($opType -eq 'Mutation') { 'mutation' } else { 'query' }
    $query = "$opPrefix { $name$argList$selection }"
    $body = @{ query = $query } | ConvertTo-Json -Compress
    try {
        $r = Invoke-RestMethod -Uri "http://localhost:$port/graphql" -Method Post -Headers $headers -Body $body -TimeoutSec 20
        if ($r.errors) {
            return [pscustomobject]@{ Status='ERROR'; Detail=$r.errors[0].message }
        }
        return [pscustomobject]@{ Status='OK'; Detail=($r.data | ConvertTo-Json -Compress -Depth 4) }
    } catch {
        $resp = $_.Exception.Response
        if ($resp) {
            $sr = New-Object IO.StreamReader($resp.GetResponseStream())
            $err = $sr.ReadToEnd()
            return [pscustomobject]@{ Status="HTTP $([int]$resp.StatusCode)"; Detail=$err }
        }
        return [pscustomobject]@{ Status='EXCEPTION'; Detail=$_.Exception.Message }
    }
}

$results = @()
foreach ($svc in $services.GetEnumerator()) {
    $name = $svc.Key; $port = $svc.Value
    Write-Host "`n========== $name (port $port) ==========" -ForegroundColor Cyan
    try {
        $resp = Invoke-WebRequest -Uri "http://localhost:$port/graphql?sdl" -UseBasicParsing -Headers $headers -TimeoutSec 10
        $sdl = [System.Text.Encoding]::UTF8.GetString($resp.Content)
    } catch {
        Write-Host "  SDL fetch failed: $($_.Exception.Message)" -ForegroundColor Red
        continue
    }

    foreach ($opType in 'Query','Mutation') {
        $block = Get-SdlBlock $sdl $opType
        $fields = Get-FieldList $block
        if ($fields.Count -eq 0) { Write-Host "  (no $opType fields)" -ForegroundColor DarkGray; continue }
        Write-Host "  -- $opType ($($fields.Count) fields)" -ForegroundColor Yellow
        foreach ($f in $fields) {
            $res = Invoke-Op $port $opType $f.Name $f.Args $f.Return
            $color = if ($res.Status -eq 'OK') { 'Green' } elseif ($res.Status -like 'HTTP*' -or $res.Status -eq 'ERROR') { 'Red' } else { 'Magenta' }
            $detail = $res.Detail; if ($detail.Length -gt 140) { $detail = $detail.Substring(0,140) + '...' }
            Write-Host ("    {0,-40} [{1}] {2}" -f $f.Name, $res.Status, $detail) -ForegroundColor $color
            $results += [pscustomobject]@{ Service=$name; Op=$opType; Field=$f.Name; Status=$res.Status; Detail=$res.Detail }
        }
    }
}

Write-Host "`n`n=========== SUMMARY ===========" -ForegroundColor Cyan
$summary = $results | Group-Object Service, Status | Select-Object @{n='Service';e={($_.Name -split ', ')[0]}}, @{n='Status';e={($_.Name -split ', ')[1]}}, Count
$summary | Sort-Object Service, Status | Format-Table -AutoSize

$results | Export-Csv -Path "$env:TEMP\audit-services-test-results.csv" -NoTypeInformation -Force
Write-Host "Detailed results: $env:TEMP\audit-services-test-results.csv" -ForegroundColor Cyan
