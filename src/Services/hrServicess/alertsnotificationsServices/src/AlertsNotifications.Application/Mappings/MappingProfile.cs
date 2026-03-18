using AutoMapper;
using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Domain.Entities;

namespace AlertsNotifications.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AlertMaster, AlertMasterDto>().ReverseMap();
        CreateMap<AlertGroup, AlertGroupDto>().ReverseMap();
        CreateMap<Circular, CircularDto>().ReverseMap();
        CreateMap<CircularSignatory, CircularSignatoryDto>().ReverseMap();
        CreateMap<CircularTemplate, CircularTemplateDto>().ReverseMap();
        CreateMap<ProbationConfirmationAlert, ProbationConfirmationAlertDto>().ReverseMap();
    }
}
