using AutoMapper;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Entities;

namespace EmployeeService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(d => d.EmployeeCode, opt => opt.MapFrom(s => s.EmployeeCode.Value))
            .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Phone != null ? s.Phone.Value : null))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email != null ? s.Email.Value : null));
    }
}
