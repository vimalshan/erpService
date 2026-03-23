$base = "http://localhost:5181"

# ── Generate JWT Token ──────────────────────────────────────────────
Add-Type -AssemblyName System.Security
function New-Jwt {
    $header = @{ alg = "HS256"; typ = "JWT" } | ConvertTo-Json -Compress
    $now = [int][double]::Parse(([DateTimeOffset]::UtcNow).ToUnixTimeSeconds().ToString())
    $payload = @{
        sub = "test-user"
        jti = [guid]::NewGuid().ToString()
        iat = $now
        exp = $now + 3600
        iss = "LovService"
        aud = "LovServiceClients"
        role = "Admin"
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" = "Admin"
    } | ConvertTo-Json -Compress

    $toBase64Url = { param($bytes) [Convert]::ToBase64String($bytes).TrimEnd('=').Replace('+','-').Replace('/','_') }
    $headerB64 = & $toBase64Url ([System.Text.Encoding]::UTF8.GetBytes($header))
    $payloadB64 = & $toBase64Url ([System.Text.Encoding]::UTF8.GetBytes($payload))

    $key = [System.Text.Encoding]::UTF8.GetBytes("LovService-SuperSecretKey-DoNotExposeInProduction-2026!!")
    $hmac = New-Object System.Security.Cryptography.HMACSHA256
    $hmac.Key = $key
    $sigBytes = $hmac.ComputeHash([System.Text.Encoding]::UTF8.GetBytes("$headerB64.$payloadB64"))
    $sigB64 = & $toBase64Url $sigBytes

    return "$headerB64.$payloadB64.$sigB64"
}

$token = New-Jwt
$authHeader = "Bearer $token"

function Test-Endpoint {
    param([string]$label, [string]$method, [string]$url, [string]$body = $null, [bool]$useAuth = $true)
    $args = @("-s", "-w", "\nHTTP_CODE:%{http_code}", "-X", $method)
    if ($useAuth) {
        $args += @("-H", "Authorization: $authHeader")
    }
    $args += @("-H", "Content-Type: application/json")
    if ($body) { $args += @("-d", $body) }
    $args += $url
    $raw = & curl.exe @args 2>&1 | Out-String
    $code = "???"
    if ($raw -match "HTTP_CODE:(\d+)") { $code = $matches[1] }
    $respBody = ($raw -replace "\nHTTP_CODE:\d+","").Trim()
    return @{ Code = $code; Body = $respBody; Label = $label }
}

Write-Output "============================================"
Write-Output " LovService.API Test Results"
Write-Output " Base URL: $base"
Write-Output "============================================"

# ── Health ──
Write-Output "`n--- Health & Swagger ---"
$r = Test-Endpoint "Health" "GET" "$base/health" -useAuth $false
Write-Output "[$($r.Code)] Health: $($r.Body)"
$r = Test-Endpoint "Swagger" "GET" "$base/swagger/index.html" -useAuth $false
Write-Output "[$($r.Code)] Swagger: $(if($r.Code -eq '200'){'Available'}else{$r.Body.Substring(0,[Math]::Min(80,$r.Body.Length))})"

# ── LOV Types ──
Write-Output "`n--- REST: LOV Types (/api/v1/lov-types) ---"
$r = Test-Endpoint "GET All LovTypes" "GET" "$base/api/v1/lov-types"
$data = $null; try { $data = $r.Body | ConvertFrom-Json } catch {}
if ($data -is [array]) {
    Write-Output "[$($r.Code)] GET All LovTypes: $($data.Count) records"
    $data | Select-Object -First 3 | ForEach-Object {
        Write-Output "    ID=$($_.lovTypeId) Name=$($_.lovTypeName)"
    }
    if ($data.Count -gt 3) { Write-Output "    ... and $($data.Count - 3) more" }
} else {
    Write-Output "[$($r.Code)] GET All LovTypes: $($r.Body.Substring(0,[Math]::Min(100,$r.Body.Length)))"
}

$r = Test-Endpoint "GET LovType by ID (1)" "GET" "$base/api/v1/lov-types/1"
Write-Output "[$($r.Code)] GET LovType(1): $($r.Body.Substring(0,[Math]::Min(120,$r.Body.Length)))"

# ── LOV Masters ──
Write-Output "`n--- REST: LOV Masters (/api/v1/lov-masters) ---"
$r = Test-Endpoint "GET All LovMasters" "GET" "$base/api/v1/lov-masters"
$data = $null; try { $data = $r.Body | ConvertFrom-Json } catch {}
if ($data -is [array]) {
    Write-Output "[$($r.Code)] GET All LovMasters: $($data.Count) records"
    $data | Select-Object -First 3 | ForEach-Object {
        Write-Output "    ID=$($_.lovId) TypeID=$($_.lovTypeId) Name=$($_.lovName)"
    }
    if ($data.Count -gt 3) { Write-Output "    ... and $($data.Count - 3) more" }
} else {
    Write-Output "[$($r.Code)] GET All LovMasters: $($r.Body.Substring(0,[Math]::Min(100,$r.Body.Length)))"
}

