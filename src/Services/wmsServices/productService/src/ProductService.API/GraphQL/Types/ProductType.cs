using ProductService.Domain.Entities;

namespace ProductService.API.GraphQL.Types;

public class ProductType : ObjectType<Product>
{
    protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
    {
        descriptor.Field(p => p.ProductId).Type<NonNullType<IntType>>();
        descriptor.Field(p => p.Sku).Type<NonNullType<StringType>>();
        descriptor.Field(p => p.Name).Type<NonNullType<StringType>>();
        descriptor.Field(p => p.Description).Type<StringType>();
        descriptor.Field(p => p.CategoryId).Type<IntType>();
        descriptor.Field(p => p.UnitOfMeasure).Type<NonNullType<StringType>>();
        descriptor.Field(p => p.WeightPerUnit).Type<DecimalType>();
        descriptor.Field(p => p.VolumePerUnit).Type<DecimalType>();
        descriptor.Field(p => p.Price).Type<DecimalType>();
        descriptor.Field(p => p.ReorderPoint).Type<DecimalType>();
        descriptor.Field(p => p.ReorderQuantity).Type<DecimalType>();
        descriptor.Field(p => p.IsActive).Type<NonNullType<BooleanType>>();
        descriptor.Field(p => p.CreatedDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(p => p.ModifiedDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(p => p.Category).Type<CategoryType>();

        descriptor.Ignore(p => p.DomainEvents);
    }
}
