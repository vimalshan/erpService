using CompetencyService.Application.DTOs;
using CompetencyService.Infrastructure.DapperQueries;

namespace CompetencyService.API.GraphQL.Types;

public class CompetencyType : ObjectType<CompetencyDto>
{
    protected override void Configure(IObjectTypeDescriptor<CompetencyDto> descriptor)
    {
        descriptor.Description("Represents a competency master record.");
        descriptor.Field(f => f.Id).Description("Unique competency identifier.");
        descriptor.Field(f => f.Name).Description("Competency name.");
        descriptor.Field(f => f.CompetencyType).Description("Type: CORE, FUNC, BEHAV, etc.");
        descriptor.Field(f => f.EffectiveDate).Description("Effective from date.");
        descriptor.Field(f => f.ClosureDate).Description("Closure date (nullable).");
    }
}
