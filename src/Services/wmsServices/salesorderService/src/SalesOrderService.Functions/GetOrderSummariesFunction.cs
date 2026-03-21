using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using SalesOrderService.Application.SalesOrders.Queries.GetAllSalesOrders;
using System.Net;
using System.Text.Json;

namespace SalesOrderService.Functions;

/// <summary>
/// HTTP-triggered function providing a lightweight read endpoint for order summaries.
/// Useful for internal tooling or dashboard queries without going through the main API.
/// </summary>
public sealed class GetOrderSummariesFunction(
    ILogger<GetOrderSummariesFunction> logger,
    ISender mediator)
{
    [Function(nameof(GetOrderSummariesFunction))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "functions/orders")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetOrderSummaries function invoked.");

        var orders = await mediator.Send(new GetAllSalesOrdersQuery(), cancellationToken);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");
        await response.WriteStringAsync(
            JsonSerializer.Serialize(orders, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }),
            cancellationToken);

        return response;
    }
}
