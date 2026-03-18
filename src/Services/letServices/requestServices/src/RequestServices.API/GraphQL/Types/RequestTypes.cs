using RequestServices.Application.DTOs;

namespace RequestServices.API.GraphQL.Types;

public class RequestMainType : ObjectType<RequestMainDto>
{
    protected override void Configure(IObjectTypeDescriptor<RequestMainDto> descriptor)
    {
        descriptor.Description("A training request header record.");

        descriptor.Field(f => f.RequestId)     .Description("Unique request identifier.");
        descriptor.Field(f => f.EmployeeUser)  .Description("Employee who raised the request.");
        descriptor.Field(f => f.RequestDate)   .Description("Date the request was created.");
        descriptor.Field(f => f.SupervisorUser).Description("Supervisor responsible for approval.");
        descriptor.Field(f => f.SubRequests)   .Description("Line items of the request.");
    }
}

public class RequestSubType : ObjectType<RequestSubDto>
{
    protected override void Configure(IObjectTypeDescriptor<RequestSubDto> descriptor)
    {
        descriptor.Description("A training request line item.");
    }
}

public class PendingRequestType : ObjectType<PendingRequestDto>
{
    protected override void Configure(IObjectTypeDescriptor<PendingRequestDto> descriptor)
    {
        descriptor.Description("A pending training request summary.");
    }
}
