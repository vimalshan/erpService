using AutoMapper;
using CurrencyManagement.Application.DTOs;
using CurrencyManagement.Domain.Entities;

namespace CurrencyManagement.Application.Common.Mappings;

/// <summary>
/// AutoMapper profile for currency domain models
/// </summary>
public class CurrencyMappingProfile : Profile
{
    public CurrencyMappingProfile()
    {
        CreateMap<Currency, CurrencyDto>()
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol.Value));

        CreateMap<ExchangeRate, ExchangeRateDto>()
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate.Value));

        CreateMap<OrganizationCurrencyMapping, OrganizationCurrencyDto>();
    }
}
