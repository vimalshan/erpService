using EnergyService.Application.DTOs;

namespace EnergyService.API.GraphQL.Types;

public class EcProcessMailIdType : ObjectType<EcProcessMailIdDto>
{
    protected override void Configure(IObjectTypeDescriptor<EcProcessMailIdDto> descriptor)
    {
        descriptor.Name("EcProcessMailId");
        descriptor.Field(f => f.PmId).Description("Mail config ID");
        descriptor.Field(f => f.PmProcessId).Description("Process ID");
        descriptor.Field(f => f.PmMailId).Description("Email address");
        descriptor.Field(f => f.PmDeliveryType).Description("Delivery type (TO/CC/BCC)");
        descriptor.Field(f => f.PmStartDate).Description("Start date");
        descriptor.Field(f => f.PmCloseDate).Description("Close date");
    }
}
