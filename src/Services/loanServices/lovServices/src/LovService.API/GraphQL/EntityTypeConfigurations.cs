using LovService.Domain.Entities;

namespace LovService.API.GraphQL;

// ObjectType configurations prevent HotChocolate from exposing the
// DomainEvents (IReadOnlyCollection<IDomainEvent>) property which HC
// cannot map to a GraphQL type. LovCategory is a value-object with
// a char Value; we expose it as a plain String instead.

public class LovTypeMastType : ObjectType<LovTypeMast>
{
    protected override void Configure(IObjectTypeDescriptor<LovTypeMast> descriptor)
    {
        descriptor.Ignore(t => t.DomainEvents);
        descriptor.Field(t => t.LovCategory)
            .Type<StringType>()
            .Resolve(ctx => ctx.Parent<LovTypeMast>().LovCategory.Value.ToString());
    }
}

public class LovMasterType : ObjectType<LovMaster>
{
    protected override void Configure(IObjectTypeDescriptor<LovMaster> descriptor)
    {
        descriptor.Ignore(t => t.DomainEvents);
    }
}

public class ProgramLovMastType : ObjectType<ProgramLovMast>
{
    protected override void Configure(IObjectTypeDescriptor<ProgramLovMast> descriptor)
    {
        descriptor.Ignore(t => t.DomainEvents);
    }
}
