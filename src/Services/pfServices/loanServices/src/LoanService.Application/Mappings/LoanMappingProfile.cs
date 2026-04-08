using AutoMapper;
using LoanService.Application.DTOs;
using LoanService.Domain.Entities;

namespace LoanService.Application.Mappings;

public class LoanMappingProfile : Profile
{
    public LoanMappingProfile()
    {
        CreateMap<LoanMain, LoanDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Repayments, o => o.MapFrom(s => s.Repayments))
            .ForMember(d => d.Deductions, o => o.MapFrom(s => s.Deductions));

        CreateMap<LoanRepayment, RepaymentDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<LoanDeduction, DeductionDto>();
    }
}
