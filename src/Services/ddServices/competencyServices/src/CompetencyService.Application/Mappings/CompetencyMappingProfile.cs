using AutoMapper;
using CompetencyService.Application.DTOs;
using CompetencyService.Domain.Entities;

namespace CompetencyService.Application.Mappings;

public class CompetencyMappingProfile : Profile
{
    public CompetencyMappingProfile()
    {
        CreateMap<CompetencyMaster, CompetencyDto>()
            .ConstructUsing(src => new CompetencyDto(
                src.Id, src.Name, src.EffectiveDate, src.ClosureDate,
                src.Remarks, src.JobCode, src.PositiveIndicator, src.NegativeIndicator,
                src.SelfDescription, src.CompetencyType, src.ParentId));

        CreateMap<CompetencyRatingScale, CompetencyRatingScaleDto>()
            .ConstructUsing(src => new CompetencyRatingScaleDto(
                src.CompetencyId, src.R1Desc, src.R2Desc, src.R3Desc, src.R4Desc, src.R5Desc));

        CreateMap<EmpSpecificCompetency, EmpSpecificCompetencyDto>()
            .ConstructUsing(src => new EmpSpecificCompetencyDto(
                src.EmpSysId, src.CompetencyId, src.CompetencyType, src.YearId,
                src.ModifiedBy, src.ModifiedOn));

        CreateMap<RoleSpecific, RoleSpecificDto>()
            .ConstructUsing(src => new RoleSpecificDto(
                src.EmpSysId, src.CompetencyId, src.EffFrom, src.EffTo));

        CreateMap<CompetencyIndicator, CompetencyIndicatorDto>()
            .ConstructUsing(src => new CompetencyIndicatorDto(
                src.SerialNo, src.Band, src.CompetencyNo, src.IndicatorFlag, src.IndicatorDefinition));
    }
}
