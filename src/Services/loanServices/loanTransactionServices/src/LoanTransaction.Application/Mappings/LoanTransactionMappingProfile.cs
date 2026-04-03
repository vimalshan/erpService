using AutoMapper;
using LoanTransaction.Application.DTOs;
using LoanTransaction.Domain.Aggregates;
using LoanTransaction.Domain.Entities;
using LoanTransaction.Domain.Interfaces;

namespace LoanTransaction.Application.Mappings;

public class LoanTransactionMappingProfile : Profile
{
    public LoanTransactionMappingProfile()
    {
        CreateMap<LoanAggregate, LoanDto>()
            .ForMember(d => d.LoanNo, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.DisbursementType, o => o.MapFrom(s => s.DisbursementType.Value))
            .ForMember(d => d.PrincipalAmount, o => o.MapFrom(s => s.PrincipalAmount.Amount))
            .ForMember(d => d.OldPrincipalAdj, o => o.MapFrom(s => s.OldPrincipalAdj.Amount))
            .ForMember(d => d.AmountPaid, o => o.MapFrom(s => s.AmountPaid.Amount))
            .ForMember(d => d.PrincipalOutstanding, o => o.MapFrom(s => s.PrincipalOutstanding.Amount))
            .ForMember(d => d.RecoveryMethod, o => o.MapFrom(s => s.RecoveryMethod.Value))
            .ForMember(d => d.ClosureType, o => o.MapFrom(s => s.ClosureType.Value))
            .ForMember(d => d.CompoundingFactor, o => o.MapFrom(s => s.CompoundingFactor.ToString()))
            .ForMember(d => d.InterestFrequency, o => o.MapFrom(s => s.InterestFrequency.ToString()))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive));

        CreateMap<LoanInstallment, LoanInstallmentDto>()
            .ForMember(d => d.IsPaid, o => o.MapFrom(s => s.IsPaid))
            .ForMember(d => d.RemainingAmount, o => o.MapFrom(s => s.RemainingAmount));

        CreateMap<LoanSettlement, LoanSettlementDto>()
            .ForMember(d => d.IsCancelled, o => o.MapFrom(s => s.IsCancelled));

        CreateMap<LoanLedger, LoanLedgerDto>()
            .ForMember(d => d.DCFlag, o => o.MapFrom(s => s.DCFlag.ToString()));

        CreateMap<EmiScheduleItem, EmiScheduleItemDto>();
    }
}
