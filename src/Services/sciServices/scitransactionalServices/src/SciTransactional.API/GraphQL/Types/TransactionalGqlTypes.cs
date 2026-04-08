using SciTransactional.Application.DTOs;

namespace SciTransactional.API.GraphQL.Types;

public sealed class NavigationGqlType : ObjectType<NavigationDto>
{
    protected override void Configure(IObjectTypeDescriptor<NavigationDto> descriptor)
    {
        descriptor.Name("Navigation");
        descriptor.Field(f => f.RequestNum).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.UserId).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.UserNum).Type<LongType>();
        descriptor.Field(f => f.RandomNum).Type<StringType>();
        descriptor.Field(f => f.UpdatedDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(f => f.SciId).Type<StringType>();
        descriptor.Field(f => f.StatusFlag).Type<StringType>();
    }
}

public sealed class NormsMainGqlType : ObjectType<NormsMainDto>
{
    protected override void Configure(IObjectTypeDescriptor<NormsMainDto> descriptor)
    {
        descriptor.Name("NormsMain");
        descriptor.Field(f => f.NormNo).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.EffectiveDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(f => f.ClosureDate).Type<DateTimeType>();
        descriptor.Field(f => f.Details).Type<NonNullType<ListType<NonNullType<ObjectType<NormsMasterDto>>>>>();
    }
}

public sealed class NormsMasterGqlType : ObjectType<NormsMasterDto>
{
    protected override void Configure(IObjectTypeDescriptor<NormsMasterDto> descriptor)
    {
        descriptor.Name("NormsMaster");
        descriptor.Field(f => f.NormId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.InputCode).Type<IntType>();
        descriptor.Field(f => f.OutputCode).Type<IntType>();
        descriptor.Field(f => f.Rate).Type<IntType>();
        descriptor.Field(f => f.NormNo).Type<LongType>();
    }
}

public sealed class AdvanceLicenseGqlType : ObjectType<AdvanceLicenseDto>
{
    protected override void Configure(IObjectTypeDescriptor<AdvanceLicenseDto> descriptor)
    {
        descriptor.Name("AdvanceLicense");
        descriptor.Field(f => f.LicenseId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.LicenseNo).Type<StringType>();
        descriptor.Field(f => f.FgCode).Type<IntType>();
        descriptor.Field(f => f.ExportObligationAmount).Type<DecimalType>();
        descriptor.Field(f => f.ExportAmount).Type<DecimalType>();
        descriptor.Field(f => f.Entitlements).Type<NonNullType<ListType<NonNullType<ObjectType<EntitlementDto>>>>>();
    }
}

public sealed class EntitlementGqlType : ObjectType<EntitlementDto>
{
    protected override void Configure(IObjectTypeDescriptor<EntitlementDto> descriptor)
    {
        descriptor.Name("Entitlement");
        descriptor.Field(f => f.LicenseId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.EntitlementRm).Type<NonNullType<IntType>>();
    }
}

public sealed class AutoMailStatusGqlType : ObjectType<AutoMailStatusDto>
{
    protected override void Configure(IObjectTypeDescriptor<AutoMailStatusDto> descriptor)
    {
        descriptor.Name("AutoMailStatus");
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.MailType).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.MailDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(f => f.MailStatus).Type<StringType>();
        descriptor.Field(f => f.MailRemarks).Type<StringType>();
    }
}

public sealed class OrderMapGqlType : ObjectType<OrderMapDto>
{
    protected override void Configure(IObjectTypeDescriptor<OrderMapDto> descriptor)
    {
        descriptor.Name("OrderMap");
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.TiedOrderDetailId).Type<DecimalType>();
        descriptor.Field(f => f.ActualLineId).Type<DecimalType>();
        descriptor.Field(f => f.MappingQuantity).Type<IntType>();
        descriptor.Field(f => f.ModifiedByUserId).Type<IntType>();
        descriptor.Field(f => f.ModifiedDate).Type<DateTimeType>();
    }
}

public sealed class DirectEntryGqlType : ObjectType<DirectEntryDto>
{
    protected override void Configure(IObjectTypeDescriptor<DirectEntryDto> descriptor)
    {
        descriptor.Name("DirectEntry");
        descriptor.Field(f => f.Id).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.TrackingNumber).Type<LongType>();
        descriptor.Field(f => f.EnteredDate).Type<DateTimeType>();
        descriptor.Field(f => f.EnteredUser).Type<StringType>();
    }
}
