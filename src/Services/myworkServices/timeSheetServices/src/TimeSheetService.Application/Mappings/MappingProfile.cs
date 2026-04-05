using AutoMapper;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Entities;

namespace TimeSheetService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TimesheetEntry → TimesheetEntryDto
        CreateMap<TimesheetEntry, TimesheetEntryDto>()
            .ForMember(d => d.TimeId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.EntryType, o => o.MapFrom(s => s.EntryType.Name))
            .ForMember(d => d.EntryTypeCode, o => o.MapFrom(s => s.EntryType.Code.ToString()))
            .ForMember(d => d.Details, o => o.MapFrom(s => s.Details));

        CreateMap<TimesheetDetail, TimesheetDetailDto>()
            .ForMember(d => d.DetailId, o => o.MapFrom(s => s.Id));

        // TcTimesheetEntry → TcTimesheetEntryDto
        CreateMap<TcTimesheetEntry, TcTimesheetEntryDto>()
            .ForMember(d => d.TimeId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.EntryType, o => o.MapFrom(s => s.EntryType.Name))
            .ForMember(d => d.EntryTypeCode, o => o.MapFrom(s => s.EntryType.Code.ToString()))
            .ForMember(d => d.Details, o => o.MapFrom(s => s.Details));

        CreateMap<TcTimesheetDetail, TcTimesheetDetailDto>()
            .ForMember(d => d.DetailId, o => o.MapFrom(s => s.Id));

        // TcProject → TcProjectDto
        CreateMap<TcProject, TcProjectDto>()
            .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.ListAll, o => o.MapFrom(s => s.ListAll.ToString()));

        // TcProjectCategory → TcProjectCategoryDto
        CreateMap<TcProjectCategory, TcProjectCategoryDto>()
            .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.Id));

        // TcSubCategory → TcSubCategoryDto
        CreateMap<TcSubCategory, TcSubCategoryDto>()
            .ForMember(d => d.SubCategoryId, o => o.MapFrom(s => s.Id));

        // TsProject → TsProjectDto
        CreateMap<TsProject, TsProjectDto>()
            .ForMember(d => d.ProjectType, o => o.MapFrom(s => s.ProjectType.ToString()))
            .ForMember(d => d.ApplyAll, o => o.MapFrom(s => s.ApplyAll.ToString()));

        // TsStage → TsStageDto
        CreateMap<TsStage, TsStageDto>();

        // TsActivity → TsActivityDto
        CreateMap<TsActivity, TsActivityDto>()
            .ForMember(d => d.ActivityId, o => o.MapFrom(s => s.Id));
    }
}
