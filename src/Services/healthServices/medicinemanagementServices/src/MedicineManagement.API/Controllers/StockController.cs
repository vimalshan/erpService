using MediatR;
using MedicineManagement.Application.DTOs;
using MedicineManagement.Application.Features.MedicineCredits.Commands;
using MedicineManagement.Application.Features.MedicineCredits.Queries;
using MedicineManagement.Application.Features.MedicineIssues.Commands;
using MedicineManagement.Application.Features.MedicineIssues.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StockController(IMediator mediator) : ControllerBase
{
    [HttpGet("balance/{medicineCode}")]
    public async Task<ActionResult<long>> GetBalance(string medicineCode, CancellationToken ct)
        => Ok(await mediator.Send(new GetStockByMedicineQuery(medicineCode), ct));

    [HttpGet("transactions/{medicineCode}")]
    public async Task<ActionResult<IReadOnlyList<MedicineCreditDto>>> GetTransactions(string medicineCode, CancellationToken ct)
        => Ok(await mediator.Send(new GetTransactionsByMedicineQuery(medicineCode), ct));

    [HttpGet("transactions/by-date")]
    public async Task<ActionResult<IReadOnlyList<MedicineCreditDto>>> GetByDateRange(
        [FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await mediator.Send(new GetTransactionsByDateRangeQuery(from, to), ct));

    [HttpPost("credit")]
    public async Task<ActionResult<MedicineCreditDto>> CreateCredit([FromBody] CreateMedicineCreditDto dto, CancellationToken ct)
    {
        var user = User.Identity?.Name ?? "API";
        return Ok(await mediator.Send(new CreateMedicineCreditCommand(
            dto.CompanyCode, dto.TransactionCode, dto.MedicineCode,
            dto.RecordType, dto.Quantity, dto.TransactionDate,
            user, 0, dto.LotNumber), ct));
    }

    [HttpPost("issue")]
    public async Task<ActionResult<MedicineIssueDto>> Issue([FromBody] CreateMedicineIssueDto dto, CancellationToken ct)
    {
        var user = User.Identity?.Name ?? "API";
        return Ok(await mediator.Send(new CreateMedicineIssueCommand(
            dto.CompanyCode, dto.TransactionNumber, dto.MedicineCode,
            dto.IssuedQuantity, dto.VisitNumber, user, "0"), ct));
    }

    [HttpGet("issues/by-visit/{visitNumber}")]
    public async Task<ActionResult<IReadOnlyList<MedicineIssueDto>>> GetIssuesByVisit(string visitNumber, CancellationToken ct)
        => Ok(await mediator.Send(new GetIssuesByVisitQuery(visitNumber), ct));

    [HttpGet("issues/by-medicine/{medicineCode}")]
    public async Task<ActionResult<IReadOnlyList<MedicineIssueDto>>> GetIssuesByMedicine(string medicineCode, CancellationToken ct)
        => Ok(await mediator.Send(new GetIssuesByMedicineQuery(medicineCode), ct));
}
