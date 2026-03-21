using UnitService.Application.DTOs;

namespace UnitService.API.GraphQL.Types;

public class EquipmentType : ObjectType<EquipmentDto>
{
    protected override void Configure(IObjectTypeDescriptor<EquipmentDto> descriptor)
    {
        descriptor.Name("Equipment");
        descriptor.Field(f => f.EquipmentId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.EquipmentName).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.UnitCode).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Category).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.StartDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(f => f.CloseDate).Type<DateTimeType>();
    }
}
