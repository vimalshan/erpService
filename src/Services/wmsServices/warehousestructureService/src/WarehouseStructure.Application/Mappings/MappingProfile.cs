using AutoMapper;
using WarehouseStructure.Application.DTOs;
using WarehouseStructure.Domain.Entities;

namespace WarehouseStructure.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Warehouse mappings
        CreateMap<Warehouse, WarehouseDto>()
            .ForMember(d => d.Address, opt => opt.MapFrom(s => s.AddressLine));

        CreateMap<CreateWarehouseDto, Warehouse>()
            .ForMember(d => d.AddressLine, opt => opt.MapFrom(s => s.Address))
            .ForMember(d => d.WarehouseId, opt => opt.Ignore())
            .ForMember(d => d.Zones, opt => opt.Ignore());

        // Zone mappings
        CreateMap<Zone, ZoneDto>()
            .ForMember(d => d.ZoneType, opt => opt.MapFrom(s => s.ZoneTypeValue));

        CreateMap<CreateZoneDto, Zone>()
            .ForMember(d => d.ZoneTypeValue, opt => opt.MapFrom(s => s.ZoneType))
            .ForMember(d => d.ZoneId, opt => opt.Ignore())
            .ForMember(d => d.Warehouse, opt => opt.Ignore());
    }
}
