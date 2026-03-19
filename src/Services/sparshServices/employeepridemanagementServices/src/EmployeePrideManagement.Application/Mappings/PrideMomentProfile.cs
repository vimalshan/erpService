using AutoMapper;
using EmployeePrideManagement.Application.DTOs;
using EmployeePrideManagement.Domain.Entities;

namespace EmployeePrideManagement.Application.Mappings;

public class PrideMomentProfile : Profile
{
    public PrideMomentProfile()
    {
        CreateMap<MomentPride, PrideMomentDto>()
            .ForMember(d => d.Location, opt => opt.MapFrom(s => s.Location.Value))
            .ForMember(d => d.Image, opt => opt.MapFrom(s => s.Image.Value));
    }
}
