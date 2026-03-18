using InvestmentService.Application.Commands;
using InvestmentService.Application.DTOs;
using InvestmentService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvestmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvestmentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<InvestmentDto>>> GetAll([FromQuery] string? status, [FromQuery] int? categoryId)
    {
        var result = await _mediator.Send(new GetAllInvestmentsQuery(status, categoryId));
        return Ok(result);
    }

    [HttpGet("{invNo:long}")]
    public async Task<ActionResult<InvestmentDto>> GetById(long invNo)
    {
        var result = await _mediator.Send(new GetInvestmentByIdQuery(invNo));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("active")]
    public async Task<ActionResult<List<InvestmentDto>>> GetActive()
    {
        var result = await _mediator.Send(new GetActiveInvestmentsQuery());
        return Ok(result);
    }

    [HttpGet("matured")]
    public async Task<ActionResult<List<InvestmentDto>>> GetMatured([FromQuery] DateTime? asOfDate)
    {
        var result = await _mediator.Send(new GetMaturedInvestmentsQuery(asOfDate ?? DateTime.UtcNow));
        return Ok(result);
    }

    [HttpGet("portfolio-summary")]
    public async Task<ActionResult<PortfolioSummaryDto>> GetPortfolioSummary()
    {
        var result = await _mediator.Send(new GetPortfolioSummaryQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<InvestmentDto>> Create([FromBody] CreateInvestmentCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { invNo = result.InvNo }, result);
    }

    [HttpPut("{invNo:long}")]
    public async Task<ActionResult<InvestmentDto>> Update(long invNo, [FromBody] UpdateInvestmentCommand command)
    {
        if (invNo != command.InvNo) return BadRequest("Investment number mismatch");
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{invNo:long}/redeem")]
    public async Task<ActionResult<SaleDetailDto>> Redeem(long invNo, [FromBody] RedeemInvestmentCommand command)
    {
        if (invNo != command.InvNo) return BadRequest("Investment number mismatch");
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{invNo:long}/sales")]
    public async Task<ActionResult<List<SaleDetailDto>>> GetSales(long invNo)
    {
        var result = await _mediator.Send(new GetSalesByInvestmentQuery(invNo));
        return Ok(result);
    }

    [HttpGet("{invNo:long}/schedules")]
    public async Task<ActionResult<List<ScheduleDetailDto>>> GetSchedules(long invNo)
    {
        var result = await _mediator.Send(new GetSchedulesByInvestmentQuery(invNo));
        return Ok(result);
    }

    [HttpPost("{invNo:long}/generate-schedule")]
    public async Task<ActionResult<List<ScheduleDetailDto>>> GenerateSchedule(long invNo, [FromQuery] long year)
    {
        var result = await _mediator.Send(new GenerateInterestScheduleCommand(invNo, year));
        return Ok(result);
    }

    [HttpPost("schedules/{schId:long}/record-receipt")]
    public async Task<ActionResult<ScheduleDetailDto>> RecordReceipt(long schId, [FromBody] RecordInterestReceiptCommand command)
    {
        if (schId != command.SchId) return BadRequest("Schedule ID mismatch");
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("schedules/pending")]
    public async Task<ActionResult<List<ScheduleDetailDto>>> GetPendingSchedules([FromQuery] DateTime? asOfDate)
    {
        var result = await _mediator.Send(new GetPendingSchedulesQuery(asOfDate ?? DateTime.UtcNow));
        return Ok(result);
    }

    [HttpPost("{invNo:long}/approve")]
    public async Task<ActionResult<bool>> Approve(long invNo, [FromBody] ApproveInvestmentCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
