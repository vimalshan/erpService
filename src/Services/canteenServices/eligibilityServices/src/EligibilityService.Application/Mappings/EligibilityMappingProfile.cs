using AutoMapper;
using EligibilityService.Application.DTOs;
using EligibilityService.Domain.Entities;

namespace EligibilityService.Application.Mappings;

public class EligibilityMappingProfile : Profile
{
    public EligibilityMappingProfile()
    {
        CreateMap<EligibilityMaster, EligibilityMasterDto>()
            .ConstructUsing(src => new EligibilityMasterDto(
                src.CanteenUnit, src.ShiftCode, src.ItemCode,
                src.EligibleLimit, src.EnteredUser, src.EnteredOn, src.TimeOfficeUnit));

        CreateMap<EligibilityMasterHistory, EligibilityMasterHistoryDto>()
            .ConstructUsing(src => new EligibilityMasterHistoryDto(
                src.CanteenUnit, src.ShiftCode, src.ItemCode,
                src.EligibleLimit, src.ModifiedUser, src.ModifiedOn));

        CreateMap<ShiftMapping, ShiftMappingDto>()
            .ConstructUsing(src => new ShiftMappingDto(
                src.CompanyCode, src.ShiftCode, src.BeforeShiftCode, src.AfterShiftCode));

        CreateMap<DaywiseEligibility, DaywiseEligibilityDto>()
            .ConstructUsing(src => new DaywiseEligibilityDto(
                src.SerialNumber, src.CompanyCode, src.EmployeeSysId, src.AttendanceDate,
                src.ProcessNumber, src.ShiftCode, src.ItemCode, src.ShiftQuantity,
                src.BeforeShiftQty, src.AfterShiftQty, src.EnteredUser, src.EnteredOn,
                src.FlexField1, src.GradeType));
    }
}
