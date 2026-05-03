$token='eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjMzA1ZTg4Mi0wMTYxLTRhMjctOGI1Ny1mNzA5ZDU0ZjQwYWEiLCJlbWFpbCI6Iml5eWFuYXJtc2VjQGdtYWlsLmNvbSIsInVuaXF1ZV9uYW1lIjoiaXl5YW5hcm1zZWMiLCJqdGkiOiJjYTkyZTAxNC0zZjhiLTQ4MWUtOGViNS1jYmQ0ZDJhNzUxOWEiLCJmaXJzdE5hbWUiOiJJeXlhbmFyIiwibGFzdE5hbWUiOiJNc2VjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQURNSU4iLCJleHAiOjE3Nzc4MTQ4MTIsImlzcyI6IkF1dGhQcm92aWRlciIsImF1ZCI6IkF1dGhQcm92aWRlckNsaWVudHMifQ.aohLXrUK6iQqJcD2D7ItVTSoKPrGz2B7YsGyFLRUujI'
$h=@{Authorization="Bearer $token"}
$base='http://localhost:5212'

function T($n,$m,$u,$b){
    Write-Host "`n=== $n : $m $u ===" -ForegroundColor Cyan
    try{
        if($b){ $r=Invoke-WebRequest -Uri $u -Headers $h -Method $m -Body $b -ContentType 'application/json' -UseBasicParsing -TimeoutSec 15 }
        else { $r=Invoke-WebRequest -Uri $u -Headers $h -Method $m -UseBasicParsing -TimeoutSec 15 }
        Write-Host "Status: $($r.StatusCode)" -ForegroundColor Green
        $c=$r.Content; if($c.Length -gt 1500){$c.Substring(0,1500)+"...[truncated]"}else{$c}
    }catch{
        Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
        if($_.ErrorDetails){$_.ErrorDetails.Message}
    }
}

# REST controller endpoints (these use Dapper + stored procs - may fail since SPs are not seeded)
T 'Health'             'GET'  "$base/health"
T 'CertList(Dapper)'   'GET'  "$base/api/certificates"
T 'CertDetails(Dapper)' 'GET' "$base/api/certificates/1"
T 'CertSites(Dapper)'  'GET'  "$base/api/certificates/1/sites"

# Minimal API (uses MediatR + EF DbContext)
T 'Minimal list'       'GET'  "$base/api/certificates/minimal"
T 'Minimal by id (1)'  'GET'  "$base/api/certificates/minimal/1"

$createBody = @{
    certificateNumber='CERT-2026-NEW'
    certificateName='ISO 27001 - Data Center'
    companyId=1
    siteId=1
    serviceId=4
    issueDate='2026-05-03T00:00:00Z'
    expiryDate='2029-05-02T00:00:00Z'
    certificateType='Initial'
    scope='Information Security Management'
} | ConvertTo-Json -Compress
T 'Minimal create'     'POST' "$base/api/certificates/minimal" $createBody

Write-Host "`n========== GraphQL ==========" -ForegroundColor Magenta
function GQL($name,$body){
    Write-Host "`n=== GQL $name ===" -ForegroundColor Magenta
    try{
        $r=Invoke-WebRequest -Uri "$base/graphql" -Method POST -Body $body -ContentType 'application/json' -Headers $h -UseBasicParsing -TimeoutSec 15
        Write-Host "Status: $($r.StatusCode)" -ForegroundColor Green
        $c=$r.Content; if($c.Length -gt 2000){$c.Substring(0,2000)+"...[truncated]"}else{$c}
    }catch{
        Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
        if($_.ErrorDetails){$_.ErrorDetails.Message}
    }
}

# These hit ICertificateService -> Dapper (may fail without stored procs)
GQL 'certificates(Dapper)' '{"query":"{ certificates { isSuccess message data { certificateId certificateNumber companyId status } } }"}'
GQL 'viewCertificateDetails(Dapper)' '{"query":"query($id:Int!){ viewCertificateDetails(certificateId:$id){ isSuccess message data { certificateId certificateNumber status } } }","variables":{"id":1}}'

# Mutations use MediatR + EF
GQL 'createCertificate' '{"query":"mutation($i:CreateCertificateDtoInput!){ createCertificate(input:$i){ certificateId certificateNumber certificateName status } }","variables":{"i":{"certificateNumber":"CERT-GQL-001","certificateName":"GQL Test ISO 9001","companyId":1,"siteId":1,"serviceId":1,"issueDate":"2026-01-01T00:00:00Z","expiryDate":"2029-01-01T00:00:00Z","certificateType":"Initial","scope":"GraphQL test"}}}'

GQL 'updateCertificate' '{"query":"mutation($i:UpdateCertificateDtoInput!){ updateCertificate(input:$i){ certificateId certificateNumber status } }","variables":{"i":{"certificateId":1,"certificateNumber":"CERT-2024-001","certificateName":"ISO 9001 - HQ NY (Updated)","companyId":1,"siteId":1,"serviceId":1,"issueDate":"2024-03-20T00:00:00Z","expiryDate":"2027-03-19T00:00:00Z","status":"Active","certificateType":"Initial","scope":"Updated scope via GraphQL"}}}'

GQL 'deleteCertificate(5)' '{"query":"mutation($id:Int!){ deleteCertificate(id:$id) }","variables":{"id":5}}'

GQL 'certificates after mutations(Dapper)' '{"query":"{ certificates { isSuccess message data { certificateId certificateNumber status } } }"}'
