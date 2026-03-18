using AutoMapper;
using TaxService.Application.DTOs;
using TaxService.Domain.Entities;

namespace TaxService.Application.Mappings;

public class TaxMappingProfile : Profile
{
    public TaxMappingProfile()
    {
        // Map TaxMarginalDetail entity to DTO
        CreateMap<TaxMarginalDetail, TaxMarginalDetailDto>()
            .ForMember(
                dest => dest.GrossIncome,
                opt => opt.MapFrom(src => src.GrossIncome.Amount))
            .ForMember(
                dest => dest.StandardDeduction,
                opt => opt.MapFrom(src => src.StandardDeduction.Amount))
            .ForMember(
                dest => dest.TaxableIncome,
                opt => opt.MapFrom(src => src.TaxableIncome.Amount))
            .ForMember(
                dest => dest.CalculatedTax,
                opt => opt.MapFrom(src => src.CalculatedTax.Amount));

        // Map ConditionalMaster entity to DTO
        CreateMap<ConditionalMaster, ConditionalMasterDto>()
            .ForMember(
                dest => dest.TotalExemption,
                opt => opt.MapFrom(src => src.TotalExemption.Amount))
            .ForMember(
                dest => dest.TotalDeduction,
                opt => opt.MapFrom(src => src.TotalDeduction.Amount));

        // Map TaxExemption to DTO
        CreateMap<TaxExemption, TaxExemptionDto>()
            .ForMember(
                dest => dest.Amount,
                opt => opt.MapFrom(src => src.Amount.Amount));

        // Map TaxDeduction to DTO
        CreateMap<TaxDeduction, TaxDeductionDto>()
            .ForMember(
                dest => dest.Amount,
                opt => opt.MapFrom(src => src.Amount.Amount));
    }
}
