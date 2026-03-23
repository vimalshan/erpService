$base = "http://localhost:7136"
$results = @()

function Test-Endpoint($name, $method, $url, $body) {
    try {
        $params = @{ Uri = $url; Method = $method; UseBasicParsing = $true }
        if ($body) { $params.ContentType = "application/json"; $params.Body = $body }
        $r = Invoke-WebRequest @params
        $content = [System.Text.Encoding]::UTF8.GetString($r.RawContentStream.ToArray())
        return @{ Name=$name; Status=$r.StatusCode; Body=$content }
    } catch {
        $code = "ERR"
        $errBody = ""
        try { $code = [int]$_.Exception.Response.StatusCode } catch {}
        try { $s = $_.Exception.Response.GetResponseStream(); $reader = [System.IO.StreamReader]::new($s); $errBody = $reader.ReadToEnd(); $reader.Close() } catch {}
        return @{ Name=$name; Status=$code; Body=$errBody }
    }
}

Write-Output "============================================"
Write-Output " LocationServices.API Test Results"
Write-Output " Base URL: $base"
Write-Output "============================================"

# --- Health & Ping ---
Write-Output "`n--- Health & Ping ---"
$r = Test-Endpoint "Health" "GET" "$base/health"
Write-Output "[$($r.Status)] $($r.Name): $($r.Body)"

$r = Test-Endpoint "Ping" "GET" "$base/api/minimal/ping"
Write-Output "[$($r.Status)] $($r.Name): $($r.Body)"

# --- REST v1 ---
Write-Output "`n--- REST API v1 ---"

$r = Test-Endpoint "GET All Mappings" "GET" "$base/api/v1/location-app-maps"
$data = $r.Body | ConvertFrom-Json -ErrorAction SilentlyContinue
$count = if ($data -is [array]) { $data.Count } else { "N/A" }
Write-Output "[$($r.Status)] $($r.Name): $count records"
if ($data -is [array] -and $data.Count -gt 0) {
    $data | Select-Object -First 3 | ForEach-Object { Write-Output "    LocID=$($_.locationId) App=$($_.appName) Active=$($_.isActive)" }
    if ($data.Count -gt 3) { Write-Output "    ... and $($data.Count - 3) more" }
}

$r = Test-Endpoint "GET Active Mappings" "GET" "$base/api/v1/location-app-maps/active"
$data = $r.Body | ConvertFrom-Json -ErrorAction SilentlyContinue
$count = if ($data -is [array]) { $data.Count } else { "N/A" }
Write-Output "[$($r.Status)] $($r.Name): $count records"

$r = Test-Endpoint "GET Count" "GET" "$base/api/v1/location-app-maps/count"
Write-Output "[$($r.Status)] $($r.Name): $($r.Body)"

$r = Test-Endpoint "GET By Location (1)" "GET" "$base/api/v1/location-app-maps/by-location/1"
$data = $r.Body | ConvertFrom-Json -ErrorAction SilentlyContinue
$count = if ($data -is [array]) { $data.Count } else { "N/A" }
Write-Output "[$($r.Status)] $($r.Name): $count records"

$r = Test-Endpoint "GET Single (1/TestApp)" "GET" "$base/api/v1/location-app-maps/1/TestApp"
Write-Output "[$($r.Status)] $($r.Name): $($r.Body)"

# --- REST v2 ---
Write-Output "`n--- REST API v2 ---"

$r = Test-Endpoint "GET Paginated (p1,sz5)" "GET" "$base/api/v2/location-app-maps?page=1&pageSize=5"
$data = $r.Body | ConvertFrom-Json -ErrorAction SilentlyContinue
Write-Output "[$($r.Status)] $($r.Name): $($r.Body.Substring(0, [Math]::Min(200, $r.Body.Length)))..."

$r = Test-Endpoint "GET Active Summary" "GET" "$base/api/v2/location-app-maps/active/summary"
$data = $r.Body | ConvertFrom-Json -ErrorAction SilentlyContinue
$count = if ($data -is [array]) { $data.Count } else { "N/A" }
Write-Output "[$($r.Status)] $($r.Name): $count records"

