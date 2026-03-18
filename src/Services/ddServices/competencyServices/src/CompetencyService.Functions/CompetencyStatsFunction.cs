using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CompetencyService.Functions;

/// <summary>HTTP-triggered function for on-demand competency statistics report generation.</summary>
public class CompetencyStatsFunction(ILogger<CompetencyStatsFunction> logger)
{
    [Function(nameof(CompetencyStatsFunction))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "competency-stats")] HttpRequestData req)
    {
        logger.LogInformation("CompetencyStatsFunction triggered.");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync("{\"status\": \"stats generation initiated\", \"timestamp\": \"" + DateTime.UtcNow + "\"}");
        return response;
    }
}
