using UnitService.Application.DTOs;

namespace UnitService.API.GraphQL.Types;

public class EquipmentStatusType : ObjectType<EquipmentStatusDto>
{
    protected override void Configure(IObjectTypeDescriptor<EquipmentStatusDto> descriptor)
    {
        descriptor.Name("EquipmentStatus");
        descriptor.Field(f => f.StatusId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.EquipmentId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.StatusDescription).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.StatusCode).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Remarks).Type<StringType>();
        descriptor.Field(f => f.Hours).Type<LongType>();
    }
}
