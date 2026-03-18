using AutoMapper;
using TimesheetService.Application.DTOs;
using TimesheetService.Domain.Entities;

namespace TimesheetService.Application.Mappings;

public sealed class TimesheetMappingProfile : Profile
{
    public TimesheetMappingProfile()
    {
        CreateMap<Timesheet, TimesheetDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.Value))
            .ForMember(d => d.ApprovalStatus, o => o.MapFrom(s => s.ApprovalStatus.Value));

        CreateMap<Timesheet, TimesheetSummaryDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.Value))
            .ForMember(d => d.ApprovalStatus, o => o.MapFrom(s => s.ApprovalStatus.Value));
    }
}