# --- Swagger ---
Write-Output "`n--- Swagger ---"
$r = Test-Endpoint "Swagger JSON" "GET" "$base/swagger/v1/swagger.json"
Write-Output "[$($r.Status)] Swagger: Available"

# --- GraphQL Queries ---
Write-Output "`n--- GraphQL Queries ---"

$r = Test-Endpoint "GQL: locationAppMaps" "POST" "$base/graphql" '{"query":"{ locationAppMaps { locationId appName isActive siteCategoryCode selfAccess deemedApproval } }"}'
$data = $r.Body | ConvertFrom-Json -ErrorAction SilentlyContinue
if ($data.data) {
    $items = $data.data.locationAppMaps
    $count = if ($items -is [array]) { $items.Count } else { "1" }
    Write-Output "[$($r.Status)] locationAppMaps: $count records"
    if ($items -is [array]) { $items | Select-Object -First 3 | ForEach-Object { Write-Output "    LocID=$($_.locationId) App=$($_.appName) Active=$($_.isActive)" } }
} elseif ($data.errors) {
    Write-Output "[$($r.Status)] locationAppMaps: ERROR - $($data.errors[0].message)"
} else {
    Write-Output "[$($r.Status)] locationAppMaps: $($r.Body.Substring(0, [Math]::Min(200, $r.Body.Length)))"
}

$r = Test-Endpoint "GQL: activeLocationAppMaps" "POST" "$base/graphql" '{"query":"{ activeLocationAppMaps { locationId appName isActive } }"}'
$data = $r.Body | ConvertFrom-Json -ErrorAction SilentlyContinue
if ($data.data) {
    $items = $data.data.activeLocationAppMaps
    $count = if ($items -is [array]) { $items.Count } else { "1" }
    Write-Output "[$($r.Status)] activeLocationAppMaps: $count records"
} elseif ($data.errors) {
    Write-Output "[$($r.Status)] activeLocationAppMaps: ERROR - $($data.errors[0].message)"
} else {
    Write-Output "[$($r.Status)] activeLocationAppMaps: $($r.Body.Substring(0, [Math]::Min(200, $r.Body.Length)))"
}

$r = Test-Endpoint "GQL: locationAppMapCount" "POST" "$base/graphql" '{"query":"{ locationAppMapCount }"}'
$data = $r.Body | ConvertFrom-Json -ErrorAction SilentlyContinue
if ($data.data) {
    Write-Output "[$($r.Status)] locationAppMapCount: $($data.data.locationAppMapCount)"
} elseif ($data.errors) {
    Write-Output "[$($r.Status)] locationAppMapCount: ERROR - $($data.errors[0].message)"
} else {
    Write-Output "[$($r.Status)] locationAppMapCount: $($r.Body.Substring(0, [Math]::Min(200, $r.Body.Length)))"
}

$r = Test-Endpoint "GQL: locationAppMapsByLocation(1)" "POST" "$base/graphql" '{"query":"{ locationAppMapsByLocation(locationId: 1) { locationId appName isActive } }"}'
$data = $r.Body | ConvertFrom-Json -ErrorAction SilentlyContinue
if ($data.data) {
    $items = $data.data.locationAppMapsByLocation
    $count = if ($items -is [array]) { $items.Count } else { "0 or 1" }
    Write-Output "[$($r.Status)] locationAppMapsByLocation(1): $count records"
} elseif ($data.errors) {
    Write-Output "[$($r.Status)] locationAppMapsByLocation(1): ERROR - $($data.errors[0].message)"
} else {
    Write-Output "[$($r.Status)] locationAppMapsByLocation(1): $($r.Body.Substring(0, [Math]::Min(200, $r.Body.Length)))"
}

Write-Output "`n============================================"
Write-Output " Test Complete"
Write-Output "============================================"
