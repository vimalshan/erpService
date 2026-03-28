using AutoMapper;

namespace AuthorizationService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AuthorizationService.Domain.Entities.Right, DTOs.RightDto>();

        CreateMap<AuthorizationService.Domain.Entities.SpecialInput, DTOs.SpecialInputDto>();

        CreateMap<AuthorizationService.Domain.Entities.SpecialInputMaster, DTOs.SpecialInputMasterDto>();

        CreateMap<AuthorizationService.Domain.Entities.TrackerRight, DTOs.TrackerRightDto>();

        CreateMap<AuthorizationService.Domain.Entities.UserRight, DTOs.UserRightDto>();
    }
}
