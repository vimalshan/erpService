$base = "http://localhost:5031/api"
$gql  = "http://localhost:5031/graphql"
$pass = 0; $fail = 0; $results = @()
$ts = [DateTimeOffset]::UtcNow.ToUnixTimeSeconds()

function Test($name, $block) {
    try {
        $r = & $block
        $script:pass++
        $script:results += "PASS: $name"
        Write-Host "PASS: $name" -F Green
        return $r
    } catch {
        $script:fail++
        $script:results += "FAIL: $name - $_"
        Write-Host "FAIL: $name - $_" -F Red
        return $null
    }
}

$h = @{ "Content-Type" = "application/json" }

# ─── 1. AUTH ───
$token = Test "POST /auth/token" {
    $r = Invoke-RestMethod "$base/v1/auth/token" -Method Post -ContentType "application/json" -Body '{"username":"admin","password":"admin123"}'
    if (-not $r.accessToken) { throw "No token" }
    $r.accessToken
}
$ah = @{ Authorization = "Bearer $token"; "Content-Type" = "application/json" }

# ─── 2. CURRENCIES CRUD ───
Test "GET /currencies (seed data)" {
    $r = Invoke-RestMethod "$base/currencies" -Headers $h
    if ($r.Count -lt 6) { throw "Expected at least 6 currencies, got $($r.Count)" }
    Write-Host "  Found $($r.Count) currencies" -F Cyan
}

Test "GET /currencies/1 (USD)" {
    $r = Invoke-RestMethod "$base/currencies/1" -Headers $h
    if ($r.name -ne "US Dollar") { throw "Expected US Dollar, got $($r.name)" }
    Write-Host "  Currency: $($r.name) ($($r.symbol))" -F Cyan
}

Test "GET /currencies/2 (EUR)" {
    $r = Invoke-RestMethod "$base/currencies/2" -Headers $h
    Write-Host "  Currency: $($r.name) ($($r.symbol))" -F Cyan
}

$newCurrId = 100 + ($ts % 900)
$newCurr = Test "POST /currencies (create)" {
    $body = @{ currencyId = $newCurrId; name = "Test Currency $ts"; symbol = "TC"; modifiedBy = 1 } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/currencies" -Method Post -Headers $h -Body $body
    if ($r.currencyId -ne $newCurrId) { throw "Expected ID $newCurrId, got $($r.currencyId)" }
    Write-Host "  Created: $($r.name) ID=$($r.currencyId)" -F Cyan
    $r
}

Test "PUT /currencies/$newCurrId (update)" {
    $body = @{ currencyId = $newCurrId; name = "Updated Currency $ts"; symbol = "UC"; modifiedBy = 1 } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/currencies/$newCurrId" -Method Put -Headers $h -Body $body
    if ($r.name -notlike "Updated*") { throw "Name not updated: $($r.name)" }
    Write-Host "  Updated: $($r.name) ($($r.symbol))" -F Cyan
}

# ─── 3. EXCHANGE RATES ───
Test "GET /exchangerates/2/1/2026/1 (EUR->USD Jan)" {
    $r = Invoke-RestMethod "$base/exchangerates/2/1/2026/1" -Headers $h
    if ($r.rate -lt 1) { throw "Rate too low: $($r.rate)" }
    Write-Host "  EUR->USD Jan 2026: Rate=$($r.rate)" -F Cyan
}

Test "GET /exchangerates/2/1/2026/2 (EUR->USD Feb)" {
    $r = Invoke-RestMethod "$base/exchangerates/2/1/2026/2" -Headers $h
    Write-Host "  EUR->USD Feb 2026: Rate=$($r.rate)" -F Cyan
}

Test "GET /exchangerates/3/1/2026/3 (GBP->USD Mar)" {
    $r = Invoke-RestMethod "$base/exchangerates/3/1/2026/3" -Headers $h
    Write-Host "  GBP->USD Mar 2026: Rate=$($r.rate)" -F Cyan
}

