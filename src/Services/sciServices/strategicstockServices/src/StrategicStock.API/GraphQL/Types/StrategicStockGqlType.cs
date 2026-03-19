using StrategicStock.Application.DTOs;

namespace StrategicStock.API.GraphQL.Types;

public sealed class StrategicStockGqlType : ObjectType<StrategicStockDto>
{
    protected override void Configure(IObjectTypeDescriptor<StrategicStockDto> descriptor)
    {
        descriptor.Name("StrategicStock");
        descriptor.Field(f => f.StrategicStockId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.CompanyUnitId).Type<IntType>();
        descriptor.Field(f => f.SciItemId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.StrategicStockType).Type<StringType>();
        descriptor.Field(f => f.MaxQty).Type<LongType>();
        descriptor.Field(f => f.EffectiveDate).Type<StringType>();
        descriptor.Field(f => f.ClosureDate).Type<StringType>();
        descriptor.Field(f => f.FilledQty).Type<LongType>();
    }
}
