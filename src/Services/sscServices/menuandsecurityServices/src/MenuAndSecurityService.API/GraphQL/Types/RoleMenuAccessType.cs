using MenuAndSecurityService.Domain.Entities;

namespace MenuAndSecurityService.API.GraphQL.Types;

public class RoleMenuAccessType : ObjectType<RoleMenuAccess>
{
    protected override void Configure(IObjectTypeDescriptor<RoleMenuAccess> descriptor)
    {
        descriptor.Name("RoleMenuAccess");
        descriptor.Field(r => r.MenuAccessId).Type<NonNullType<LongType>>();
        descriptor.Field(r => r.MenuId).Type<NonNullType<LongType>>();
        descriptor.Field(r => r.MenuRoleId).Type<NonNullType<LongType>>();
        descriptor.Field(r => r.RoleModifiedBy).Type<LongType>();
        descriptor.Field(r => r.RoleModifiedOn).Type<DateTimeType>();
        descriptor.Field(r => r.Menu).Type<MenuType>();
        descriptor.Ignore(r => r.DomainEvents);
    }
}
