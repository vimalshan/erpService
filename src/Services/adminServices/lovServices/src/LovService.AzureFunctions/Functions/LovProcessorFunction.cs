using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace LovService.AzureFunctions.Functions;

/// <summary>
/// HTTP-triggered Azure Function for processing LOV data exports.
/// Demonstrates Azure Functions + HTTP trigger pattern.
/// </summary>
public class LovProcessorFunction(ILogger<LovProcessorFunction> logger)
{
    [Function("ExportLovTypes")]
    public async Task<HttpResponseData> ExportLovTypesAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "lov/export/types")] HttpRequestData req,
        FunctionContext context)
    {
        logger.LogInformation("LOV Types export function triggered at {Time}", DateTime.UtcNow);

        // In production: query DB via Dapper, serialize to JSON/CSV, upload to Blob
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync("{\"status\": \"exported\", \"timestamp\": \"" + DateTime.UtcNow + "\"}");
        return response;
    }

    [Function("ProcessLovEvent")]
    public async Task ProcessLovEventAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "lov/events")] HttpRequestData req,
        FunctionContext context)
    {
        logger.LogInformation("LOV event processing function triggered");

        var body = await req.ReadAsStringAsync();
        logger.LogInformation("Processing LOV event payload: {Payload}", body);

        // In production: dispatch domain event, update read models, etc.
    }
}
