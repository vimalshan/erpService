$tok=(Get-Content e:\ERPMicroservice\tmp\jwt.txt -Raw).Trim()
$q = 'mutation($i: UpdateFindingInput!) { updateFinding(input:$i) { finding { findingId } message } }'
$vars = @{ i = @{ findingId = 1; status='Open'; response='ok' } }
$body = @{ query = $q; variables = $vars } | ConvertTo-Json -Compress -Depth 10
$tmp=New-TemporaryFile
$body | Set-Content $tmp -Encoding utf8
curl.exe -s -X POST http://localhost:5146/graphql -H "Authorization: Bearer $tok" -H "Content-Type: application/json" --data-binary "@$tmp"
Remove-Item $tmp
