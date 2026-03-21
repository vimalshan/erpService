using OrderService.Application.DTOs;

namespace OrderService.API.GraphQL.Types;

public class OrderType : ObjectType<OrderDto>
{
    protected override void Configure(IObjectTypeDescriptor<OrderDto> descriptor)
    {
        descriptor.Name("Order");
        descriptor.Field(o => o.OrderId).Type<NonNullType<IntType>>();
        descriptor.Field(o => o.OrderNumber).Type<NonNullType<StringType>>();
        descriptor.Field(o => o.CustomerId).Type<NonNullType<IntType>>();
        descriptor.Field(o => o.Status).Type<NonNullType<StringType>>();
        descriptor.Field(o => o.TotalAmount).Type<NonNullType<DecimalType>>();
        descriptor.Field(o => o.Items).Type<NonNullType<ListType<NonNullType<ObjectType<OrderItemDto>>>>>();
    }
}

public class OrderItemType : ObjectType<OrderItemDto>
{
    protected override void Configure(IObjectTypeDescriptor<OrderItemDto> descriptor)
    {
        descriptor.Name("OrderItem");
        descriptor.Field(i => i.OrderItemId).Type<NonNullType<IntType>>();
        descriptor.Field(i => i.ProductId).Type<NonNullType<IntType>>();
        descriptor.Field(i => i.Quantity).Type<NonNullType<IntType>>();
        descriptor.Field(i => i.UnitPrice).Type<NonNullType<DecimalType>>();
        descriptor.Field(i => i.LineTotal).Type<NonNullType<DecimalType>>();
    }
}
