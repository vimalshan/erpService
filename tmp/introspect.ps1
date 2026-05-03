$tok=(Get-Content e:\ERPMicroservice\tmp\jwt.txt -Raw).Trim()
foreach($t in 'BulkUpdateFindingsStatusPayload','FindingStatusesPayload','FindingCategoriesPayload','UpdateFindingPayload','CloseFindingPayload'){
$q='{ __type(name:"'+$t+'") { fields { name type { name kind ofType { name kind } } } } }'
$b=@{query=$q}|ConvertTo-Json -Compress
$tmp=New-TemporaryFile
$b|Set-Content $tmp -Encoding utf8
$r=curl.exe -s -X POST http://localhost:5146/graphql -H "Authorization: Bearer $tok" -H "Content-Type: application/json" --data-binary "@$tmp"
Remove-Item $tmp
Write-Host "==$t=="
($r|ConvertFrom-Json).data.__type.fields|ForEach-Object{
  $tn=if($_.type.ofType.name){$_.type.ofType.name}else{$_.type.name}
  "  $($_.name): $tn"
}
}
