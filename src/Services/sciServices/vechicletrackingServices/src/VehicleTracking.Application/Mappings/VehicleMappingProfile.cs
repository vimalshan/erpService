using AutoMapper;
using VehicleTracking.Application.DTOs;
using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Application.Mappings;

public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        CreateMap<VehicleMaster, VehicleMasterDto>()
            .ForMember(d => d.FullRegistration, opt => opt.MapFrom(s =>
                $"{s.RegNum1}-{s.RegNum2}-{s.RegNum3}-{s.RegNum4}"));

        CreateMap<VehicleStage, VehicleStageDto>()
            .ForMember(d => d.StageName, opt => opt.MapFrom(s => s.Stage != null ? s.Stage.OptionName : null));

        CreateMap<VehicleTransaction, VehicleTransactionDto>()
            .ForMember(d => d.PurposeName, opt => opt.MapFrom(s => s.Purpose != null ? s.Purpose.PurposeName : null));

        CreateMap<VehicleInvoice, VehicleInvoiceDto>();
        CreateMap<StageMaster, StageMasterDto>();

        CreateMap<PurposeMaster, PurposeMasterDto>()
            .ForMember(d => d.Stages, opt => opt.MapFrom(s => s.PurposeStages));
        CreateMap<PurposeStage, PurposeStageDto>()
            .ForMember(d => d.StageName, opt => opt.MapFrom(s => s.Stage != null ? s.Stage.OptionName : null));

        CreateMap<DecisionFlag, DecisionFlagDto>();
        CreateMap<WeightInformation, WeightInfoDto>();
    }
}
