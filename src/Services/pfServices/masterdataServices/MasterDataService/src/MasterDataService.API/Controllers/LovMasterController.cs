using MasterDataService.Application.DTOs;
using MasterDataService.Application.Features.LovMaster.Commands;
using MasterDataService.Application.Features.LovMaster.Queries;
using MasterDataService.Infrastructure.Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class LovMasterController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDapperQueryService _dapperQueryService;

    public LovMasterController(IMediator mediator, IDapperQueryService dapperQueryService)
    {
        _mediator = mediator;
        _dapperQueryService = dapperQueryService;
    }

    /// <summary>Get all LOV values, optionally filtered by category</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<LovMasterDto>>> GetAll([FromQuery] string? category, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllLovQuery(category), cancellationToken);
        return Ok(result);
    }

    /// <summary>Get LOV by ID</summary>
    [HttpGet("{id:decimal}")]
    public async Task<ActionResult<LovMasterDto>> GetById(decimal id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLovByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get LOV by category using Dapper</summary>
    [HttpGet("category/{category}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<LovMasterDto>>> GetByCategory(string category, CancellationToken cancellationToken)
    {
        var result = await _dapperQueryService.GetLovByCategoryDapperAsync(category, cancellationToken);
        return Ok(result);
    }

    /// <summary>Create new LOV value</summary>
    [HttpPost]
    public async Task<ActionResult<LovMasterDto>> Create([FromBody] CreateLovMasterDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateLovCommand(dto.LovCode, dto.LovDescription, dto.LovValue, dto.LovCategory);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.LovId }, result);
    }

    /// <summary>Activate a LOV value</summary>
    [HttpPut("{id:decimal}/activate")]
    public async Task<IActionResult> Activate(decimal id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ActivateLovCommand(id), cancellationToken);
        return result ? NoContent() : NotFound();
    }

    /// <summary>Deactivate a LOV value</summary>
    [HttpPut("{id:decimal}/deactivate")]
    public async Task<IActionResult> Deactivate(decimal id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeactivateLovCommand(id), cancellationToken);
        return result ? NoContent() : NotFound();
    }
}
