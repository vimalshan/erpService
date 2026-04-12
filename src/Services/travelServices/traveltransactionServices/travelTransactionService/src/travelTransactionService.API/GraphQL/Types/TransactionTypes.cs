using travelTransactionService.Application.DTOs;

namespace travelTransactionService.API.GraphQL.Types;

public class VendorMasterType : ObjectType<VendorMasterDto>
{
    protected override void Configure(IObjectTypeDescriptor<VendorMasterDto> descriptor)
    {
        descriptor.Name("VendorMaster");

        descriptor.Field(v => v.VendorId).Type<NonNullType<LongType>>();
        descriptor.Field(v => v.Name).Type<NonNullType<StringType>>();
        descriptor.Field(v => v.AddressLine1).Type<StringType>();
        descriptor.Field(v => v.AddressLine2).Type<StringType>();
        descriptor.Field(v => v.AddressLine3).Type<StringType>();
        descriptor.Field(v => v.CityCode).Type<LongType>();
        descriptor.Field(v => v.ItPanNumber).Type<StringType>();
        descriptor.Field(v => v.PhoneNumber).Type<StringType>();
        descriptor.Field(v => v.AccountNumber).Type<StringType>();
        descriptor.Field(v => v.BankName).Type<StringType>();
        descriptor.Field(v => v.CategoryType).Type<NonNullType<StringType>>();
    }
}

public class TaxMasterType : ObjectType<TaxMasterDto>
{
    protected override void Configure(IObjectTypeDescriptor<TaxMasterDto> descriptor)
    {
        descriptor.Name("TaxMaster");

        descriptor.Field(t => t.TaxVendorId).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.TaxType).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.TaxRate).Type<DecimalType>();
        descriptor.Field(t => t.TaxEffectiveDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(t => t.TaxCloseDate).Type<DateTimeType>();
        descriptor.Field(t => t.Components).Type<ListType<NonNullType<ObjectType<TaxComponentDto>>>>();
    }
}

public class JaiInterfaceLineType : ObjectType<JaiInterfaceLineDto>
{
    protected override void Configure(IObjectTypeDescriptor<JaiInterfaceLineDto> descriptor)
    {
        descriptor.Name("JaiInterfaceLine");

        descriptor.Field(j => j.InterfaceLineId).Type<DecimalType>();
        descriptor.Field(j => j.OrgId).Type<NonNullType<DecimalType>>();
        descriptor.Field(j => j.PartyId).Type<NonNullType<DecimalType>>();
        descriptor.Field(j => j.PartySiteId).Type<NonNullType<DecimalType>>();
        descriptor.Field(j => j.ImportModule).Type<NonNullType<StringType>>();
        descriptor.Field(j => j.TransactionNum).Type<NonNullType<StringType>>();
        descriptor.Field(j => j.TransactionLineNum).Type<NonNullType<DecimalType>>();
        descriptor.Field(j => j.ErrorFlag).Type<StringType>();
        descriptor.Field(j => j.ImportStatus).Type<StringType>();
        descriptor.Field(j => j.BatchId).Type<DecimalType>();
        descriptor.Field(j => j.InvoiceId).Type<DecimalType>();
        descriptor.Field(j => j.Type).Type<StringType>();
        descriptor.Field(j => j.SgstAmount).Type<DecimalType>();
        descriptor.Field(j => j.CgstAmount).Type<DecimalType>();
        descriptor.Field(j => j.IgstAmount).Type<DecimalType>();
        descriptor.Field(j => j.JvNumber).Type<LongType>();
        descriptor.Field(j => j.TaxLines).Type<ListType<NonNullType<ObjectType<JaiInterfaceTaxLineDto>>>>();
    }
}
