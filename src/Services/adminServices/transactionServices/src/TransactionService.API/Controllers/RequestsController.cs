namespace TransactionService.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Commands.ApproveRequest;
using TransactionService.Application.Commands.SubmitRequest;
using TransactionService.Application.DTOs;
using TransactionService.Application.ExternalServices;
using TransactionService.Application.Queries.GetRequests;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class RequestsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStationeryServiceClient _stationeryClient;
    private readonly ILocationServiceClient _locationClient;
    private readonly IFinyearServiceClient _finyearClient;

    public RequestsController(
        IMediator mediator,
        IStationeryServiceClient stationeryClient,
        ILocationServiceClient locationClient,
        IFinyearServiceClient finyearClient)
    {
        _mediator = mediator;
        _stationeryClient = stationeryClient;
        _locationClient = locationClient;
        _finyearClient = finyearClient;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RequestSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] long? locationId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllRequestsQuery(locationId), ct);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(RequestMainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRequestByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("employee/{empSysId:long}")]
    [ProducesResponseType(typeof(IEnumerable<RequestSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(long empSysId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRequestsByEmployeeQuery(empSysId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    public async Task<IActionResult> Submit(
        [FromBody] SubmitRequestCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{requestSubId:long}/approve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(
        long requestSubId, [FromBody] ApproveRequestCommand command, CancellationToken ct)
    {
        if (requestSubId != command.RequestSubId)
            return BadRequest("ID mismatch.");

        var result = await _mediator.Send(command, ct);
        return result ? NoContent() : NotFound();
    }

    // ── External Service Lookups ──

    /// <summary>Get stationery items from StationeryService for request line selection.</summary>
    [HttpGet("lookup/stationery-items")]
    [ProducesResponseType(typeof(IReadOnlyList<StationeryItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStationeryItems(
        [FromQuery] long? locationId, CancellationToken ct)
    {
        var items = locationId.HasValue
            ? await _stationeryClient.GetItemsByLocationAsync(locationId.Value, ct)
            : await _stationeryClient.GetAllItemsAsync(ct);
        return Ok(items);
    }

    /// <summary>Get a specific stationery item by ID from StationeryService.</summary>
    [HttpGet("lookup/stationery-items/{itemId:long}")]
    [ProducesResponseType(typeof(StationeryItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStationeryItemById(long itemId, CancellationToken ct)
    {
        var item = await _stationeryClient.GetItemByIdAsync(itemId, ct);
        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>Get active locations from LocationService.</summary>
    [HttpGet("lookup/locations")]
    [ProducesResponseType(typeof(IReadOnlyList<LocationAppMapDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocations(CancellationToken ct)
    {
        var locations = await _locationClient.GetActiveLocationsAsync(ct);
        return Ok(locations);
    }

    /// <summary>Get current financial year from FinyearService.</summary>
    [HttpGet("lookup/current-finyear")]
    [ProducesResponseType(typeof(FinancialYearDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentFinancialYear(CancellationToken ct)
    {
        var fy = await _finyearClient.GetCurrentFinancialYearAsync(ct);
        return fy is null ? NotFound() : Ok(fy);
    }
}
