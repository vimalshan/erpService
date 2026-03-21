using ExpenseService.Application.DTOs;

namespace ExpenseService.API.GraphQL.Types;

public class DaSummaryType : ObjectType<DaSummaryDto>
{
    protected override void Configure(IObjectTypeDescriptor<DaSummaryDto> descriptor)
    {
        descriptor.Name("DASummary");
        descriptor.Field(d => d.RequestId).Description("Request ID");
        descriptor.Field(d => d.AdminAmount).Description("Admin DA amount");
        descriptor.Field(d => d.SelfAmount).Description("Self DA amount");
        descriptor.Field(d => d.TotalAmount).Description("Total DA amount");
    }
}
