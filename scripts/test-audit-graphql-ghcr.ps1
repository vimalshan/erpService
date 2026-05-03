param(
    [string]$Token = $env:AUDIT_ACCESS_TOKEN
)

$ErrorActionPreference = 'Continue'

if ([string]::IsNullOrWhiteSpace($Token)) {
    throw 'Provide a bearer token via -Token or AUDIT_ACCESS_TOKEN.'
}

function Invoke-GraphQLTest {
    param(
        [string]$Name,
        [string]$Url,
        [string]$Query
    )

    try {
        $body = @{ query = $Query } | ConvertTo-Json -Compress
        $response = Invoke-RestMethod -Uri $Url -Method Post -Headers @{ Authorization = "Bearer $Token" } -ContentType 'application/json' -Body $body -TimeoutSec 40
        Write-Output "=== $Name ==="
        Write-Output ($response | ConvertTo-Json -Depth 12)
    }
    catch {
        $message = if ($_.Exception.Response) {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $reader.ReadToEnd()
        }
        elseif ($_.ErrorDetails.Message) {
            $_.ErrorDetails.Message
        }
        else {
            $_.Exception.Message
        }
        Write-Output "=== $Name ==="
        Write-Output $message
    }
    finally {
        Write-Output ''
    }
}

$tests = @(
    @{ Name = '5001 action query'; Url = 'http://localhost:5001/graphql'; Query = 'query { allActions { id action dueDate } }' },
    @{ Name = '5001 action mutation'; Url = 'http://localhost:5001/graphql'; Query = 'mutation { completeAction(id: -1) }' },
    @{ Name = '5002 audit query'; Url = 'http://localhost:5002/graphql'; Query = 'query { viewAudits { isSuccess message errorCode data { auditId companyId status } } }' },
    @{ Name = '5002 audit mutation'; Url = 'http://localhost:5002/graphql'; Query = 'mutation { changeAuditStatus(auditId: -1, newStatus: "Closed") }' },
    @{ Name = '5003 certificate query'; Url = 'http://localhost:5003/graphql'; Query = 'query { certificates { isSuccess message errorCode data { certificateId certificateNumber status } } }' },
    @{ Name = '5003 certificate mutation'; Url = 'http://localhost:5003/graphql'; Query = 'mutation { deleteCertificate(id: -1) }' },
    @{ Name = '5004 contract query'; Url = 'http://localhost:5004/graphql'; Query = 'query { masterSiteList { isSuccess message errorCode data { id siteName companyId } } }' },
    @{ Name = '5004 contract mutation'; Url = 'http://localhost:5004/graphql'; Query = 'mutation { deleteContract(contractId: -1) }' },
    @{ Name = '5005 finance query'; Url = 'http://localhost:5005/graphql'; Query = 'query { InvoiceListPage(pageNumber: 1, pageSize: 5) { isSuccess message errorCode data { items { invoice status } } } }' },
    @{ Name = '5005 finance mutation'; Url = 'http://localhost:5005/graphql'; Query = 'mutation { UpdatePlannedPaymentDate(invoiceNumber: ["TEST-NO-INVOICE"], plannedDates: "2026-04-21T00:00:00Z") { isSuccess message errorCode data } }' },
    @{ Name = '5006 findings query'; Url = 'http://localhost:5006/graphql'; Query = 'query { findingsStatistics { totalCount openCount acceptedCount closedCount overdueCount } }' },
    @{ Name = '5006 findings mutation'; Url = 'http://localhost:5006/graphql'; Query = 'mutation { closeFinding(input: { findingId: -1, closureNotes: "curl test" }) { message timestamp } }' },
    @{ Name = '5007 notification query'; Url = 'http://localhost:5007/graphql'; Query = 'query { notifications(pageNumber: 1, pageSize: 5) { isSuccess message errorCode data { currentPage totalItems items { infoId subject readStatus } } } }' },
    @{ Name = '5007 notification mutation'; Url = 'http://localhost:5007/graphql'; Query = 'mutation { deleteNotification(notificationId: -1) }' },
    @{ Name = '5008 schedule query'; Url = 'http://localhost:5008/graphql'; Query = 'query { viewAuditSchedules(calendarScheduleFilter: { companyIds: [1], serviceIds: [1], siteIds: [1], statuses: [], fromDate: "2024-01-01", toDate: "2024-12-31" }) { isSuccess message errorCode data { siteAuditId auditId status } } }' },
    @{ Name = '5008 schedule mutation'; Url = 'http://localhost:5008/graphql'; Query = 'mutation { deleteSchedule(auditSiteAuditId: -1) }' },
    @{ Name = '5009 settings query'; Url = 'http://localhost:5009/graphql'; Query = 'query { userCompanyDetails(userId: 1) { isSuccess message errorCode data { userStatus isAdmin } } }' },
    @{ Name = '5009 settings mutation'; Url = 'http://localhost:5009/graphql'; Query = 'mutation { deactivateUser(userId: -1, modifiedBy: null) }' },
    @{ Name = '5010 documents graphql probe'; Url = 'http://localhost:5010/graphql'; Query = 'query { __typename }' },
    @{ Name = '5011 overview query'; Url = 'http://localhost:5011/graphql'; Query = 'query { widgetForFinancials { financialStatus financialCount financialPercentage } }' },
    @{ Name = '5011 overview mutation probe'; Url = 'http://localhost:5011/graphql'; Query = 'mutation { __typename }' }
)

foreach ($test in $tests) {
    Invoke-GraphQLTest -Name $test.Name -Url $test.Url -Query $test.Query
}
