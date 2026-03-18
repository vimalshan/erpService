using AutoMapper;
using CompensationService.Domain.Entities;
using CompensationService.Application.DTOs;
using CompensationService.Application.Commands;

namespace CompensationService.Application.Mappings;

/// <summary>
/// AutoMapper profile for CompensationGrade mappings
/// </summary>
public class CompensationGradeMappingProfile : Profile
{
    public CompensationGradeMappingProfile()
    {
        CreateMap<CompensationGrade, CompensationGradeDto>()
            .ForMember(dest => dest.GradeId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.GradeCode, opt => opt.MapFrom(src => src.GradeCode.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value))
            .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.SalaryStructure.BaseSalary))
            .ForMember(dest => dest.HraPercentage, opt => opt.MapFrom(src => src.SalaryStructure.HraPercentage))
            .ForMember(dest => dest.DaPercentage, opt => opt.MapFrom(src => src.SalaryStructure.DaPercentage))
            .ForMember(dest => dest.CalculatedHRA, opt => opt.MapFrom(src => src.SalaryStructure.CalculateHRA()))
            .ForMember(dest => dest.CalculatedDA, opt => opt.MapFrom(src => src.SalaryStructure.CalculateDA()))
            .ForMember(dest => dest.TotalSalary, opt => opt.MapFrom(src => src.SalaryStructure.CalculateTotalSalary()));

        CreateMap<CreateCompensationGradeDto, CreateCompensationGradeCommand>();
        CreateMap<UpdateCompensationGradeDto, UpdateCompensationGradeCommand>();
        CreateMap<ChangeGradeStatusDto, ChangeCompensationGradeStatusCommand>();
    }
}
