using ProxyModule.Application.DTOs;

namespace ProxyModule.API.GraphQL.Types;

public class ProxyRightGqlType : ObjectType<ProxyRightDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProxyRightDto> descriptor)
    {
        descriptor.Name("ProxyRight");

        descriptor.Field(f => f.ProxyId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.ProxyUserId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.DelegatedUserId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.ProxyStartDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(f => f.ProxyEndDate).Type<DateTimeType>();
        descriptor.Field(f => f.ProxyType).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.ProxyStatus).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Scope).Type<StringType>();
        descriptor.Field(f => f.Notes).Type<StringType>();
        descriptor.Field(f => f.CreatedBy).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.CreatedOn).Type<NonNullType<DateTimeType>>();
        descriptor.Field(f => f.UpdatedBy).Type<LongType>();
        descriptor.Field(f => f.UpdatedOn).Type<DateTimeType>();
        descriptor.Field(f => f.IsCurrentlyActive).Type<NonNullType<BooleanType>>();
    }
}