$newRateId = 200 + ($ts % 800)
Test "POST /exchangerates (set new rate)" {
    $body = @{ rateId = $newRateId; financialYear = 2026; month = 6; fromCurrencyId = 1; toCurrencyId = 2; rate = 0.85; modifiedBy = 1 } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/exchangerates" -Method Post -Headers $h -Body $body
    Write-Host "  Set rate: USD->EUR Jun 2026 = $($r.rate)" -F Cyan
}

Test "POST /exchangerates (update existing rate)" {
    $body = @{ rateId = $newRateId; financialYear = 2026; month = 6; fromCurrencyId = 1; toCurrencyId = 2; rate = 0.87; modifiedBy = 1 } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/exchangerates" -Method Post -Headers $h -Body $body
    Write-Host "  Updated rate: USD->EUR Jun 2026 = $($r.rate)" -F Cyan
}

# ─── 4. CURRENCY CONVERSION ───
Test "POST /exchangerates/convert (EUR->USD)" {
    $body = @{ fromCurrencyId = 2; toCurrencyId = 1; amount = 1000; financialYear = 2026; month = 1 } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/exchangerates/convert" -Method Post -Headers $h -Body $body
    if ($r.convertedAmount -le 0) { throw "Converted amount should be positive: $($r.convertedAmount)" }
    Write-Host "  1000 EUR = $($r.convertedAmount) USD (rate: $($r.exchangeRate))" -F Cyan
}

Test "POST /exchangerates/convert (GBP->USD)" {
    $body = @{ fromCurrencyId = 3; toCurrencyId = 1; amount = 500; financialYear = 2026; month = 3 } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/exchangerates/convert" -Method Post -Headers $h -Body $body
    Write-Host "  500 GBP = $($r.convertedAmount) USD (rate: $($r.exchangeRate))" -F Cyan
}

Test "POST /exchangerates/convert (INR->USD)" {
    $body = @{ fromCurrencyId = 4; toCurrencyId = 1; amount = 100000; financialYear = 2026; month = 3 } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/exchangerates/convert" -Method Post -Headers $h -Body $body
    Write-Host "  100000 INR = $($r.convertedAmount) USD (rate: $($r.exchangeRate))" -F Cyan
}

# ─── 5. ORGANIZATION CURRENCIES ───
Test "GET /organizationcurrencies/100" {
    $r = Invoke-RestMethod "$base/organizationcurrencies/100" -Headers $h
    if ($r.Count -lt 1) { throw "Expected at least 1 mapping, got $($r.Count)" }
    Write-Host "  Org 100 has $($r.Count) currencies" -F Cyan
}

Test "GET /organizationcurrencies/101" {
    $r = Invoke-RestMethod "$base/organizationcurrencies/101" -Headers $h
    Write-Host "  Org 101 has $($r.Count) currencies" -F Cyan
}

$newOrgId = 500 + ($ts % 500)
Test "POST /organizationcurrencies (map org to currency)" {
    $body = @{ organizationId = $newOrgId; currencyId = 1; modifiedBy = 1 } | ConvertTo-Json
    $r = Invoke-RestMethod "$base/organizationcurrencies" -Method Post -Headers $h -Body $body
    Write-Host "  Mapped Org $newOrgId to Currency $($r.currencyId)" -F Cyan
}

# ─── 6. DELETE CURRENCY (cleanup) ───
Test "DELETE /currencies/$newCurrId" {
    Invoke-RestMethod "$base/currencies/$newCurrId" -Method Delete -Headers $h
    Write-Host "  Deleted currency $newCurrId" -F Cyan
}

# ─── 7. GRAPHQL QUERIES ───
Write-Host "`n--- GraphQL Queries ---" -F Yellow

function GQL($query) {
    $body = @{ query = $query } | ConvertTo-Json -Depth 5
    $r = Invoke-RestMethod $gql -Method Post -Headers $h -Body $body
    if ($r.errors) { throw ($r.errors | ConvertTo-Json -Compress) }
    $r.data
}

Test "GQL: currencies" {
    $d = GQL '{ currencies { currencyId name symbol } }'
    Write-Host "  Currencies: $($d.currencies.Count)" -F Cyan
}

Test "GQL: currency(id:1)" {
    $d = GQL '{ currency(id: 1) { currencyId name symbol } }'
    Write-Host "  Currency: $($d.currency.name) ($($d.currency.symbol))" -F Cyan
}

