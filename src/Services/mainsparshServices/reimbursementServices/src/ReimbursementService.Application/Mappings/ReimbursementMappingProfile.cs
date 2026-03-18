using AutoMapper;
using ReimbursementService.Application.DTOs;
using ReimbursementService.Domain.Entities;

namespace ReimbursementService.Application.Mappings;

public sealed class ReimbursementMappingProfile : Profile
{
    public ReimbursementMappingProfile()
    {
        CreateMap<ReimbursementTransaction, ReimbursementDto>()
            .ConstructUsing(src => new ReimbursementDto(
                src.ReimId,
                src.ReimRefNo,
                src.EmpSysId,
                src.ReimType.ToString().ToUpperInvariant(),
                src.Amount.Amount,
                src.Amount.Currency,
                src.ReimDate,
                src.ExpenseDate,
                src.Description,
                src.Location,
                src.Status.ToString().ToUpperInvariant(),
                src.ApprovalLevel,
                src.ApprovedBy,
                src.ApprovedOn,
                src.RejectionReason,
                src.PaymentDate,
                src.CreatedBy,
                src.CreatedOn,
                src.UpdatedBy,
                src.UpdatedOn));
    }
}
