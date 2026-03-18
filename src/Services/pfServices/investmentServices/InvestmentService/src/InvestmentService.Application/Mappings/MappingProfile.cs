using AutoMapper;
using InvestmentService.Application.DTOs;
using InvestmentService.Domain.Entities;

namespace InvestmentService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Investment, InvestmentDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : null))
            .ForMember(d => d.SubCategoryName, o => o.MapFrom(s => s.SubCategory != null ? s.SubCategory.Name : null))
            .ForMember(d => d.BrokerName, o => o.MapFrom(s => s.Broker != null ? s.Broker.BrokerName : null));

        CreateMap<SaleDetail, SaleDetailDto>();
        CreateMap<ScheduleDetail, ScheduleDetailDto>();
        CreateMap<InvestmentCategory, CategoryDto>();
        CreateMap<InvestmentSubCategory, SubCategoryDto>();
        CreateMap<Broker, BrokerDto>();
        CreateMap<CreditAgency, CreditAgencyDto>();
        CreateMap<CreditRating, CreditRatingDto>();
    }
}
