$token='eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiJjYTkyZTAxNC0zZjhiLTQ4MWUtOGViNS1jYmQ0ZDJhNzUxOWEiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc4MTQ4MTIsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.aohLXrUK6iQqJcD2D7ItVTSoKPrGz2B7YsGyFLRUujI'
$h = @{ Authorization = "Bearer $token" }
$base = 'http://localhost:5211'

function Test-Endpoint($name, $method, $url, $body) {
    Write-Host ""
    Write-Host "=== $name : $method $url ===" -ForegroundColor Cyan
    try {
        if ($body) {
            $r = Invoke-WebRequest -Uri $url -Headers $h -Method $method -Body $body -ContentType 'application/json' -UseBasicParsing -TimeoutSec 15
        } else {
            $r = Invoke-WebRequest -Uri $url -Headers $h -Method $method -UseBasicParsing -TimeoutSec 15
        }
        Write-Host "Status: $($r.StatusCode)" -ForegroundColor Green
        $c = $r.Content; if ($c.Length -gt 1500) { $c.Substring(0,1500) + "...[truncated]" } else { $c }
    } catch {
        Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.ErrorDetails) { $_.ErrorDetails.Message }
    }
}

Test-Endpoint 'Health'             'GET'  "$base/health"
Test-Endpoint 'Actions list'       'GET'  "$base/api/actions?pageNumber=1&pageSize=10"
Test-Endpoint 'Actions HighPrio'   'GET'  "$base/api/actions?isHighPriority=true&pageNumber=1&pageSize=10"
Test-Endpoint 'Categories filter'  'GET'  "$base/api/actions/filters/categories"
Test-Endpoint 'Companies filter'   'GET'  "$base/api/actions/filters/companies"
Test-Endpoint 'Services filter'    'GET'  "$base/api/actions/filters/services"
Test-Endpoint 'Sites filter'       'GET'  "$base/api/actions/filters/sites"
Test-Endpoint 'Minimal list'       'GET'  "$base/api/actions/minimal"
Test-Endpoint 'Minimal by id'      'GET'  "$base/api/actions/minimal/1"

$createBody = @{
    action       = 'Verify nonconformity closure'
    dueDate      = (Get-Date).AddDays(5).ToString('yyyy-MM-ddTHH:mm:ssZ')
    highPriority = $true
    message      = 'Verify NC1 evidence pack'
    language     = 'en'
    service      = 'ISO 9001'
    site         = 'Headquarters - New York'
    entityType   = 'Finding'
    entityId     = 99
    subject      = 'NC closure'
    snowLink     = 'https://snow.example.com/RITM0099'
} | ConvertTo-Json -Compress
Test-Endpoint 'Create action'      'POST' "$base/api/actions" $createBody

Write-Host ""
Write-Host "===================== GraphQL =====================" -ForegroundColor Magenta

function GQL($name, $body) {
    Write-Host ""
    Write-Host "=== GQL $name ===" -ForegroundColor Magenta
    $hh = @{ Authorization = "Bearer $token"; Accept = 'application/json' }
    try {
        $r = Invoke-WebRequest -Uri "$base/graphql" -Method POST -Body $body -ContentType 'application/json' -Headers $hh -UseBasicParsing -TimeoutSec 15
        Write-Host "Status: $($r.StatusCode)" -ForegroundColor Green
        $c = $r.Content; if ($c.Length -gt 2000) { $c.Substring(0,2000)+"...[truncated]" } else { $c }
    } catch {
        Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.ErrorDetails) { $_.ErrorDetails.Message }
    }
}

GQL 'allActions' '{"query":"{ allActions { id action dueDate highPriority service site entityType entityId } }"}'
GQL 'actionById(1)' '{"query":"query($id:Int!){ actionById(id:$id){ id action highPriority site service } }","variables":{"id":1}}'
GQL 'actionsByEntity' '{"query":"query($t:String!,$e:Int!){ actionsByEntity(entityType:$t, entityId:$e){ id action site } }","variables":{"t":"Finding","e":1}}'
GQL 'actions paginated' '{"query":"query{ actions(category:null, company:null, service:null, site:null, isHighPriority:false, pageNumber:1, pageSize:5){ isSuccess message data { currentPage totalItems totalPages items { id action highPriority site } } } }"}'
GQL 'actionCategoriesFilter' '{"query":"{ actionCategoriesFilter(companies:null, services:null, sites:null){ isSuccess data { id label } } }"}'
GQL 'actionSitesFilter' '{"query":"{ actionSitesFilter(companies:null, categories:null, services:null){ isSuccess data { id label } } }"}'

GQL 'createAction mutation' '{"query":"mutation($i:CreateActionDtoInput!){ createAction(input:$i){ id action highPriority site } }","variables":{"i":{"action":"GQL test create","dueDate":"2026-12-01T00:00:00Z","highPriority":true,"message":"via GraphQL","language":"en","service":"ISO 14001","site":"Berlin Branch","entityType":"Finding","entityId":555,"subject":"GQL","snowLink":null}}}'
