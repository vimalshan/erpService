using HotChocolate.Types;
using AdminService.Domain.Entities;

namespace AdminService.API.GraphQL.Types;

public class AdminMasterType : ObjectType<AdminMaster>
{
    protected override void Configure(IObjectTypeDescriptor<AdminMaster> descriptor)
    {
        descriptor.Field(f => f.AdminId).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.AdminName).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.AdminPic).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.AdminUnitId).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.AdminUnitHeadSysId).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.AdminLocStatus).Type<StringType>();
        descriptor.Field(f => f.UserMaps).Type<ListType<AdminUserMapType>>();
        descriptor.Field(f => f.AccessRights).Type<ListType<AdminAccessRightsType>>();

        // Ignore BaseEntity properties
        descriptor.Ignore(f => f.CreatedOn);
        descriptor.Ignore(f => f.CreatedBy);
        descriptor.Ignore(f => f.ModifiedOn);
        descriptor.Ignore(f => f.ModifiedBy);
        descriptor.Ignore(f => f.DomainEvents);
    }
}

public class AdminUserMapType : ObjectType<AdminUserMap>
{
    protected override void Configure(IObjectTypeDescriptor<AdminUserMap> descriptor)
    {
        descriptor.Field(f => f.AdminMapId).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.AdminBookType).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.AdminMode).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.AdminEmpSysId).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.AdminId).Type<NonNullType<StringType>>();
        descriptor.Ignore(f => f.Admin);
        descriptor.Ignore(f => f.CreatedOn);
        descriptor.Ignore(f => f.CreatedBy);
        descriptor.Ignore(f => f.ModifiedOn);
        descriptor.Ignore(f => f.ModifiedBy);
        descriptor.Ignore(f => f.DomainEvents);
    }
}

public class AdminAccessRightsType : ObjectType<AdminAccessRights>
{
    protected override void Configure(IObjectTypeDescriptor<AdminAccessRights> descriptor)
    {
        descriptor.Field(f => f.AdminRightsId).Type<NonNullType<StringType>>();
        descriptor.Ignore(f => f.Admin);
        descriptor.Ignore(f => f.CreatedOn);
        descriptor.Ignore(f => f.CreatedBy);
        descriptor.Ignore(f => f.ModifiedOn);
        descriptor.Ignore(f => f.ModifiedBy);
        descriptor.Ignore(f => f.DomainEvents);
    }
}

public class AdminFinUserMapType : ObjectType<AdminFinUserMap>
{
    protected override void Configure(IObjectTypeDescriptor<AdminFinUserMap> descriptor)
    {
        descriptor.Field(f => f.FinanceMapId).Type<NonNullType<StringType>>();
        descriptor.Ignore(f => f.CreatedOn);
        descriptor.Ignore(f => f.CreatedBy);
        descriptor.Ignore(f => f.ModifiedOn);
        descriptor.Ignore(f => f.ModifiedBy);
        descriptor.Ignore(f => f.DomainEvents);
    }
}

public class AdminAccessRightsLogType : ObjectType<AdminAccessRightsLog>
{
    protected override void Configure(IObjectTypeDescriptor<AdminAccessRightsLog> descriptor)
    {
        descriptor.Field(f => f.AdminLogId).Type<NonNullType<StringType>>();
        descriptor.Ignore(f => f.AccessRights);
        descriptor.Ignore(f => f.CreatedOn);
        descriptor.Ignore(f => f.CreatedBy);
        descriptor.Ignore(f => f.ModifiedOn);
        descriptor.Ignore(f => f.ModifiedBy);
        descriptor.Ignore(f => f.DomainEvents);
    }
}
