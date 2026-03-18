using AutoMapper;
using CanteenUnit.Application.DTOs;
using CanteenUnit.Domain.Entities;

namespace CanteenUnit.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CanteenUnitMaster, CanteenUnitMasterDto>().ReverseMap();
        CreateMap<CanteenMaster, CanteenMasterDto>().ReverseMap();
        CreateMap<CanteenUnitAccess, CanteenUnitAccessDto>().ReverseMap();
    }
}
