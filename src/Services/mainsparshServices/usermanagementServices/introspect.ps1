$q = @{ query = '{ __schema { queryType { fields { name } } mutationType { fields { name } } } }' } | ConvertTo-Json
$r = Invoke-WebRequest -Uri "http://localhost:5243/graphql" -Method POST -Body $q -ContentType "application/json" -UseBasicParsing
$json = [System.Text.Encoding]::UTF8.GetString($r.Content) | ConvertFrom-Json
Write-Host "Queries:"
$json.data.__schema.queryType.fields | ForEach-Object { Write-Host "  $($_.name)" }
Write-Host "Mutations:"
$json.data.__schema.mutationType.fields | ForEach-Object { Write-Host "  $($_.name)" }
