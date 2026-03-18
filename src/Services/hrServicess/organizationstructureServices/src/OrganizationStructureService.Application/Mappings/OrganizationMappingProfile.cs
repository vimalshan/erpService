using AutoMapper;
using OrganizationStructureService.Application.DTOs;
using OrganizationStructureService.Domain.Entities;

namespace OrganizationStructureService.Application.Mappings;

public class OrganizationMappingProfile : Profile
{
    public OrganizationMappingProfile()
    {
        CreateMap<Business, BusinessDto>()
            .ConstructUsing(src => new BusinessDto(
                src.BusinessId, src.BusinessName, src.BusinessShortName,
                src.BusinessCode, src.BusinessCompanyId, src.BusinessCompanyCode,
                src.LiveFlag.Value, src.UpdatedOn, src.UpdatedBy));

        CreateMap<Unit, UnitDto>()
            .ConstructUsing(src => new UnitDto(
                src.UnitId, src.UnitName, src.UnitShortName, src.UnitCode.Value,
                src.UnitBusinessId, src.UnitBusinessCode, src.LiveFlag.Value,
                src.OrgId, src.ReportFlag, src.UpdatedOn, src.UpdatedBy));

        CreateMap<Department, DepartmentDto>()
            .ConstructUsing(src => new DepartmentDto(
                src.DepartmentId, src.DepartmentName, src.DepartmentCode,
                src.LiveFlag != null ? src.LiveFlag.Value : null,
                src.UpdatedOn, src.UpdatedBy));

        CreateMap<Division, DivisionDto>()
            .ConstructUsing(src => new DivisionDto(
                src.DivisionId, src.DivisionCode, src.DivisionName,
                src.LiveFlag != null ? src.LiveFlag.Value : null,
                src.UpdatedOn, src.UpdatedBy));

        CreateMap<Grade, GradeDto>()
            .ConstructUsing(src => new GradeDto(
                src.GradeId, src.GradeCode, src.GradeName, src.GradeDesignation,
                src.GradeCategoryCode,
                src.LiveFlag != null ? src.LiveFlag.Value : null,
                src.ManagementCategoryCode, src.Priority));

        CreateMap<Position, PositionDto>()
            .ConstructUsing(src => new PositionDto(
                src.PositionId, src.PosUnitCode, src.PosGradeId, src.PositionName,
                src.PositionDesignation, src.PosEffectiveDate, src.PosClosedDate,
                src.ReferenceCode, src.DeletedFlag, src.Ctc));

        CreateMap<Site, SiteDto>()
            .ConstructUsing(src => new SiteDto(
                src.SiteId, src.SiteName, src.SiteShortName, src.AddressLine1,
                src.AddressLine2, src.AddressPin, src.SiteCityCode, src.SiteCategoryCode,
                src.Phone1, src.LiveFlag != null ? src.LiveFlag.Value : null));
    }
}
