using MediatR;
using RequestServices.Application.Commands.ApproveRequest;
using RequestServices.Application.Commands.CancelRequest;
using RequestServices.Application.Commands.CreateRequest;
using RequestServices.Application.DTOs;

namespace RequestServices.API.GraphQL.Mutations;

public class RequestMutation
{
    [GraphQLDescription("Create a new training request.")]
    public async Task<RequestMainDto> CreateRequest(
        [Service] IMediator mediator,
        CreateRequestInput input,
        CancellationToken ct)
    {
        var command = new CreateRequestCommand(
            input.RequestId, input.EmployeeUser, input.RequestDate, input.SupervisorUser,
            input.TrainingNeed, input.CourseId, input.CourseDescription,
            input.StartDate, input.EndDate, input.BusinessBenefit, input.ExpectedCompetency);

        return await mediator.Send(command, ct);
    }

    [GraphQLDescription("Approve a training request.")]
    public async Task<bool> ApproveRequest(
        [Service] IMediator mediator,
        ApproveRequestInput input,
        CancellationToken ct)
    {
        var command = new ApproveRequestCommand(
            input.RequestId, input.SerialNumber,
            input.ApprovalNumber, input.ApprovalRemark, input.ApprovalUser);

        return await mediator.Send(command, ct);
    }

    [GraphQLDescription("Cancel a training request.")]
    public async Task<bool> CancelRequest(
        [Service] IMediator mediator,
        CancelRequestInput input,
        CancellationToken ct)
        => await mediator.Send(new CancelRequestCommand(input.RequestId, input.SerialNumber, input.Remark), ct);
}

public record CreateRequestInput(
    long   RequestId, string EmployeeUser, DateTime RequestDate, string SupervisorUser,
    string TrainingNeed, long CourseId, string CourseDescription,
    DateTime StartDate, DateTime EndDate, string BusinessBenefit, string ExpectedCompetency);

public record ApproveRequestInput(
    long RequestId, long SerialNumber, long ApprovalNumber, string ApprovalRemark, string ApprovalUser);

public record CancelRequestInput(long RequestId, long SerialNumber, string Remark);
