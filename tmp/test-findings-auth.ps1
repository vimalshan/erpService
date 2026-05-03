param(
    [Parameter(Mandatory = $true)]
    [string]$Token
)

$headers = @{ Authorization = "Bearer $Token" }

$tests = @(
    @{
        Name = 'findings query'
        Query = 'query { findingsStatistics { totalCount openCount acceptedCount closedCount overdueCount } }'
    },
    @{
        Name = 'findings mutation'
        Query = 'mutation { closeFinding(findingId: -1, closureNotes: "curl test") { message timestamp } }'
    }
)

foreach ($test in $tests) {
    Write-Output "=== $($test.Name) ==="
    try {
        $body = @{ query = $test.Query } | ConvertTo-Json -Compress
        $response = Invoke-RestMethod -Uri 'http://localhost:5006/graphql' -Method Post -Headers $headers -ContentType 'application/json' -Body $body -TimeoutSec 40
        $response | ConvertTo-Json -Depth 12
    }
    catch {
        if ($_.Exception.Response) {
            Write-Output $_.Exception.Response.StatusCode.value__
            Write-Output $_.Exception.Response.StatusDescription
        }

        if ($_.ErrorDetails.Message) {
            Write-Output $_.ErrorDetails.Message
        }
        else {
            Write-Output $_.Exception.Message
        }
    }

    Write-Output ''
}