$r = Test-Endpoint "GET LovMaster by ID (1)" "GET" "$base/api/v1/lov-masters/1"
Write-Output "[$($r.Code)] GET LovMaster(1): $($r.Body.Substring(0,[Math]::Min(120,$r.Body.Length)))"

$r = Test-Endpoint "GET LovMasters by Type (1)" "GET" "$base/api/v1/lov-masters/by-type/1"
$data = $null; try { $data = $r.Body | ConvertFrom-Json } catch {}
if ($data -is [array]) {
    Write-Output "[$($r.Code)] GET LovMasters by-type(1): $($data.Count) records"
} else {
    Write-Output "[$($r.Code)] GET LovMasters by-type(1): $($r.Body.Substring(0,[Math]::Min(120,$r.Body.Length)))"
}

# ── Item Data ──
Write-Output "`n--- REST: Item Data (/api/v1/item-data) ---"
$r = Test-Endpoint "GET All ItemData" "GET" "$base/api/v1/item-data"
$data = $null; try { $data = $r.Body | ConvertFrom-Json } catch {}
if ($data -is [array]) {
    Write-Output "[$($r.Code)] GET All ItemData: $($data.Count) records"
    $data | Select-Object -First 3 | ForEach-Object {
        Write-Output "    ID=$($_.itemId) Cat=$($_.catName) Item=$($_.itemName) Price=$($_.price)"
    }
    if ($data.Count -gt 3) { Write-Output "    ... and $($data.Count - 3) more" }
} else {
    Write-Output "[$($r.Code)] GET All ItemData: $($r.Body.Substring(0,[Math]::Min(100,$r.Body.Length)))"
}

$r = Test-Endpoint "GET ItemData by ID (1)" "GET" "$base/api/v1/item-data/1"
Write-Output "[$($r.Code)] GET ItemData(1): $($r.Body.Substring(0,[Math]::Min(120,$r.Body.Length)))"

$r = Test-Endpoint "Search ItemData" "GET" "$base/api/v1/item-data/search?catName=Office"
$data = $null; try { $data = $r.Body | ConvertFrom-Json } catch {}
if ($data -is [array]) {
    Write-Output "[$($r.Code)] Search ItemData(catName=Office): $($data.Count) records"
} else {
    Write-Output "[$($r.Code)] Search ItemData(catName=Office): $($r.Body.Substring(0,[Math]::Min(120,$r.Body.Length)))"
}

# ── GraphQL Queries ──
Write-Output "`n--- GraphQL Queries ---"

$queries = @(
    @{ name="lovTypes"; body='{"query":"{ lovTypes { lovTypeId lovTypeName } }"}' },
    @{ name="lovType(1)"; body='{"query":"{ lovType(id: 1) { lovTypeId lovTypeName } }"}' },
    @{ name="lovMasters"; body='{"query":"{ lovMasters { lovId lovTypeId lovName } }"}' },
    @{ name="lovMastersByType(1)"; body='{"query":"{ lovMastersByType(lovTypeId: 1) { lovId lovName } }"}' },
    @{ name="itemData"; body='{"query":"{ itemData { id catName itemName make uom price } }"}' },
    @{ name="searchItemData"; body='{"query":"{ searchItemData(catName: \"Electronics\") { id catName itemName } }"}' }
)

foreach ($q in $queries) {
    try {
        $resp = Invoke-WebRequest -Uri "$base/graphql" -Method POST -ContentType "application/json" -Body $q.body -UseBasicParsing
        $code = $resp.StatusCode
        $text = [System.Text.Encoding]::UTF8.GetString([byte[]]$resp.Content)
    } catch {
        $code = $_.Exception.Response.StatusCode.value__
        $reader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
        $text = $reader.ReadToEnd()
    }
    $data = $null; try { $data = $text | ConvertFrom-Json } catch {}
    if ($data.data) {
        $field = ($data.data.PSObject.Properties | Select-Object -First 1)
        $val = $field.Value
        if ($val -is [array] -or ($val.PSObject -and $val -is [System.Collections.IEnumerable] -and $val -isnot [string])) {
            $arr = @($val)
            Write-Output "[$code] $($q.name): $($arr.Count) records"
        } elseif ($null -ne $val) {
            $json = $val | ConvertTo-Json -Compress
            Write-Output "[$code] $($q.name): $($json.Substring(0,[Math]::Min(120,$json.Length)))"
        } else {
            Write-Output "[$code] $($q.name): null"
        }
    } elseif ($data.errors) {
        Write-Output "[$code] $($q.name): ERROR - $($data.errors[0].message)"
    } else {
        Write-Output "[$code] $($q.name): $($text.Substring(0,[Math]::Min(120,$text.Length)))"
    }
}

Write-Output "`n============================================"
Write-Output " Test Complete"
Write-Output "============================================"
