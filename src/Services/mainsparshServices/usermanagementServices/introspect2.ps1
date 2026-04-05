$q = @{ query = '{ __type(name: "CreateUserPolicyCommandInput") { inputFields { name type { name kind ofType { name } } } } }' } | ConvertTo-Json
$json = Invoke-RestMethod -Uri "http://localhost:5243/graphql" -Method POST -Body $q -ContentType "application/json"
Write-Host "CreateUserPolicyCommandInput fields:"
$json.data.__type.inputFields | ForEach-Object { $tn = if($_.type.name){$_.type.name}else{$_.type.ofType.name}; Write-Host "  $($_.name) ($tn $($_.type.kind))" }

$q2 = @{ query = '{ __type(name: "CreateWebsiteContactCommandInput") { inputFields { name type { name kind ofType { name } } } } }' } | ConvertTo-Json
$json2 = Invoke-RestMethod -Uri "http://localhost:5243/graphql" -Method POST -Body $q2 -ContentType "application/json"
Write-Host "`nCreateWebsiteContactCommandInput fields:"
$json2.data.__type.inputFields | ForEach-Object { $tn = if($_.type.name){$_.type.name}else{$_.type.ofType.name}; Write-Host "  $($_.name) ($tn $($_.type.kind))" }
