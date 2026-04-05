$q = @{ query = '{ __schema { queryType { fields { name } } mutationType { fields { name } } } }' } | ConvertTo-Json
$r = Invoke-WebRequest -Uri "http://localhost:5272/graphql" -Method POST -Body $q -ContentType "application/json" -UseBasicParsing
$json = [System.Text.Encoding]::UTF8.GetString($r.Content) | ConvertFrom-Json
Write-Host "Queries:"
$json.data.__schema.queryType.fields | ForEach-Object { Write-Host "  $($_.name)" }
Write-Host "Mutations:"
$json.data.__schema.mutationType.fields | ForEach-Object { Write-Host "  $($_.name)" }

# Test a query with full fields
$q2 = @{ query = '{ timesheetById(id: 1) { timesheetId employeeId workDate totalHours status approvalStatus } }' } | ConvertTo-Json
$r2 = Invoke-WebRequest -Uri "http://localhost:5272/graphql" -Method POST -Body $q2 -ContentType "application/json" -UseBasicParsing
Write-Host "`nSample query result:"
Write-Host ([System.Text.Encoding]::UTF8.GetString($r2.Content))
