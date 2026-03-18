namespace CheckupManagementService.Infrastructure.Mapping;

using AutoMapper;
using CheckupManagementService.Domain.Entities;
using CheckupManagementService.DTOs;

/// <summary>
/// AutoMapper profile for entity to DTO mappings
/// </summary>
public class CheckupMappingProfile : Profile
{
    public CheckupMappingProfile()
    {
        // Checkup Master mappings
        CreateMap<CheckupMaster, CheckupMasterDto>()
            .ReverseMap()
            .ForMember(dest => dest.CompanyCode, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupCode, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupName, opt => opt.Ignore())
            .ForMember(dest => dest.EffectiveDate, opt => opt.Ignore())
            .ForMember(dest => dest.CloseDate, opt => opt.Ignore())
            .ForMember(dest => dest.Flag, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupOthers, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupTests, opt => opt.Ignore())
            .ForMember(dest => dest.HealthMains, opt => opt.Ignore());

        CreateMap<CreateCheckupDto, CheckupMaster>()
            .ForMember(dest => dest.CheckupMasterId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.CompanyCode, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupCode, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupName, opt => opt.Ignore())
            .ForMember(dest => dest.EffectiveDate, opt => opt.Ignore())
            .ForMember(dest => dest.CloseDate, opt => opt.Ignore())
            .ForMember(dest => dest.Flag, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
            .ForMember(dest => dest.CheckupOthers, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupTests, opt => opt.Ignore())
            .ForMember(dest => dest.HealthMains, opt => opt.Ignore());

        CreateMap<UpdateCheckupDto, CheckupMaster>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Health Main mappings
        CreateMap<HealthMain, HealthMainDto>()
            .ReverseMap()
            .ForMember(dest => dest.CompanyCode, opt => opt.Ignore())
            .ForMember(dest => dest.EntryEmployeeNumber, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupCode, opt => opt.Ignore())
            .ForMember(dest => dest.TextField2, opt => opt.Ignore())
            .ForMember(dest => dest.TextField3, opt => opt.Ignore())
            .ForMember(dest => dest.TextField4, opt => opt.Ignore())
            .ForMember(dest => dest.TextField5, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupMaster, opt => opt.Ignore())
            .ForMember(dest => dest.HealthSubs, opt => opt.Ignore());

        CreateMap<CreateHealthExaminationDto, HealthMain>()
            .ForMember(dest => dest.CheckupMasterId, opt => opt.MapFrom(src => src.CheckupMasterId))
            .ForMember(dest => dest.HealthId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.CompanyCode, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupDate, opt => opt.Ignore())
            .ForMember(dest => dest.HealthNumber, opt => opt.Ignore())
            .ForMember(dest => dest.EntryEmployeeNumber, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupCode, opt => opt.Ignore())
            .ForMember(dest => dest.TextField2, opt => opt.Ignore())
            .ForMember(dest => dest.TextField3, opt => opt.Ignore())
            .ForMember(dest => dest.TextField4, opt => opt.Ignore())
            .ForMember(dest => dest.TextField5, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CheckupMaster, opt => opt.Ignore())
            .ForMember(dest => dest.HealthSubs, opt => opt.Ignore());

        // Health Sub mappings
        CreateMap<HealthSub, HealthTestResultDto>();
        CreateMap<HealthTestResultDto, HealthSub>()
            .ForMember(dest => dest.HealthNumber, opt => opt.Ignore())
            .ForMember(dest => dest.TestCode, opt => opt.Ignore())
            .ForMember(dest => dest.TestType, opt => opt.Ignore())
            .ForMember(dest => dest.EmployeeNumber, opt => opt.Ignore())
            .ForMember(dest => dest.TestRemarks, opt => opt.Ignore())
            .ForMember(dest => dest.TestDate, opt => opt.Ignore())
            .ForMember(dest => dest.ValidationFlag, opt => opt.Ignore())
            .ForMember(dest => dest.TextField2, opt => opt.Ignore())
            .ForMember(dest => dest.TextField3, opt => opt.Ignore())
            .ForMember(dest => dest.TextField4, opt => opt.Ignore())
            .ForMember(dest => dest.TextField5, opt => opt.Ignore())
            .ForMember(dest => dest.DoctorRemarks, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.HealthMain, opt => opt.Ignore())
            .ForMember(dest => dest.TestValue, opt => opt.MapFrom(src => src.TestValue))
            .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks));

