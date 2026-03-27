using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionProcessing.Application.Commands;
using TransactionProcessing.Application.DTOs;
using TransactionProcessing.Application.Queries;

namespace TransactionProcessing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class TransactionsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:long}")]
    public async Task<ActionResult<FinancialTransactionDto>> GetById(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTransactionByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("batch/{batchId:long}")]
    public async Task<ActionResult<IReadOnlyList<FinancialTransactionDto>>> GetByBatch(long batchId, CancellationToken ct) =>
        Ok(await mediator.Send(new GetTransactionsByBatchQuery(batchId), ct));

    [HttpGet("ledger")]
    public async Task<ActionResult<IReadOnlyList<TransactionLedgerDto>>> GetLedger(
        [FromQuery] DateTime? from, [FromQuery] DateTime? to,
        [FromQuery] string? status, [FromQuery] string? txnType,
        [FromQuery] int pageSize = 50, [FromQuery] int page = 1, CancellationToken ct = default) =>
        Ok(await mediator.Send(new GetTransactionLedgerQuery(from, to, status, txnType, pageSize, page), ct));

    [HttpPost("cash-transfer")]
    public async Task<ActionResult<FinancialTransactionDto>> ProcessCashTransfer(
        ProcessCashTransferCommand command, CancellationToken ct) =>
        CreatedAtAction(nameof(GetById), new { id = 0 }, await mediator.Send(command, ct));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SettlementsController(IMediator mediator) : ControllerBase
{
    [HttpGet("deal/{dealId:long}")]
    public async Task<ActionResult<IReadOnlyList<DealSettlementDto>>> GetByDeal(long dealId, CancellationToken ct) =>
        Ok(await mediator.Send(new GetSettlementsByDealQuery(dealId), ct));

    [HttpPost]
    public async Task<ActionResult<DealSettlementDto>> ProcessSettlement(
        ProcessDealSettlementCommand command, CancellationToken ct) =>
        Created("", await mediator.Send(command, ct));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class DisbursementsController(IMediator mediator) : ControllerBase
{
    [HttpGet("loan/{loanId:long}")]
    public async Task<ActionResult<IReadOnlyList<LoanDisbursementDto>>> GetByLoan(long loanId, CancellationToken ct) =>
        Ok(await mediator.Send(new GetDisbursementsByLoanQuery(loanId), ct));

    [HttpPost]
    public async Task<ActionResult<LoanDisbursementDto>> ProcessDisbursement(
        ProcessLoanDisbursementCommand command, CancellationToken ct) =>
        Created("", await mediator.Send(command, ct));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class RepaymentsController(IMediator mediator) : ControllerBase
{
    [HttpGet("loan/{loanId:long}")]
    public async Task<ActionResult<IReadOnlyList<LoanRepaymentDto>>> GetByLoan(long loanId, CancellationToken ct) =>
        Ok(await mediator.Send(new GetRepaymentsByLoanQuery(loanId), ct));

    [HttpPost]
    public async Task<ActionResult<LoanRepaymentDto>> ProcessRepayment(
        ProcessLoanRepaymentCommand command, CancellationToken ct) =>
        Created("", await mediator.Send(command, ct));
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class BatchesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TransactionBatchDto>> Create(
        CreateTransactionBatchCommand command, CancellationToken ct) =>
        Created("", await mediator.Send(command, ct));

    [HttpPost("{batchId:long}/complete")]
    public async Task<ActionResult<TransactionBatchDto>> Complete(long batchId, [FromBody] long completedBy, CancellationToken ct) =>
        Ok(await mediator.Send(new CompleteTransactionBatchCommand(batchId, completedBy), ct));
}
