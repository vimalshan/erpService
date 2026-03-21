using FinanceService.Domain.Entities;
using HotChocolate.Types;

namespace FinanceService.API.GraphQL.Types;

public class InvoiceType : ObjectType<ApInvoice>
{
    protected override void Configure(IObjectTypeDescriptor<ApInvoice> descriptor)
    {
        descriptor.Description("Represents an AP Invoice.");
        descriptor.BindFieldsExplicitly();
        descriptor.Field(f => f.InvoiceId);
        descriptor.Field(f => f.InvoiceNum);
        descriptor.Field(f => f.InvoiceTypeLookupCode);
        descriptor.Field(f => f.InvoiceDate);
        descriptor.Field(f => f.VendorId);
        descriptor.Field(f => f.VendorSiteId);
        descriptor.Field(f => f.InvoiceAmount);
        descriptor.Field(f => f.InvoiceCurrencyCode);
        descriptor.Field(f => f.ExchangeRate);
        descriptor.Field(f => f.ExchangeRateType);
        descriptor.Field(f => f.TermsId);
        descriptor.Field(f => f.Description);
        descriptor.Field(f => f.Status);
        descriptor.Field(f => f.AgencyId);
        descriptor.Field(f => f.InvoiceLines);
    }
}

public class InvoiceLineType : ObjectType<ApInvoiceLine>
{
    protected override void Configure(IObjectTypeDescriptor<ApInvoiceLine> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(f => f.InvoiceId);
        descriptor.Field(f => f.LineNumber);
        descriptor.Field(f => f.LineTypeLookupCode);
        descriptor.Field(f => f.Amount);
        descriptor.Field(f => f.Description);
        descriptor.Field(f => f.AccountCode);
        descriptor.Field(f => f.ProjectCode);
        descriptor.Field(f => f.SgstAmt);
        descriptor.Field(f => f.CgstAmt);
        descriptor.Field(f => f.IgstAmt);
    }
}
