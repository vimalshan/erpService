using AutoMapper;
using UtilityService.Application.DTOs;
using UtilityService.Domain.Entities;

namespace UtilityService.Application.Mappings;

public class ToadPlanSqlMappingProfile : Profile
{
    public ToadPlanSqlMappingProfile()
    {
        CreateMap<ToadPlanSql, ToadPlanSqlDto>()
            .ForMember(d => d.StatementId, o => o.MapFrom(s => s.StatementId.Value));

        CreateMap<CreateToadPlanSqlDto, ToadPlanSql>()
            .ConstructUsing(src => ToadPlanSql.Create(src.Username, src.StatementId, src.Statement, src.Timestamp));
    }
}
