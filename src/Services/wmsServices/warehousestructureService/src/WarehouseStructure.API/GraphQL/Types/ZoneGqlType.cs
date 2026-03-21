using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.API.GraphQL.Types;

public class ZoneGqlType : ObjectType<ZoneDto>
{
    protected override void Configure(IObjectTypeDescriptor<ZoneDto> descriptor)
    {
        descriptor.Name("Zone");
        descriptor.Field(z => z.ZoneId).Type<NonNullType<IntType>>();
        descriptor.Field(z => z.WarehouseId).Type<NonNullType<IntType>>();
        descriptor.Field(z => z.Code).Type<NonNullType<StringType>>();
        descriptor.Field(z => z.Name).Type<NonNullType<StringType>>();
        descriptor.Field(z => z.ZoneType).Type<NonNullType<StringType>>();
        descriptor.Field(z => z.Description).Type<StringType>();
        descriptor.Field(z => z.IsActive).Type<NonNullType<BooleanType>>();
    }
}
