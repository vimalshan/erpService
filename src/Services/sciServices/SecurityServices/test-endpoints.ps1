$BASE = "http://localhost:5009/api/v1"
$ErrorActionPreference = "Continue"

# 1. Get token
$tok = (curl.exe -s -X POST "$BASE/auth/token" -H "Content-Type: application/json" --data-binary "@$env:TEMP\login.json" | ConvertFrom-Json).accessToken
if ($tok.Length -lt 50) { Write-Host "ERROR: Could not get token" -ForegroundColor Red; exit 1 }
Write-Host "TOKEN OK (len=$($tok.Length))" -ForegroundColor Green
$H = "Authorization: Bearer $tok"

# 2. CRUD roles
Write-Host "`n=== ROLES ===" -ForegroundColor Yellow
$r = curl.exe -s -w "`n%{http_code}" -X POST "$BASE/roles" -H $H -H "Content-Type: application/json" --data-binary "@$env:TEMP\role20.json"
$lines = $r -split "`n"; Write-Host "POST /roles (20): $($lines[0]) [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" -X PUT "$BASE/roles/20" -H $H -H "Content-Type: application/json" --data-binary "@$env:TEMP\roleupd20.json"
$lines = $r -split "`n"; Write-Host "PUT  /roles/20:   $($lines[0]) [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" "$BASE/roles/20" -H $H
$lines = $r -split "`n"; Write-Host "GET  /roles/20:   $($lines[0]) [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" "$BASE/roles" -H $H
$lines = $r -split "`n"; Write-Host "GET  /roles:      ... [HTTP $($lines[1])]"

# 3. CRUD users
Write-Host "`n=== USERS ===" -ForegroundColor Yellow
$r = curl.exe -s -w "`n%{http_code}" -X POST "$BASE/users" -H $H -H "Content-Type: application/json" --data-binary "@$env:TEMP\user200.json"
$lines = $r -split "`n"; Write-Host "POST /users (200):        $($lines[0]) [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" "$BASE/users/200" -H $H
$lines = $r -split "`n"; Write-Host "GET  /users/200:          $($lines[0]) [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" -X PUT "$BASE/users/200" -H $H -H "Content-Type: application/json" --data-binary "@$env:TEMP\userupd200.json"
$lines = $r -split "`n"; Write-Host "PUT  /users/200:          $($lines[0]) [HTTP $($lines[1])]"

# 4. Role assignment
Write-Host "`n=== ROLE ASSIGNMENT ===" -ForegroundColor Yellow
$r = curl.exe -s -w "`n%{http_code}" -X POST "$BASE/users/200/roles" -H $H -H "Content-Type: application/json" --data-binary "@$env:TEMP\assign200.json"
$lines = $r -split "`n"; Write-Host "POST /users/200/roles:    $($lines[0]) [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" "$BASE/users/200/roles" -H $H
$lines = $r -split "`n"; Write-Host "GET  /users/200/roles:    $($lines[0]) [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" -X DELETE "$BASE/users/200/roles/1" -H $H
$lines = $r -split "`n"; Write-Host "DELETE /users/200/roles/1: $($lines[0]) [HTTP $($lines[1])]"

# 5. Deactivate user
$r = curl.exe -s -w "`n%{http_code}" -X DELETE "$BASE/users/200" -H $H
$lines = $r -split "`n"; Write-Host "DELETE /users/200:         $($lines[0]) [HTTP $($lines[1])]"

# 6. Menus
Write-Host "`n=== MENUS ===" -ForegroundColor Yellow
$r = curl.exe -s -w "`n%{http_code}" "$BASE/menus" -H $H
$lines = $r -split "`n"; Write-Host "GET /menus: ... [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" "$BASE/menus/role/1" -H $H
$lines = $r -split "`n"; Write-Host "GET /menus/role/1: $($lines[0]) [HTTP $($lines[1])]"

# 7. Health
Write-Host "`n=== HEALTH ===" -ForegroundColor Yellow
$r = curl.exe -s -w "`n%{http_code}" http://localhost:5009/health
$lines = $r -split "`n"; Write-Host "GET /health:       $($lines[0]) [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" http://localhost:5009/health/ready
$lines = $r -split "`n"; Write-Host "GET /health/ready: $($lines[0]) [HTTP $($lines[1])]"

# 8. Scalar docs
Write-Host "`n=== SCALAR DOCS ===" -ForegroundColor Yellow
$r = curl.exe -s -w "`n%{http_code}" -o NUL http://localhost:5009/scalar/v1
Write-Host "GET /scalar/v1: [HTTP $r]"

# 9. GraphQL
Write-Host "`n=== GRAPHQL ===" -ForegroundColor Yellow
$r = curl.exe -s -w "`n%{http_code}" -X POST http://localhost:5009/graphql -H "Content-Type: application/json" -H $H --data-binary "@$env:TEMP\gql_users.json"
$lines = $r -split "`n"; Write-Host "GraphQL users query: $($lines[0]) [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" -X POST http://localhost:5009/graphql -H "Content-Type: application/json" -H $H --data-binary "@$env:TEMP\gql_roles.json"
$lines = $r -split "`n"; Write-Host "GraphQL roles query: $($lines[0]) [HTTP $($lines[1])]"

# 10. Minimal APIs
Write-Host "`n=== MINIMAL APIS ===" -ForegroundColor Yellow
$r = curl.exe -s -w "`n%{http_code}" "$BASE/ping"
$lines = $r -split "`n"; Write-Host "GET /ping: $($lines[0]) [HTTP $($lines[1])]"

$r = curl.exe -s -w "`n%{http_code}" "$BASE/users/search?q=admin" -H $H
$lines = $r -split "`n"; Write-Host "GET /users/search?q=admin: $($lines[0]) [HTTP $($lines[1])]"

Write-Host "`n=== ALL DONE ===" -ForegroundColor Green
