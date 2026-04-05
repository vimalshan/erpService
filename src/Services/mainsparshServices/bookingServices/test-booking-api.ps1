#!/usr/bin/env pwsh
# BookingService API Test Script
$BASE = "http://localhost:5127"
$PASS = 0; $FAIL = 0

function ok([string]$Name, [bool]$Cond, [string]$Detail = "") {
    if ($Cond) { Write-Host "  [PASS] $Name" -ForegroundColor Green; $script:PASS++ }
    else       { Write-Host "  [FAIL] $Name  $Detail" -ForegroundColor Red; $script:FAIL++ }
}

$stamp = Get-Date -Format "HHmmssff"
Write-Host "`n================================================" -ForegroundColor Cyan
Write-Host "  BookingService Tests  http://localhost:5127"   -ForegroundColor Cyan
Write-Host "================================================`n" -ForegroundColor Cyan

# 1. Health
Write-Host "[ Health ]" -ForegroundColor Yellow
try {
    $h = Invoke-RestMethod -Uri "$BASE/health" -UseBasicParsing
    ok "GET /health - status Healthy" ($h.status -eq "Healthy")
} catch { ok "GET /health" $false $_.Exception.Message }

# 2. Auth
Write-Host "`n[ Auth ]" -ForegroundColor Yellow
try {
    $authResp = Invoke-RestMethod -Method POST -Uri "$BASE/api/auth/token" `
        -Body '{"userId":1,"email":"admin@booking.test","roles":["Admin","Manager","User"]}' `
        -ContentType "application/json" -UseBasicParsing
    $token = $authResp.token
    ok "POST /api/auth/token - token received" ($token -and $token.Length -gt 20)
} catch { ok "POST /api/auth/token" $false $_.Exception.Message; exit 1 }
$H = @{ Authorization = "Bearer $token" }

# 3. REST Controller
Write-Host "`n[ REST - /api/bookings ]" -ForegroundColor Yellow
try {
    $list = Invoke-RestMethod -Uri "$BASE/api/bookings" -Headers $H -UseBasicParsing
    ok "GET /api/bookings - returns 200" $true
    ok "GET /api/bookings - paged result" ($null -ne $list.items)
} catch { ok "GET /api/bookings" $false $_.Exception.Message }

$bookingId = 0
try {
    $body1 = '{"bookingAppNo":"BK-REST-' + $stamp + '","bookingTitle":"REST Test ' + $stamp + '","locationCode":"LOC01","bookingDate":"2026-06-01T00:00:00Z","createdBy":1}'
    $created = Invoke-RestMethod -Method POST -Uri "$BASE/api/bookings" -Headers $H -Body $body1 -ContentType "application/json" -UseBasicParsing
    $bookingId = $created.bookingId
    ok "POST /api/bookings - created with ID" ($bookingId -gt 0)
} catch { ok "POST /api/bookings" $false $_.Exception.Message }

if ($bookingId -gt 0) {
    try {
        $detail = Invoke-RestMethod -Uri "$BASE/api/bookings/$bookingId" -Headers $H -UseBasicParsing
        ok "GET /api/bookings/{id} - correct ID" ($detail.bookingId -eq $bookingId)
    } catch { ok "GET /api/bookings/{id}" $false $_.Exception.Message }
    try {
        $att = Invoke-RestMethod -Uri "$BASE/api/bookings/$bookingId/attendees" -Headers $H -UseBasicParsing
        ok "GET /api/bookings/{id}/attendees - returns 200" $true
    } catch { ok "GET /api/bookings/{id}/attendees" $false $_.Exception.Message }
}

# 4. Minimal API
Write-Host "`n[ Minimal API - /api/v2/bookings ]" -ForegroundColor Yellow
try {
    $v2list = Invoke-RestMethod -Uri "$BASE/api/v2/bookings" -Headers $H -UseBasicParsing
    ok "GET /api/v2/bookings - returns 200" $true
    ok "GET /api/v2/bookings - paged result" ($null -ne $v2list.items)
} catch { ok "GET /api/v2/bookings" $false $_.Exception.Message }

