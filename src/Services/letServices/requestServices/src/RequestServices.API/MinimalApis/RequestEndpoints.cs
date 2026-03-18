using MediatR;
using RequestServices.Application.Commands.ApproveRequest;
using RequestServices.Application.Commands.CancelRequest;
using RequestServices.Application.Commands.CreateRequest;
using RequestServices.Application.DTOs;
using RequestServices.Application.Queries.GetPendingRequests;
using RequestServices.Application.Queries.GetRequestById;

namespace RequestServices.API.MinimalApis;

/// <summary>Minimal API endpoints providing an alternative to controller-based routing.</summary>
public static class RequestEndpoints
{
    public static WebApplication MapRequestEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/requests")
            .WithTags("Requests (Minimal API)")
            .RequireAuthorization()
            .WithOpenApi();

        group.MapGet("/{requestId:long}", async (
            long requestId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRequestByIdQuery(requestId), ct);
            return Results.Ok(result);
        })
        .WithName("GetRequestMinimal")
        .WithSummary("Get training request by ID")
        .Produces<RequestMainDto>();

        group.MapGet("/pending/{supervisorUser}", async (
            string supervisorUser, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPendingRequestsQuery(supervisorUser), ct);
            return Results.Ok(result);
        })
        .WithName("GetPendingRequestsMinimal")
        .WithSummary("Get pending requests for supervisor")
        .Produces<IEnumerable<PendingRequestDto>>();

        group.MapPost("/", async (
            CreateRequestDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new CreateRequestCommand(
                dto.RequestId, dto.EmployeeUser, dto.RequestDate, dto.SupervisorUser,
                dto.TrainingNeed, dto.CourseId, dto.CourseDescription,
                dto.StartDate, dto.EndDate, dto.BusinessBenefit, dto.ExpectedCompetency);

            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/requests/{result.RequestId}", result);
        })
        .WithName("CreateRequestMinimal")
        .WithSummary("Create a new training request")
        .Produces<RequestMainDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapPost("/{requestId:long}/approve", async (
            long requestId, ApproveRequestDto dto, IMediator mediator, CancellationToken ct) =>
        {
            var command = new ApproveRequestCommand(
                requestId, dto.SerialNumber, dto.ApprovalNumber, dto.ApprovalRemark, dto.ApprovalUser);
            await mediator.Send(command, ct);
            return Results.Ok(new { message = "Approved." });
        })
        .WithName("ApproveRequestMinimal")
        .WithSummary("Approve a training request");

        group.MapPost("/{requestId:long}/cancel", async (
            long requestId, CancelRequestDto dto, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new CancelRequestCommand(requestId, dto.SerialNumber, dto.Remark), ct);
            return Results.Ok(new { message = "Cancelled." });
        })
        .WithName("CancelRequestMinimal")
        .WithSummary("Cancel a training request");

        return app;
    }
}
