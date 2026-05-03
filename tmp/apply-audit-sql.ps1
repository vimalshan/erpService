$ErrorActionPreference = 'Continue'
$srv = '(localdb)\MSSQLLocalDB'
$db = 'ERPAuditDB'
$root = 'e:\ERPMicroservice\src\Services\auditServices\auditapiServices'
$logFile = 'e:\ERPMicroservice\tmp\db-setup.log'
Remove-Item $logFile -ErrorAction Ignore

function Apply-SqlFolder($folder, $label) {
    Add-Content $logFile "==== $label ($folder) ===="
    if (-not (Test-Path $folder)) { Add-Content $logFile "MISSING: $folder"; return }
    $files = Get-ChildItem $folder -Filter *.sql -Recurse | Sort-Object FullName
    $ok = 0; $fail = 0
    foreach ($f in $files) {
        $out = & sqlcmd -S $srv -E -C -d $db -i $f.FullName -b 2>&1
        if ($LASTEXITCODE -eq 0) { $ok++ } else {
            $fail++
            Add-Content $logFile "FAIL: $($f.FullName)"
            Add-Content $logFile ($out | Out-String)
        }
    }
    Add-Content $logFile "Result: OK=$ok FAIL=$fail of $($files.Count)"
}

Apply-SqlFolder (Join-Path $root 'tables') 'TABLES'
Apply-SqlFolder (Join-Path $root 'insert-scripts') 'SEEDS'
Apply-SqlFolder (Join-Path $root 'Stored-procedure') 'STORED PROCS'
Get-Content $logFile
