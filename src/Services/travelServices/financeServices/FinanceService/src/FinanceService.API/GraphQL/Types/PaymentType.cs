using FinanceService.Domain.Entities;
using HotChocolate.Types;

namespace FinanceService.API.GraphQL.Types;

public class PaymentType : ObjectType<TravelAccount>
{
    protected override void Configure(IObjectTypeDescriptor<TravelAccount> descriptor)
    {
        descriptor.Description("Represents a Travel Account payment entry.");
        descriptor.BindFieldsExplicitly();
        descriptor.Field(f => f.TransactionNumber);
        descriptor.Field(f => f.UnitCode);
        descriptor.Field(f => f.UserCode);
        descriptor.Field(f => f.DebitCreditFlag);
        descriptor.Field(f => f.TransactionAmount);
        descriptor.Field(f => f.AccountCode);
        descriptor.Field(f => f.Remarks);
        descriptor.Field(f => f.AccountType);
        descriptor.Field(f => f.JvPostingStatus);
    }
}

public class PaymentTermType : ObjectType<PaymentTerm>
{
    protected override void Configure(IObjectTypeDescriptor<PaymentTerm> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(f => f.TermId);
        descriptor.Field(f => f.Name);
        descriptor.Field(f => f.EnabledFlag);
        descriptor.Field(f => f.DueCutoffDay);
        descriptor.Field(f => f.Description);
    }
}

public class JvPostingDetailType : ObjectType<JvPostingDetail>
{
    protected override void Configure(IObjectTypeDescriptor<JvPostingDetail> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(f => f.JvIntCode);
        descriptor.Field(f => f.JvDocNum);
        descriptor.Field(f => f.CompanyCode);
        descriptor.Field(f => f.GradeType);
        descriptor.Field(f => f.StartDate);
        descriptor.Field(f => f.EndDate);
        descriptor.Field(f => f.Comment);
        descriptor.Field(f => f.Status);
    }
}
