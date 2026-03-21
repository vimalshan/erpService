using HotChocolate;
using HotChocolate.Data;
using TourPlanService.Domain.Entities;

namespace TourPlanService.API.GraphQL.Types;

public sealed class TourPlanType : ObjectType<TourPlan>
{
    protected override void Configure(IObjectTypeDescriptor<TourPlan> descriptor)
    {
        descriptor.Description("Represents a Tour Plan record.");
        descriptor.Field(x => x.TpId).Description("Tour Plan ID");
        descriptor.Field(x => x.TpEmpSysId).Description("Employee System ID");
        descriptor.Field(x => x.TpStatus).Description("Current status");
        descriptor.Field(x => x.TpCategory).Description("DOM or INT");
        descriptor.Field(x => x.TpStartDate).Description("Travel start date");
        descriptor.Field(x => x.TpEndDate).Description("Travel end date");
        descriptor.Field(x => x.TpPurpose).Description("Purpose of travel");
        descriptor.Field(x => x.TpFromCityName).Description("Departure city");
        descriptor.Field(x => x.TpToCityName).Description("Destination city");
        descriptor.Field(x => x.Advances).Description("Tour advances");
        descriptor.Field(x => x.Agendas).Description("Tour agendas");
    }
}

public sealed class ForexRequisitionType : ObjectType<ForexRequisition>
{
    protected override void Configure(IObjectTypeDescriptor<ForexRequisition> descriptor)
    {
        // ForexRequisition-specific GraphQL type configuration
    }
}
