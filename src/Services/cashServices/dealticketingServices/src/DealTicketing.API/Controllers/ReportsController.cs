using DealTicketing.Application.Common.Interfaces;
using DealTicketing.Infrastructure.ReadRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealTicketing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ReportsController(DealDapperReadRepository dapperRepo) : ControllerBase
{
    /// <summary>Daily deal summary (Dapper-powered).</summary>
    [HttpGet("deal-summary")]
    public async Task<IActionResult> GetDealSummary(
        [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, CancellationToken ct)
    {
        var result = await dapperRepo.GetDealSummaryAsync(fromDate, toDate, ct);
        return Ok(result);
    }

    /// <summary>P&L report using settlement data.</summary>
    [HttpGet("pnl")]
    [Authorize(Roles = "Finance,Admin")]
    public async Task<IActionResult> GetPnl([FromQuery] DateTime fromDate, CancellationToken ct)
    {
        var result = await dapperRepo.GetPnlReportAsync(fromDate, ct);
        return Ok(result);
    }

    /// <summary>Pending approvals report (Dapper).</summary>
    [HttpGet("pending-approvals")]
    [Authorize(Roles = "DealApprover,Admin")]
    public async Task<IActionResult> GetPendingApprovals(CancellationToken ct)
    {
        var result = await dapperRepo.GetPendingApprovalsDapperAsync(ct);
        return Ok(result);
    }
}
