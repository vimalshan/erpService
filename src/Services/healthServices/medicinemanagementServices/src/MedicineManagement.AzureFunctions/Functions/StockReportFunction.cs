using MedicineManagement.Infrastructure.Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace MedicineManagement.AzureFunctions.Functions;

public class StockReportFunction(DapperQueryService dapperQuery, ILogger<StockReportFunction> logger)
{
    /// <summary>
    /// HTTP-triggered function that returns a full stock summary report.
    /// </summary>
    [Function("StockReport")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "reports/stock-summary")] HttpRequestData req,
        CancellationToken ct)
    {
        logger.LogInformation("StockReport function triggered");

        var stockSummary = await dapperQuery.GetStockSummaryAsync(ct);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(JsonSerializer.Serialize(stockSummary), ct);
        return response;
    }
}
