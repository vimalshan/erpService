using MasterDataService.Application.DTOs;
using MasterDataService.Application.Queries.Coupon;
using MasterDataService.Application.Queries.TaxSlab;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CouponsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CouponsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<CouponDto>>> GetAll()
        => Ok(await _mediator.Send(new GetAllCouponsQuery()));

    [HttpGet("airline/{airline}")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<CouponDto>>> GetByAirline(string airline)
        => Ok(await _mediator.Send(new GetCouponsByAirlineQuery(airline)));

    [HttpGet("expired")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<CouponDto>>> GetExpired()
        => Ok(await _mediator.Send(new GetExpiredCouponsQuery()));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaxSlabsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaxSlabsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<TaxSlabDto>>> GetAll()
        => Ok(await _mediator.Send(new GetAllTaxSlabsQuery()));

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<TaxSlabDto>>> GetActive()
        => Ok(await _mediator.Send(new GetActiveTaxSlabsQuery()));
}
