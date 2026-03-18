using AutoMapper;
using ProjectService.Application.DTOs;
using ProjectService.Domain.Entities;

namespace ProjectService.Application.Mappings;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ProjectMain, ProjectMainDto>()
            .ForMember(d => d.ProjectTypeName, opt => opt.MapFrom(s => s.ProjectType != null ? s.ProjectType.ProjTypeName : null))
            .ForMember(d => d.LocationName, opt => opt.MapFrom(s => s.Location != null ? s.Location.LocName : null))
            .ForMember(d => d.ProcessName, opt => opt.MapFrom(s => s.Process != null ? s.Process.ProcName : null))
            .ForMember(d => d.ProjStatus, opt => opt.MapFrom(s => s.ProjStatus.ToString()));

        CreateMap<ProjectMaster, ProjectMasterDto>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.CategoryName : null))
            .ForMember(d => d.ProjectListAll, opt => opt.MapFrom(s => s.ProjectListAll.ToString()));

        CreateMap<ProjectMember, ProjectMemberDto>()
            .ForMember(d => d.FunctionName, opt => opt.MapFrom(s => s.Function != null ? s.Function.ProjFuncName : null));

        CreateMap<ProjectStatusHistory, ProjectStatusHistoryDto>();
        CreateMap<ProjectHold, ProjectHoldDto>()
            .ForMember(d => d.ProjHoldType, opt => opt.MapFrom(s => s.ProjHoldType.ToString()));
        CreateMap<ProjectApprovalDetail, ProjectApprovalDetailDto>()
            .ForMember(d => d.ProjApprType, opt => opt.MapFrom(s => s.ProjApprType.ToString()))
            .ForMember(d => d.ProjApprStatus, opt => opt.MapFrom(s => s.ProjApprStatus.ToString()));

        CreateMap<ProjectTypeMaster, ProjectTypeMasterDto>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.ProjCatName : null))
            .ForMember(d => d.Deliverables, opt => opt.MapFrom(s => s.DeliverableMaps))
            .ForMember(d => d.Objectives, opt => opt.MapFrom(s => s.ObjectiveMaps))
            .ForMember(d => d.Scopes, opt => opt.MapFrom(s => s.ScopeMaps));

        CreateMap<ProjectTypeDeliverableMap, ProjectTypeDeliverableMapDto>();
        CreateMap<ProjectTypeObjectiveMap, ProjectTypeObjectiveMapDto>();
        CreateMap<ProjectTypeScopeMap, ProjectTypeScopeMapDto>();

        CreateMap<ProjectLocation, ProjectLocationDto>();
        CreateMap<ProjectProcess, ProjectProcessDto>();
        CreateMap<ProjectDepartment, ProjectDepartmentDto>();
        CreateMap<ProjectFunction, ProjectFunctionDto>();
        CreateMap<ProjectCategoryMaster, ProjectCategoryDto>();
        CreateMap<ProjectTypeCategoryMaster, ProjectTypeCategoryDto>();
    }
}
