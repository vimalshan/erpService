$tok=(Get-Content e:\ERPMicroservice\tmp\jwt.txt -Raw).Trim()
$names=@('FindingStatusesPayload','FindingCategoriesPayload','BulkUpdateFindingsStatusPayload','UpdateFindingPayload','CloseFindingPayload','UpdateFindingInput','CloseFindingInput','BulkUpdateFindingsStatusInput')
foreach($n in $names){
 $q='{ __type(name:"'+$n+'") { name fields { name } inputFields { name } } }'
 $b=@{query=$q}|ConvertTo-Json -Compress
 $tmp=New-TemporaryFile;$b|Set-Content $tmp -Encoding utf8
 $resp=curl.exe -s -X POST http://localhost:5146/graphql -H "Authorization: Bearer $tok" -H "Content-Type: application/json" --data-binary "@$tmp"
 Remove-Item $tmp
 Write-Output "== $n =="
 Write-Output $resp
}
