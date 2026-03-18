using AutoMapper;
using HRService.Domain.Entities;

namespace HRService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Employee Mappings
        CreateMap<Employee, DTOs.EmployeeDto>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.EmploymentType, opt => opt.MapFrom(src => src.EmploymentType.ToString()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber != null ? src.PhoneNumber.Value : null))
            .ReverseMap();

        // Department Mappings
        CreateMap<Department, DTOs.DepartmentDto>()
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Id))
            .ReverseMap();

        // Leave Mappings
        CreateMap<EmployeeLeave, DTOs.LeaveDto>()
            .ForMember(dest => dest.LeaveId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap();

        // Attendance Mappings
        CreateMap<Attendance, DTOs.AttendanceDto>()
            .ForMember(dest => dest.AttendanceId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap();

        // Salary Mappings
        CreateMap<EmployeeSalary, DTOs.SalaryDto>()
            .ForMember(dest => dest.SalaryId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap();
    }
}
