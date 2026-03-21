using AutoMapper;
using MasterDataService.Application.DTOs;
using MasterDataService.Domain.Entities;

namespace MasterDataService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<GuestHouse, GuestHouseDto>();
        CreateMap<GuestHouseRoom, GuestHouseRoomDto>();
        CreateMap<GuestRoomAvailability, GuestRoomAvailabilityDto>();
        CreateMap<GlCodeCombination, GlCodeCombinationDto>()
            .ForMember(d => d.ConcatenatedSegments, o => o.MapFrom(s => s.Segments.ConcatenatedSegments))
            .ForMember(d => d.AccountType, o => o.MapFrom(s => s.Segments.AccountType));
        CreateMap<Coupon, CouponDto>();
        CreateMap<TaxSlab, TaxSlabDto>();
        CreateMap<Area, AreaDto>();
        CreateMap<Route, RouteDto>();
    }
}
