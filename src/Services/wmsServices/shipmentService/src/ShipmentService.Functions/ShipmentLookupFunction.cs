using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MediatR;
using System.Net;
using ShipmentService.Application.Features.Shipments.Queries.GetShipmentById;

namespace ShipmentService.Functions;

/// <summary>HTTP-triggered function for lightweight shipment lookup (e.g., from partner integrations).</summary>
public sealed class ShipmentLookupFunction
{
    private readonly ILogger<ShipmentLookupFunction> _logger;
    private readonly IMediator _mediator;

    public ShipmentLookupFunction(ILogger<ShipmentLookupFunction> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [Function(nameof(ShipmentLookupFunction))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "shipments/{id:int}")] HttpRequestData req,
        int id,
        CancellationToken ct)
    {
        _logger.LogInformation("Shipment lookup for ID {ShipmentId}", id);

        try
        {
            var shipment = await _mediator.Send(new GetShipmentByIdQuery(id), ct);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(shipment, ct);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error looking up shipment {Id}", id);
            var err = req.CreateResponse(HttpStatusCode.NotFound);
            await err.WriteStringAsync($"Shipment {id} not found.", ct);
            return err;
        }
    }
}
