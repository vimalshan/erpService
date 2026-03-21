using ProductService.Domain.Entities;

namespace ProductService.API.GraphQL.Types;

public class CategoryType : ObjectType<Category>
{
    protected override void Configure(IObjectTypeDescriptor<Category> descriptor)
    {
        descriptor.Field(c => c.CategoryId).Type<NonNullType<IntType>>();
        descriptor.Field(c => c.CategoryName).Type<NonNullType<StringType>>();
        descriptor.Field(c => c.ParentCategoryId).Type<IntType>();
        descriptor.Field(c => c.Description).Type<StringType>();
        descriptor.Field(c => c.ParentCategory).Type<CategoryType>();
        descriptor.Field(c => c.SubCategories).Type<ListType<CategoryType>>();
        descriptor.Field(c => c.Products).Type<ListType<ProductType>>();

        descriptor.Ignore(c => c.DomainEvents);
    }
}
