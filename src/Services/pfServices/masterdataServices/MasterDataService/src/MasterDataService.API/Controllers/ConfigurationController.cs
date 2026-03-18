using MasterDataService.Application.DTOs;
using MasterDataService.Application.Features.Configuration.Commands;
using MasterDataService.Application.Features.Configuration.Queries;
using MasterDataService.Infrastructure.Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ConfigurationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDapperQueryService _dapperQueryService;

    public ConfigurationController(IMediator mediator, IDapperQueryService dapperQueryService)
    {
        _mediator = mediator;
        _dapperQueryService = dapperQueryService;
    }

    /// <summary>Get all configurations</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConfigurationDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllConfigurationsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Get all configurations via Dapper</summary>
    [HttpGet("dapper")]
    public async Task<ActionResult<IEnumerable<ConfigurationDto>>> GetAllDapper(CancellationToken cancellationToken)
    {
        var result = await _dapperQueryService.GetAllConfigurationsDapperAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>Get configuration by ID</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ConfigurationDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetConfigurationByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Get configuration by key</summary>
    [HttpGet("key/{key}")]
    public async Task<ActionResult<ConfigurationDto>> GetByKey(string key, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetConfigurationByKeyQuery(key), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Create new configuration</summary>
    [HttpPost]
    public async Task<ActionResult<ConfigurationDto>> Create([FromBody] CreateConfigurationDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateConfigurationCommand(dto.ConfigKey, dto.ConfigValue, dto.ConfigType, dto.ConfigDescription, dto.CreatedBy);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.ConfigId }, result);
    }

    /// <summary>Update configuration</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateConfigurationDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateConfigurationCommand(id, dto.ConfigValue, dto.ConfigType);
        var result = await _mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }
}
