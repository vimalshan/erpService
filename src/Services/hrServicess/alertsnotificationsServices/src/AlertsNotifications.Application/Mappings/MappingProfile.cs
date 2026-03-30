using AutoMapper;
using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Domain.Entities;

namespace AlertsNotifications.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AlertMaster, AlertMasterDto>()
            .ForMember(d => d.AlertUnitSpecific,
                opt => opt.MapFrom(s => s.AlertUnitSpecific.HasValue ? s.AlertUnitSpecific.Value.ToString() : null));

        CreateMap<AlertMasterDto, AlertMaster>()
            .ForMember(d => d.AlertUnitSpecific,
                opt => opt.MapFrom(s => !string.IsNullOrWhiteSpace(s.AlertUnitSpecific)
                    ? (char?)s.AlertUnitSpecific![0]
                    : null));

        CreateMap<AlertGroup, AlertGroupDto>()
            .ForMember(d => d.AlertGroupType,
                opt => opt.MapFrom(s => s.AlertGroupType.ToString()));

        CreateMap<AlertGroupDto, AlertGroup>()
            .ForMember(d => d.AlertGroupType,
                opt => opt.MapFrom(s => !string.IsNullOrWhiteSpace(s.AlertGroupType)
                    ? s.AlertGroupType[0]
                    : default(char)));

        CreateMap<Circular, CircularDto>()
            .ForMember(d => d.CircularSparshFlag,
                opt => opt.MapFrom(s => s.CircularSparshFlag.ToString()))
            .ForMember(d => d.CircularStatus,
                opt => opt.MapFrom(s => s.CircularStatus.ToString()))
            .ForMember(d => d.CircularAttachEmpFlag,
                opt => opt.MapFrom(s => s.CircularAttachEmpFlag.HasValue ? s.CircularAttachEmpFlag.Value.ToString() : null));

        CreateMap<CircularDto, Circular>()
            .ForMember(d => d.CircularSparshFlag,
                opt => opt.MapFrom(s => !string.IsNullOrWhiteSpace(s.CircularSparshFlag)
                    ? s.CircularSparshFlag[0]
                    : default(char)))
            .ForMember(d => d.CircularStatus,
                opt => opt.MapFrom(s => !string.IsNullOrWhiteSpace(s.CircularStatus)
                    ? s.CircularStatus[0]
                    : default(char)))
            .ForMember(d => d.CircularAttachEmpFlag,
                opt => opt.MapFrom(s => !string.IsNullOrWhiteSpace(s.CircularAttachEmpFlag)
                    ? (char?)s.CircularAttachEmpFlag![0]
                    : null));

        CreateMap<CircularSignatory, CircularSignatoryDto>().ReverseMap();
        CreateMap<CircularTemplate, CircularTemplateDto>().ReverseMap();
        CreateMap<ProbationConfirmationAlert, ProbationConfirmationAlertDto>().ReverseMap();
    }
}
