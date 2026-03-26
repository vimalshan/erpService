using AutoMapper;
using ReferenceDataService.Application.DTOs;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LovMaster, LovMasterDto>();
        CreateMap<LovTypeMaster, LovTypeMasterDto>();
        CreateMap<PathToSqlServer, PathToSqlServerDto>();
    }
}