try {
    $stamp2 = Get-Date -Format "HHmmssff"
    $body2 = '{"bookingAppNo":"BK-MIN-' + $stamp2 + '","bookingTitle":"Minimal ' + $stamp2 + '","locationCode":"LOC02","bookingDate":"2026-07-01T00:00:00Z","createdBy":1}'
    $v2 = Invoke-RestMethod -Method POST -Uri "$BASE/api/v2/bookings" -Headers $H -Body $body2 -ContentType "application/json" -UseBasicParsing
    ok "POST /api/v2/bookings - created with ID" ($v2.bookingId -gt 0)
} catch { ok "POST /api/v2/bookings" $false $_.Exception.Message }

if ($bookingId -gt 0) {
    try {
        $v2d = Invoke-RestMethod -Uri "$BASE/api/v2/bookings/$bookingId" -Headers $H -UseBasicParsing
        ok "GET /api/v2/bookings/{id} - correct ID" ($v2d.bookingId -eq $bookingId)
    } catch { ok "GET /api/v2/bookings/{id}" $false $_.Exception.Message }
}

# 5. GraphQL
Write-Host "`n[ GraphQL - /graphql ]" -ForegroundColor Yellow
try {
    $gqlBody = '{"query":"{ bookings { bookingId bookingAppNo status } }"}'
    $gql1 = Invoke-RestMethod -Method POST -Uri "$BASE/graphql" -Body $gqlBody -ContentType "application/json" -UseBasicParsing
    ok "GraphQL getBookings - no errors" (-not $gql1.errors)
    ok "GraphQL getBookings - data returned" ($null -ne $gql1.data)
} catch { ok "GraphQL getBookings" $false $_.Exception.Message }

if ($bookingId -gt 0) {
    try {
        $gqlBody2 = '{"query":"{ bookingById(id:' + $bookingId + ') { bookingId bookingTitle status } }"}'
        $gql2 = Invoke-RestMethod -Method POST -Uri "$BASE/graphql" -Body $gqlBody2 -ContentType "application/json" -UseBasicParsing
        ok "GraphQL getBookingById - no errors" (-not $gql2.errors)
        ok "GraphQL getBookingById - correct ID" ($gql2.data.bookingById.bookingId -eq $bookingId)
    } catch { ok "GraphQL getBookingById" $false $_.Exception.Message }
}

try {
    $stamp3  = Get-Date -Format "HHmmssff"
    $dateStr = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ssZ")
    $mutBody = '{"query":"mutation { createBooking(bookingAppNo: \"BK-GQL-' + $stamp3 + '\" bookingTitle: \"GQL ' + $stamp3 + '\" locationCode: \"LOC03\" bookingDate: \"' + $dateStr + '\" createdBy: 1) { bookingId status } }"}'
    $gql3 = Invoke-RestMethod -Method POST -Uri "$BASE/graphql" -Body $mutBody -ContentType "application/json" -UseBasicParsing
    ok "GraphQL createBooking - no errors" (-not $gql3.errors)
    ok "GraphQL createBooking - returns ID" ($gql3.data.createBooking.bookingId -gt 0)
} catch { ok "GraphQL createBooking" $false $_.Exception.Message }

# 6. RabbitMQ
Write-Host "`n[ RabbitMQ - /api/rabbitmq/test ]" -ForegroundColor Yellow
try {
    $rmq = Invoke-RestMethod -Uri "$BASE/api/rabbitmq/test" -UseBasicParsing
    ok "RabbitMQ test - connected, message published" ($rmq.status -eq "connected")
} catch {
    $resp = $_.Exception.Response
    if ($resp -and [int]$resp.StatusCode -eq 503) {
        ok "RabbitMQ test - graceful 503 (broker unavailable)" $true
    } else {
        ok "RabbitMQ test" $false $_.Exception.Message
    }
}

# Summary
$total = $PASS + $FAIL
Write-Host "`n================================================" -ForegroundColor Cyan
Write-Host "  Results: $PASS / $total passed" -ForegroundColor $(if ($FAIL -eq 0) { "Green" } else { "Yellow" })
if ($FAIL -gt 0) { Write-Host "  $FAIL test(s) FAILED" -ForegroundColor Red }
else             { Write-Host "  ALL TESTS PASSED"    -ForegroundColor Green }
Write-Host "================================================`n" -ForegroundColor Cyan
