using HotChocolate.Types;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.API.GraphQL.Types;

public class ProductionPlantType : ObjectType<ProductionPlant>
{
    protected override void Configure(IObjectTypeDescriptor<ProductionPlant> descriptor)
    {
        descriptor.Description("Represents a production manufacturing plant.");

        descriptor.Field(f => f.ProductionPlantId).Description("The unique identifier.");
        descriptor.Field(f => f.CompanyUnitId).Description("The company unit ID.");
        descriptor.Field(f => f.PlantName).Description("The plant name.");
        descriptor.Field(f => f.Location).Description("The plant location.");
        descriptor.Field(f => f.ProductionPlans).Description("Associated production plans.");
        descriptor.Field(f => f.ProductMaps).Description("Associated product mappings.");

        descriptor.Ignore(f => f.DomainEvents);
    }
}

public class ProductionPlanType : ObjectType<ProductionPlan>
{
    protected override void Configure(IObjectTypeDescriptor<ProductionPlan> descriptor)
    {
        descriptor.Description("Represents a production plan for a specific item at a plant.");

        descriptor.Field(f => f.ProductionPlantId).Description("The plant identifier.");
        descriptor.Field(f => f.SciItemId).Description("The item identifier.");
        descriptor.Field(f => f.QtyPerDay).Description("Quantity planned per day.");
        descriptor.Field(f => f.PlanStartDate).Description("Plan start date.");
        descriptor.Field(f => f.PlanClosureDate).Description("Plan closure date.");

        descriptor.Ignore(f => f.DomainEvents);
    }
}

public class NormsMainType : ObjectType<NormsMain>
{
    protected override void Configure(IObjectTypeDescriptor<NormsMain> descriptor)
    {
        descriptor.Description("Represents production norms.");

        descriptor.Field(f => f.NormNo).Description("The norm number.");
        descriptor.Field(f => f.NormEffDate).Description("Effective date.");
        descriptor.Field(f => f.NormClsDate).Description("Closing date.");
        descriptor.Field(f => f.NormsMasters).Description("Associated norms masters.");

        descriptor.Ignore(f => f.DomainEvents);
    }
}
