using AutoMapper;
using EmployeeService.Application.Commands.Employees;
using EmployeeService.Application.Queries.Employees;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Application.Mappings
{
    /// <summary>
    /// AutoMapper configuration for Employee Service
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Employee mappings
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EmployeeNumber, opt => opt.MapFrom(src => src.EmploymentDetails.EmployeeNumber))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.EmploymentDetails.UserId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.PersonalInfo.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.PersonalInfo.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.PersonalInfo.LastName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.PersonalInfo.GetFullName()))
                .ForMember(dest => dest.NickName, opt => opt.MapFrom(src => src.EmploymentDetails.NickName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.PersonalInfo.DateOfBirth))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.GetAge()))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.PersonalInfo.Gender.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ContactInfo.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ContactInfo.PhoneNumber))
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => src.OrganizationalAssignment.Designation))
                .ForMember(dest => dest.GradeCode, opt => opt.MapFrom(src => src.GradeInfo.GradeCode))
                .ForMember(dest => dest.GradeName, opt => opt.MapFrom(src => src.GradeInfo.GradeName))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.OrganizationalAssignment.Unit))
                .ForMember(dest => dest.JoiningDate, opt => opt.MapFrom(src => src.EmploymentDetails.JoiningDate))
                .ForMember(dest => dest.YearsOfService, opt => opt.MapFrom(src => src.GetYearsOfService()));

            CreateMap<Employee, EmployeeDetailedDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PersonalInfo, opt => opt.MapFrom(src => src.PersonalInfo))
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo))
                .ForMember(dest => dest.EmploymentDetails, opt => opt.MapFrom(src => src.EmploymentDetails))
                .ForMember(dest => dest.GradeInfo, opt => opt.MapFrom(src => src.GradeInfo))
                .ForMember(dest => dest.OrganizationalAssignment, opt => opt.MapFrom(src => src.OrganizationalAssignment))
                .ForMember(dest => dest.SalaryInfo, opt => opt.MapFrom(src => src.SalaryInfo));

            CreateMap<PersonalInfo, PersonalInfoDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));
            CreateMap<ContactInfo, ContactInfoDto>();
            CreateMap<EmploymentDetails, EmploymentDetailsDto>();
            CreateMap<GradeInfo, GradeInfoDto>();
            CreateMap<OrganizationalAssignment, OrganizationalAssignmentDto>();
            CreateMap<SalaryInfo, SalaryInfoDto>();

            // Appraisal mappings
            CreateMap<EmployeeAppraisal, AppraisalSummaryDto>()
                .ForMember(dest => dest.AppraisalId, opt => opt.MapFrom(src => src.Id));

            // Career Plan mappings
            CreateMap<EmployeeCareerPlan, CareerPlanSummaryDto>()
                .ForMember(dest => dest.CareerPlanId, opt => opt.MapFrom(src => src.Id));

            // Benefit mappings
            CreateMap<EmployeeBenefit, BenefitSummaryDto>()
                .ForMember(dest => dest.BenefitId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
