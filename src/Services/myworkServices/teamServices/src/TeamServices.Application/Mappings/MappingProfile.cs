using AutoMapper;
using TeamServices.Domain.Entities;

namespace TeamServices.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TeamMaster, DTOs.TeamDto>()
            .ForMember(d => d.TeamId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.TeamName, opt => opt.MapFrom(s => s.TeamName));

        CreateMap<TeamEmployeeMap, DTOs.TeamEmployeeMapDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.EmployeeSysId, opt => opt.MapFrom(s => s.EmployeeSysId));

        CreateMap<TeamUnitMap, DTOs.TeamUnitMapDto>()
            .ForMember(d => d.MapId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.UnitId, opt => opt.MapFrom(s => s.UnitId))
            .ForMember(d => d.GradeCategory, opt => opt.MapFrom(s => s.GradeCategory));
    }
}
