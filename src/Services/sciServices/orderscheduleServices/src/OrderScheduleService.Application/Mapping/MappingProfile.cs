namespace OrderScheduleService.Application.Mapping;

using AutoMapper;
using OrderScheduleService.Domain.Aggregates;
using OrderScheduleService.Domain.Entities;
using OrderScheduleService.Application.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Tied Order Mappings
        CreateMap<TiedOrderAggregate, TiedOrderDto>()
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details))
            .ReverseMap();

        CreateMap<OrderDetail, TiedOrderDetailDto>().ReverseMap();

        // Schedule Mappings
        CreateMap<ScheduleAggregate, ScheduleDto>()
            .ForMember(dest => dest.ScheduleDetails, opt => opt.MapFrom(src => src.ScheduleDetails))
            .ReverseMap();

        CreateMap<ScheduleDetail, ScheduleDetailDto>().ReverseMap();

        // Shift Mappings
        CreateMap<Shift, ShiftDto>().ReverseMap();
    }
}
