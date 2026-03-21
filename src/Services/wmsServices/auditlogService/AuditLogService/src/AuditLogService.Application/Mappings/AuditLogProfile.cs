using AutoMapper;
using AuditLogService.Application.DTOs;
using AuditLogService.Domain.Entities;

namespace AuditLogService.Application.Mappings;

public class AuditLogProfile : Profile
{
    public AuditLogProfile()
    {
        CreateMap<AuditLogEntry, AuditLogDto>()
            .ForMember(d => d.LogId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Action, opt => opt.MapFrom(s => s.Action.Value))
            .ForMember(d => d.OldValues, opt => opt.MapFrom(s => s.ChangeData.OldValues))
            .ForMember(d => d.NewValues, opt => opt.MapFrom(s => s.ChangeData.NewValues));
    }
}
