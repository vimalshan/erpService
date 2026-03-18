using ReimbursementService.Application.DTOs;
using ReimbursementService.Domain.Entities;
using ReimbursementService.Domain.Enums;
using ReimbursementService.Domain.Interfaces;

namespace ReimbursementService.API.GraphQL;

// ─── GraphQL Types ─────────────────────────────────────────────────────────────

[GraphQLName("ReimbursementTransaction")]
public sealed class ReimbursementType
{
    public long ReimId { get; set; }
    public string ReimRefNo { get; set; } = default!;
    public long EmpSysId { get; set; }
    public string ReimType { get; set; } = default!;
    public decimal ReimAmount { get; set; }
    public string ReimCurrency { get; set; } = default!;
    public DateOnly ReimDate { get; set; }
    public DateOnly ExpenseDate { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string Status { get; set; } = default!;
    public int? ApprovalLevel { get; set; }
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public string? RejectionReason { get; set; }
    public DateOnly? PaymentDate { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
}

// ─── Query ─────────────────────────────────────────────────────────────────────

public sealed class ReimbursementQuery
{
    [GraphQLDescription("Get a reimbursement by its ID.")]
    public async Task<ReimbursementType?> GetReimbursementById(
        long id,
        [Service] IReimbursementRepository repository)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : MapToGql(entity);
    }

    [GraphQLDescription("Get all reimbursements for an employee.")]
    public async Task<IEnumerable<ReimbursementType>> GetReimbursementsByEmployee(
        long empSysId,
        [Service] IReimbursementRepository repository)
    {
        var entities = await repository.GetByEmployeeAsync(empSysId);
        return entities.Select(MapToGql);
    }

    [GraphQLDescription("List reimbursements by status.")]
    public async Task<IEnumerable<ReimbursementType>> GetReimbursementsByStatus(
        string status,
        [Service] IReimbursementRepository repository)
    {
        if (!Enum.TryParse<ReimbursementStatus>(status, true, out var statusEnum))
            throw new ArgumentException($"Invalid status: {status}");
        var entities = await repository.GetByStatusAsync(statusEnum);
        return entities.Select(MapToGql);
    }

    private static ReimbursementType MapToGql(ReimbursementTransaction e) => new()
    {
        ReimId = e.ReimId,
        ReimRefNo = e.ReimRefNo,
        EmpSysId = e.EmpSysId,
        ReimType = e.ReimType.ToString().ToUpperInvariant(),
        ReimAmount = e.Amount.Amount,
        ReimCurrency = e.Amount.Currency,
        ReimDate = e.ReimDate,
        ExpenseDate = e.ExpenseDate,
        Description = e.Description,
        Location = e.Location,
        Status = e.Status.ToString().ToUpperInvariant(),
        ApprovalLevel = e.ApprovalLevel,
        ApprovedBy = e.ApprovedBy,
        ApprovedOn = e.ApprovedOn,
        RejectionReason = e.RejectionReason,
        PaymentDate = e.PaymentDate,
        CreatedBy = e.CreatedBy,
        CreatedOn = e.CreatedOn
    };
}

// ─── Mutations ─────────────────────────────────────────────────────────────────

public sealed class ReimbursementMutation
{
    [GraphQLDescription("Submit a reimbursement for approval.")]
    public async Task<bool> SubmitReimbursement(
        long reimId,
        [Service] IReimbursementRepository repository)
    {
        var entity = await repository.GetByIdAsync(reimId)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Reimbursement {reimId} not found.");
        entity.Submit();
        await repository.UpdateAsync(entity);
        return true;
    }
}
