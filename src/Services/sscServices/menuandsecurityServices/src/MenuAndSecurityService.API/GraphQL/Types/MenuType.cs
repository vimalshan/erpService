using MenuAndSecurityService.Domain.Entities;

namespace MenuAndSecurityService.API.GraphQL.Types;

public class MenuType : ObjectType<MenuMaster>
{
    protected override void Configure(IObjectTypeDescriptor<MenuMaster> descriptor)
    {
        descriptor.Name("Menu");
        descriptor.Field(m => m.MenuId).Type<NonNullType<LongType>>();
        descriptor.Field(m => m.MenuName).Type<NonNullType<StringType>>();
        descriptor.Field(m => m.MenuPageName).Type<NonNullType<StringType>>();
        descriptor.Field(m => m.MenuParentId).Type<LongType>();
        descriptor.Field(m => m.MenuDisplayOrder).Type<NonNullType<IntType>>();
        descriptor.Field(m => m.ModifiedBy).Type<NonNullType<LongType>>();
        descriptor.Field(m => m.ModifiedOn).Type<NonNullType<DateTimeType>>();
        descriptor.Field(m => m.Children).Type<ListType<MenuType>>();
        descriptor.Field(m => m.RoleMenuAccesses).Type<ListType<RoleMenuAccessType>>();
        descriptor.Ignore(m => m.Parent);
        descriptor.Ignore(m => m.DomainEvents);
    }
}
