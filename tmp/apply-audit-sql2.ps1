$srv = '(localdb)\MSSQLLocalDB'
$db = 'ERPAuditDB'
$root = 'e:\ERPMicroservice\src\Services\auditServices\auditapiServices'
$logFile = 'e:\ERPMicroservice\tmp\db-setup2.log'
Remove-Item $logFile -ErrorAction Ignore

function Apply-Pass($folder, $pass) {
    if (-not (Test-Path $folder)) { return @{Ok=0;Fail=0;Files=@()} }
    $files = Get-ChildItem $folder -Filter *.sql -Recurse | Sort-Object FullName
    $ok = 0; $failed = @()
    foreach ($f in $files) {
        $out = & sqlcmd -S $srv -E -C -d $db -i $f.FullName -b 2>&1
        if ($LASTEXITCODE -eq 0) { $ok++ } else {
            $failed += $f
            Add-Content $logFile "[Pass $pass] FAIL $($f.Name): $(($out | Out-String).Trim())"
        }
    }
    return @{Ok=$ok; Failed=$failed; Total=$files.Count}
}

Write-Host "=== TABLES ==="
for ($i=1; $i -le 4; $i++) {
    $r = Apply-Pass (Join-Path $root 'tables') $i
    Write-Host "Pass ${i}: OK=$($r.Ok)/$($r.Total)"
    if ($r.Failed.Count -eq 0) { break }
}

Write-Host "=== SEEDS ==="
for ($i=1; $i -le 3; $i++) {
    $r = Apply-Pass (Join-Path $root 'insert-scripts') $i
    Write-Host "Pass ${i}: OK=$($r.Ok)/$($r.Total)"
    if ($r.Failed.Count -eq 0) { break }
}

Write-Host "=== STORED PROCS ==="
$r = Apply-Pass (Join-Path $root 'Stored-procedure') 1
Write-Host "OK=$($r.Ok)/$($r.Total)"

# Verify tables
$tables = & sqlcmd -S $srv -E -C -d $db -h -1 -W -Q "SELECT COUNT(*) FROM sys.tables; SELECT COUNT(*) FROM sys.procedures;"
Write-Host "DB OBJECTS:"
Write-Host $tables
