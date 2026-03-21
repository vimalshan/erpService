using AutoMapper;
using AdminService.Application.DTOs;
using AdminService.Domain.Entities;

namespace AdminService.Application.Mappings;

/// <summary>
/// AutoMapper profile for entity to DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // AdminUnit mappings
        CreateMap<AdminUnit, AdminUnitDto>().ReverseMap();
        CreateMap<CreateAdminUnitRequest, AdminUnit>();
        CreateMap<UpdateAdminUnitRequest, AdminUnit>();

        // FinanceUnit mappings
        CreateMap<FinanceUnit, FinanceUnitDto>().ReverseMap();

        // AdminAccess mappings
        CreateMap<AdminAccess, object>();

        // AdminContact mappings
        CreateMap<AdminContact, object>();
    }
}
