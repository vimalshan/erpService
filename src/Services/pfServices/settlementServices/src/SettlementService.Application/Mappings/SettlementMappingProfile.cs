using AutoMapper;
using SettlementService.Application.DTOs;
using SettlementService.Domain.Aggregates;
using SettlementService.Domain.Entities;

namespace SettlementService.Application.Mappings;

public class SettlementMappingProfile : Profile
{
    public SettlementMappingProfile()
    {
        CreateMap<Settlement, SettlementDto>()
            .ForMember(d => d.SettlementNumber, o => o.MapFrom(s => s.StSetNum))
            .ForMember(d => d.TrustCode, o => o.MapFrom(s => s.StTrustCode))
            .ForMember(d => d.MemberNo, o => o.MapFrom(s => s.StMemberNo))
            .ForMember(d => d.SettlementType, o => o.MapFrom(s => s.StSetType))
            .ForMember(d => d.SettlementDate, o => o.MapFrom(s => s.StSetDate))
            .ForMember(d => d.DolDate, o => o.MapFrom(s => s.StDolDat))
            .ForMember(d => d.Reason, o => o.MapFrom(s => s.StReason))
            .ForMember(d => d.UpdatedOn, o => o.MapFrom(s => s.StUpdOn))
            .ForMember(d => d.UpdatedByEmpSysId, o => o.MapFrom(s => s.StUpdByEmpSysId))
            .ForMember(d => d.AccountDate, o => o.MapFrom(s => s.StAccDate))
            .ForMember(d => d.FinYear, o => o.MapFrom(s => s.StFinYear))
            .ForMember(d => d.JvVoucherType, o => o.MapFrom(s => s.StJvVoucherType))
            .ForMember(d => d.JvNo, o => o.MapFrom(s => s.StJvNo))
            .ForMember(d => d.SetIntFlag, o => o.MapFrom(s => s.StSetIntFlg))
            .ForMember(d => d.TaxStatus, o => o.MapFrom(s => s.StTaxSts))
            .ForMember(d => d.TaxRate, o => o.MapFrom(s => s.StTaxRate))
            .ForMember(d => d.SettlementAmount, o => o.MapFrom(s => s.StSettlementAmount))
            .ForMember(d => d.Status, o => o.MapFrom(s => ((char)s.StStatus).ToString()))
            .ForMember(d => d.Deductions, o => o.MapFrom(s => s.Deductions))
            .ForMember(d => d.Approvals, o => o.MapFrom(s => s.Approvals))
            .ForMember(d => d.Payments, o => o.MapFrom(s => s.Payments));

        CreateMap<SettlementDeduction, DeductionDto>()
            .ForMember(d => d.DeductionId, o => o.MapFrom(s => s.SetDedId))
            .ForMember(d => d.SettlementNumber, o => o.MapFrom(s => s.SetNum))
            .ForMember(d => d.DeductionType, o => o.MapFrom(s => s.DedType))
            .ForMember(d => d.Amount, o => o.MapFrom(s => s.DedAmount))
            .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreatedOn));

        CreateMap<SettlementApproval, ApprovalDto>()
            .ForMember(d => d.ApprovalId, o => o.MapFrom(s => s.AprId))
            .ForMember(d => d.SettlementNumber, o => o.MapFrom(s => s.SetNum))
            .ForMember(d => d.Level, o => o.MapFrom(s => s.AprLevel))
            .ForMember(d => d.ApprovedBySysId, o => o.MapFrom(s => s.AprBySysId))
            .ForMember(d => d.Status, o => o.MapFrom(s => ((char)s.AprStatus).ToString()))
            .ForMember(d => d.Remarks, o => o.MapFrom(s => s.AprRemarks))
            .ForMember(d => d.ApprovalDate, o => o.MapFrom(s => s.AprDate));

        CreateMap<SettlementPayment, PaymentDto>()
            .ForMember(d => d.PaymentId, o => o.MapFrom(s => s.PayId))
            .ForMember(d => d.SettlementNumber, o => o.MapFrom(s => s.SetNum))
            .ForMember(d => d.PaymentMode, o => o.MapFrom(s => s.PayMode))
            .ForMember(d => d.Amount, o => o.MapFrom(s => s.PayAmount))
            .ForMember(d => d.PaymentDate, o => o.MapFrom(s => s.PayDate))
            .ForMember(d => d.ReferenceNo, o => o.MapFrom(s => s.PayRefNo))
            .ForMember(d => d.Status, o => o.MapFrom(s => ((char)s.PayStatus).ToString()));
    }
}
