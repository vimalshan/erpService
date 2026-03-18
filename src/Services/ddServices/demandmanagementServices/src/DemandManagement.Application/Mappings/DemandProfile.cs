using AutoMapper;
using DemandManagement.Application.DTOs;
using DemandManagement.Domain.Entities;

namespace DemandManagement.Application.Mappings;

public class DemandProfile : Profile
{
    public DemandProfile()
    {
        CreateMap<DemandMaster, DemandDto>().ReverseMap();
        CreateMap<CreateDemandRequest, DemandMaster>()
            .ForMember(dest => dest.DemandStatus, opt => opt.MapFrom(_ => "O"))
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
