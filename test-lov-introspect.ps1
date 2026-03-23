$body = '{"query":"{ __schema { queryType { fields { name } } } }"}'
try {
    $r = Invoke-WebRequest -Uri "http://localhost:5181/graphql" -Method POST -ContentType "application/json" -Body $body -UseBasicParsing
    $text = [System.Text.Encoding]::UTF8.GetString([byte[]]$r.Content)
    Write-Host $text
} catch {
    Write-Host "Error: $($_.Exception.Message)"
    if ($_.Exception.Response) {
        $reader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
        Write-Host $reader.ReadToEnd()
    }
}
