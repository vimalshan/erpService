using ActionService.Application.DTOs;

namespace ActionService.GraphQL.Types;

public class ActionType : ObjectType<ActionDto>
{
    protected override void Configure(IObjectTypeDescriptor<ActionDto> descriptor)
    {
        descriptor.Name("Action");
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.Action).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.DueDate).Type<DateTimeType>();
        descriptor.Field(f => f.HighPriority).Type<NonNullType<BooleanType>>();
        descriptor.Field(f => f.Message).Type<StringType>();
        descriptor.Field(f => f.Language).Type<StringType>();
        descriptor.Field(f => f.Service).Type<StringType>();
        descriptor.Field(f => f.Site).Type<StringType>();
        descriptor.Field(f => f.EntityType).Type<StringType>();
        descriptor.Field(f => f.EntityId).Type<IntType>();
        descriptor.Field(f => f.Subject).Type<StringType>();
        descriptor.Field(f => f.SnowLink).Type<StringType>();
    }
}