Test "GQL: exchangeRate" {
    $d = GQL '{ exchangeRate(fromCurrencyId: 2, toCurrencyId: 1, financialYear: 2026, month: 1) { rateId rate fromCurrencyId toCurrencyId } }'
    Write-Host "  Rate: $($d.exchangeRate.rate)" -F Cyan
}

Test "GQL: convertAmount" {
    $d = GQL '{ convertAmount(fromCurrencyId: 2, toCurrencyId: 1, amount: 1000, financialYear: 2026, month: 1) { originalAmount convertedAmount exchangeRate } }'
    Write-Host "  1000 EUR = $($d.convertAmount.convertedAmount) USD" -F Cyan
}

Test "GQL: organizationCurrencies" {
    $d = GQL '{ organizationCurrencies(organizationId: 100) { organizationId currencyId } }'
    Write-Host "  Org 100 currencies: $($d.organizationCurrencies.Count)" -F Cyan
}

# ─── 8. GRAPHQL MUTATIONS ───
Write-Host "`n--- GraphQL Mutations ---" -F Yellow

$gqlCurrId = 300 + ($ts % 700)
Test "GQL Mutation: createCurrency" {
    $d = GQL "mutation { createCurrency(input: { currencyId: $gqlCurrId, name: ""GQL Currency"", symbol: ""GQ"", modifiedBy: 1 }) { currencyId name symbol } }"
    Write-Host "  Created: $($d.createCurrency.name) ID=$($d.createCurrency.currencyId)" -F Cyan
}

Test "GQL Mutation: updateCurrency" {
    $d = GQL "mutation { updateCurrency(input: { currencyId: $gqlCurrId, name: ""GQL Updated"", symbol: ""GU"", modifiedBy: 1 }) { currencyId name symbol } }"
    Write-Host "  Updated: $($d.updateCurrency.name) ($($d.updateCurrency.symbol))" -F Cyan
}

$gqlRateId = 5000 + ($ts % 600)
Test "GQL Mutation: setExchangeRate" {
    $d = GQL "mutation { setExchangeRate(input: { rateId: $gqlRateId, financialYear: 2026, month: 7, fromCurrencyId: 3, toCurrencyId: 2, rate: 1.15, modifiedBy: 1 }) { rateId rate fromCurrencyId toCurrencyId } }"
    Write-Host "  Set rate: GBP->EUR Jul 2026 = $($d.setExchangeRate.rate)" -F Cyan
}

$gqlOrgId = 600 + ($ts % 400)
Test "GQL Mutation: mapOrganizationCurrency" {
    $d = GQL "mutation { mapOrganizationCurrency(input: { organizationId: $gqlOrgId, currencyId: 2, modifiedBy: 1 }) { organizationId currencyId } }"
    Write-Host "  Mapped Org $gqlOrgId to Currency $($d.mapOrganizationCurrency.currencyId)" -F Cyan
}

Test "GQL Mutation: deleteCurrency" {
    $d = GQL "mutation { deleteCurrency(currencyId: $gqlCurrId) }"
    Write-Host "  Deleted: $($d.deleteCurrency)" -F Cyan
}

# ─── 9. HEALTH CHECK ───
Test "GET /health" {
    $r = Invoke-RestMethod "http://localhost:5031/health"
    Write-Host "  Health: $($r.status)" -F Cyan
}

# ─── 10. DOMAIN EVENTS ───
Write-Host "`n--- Domain Events & RabbitMQ ---" -F Yellow
Write-Host "Domain events dispatched via MediatR -> Event Handlers log + publish to RabbitMQ (graceful degradation)." -F Cyan
Write-Host "Events: CurrencyCreated, CurrencyUpdated, CurrencyDeleted, ExchangeRateSet, OrganizationCurrencyMapped" -F Cyan

# ─── SUMMARY ───
Write-Host "`n========================================" -F White
Write-Host " RESULTS: $pass PASSED, $fail FAILED" -F $(if ($fail -eq 0) { "Green" } else { "Red" })
Write-Host "========================================" -F White
$results | ForEach-Object { Write-Host $_ -F $(if ($_ -match "^PASS") { "Green" } else { "Red" }) }
