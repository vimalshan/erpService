using AutoMapper;
using MobileAppManagement.Application.DTOs;
using MobileAppManagement.Domain.Entities;

namespace MobileAppManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AppDeviceDetail, AppDeviceDetailDto>();
        CreateMap<LoginDetail, LoginDetailDto>();
        CreateMap<AppRegistration, AppRegistrationDto>();
    }
}
