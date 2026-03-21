param()
$base = "http://127.0.0.1:5070"
$sep = "=" * 60

function Show-Result($label, $code, $body) {
    Write-Host "$sep"
    Write-Host "  $label"
    Write-Host "  Status: $code"
    if ($body) { Write-Host "  Response: $body" }
}

# 1. AUTH
$tokenResp = Invoke-RestMethod "$base/api/Auth/token" -Method POST -ContentType "application/json" -Body '{"username":"admin","password":"admin123"}'
$tok = $tokenResp.token
Show-Result "[1] POST /api/Auth/token" "200 OK" "Token: $($tok.Substring(0,40))..."
$h = @{ Authorization = "Bearer $tok" }

# 2. HEALTH
$hc = Invoke-RestMethod "$base/health"
Show-Result "[2] GET /health" "200 OK" $hc

# 3. GET ComplaintGroups (empty)
$r = Invoke-RestMethod "$base/api/ComplaintGroups" -Headers $h
Show-Result "[3] GET /api/ComplaintGroups" "200 OK" "Count=$($r.Count)"

# 4. POST ComplaintGroups
try {
    $b = '{"groupSrc":1001,"description":"NCR Group Alpha","groupRef":"GRP-ALPHA","frequency":"D","target":8}'
    $r = Invoke-RestMethod "$base/api/ComplaintGroups" -Method POST -ContentType "application/json" -Headers $h -Body $b
    Show-Result "[4] POST /api/ComplaintGroups" "201 Created" ($r | ConvertTo-Json -Compress)
} catch {
    Show-Result "[4] POST /api/ComplaintGroups" $_.Exception.Response.StatusCode $_.ErrorDetails.Message
}

# 5. GET ComplaintGroups after create
$r = Invoke-RestMethod "$base/api/ComplaintGroups" -Headers $h
Show-Result "[5] GET /api/ComplaintGroups (after create)" "200 OK" "Count=$($r.Count) | $($r | ConvertTo-Json -Compress)"

# 6. POST Complaint
try {
    $td = (Get-Date).AddDays(3).ToString("yyyy-MM-ddTHH:mm:ss")
    $b = "{`"groupSrc`":1001,`"description`":`"Machine vibration above threshold`",`"raisedBy`":1001,`"targetDate`":`"$td`"}"
    $r = Invoke-RestMethod "$base/api/Complaints" -Method POST -ContentType "application/json" -Headers $h -Body $b
    $global:tNum = $r
    Show-Result "[6] POST /api/Complaints" "201 Created" "TicketNum=$r"
} catch {
    Show-Result "[6] POST /api/Complaints" $_.Exception.Response.StatusCode $_.ErrorDetails.Message
}

# 7. GET Complaints paged
$r = Invoke-RestMethod "$base/api/Complaints?page=1&pageSize=10" -Headers $h
Show-Result "[7] GET /api/Complaints (paged)" "200 OK" "Count=$($r.Count)"

# 8. GET Complaint by ID
if ($global:tNum) {
    try {
        $r = Invoke-RestMethod "$base/api/Complaints/$($global:tNum)" -Headers $h
        Show-Result "[8] GET /api/Complaints/$($global:tNum)" "200 OK" ($r | ConvertTo-Json -Compress -Depth 3)
    } catch {
        Show-Result "[8] GET /api/Complaints/$($global:tNum)" $_.Exception.Response.StatusCode $_.ErrorDetails.Message
    }

    # 9. POST UpdateAction
    try {
        $b = "{`"ticketNum`":$($global:tNum),`"level`":`"P`",`"remark`":`"Root cause identified`"}"
        $r = Invoke-RestMethod "$base/api/ComplaintActions" -Method POST -ContentType "application/json" -Headers $h -Body $b
        Show-Result "[9] POST /api/ComplaintActions" "204/200 OK" ""
    } catch {
        Show-Result "[9] POST /api/ComplaintActions" $_.Exception.Response.StatusCode $_.ErrorDetails.Message
    }

    # 10. GET Status (Dapper)
    try {
        $r = Invoke-RestMethod "$base/api/Complaints/$($global:tNum)/status" -Headers $h
        Show-Result "[10] GET /api/Complaints/$($global:tNum)/status" "200 OK" ($r | ConvertTo-Json -Compress)
    } catch {
        Show-Result "[10] GET .../status" $_.Exception.Response.StatusCode $_.ErrorDetails.Message
    }

    # 11. Close Complaint
    try {
        $b = "{`"ticketNum`":$($global:tNum),`"closedBy`":1001,`"remark`":`"Resolved`"}"
        $r = Invoke-RestMethod "$base/api/Complaints/$($global:tNum)/close" -Method POST -ContentType "application/json" -Headers $h -Body $b
        Show-Result "[11] POST .../close" "204/200 OK" ""
    } catch {
        Show-Result "[11] POST .../close" $_.Exception.Response.StatusCode $_.ErrorDetails.Message
    }

    # 12. Reopen Complaint
    try {
        $b = "{`"ticketNum`":$($global:tNum),`"reopenedBy`":1001,`"reason`":`"Customer not satisfied`"}"
        $r = Invoke-RestMethod "$base/api/Complaints/$($global:tNum)/reopen" -Method POST -ContentType "application/json" -Headers $h -Body $b
        Show-Result "[12] POST .../reopen" "204/200 OK" ""
    } catch {
        Show-Result "[12] POST .../reopen" $_.Exception.Response.StatusCode $_.ErrorDetails.Message
    }
}

# 13. Minimal API
try {
    $r = Invoke-RestMethod "$base/api/minimal/complaints?page=1&pageSize=5" -Headers $h
    Show-Result "[13] GET /api/minimal/complaints" "200 OK" "Count=$($r.Count)"
} catch {
    Show-Result "[13] GET /api/minimal/complaints" $_.Exception.Response.StatusCode $_.ErrorDetails.Message
}

# 14. GraphQL query
try {
    $gql = '{"query":"{ complaints { items { ticketNum description } } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Headers $h -Body $gql
    Show-Result "[14] GraphQL query (complaints)" "200 OK" ($r | ConvertTo-Json -Compress -Depth 4)
} catch {
    Show-Result "[14] GraphQL query" $_.Exception.Response.StatusCode $_.ErrorDetails.Message
}

# 15. GraphQL introspection
try {
    $gql = '{"query":"{ __schema { types { name kind } } }"}'
    $r = Invoke-RestMethod "$base/graphql" -Method POST -ContentType "application/json" -Body $gql
    $types = $r.data.__schema.types | Where-Object { $_.kind -eq "OBJECT" -and -not $_.name.StartsWith("__") }
    Show-Result "[15] GraphQL introspection" "200 OK" "Types: $($types.name -join ', ')"
} catch {
    Show-Result "[15] GraphQL introspection" $_.Exception.Response.StatusCode $_.ErrorDetails.Message
}

Write-Host "$sep"
Write-Host "  ALL TESTS COMPLETE"
Write-Host "$sep"
