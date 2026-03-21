using FleetManagement.Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FleetManagement.Functions;

public class FleetReportFunction(ILoggerFactory loggerFactory, IDapperQueryService dapperService, IBlobStorageService blobService)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<FleetReportFunction>();

    [Function("DailyFleetReport")]
    public async Task Run([TimerTrigger("0 0 6 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("DailyFleetReport function triggered at {Time}", DateTime.UtcNow);

        const string sql = """
            SELECT v.code, v.license_plate, v.status,
                   (SELECT COUNT(*) FROM Trip WHERE vehicle_id = v.vehicle_id AND status = 'COMPLETED' AND trip_date = CAST(GETDATE()-1 AS DATE)) AS TripsYesterday
            FROM Vehicle v WHERE v.is_active = 1
            ORDER BY v.code
            """;

        var data = await dapperService.QueryAsync<dynamic>(sql, ct: ct);
        var reportContent = System.Text.Json.JsonSerializer.Serialize(data);
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(reportContent));
        var fileName = $"reports/fleet-report-{DateTime.UtcNow:yyyy-MM-dd}.json";
        await blobService.UploadFileAsync("fleet-reports", fileName, stream, "application/json", ct);

        _logger.LogInformation("Fleet report uploaded to blob storage: {FileName}", fileName);
    }
}