        // Test Master mappings
        CreateMap<TestMaster, TestMasterDto>();
        CreateMap<CreateTestMasterDto, TestMaster>()
            .ForMember(dest => dest.TestCode, opt => opt.Ignore())
            .ForMember(dest => dest.CheckboxFlag, opt => opt.Ignore())
            .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CloseDate, opt => opt.Ignore())
            .ForMember(dest => dest.CloseFlag, opt => opt.Ignore())
            .ForMember(dest => dest.RangeValue, opt => opt.Ignore())
            .ForMember(dest => dest.TestGroup, opt => opt.Ignore())
            .ForMember(dest => dest.TestId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CheckupTests, opt => opt.Ignore())
            .ForMember(dest => dest.HealthMinMaxValues, opt => opt.Ignore())
            .ForMember(dest => dest.HealthEntryLovs, opt => opt.Ignore());

        // Checkup Others mappings
        CreateMap<CheckupOthers, CheckupOthersDto>()
            .ReverseMap()
            .ForMember(dest => dest.CompanyCode, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupCode, opt => opt.Ignore())
            .ForMember(dest => dest.OtherSerialNumber, opt => opt.Ignore())
            .ForMember(dest => dest.MandatoryFlag, opt => opt.Ignore())
            .ForMember(dest => dest.FieldTypeCode, opt => opt.Ignore())
            .ForMember(dest => dest.EffectiveDate, opt => opt.Ignore())
            .ForMember(dest => dest.CloseDate, opt => opt.Ignore())
            .ForMember(dest => dest.FieldTypeName, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.FieldType, opt => opt.Ignore())
            .ForMember(dest => dest.ListOfValues, opt => opt.Ignore());

        CreateMap<CreateCheckupOthersDto, CheckupOthers>()
            .ForMember(dest => dest.CompanyCode, opt => opt.Ignore())
            .ForMember(dest => dest.CheckupCode, opt => opt.Ignore())
            .ForMember(dest => dest.OtherSerialNumber, opt => opt.Ignore())
            .ForMember(dest => dest.MandatoryFlag, opt => opt.Ignore())
            .ForMember(dest => dest.FieldTypeCode, opt => opt.Ignore())
            .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CloseDate, opt => opt.Ignore())
            .ForMember(dest => dest.FieldTypeName, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.FieldType, opt => opt.Ignore())
            .ForMember(dest => dest.ListOfValues, opt => opt.Ignore());

        // Health Check Card mappings
        CreateMap<HealthCheckCard, HealthCheckCardDto>();
        CreateMap<HealthCheckCardDto, HealthCheckCard>()
            .ForMember(dest => dest.HealthNumber, opt => opt.Ignore())
            .ForMember(dest => dest.EmployeeDate, opt => opt.Ignore())
            .ForMember(dest => dest.CompanyCode, opt => opt.Ignore())
            .ForMember(dest => dest.PersonalDetails, opt => opt.Ignore())
            .ForMember(dest => dest.ScreeningDetails, opt => opt.Ignore())
            .ForMember(dest => dest.AdviceRemark1, opt => opt.Ignore())
            .ForMember(dest => dest.DoctorDate1, opt => opt.Ignore())
            .ForMember(dest => dest.AdviceFollowup1, opt => opt.Ignore())
            .ForMember(dest => dest.AdviceRemark2, opt => opt.Ignore())
            .ForMember(dest => dest.DoctorDate2, opt => opt.Ignore())
            .ForMember(dest => dest.AdviceFollowup2, opt => opt.Ignore());
    }
}
