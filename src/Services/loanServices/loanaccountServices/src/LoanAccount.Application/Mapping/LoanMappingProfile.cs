using AutoMapper;
using LoanAccount.Application.DTOs;
using LoanAccount.Domain.Entities;

namespace LoanAccount.Application.Mapping;

/// <summary>
/// AutoMapper profile for loan domain entities and DTOs
/// </summary>
public class LoanMappingProfile : Profile
{
    public LoanMappingProfile()
    {
        // LoanMain mappings
        CreateMap<LoanMain, LoanResponse>()
            .ForMember(dest => dest.LoanNo, opt => opt.MapFrom(src => src.LoanNo))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmpSysId))
            .ForMember(dest => dest.PrincipalAmount, opt => opt.MapFrom(src => src.PrincipalAmount.Amount))
            .ForMember(dest => dest.OutstandingAmount, opt => opt.MapFrom(src => src.PrincipalOutstanding.Amount))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.LoanStatus.Status))
            .ForMember(dest => dest.LoanDate, opt => opt.MapFrom(src => src.LoanDate))
            .ForMember(dest => dest.FirstInstallmentDate, opt => opt.MapFrom(src => src.FirstInstallmentDate))
            .ForMember(dest => dest.ClosureDate, opt => opt.MapFrom(src => src.LoanClosureDate));

        // LoanInstallment mappings
        CreateMap<LoanInstallment, InstallmentResponse>()
            .ForMember(dest => dest.InstallmentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LoanNo, opt => opt.MapFrom(src => src.LoanNo))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.InstallmentAmount.Amount))
            .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.InterestRate.Rate))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.InstallmentDate))
            .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => (DateTime?)null))
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(src => src.PrincipalRecovered.Amount > 0));

        // LoanLedger mappings
        CreateMap<LoanLedger, LoanLedgerEntryResponse>()
            .ForMember(dest => dest.LedgerId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.TransactionAmount.Amount));

        // Interest rate mappings
        CreateMap<LoanEmployeeInterestRate, InterestRateResponse>()
            .ForMember(dest => dest.RateId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.InterestRate.Rate))
            .ForMember(dest => dest.EMIAmount, opt => opt.MapFrom(src => src.EMIAmount.Amount));

        // Settlement mappings
        CreateMap<LoanSettlement, LoanSettlementResponse>()
            .ForMember(dest => dest.SettlementId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SettlementType, opt => opt.MapFrom(src => src.SettlementType.Type))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.InstallmentAmount.Amount));
    }
}
