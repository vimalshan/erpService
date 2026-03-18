using MasterDataService.Application.DTOs;
using MasterDataService.Application.Features.RateMaster.Commands;
using MasterDataService.Application.Features.RateMaster.Queries;
using MasterDataService.Infrastructure.Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class RateMasterController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IDapperQueryService _dapperQueryService;

    public RateMasterController(IMediator mediator, IDapperQueryService dapperQueryService)
    {
        _mediator = mediator;
        _dapperQueryService = dapperQueryService;
    }

    /// <summary>Get all rates optionally filtered by trust code</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RateMasterDto>>> GetAll([FromQuery] string? trustCode, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllRatesQuery(trustCode), cancellationToken);
        return Ok(result);
    }

    /// <summary>Get rates by trust code via Dapper</summary>
    [HttpGet("trust/{trustCode}/dapper")]
    public async Task<ActionResult<IEnumerable<RateMasterDto>>> GetByTrustDapper(string trustCode, CancellationToken cancellationToken)
    {
        var result = await _dapperQueryService.GetRatesByTrustCodeDapperAsync(trustCode, cancellationToken);
        return Ok(result);
    }

    /// <summary>Create new rate</summary>
    [HttpPost]
    public async Task<ActionResult<RateMasterDto>> Create([FromBody] CreateRateMasterDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateRateCommand(dto.TrustCode, dto.RateTypeCode, dto.RateEffectiveDate, dto.RateValue);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { trustCode = result.TrustCode }, result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class FundTypeController : ControllerBase
{
    private readonly IDapperQueryService _dapperQueryService;

    public FundTypeController(IDapperQueryService dapperQueryService) => _dapperQueryService = dapperQueryService;

    /// <summary>Get all fund types</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<FundTypeDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _dapperQueryService.GetAllFundTypesDapperAsync(cancellationToken);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class StatusMasterController : ControllerBase
{
    private readonly IDapperQueryService _dapperQueryService;

    public StatusMasterController(IDapperQueryService dapperQueryService) => _dapperQueryService = dapperQueryService;

    /// <summary>Get statuses by type</summary>
    [HttpGet("{statusType}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<StatusMasterDto>>> GetByType(string statusType, CancellationToken cancellationToken)
    {
        var result = await _dapperQueryService.GetStatusByTypeDapperAsync(statusType, cancellationToken);
        return Ok(result);
    }
}
