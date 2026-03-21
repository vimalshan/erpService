using WarehouseStructure.Application.DTOs;

namespace WarehouseStructure.API.GraphQL.Types;

public class WarehouseGqlType : ObjectType<WarehouseDto>
{
    protected override void Configure(IObjectTypeDescriptor<WarehouseDto> descriptor)
    {
        descriptor.Name("Warehouse");
        descriptor.Field(w => w.WarehouseId).Type<NonNullType<IntType>>();
        descriptor.Field(w => w.Code).Type<NonNullType<StringType>>();
        descriptor.Field(w => w.Name).Type<NonNullType<StringType>>();
        descriptor.Field(w => w.Address).Type<StringType>();
        descriptor.Field(w => w.City).Type<StringType>();
        descriptor.Field(w => w.State).Type<StringType>();
        descriptor.Field(w => w.Country).Type<StringType>();
        descriptor.Field(w => w.PostalCode).Type<StringType>();
        descriptor.Field(w => w.Phone).Type<StringType>();
        descriptor.Field(w => w.Email).Type<StringType>();
        descriptor.Field(w => w.IsActive).Type<NonNullType<BooleanType>>();
        descriptor.Field(w => w.Zones).Type<ListType<ZoneGqlType>>();
    }
}
