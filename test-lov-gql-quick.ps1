$body = '{"query":"{ lovTypes { lovTypeId lovTypeName } }"}'
try {
    $r = Invoke-WebRequest -Uri "http://localhost:5181/graphql" -Method POST -ContentType "application/json" -Body $body -UseBasicParsing
    $text = [System.Text.Encoding]::UTF8.GetString([byte[]]$r.Content)
    Write-Host "STATUS: $($r.StatusCode)"
    Write-Host $text
} catch {
    Write-Host "Error: $($_.Exception.Message)"
}
