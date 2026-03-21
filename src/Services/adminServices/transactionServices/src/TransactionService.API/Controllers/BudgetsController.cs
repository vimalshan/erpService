namespace TransactionService.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Application.Commands.AllocateBudget;
using TransactionService.Application.DTOs;
using TransactionService.Application.ExternalServices;
using TransactionService.Application.Queries.GetBudget;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFinyearServiceClient _finyearClient;
    private readonly ILocationServiceClient _locationClient;
    private readonly ILovServiceClient _lovClient;

    public BudgetsController(
        IMediator mediator,
        IFinyearServiceClient finyearClient,
        ILocationServiceClient locationClient,
        ILovServiceClient lovClient)
    {
        _mediator = mediator;
        _finyearClient = finyearClient;
        _locationClient = locationClient;
        _lovClient = lovClient;
    }

    [HttpGet("department")]
    [ProducesResponseType(typeof(BudgetSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeptBudget(
        [FromQuery] long locationId, [FromQuery] long deptId,
        [FromQuery] long finYearId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDeptBudgetQuery(locationId, deptId, finYearId), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("department/location/{locationId:long}")]
    [ProducesResponseType(typeof(IEnumerable<DeptBudgetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDeptBudgetsByLocation(
        long locationId, [FromQuery] long finYearId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetDeptBudgetsByLocationQuery(locationId, finYearId), ct);
        return Ok(result);
    }

    [HttpGet("unit/location/{locationId:long}")]
    [ProducesResponseType(typeof(IEnumerable<UnitBudgetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnitBudgetsByLocation(
        long locationId, [FromQuery] long finYearId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetUnitBudgetsByLocationQuery(locationId, finYearId), ct);
        return Ok(result);
    }

    [HttpPost("department")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AllocateDeptBudget(
        [FromBody] AllocateDeptBudgetCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPost("unit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AllocateUnitBudget(
        [FromBody] AllocateUnitBudgetCommand command, CancellationToken ct)
    {
        await _mediator.Send(command, ct);
        return NoContent();
    }

    // ── External Service Lookups ──

    /// <summary>Get all financial years from FinyearService for budget allocation.</summary>
    [HttpGet("lookup/financial-years")]
    [ProducesResponseType(typeof(IReadOnlyList<FinancialYearDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFinancialYears(CancellationToken ct)
    {
        var years = await _finyearClient.GetAllFinancialYearsAsync(ct);
        return Ok(years);
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

    /// <summary>Get active locations from LocationService.</summary>
    [HttpGet("lookup/locations")]
    [ProducesResponseType(typeof(IReadOnlyList<LocationAppMapDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocations(CancellationToken ct)
    {
        var locations = await _locationClient.GetActiveLocationsAsync(ct);
        return Ok(locations);
    }

    /// <summary>Get LOV types from LovService for category/unit lookups.</summary>
    [HttpGet("lookup/lov-types")]
    [ProducesResponseType(typeof(IReadOnlyList<LovTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLovTypes(CancellationToken ct)
    {
        var types = await _lovClient.GetAllLovTypesAsync(ct);
        return Ok(types);
    }

    /// <summary>Get LOV masters by type from LovService.</summary>
    [HttpGet("lookup/lov-masters/{lovTypeId:long}")]
    [ProducesResponseType(typeof(IReadOnlyList<LovMasterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLovMastersByType(long lovTypeId, CancellationToken ct)
    {
        var masters = await _lovClient.GetLovMastersByTypeAsync(lovTypeId, ct);
        return Ok(masters);
    }

    /// <summary>Search item data from LovService.</summary>
    [HttpGet("lookup/item-data")]
    [ProducesResponseType(typeof(IReadOnlyList<ItemDataDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchItemData(
        [FromQuery] string? catName, [FromQuery] string? itemName, CancellationToken ct)
    {
        var items = (catName is not null || itemName is not null)
            ? await _lovClient.SearchItemDataAsync(catName, itemName, ct)
            : await _lovClient.GetAllItemDataAsync(ct);
        return Ok(items);
    }
}
