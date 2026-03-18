using AutoMapper;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Application.Mappings;

/// <summary>
/// AutoMapper profile for Employee mappings
/// </summary>
public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        // Employee to DTO
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.GrossCTC, opt => opt.MapFrom(src => src.GrossCTC.Amount))
            .ForMember(dest => dest.BasicSalary, opt => opt.MapFrom(src => src.BasicSalary.Amount));

        // CreateEmployeeDto to Employee - handled in handler
        // UpdateEmployeeDto - handled in handler

        // SalaryIncrementLog to DTO
        CreateMap<SalaryIncrementLog, SalaryIncrementLogDto>()
            .ForMember(dest => dest.OldCTC, opt => opt.MapFrom(src => src.OldCTC.Amount))
            .ForMember(dest => dest.NewCTC, opt => opt.MapFrom(src => src.NewCTC.Amount))
            .ForMember(dest => dest.IncrementPercentage, opt => opt.MapFrom(src => src.IncrementPercentage.Value));

        // Money value object mappings
        CreateMap<Money, decimal>().ConvertUsing(src => src.Amount);

        // Percentage value object mappings
        CreateMap<Percentage, decimal>().ConvertUsing(src => src.Value);
    }
}
