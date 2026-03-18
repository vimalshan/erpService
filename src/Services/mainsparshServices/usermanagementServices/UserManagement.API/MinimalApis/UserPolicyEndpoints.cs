using MediatR;
using UserManagement.Application.Features.UserPolicy.Commands.CreateUserPolicy;
using UserManagement.Application.Features.UserPolicy.Commands.DeleteUserPolicy;
using UserManagement.Application.Features.UserPolicy.Commands.UpdateUserPolicy;
using UserManagement.Application.Features.UserPolicy.Queries.GetAllUserPolicies;
using UserManagement.Application.Features.UserPolicy.Queries.GetUserPolicyById;
using UserManagement.Application.Features.WebsiteContact.Commands.CreateWebsiteContact;
using UserManagement.Application.Features.WebsiteContact.Queries;

namespace UserManagement.API.MinimalApis;

public static class UserPolicyEndpoints
{
    public static IEndpointRouteBuilder MapUserPolicyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/policies")
            .WithTags("UserPolicy-MinimalAPI")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, string? policyType, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllUserPoliciesQuery(policyType), ct);
            return Results.Ok(result);
        }).WithName("GetPoliciesV2").WithSummary("Get all user policies (Minimal API)");

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetUserPolicyByIdQuery(id), ct);
            return Results.Ok(result);
        }).WithName("GetPolicyByIdV2").WithSummary("Get policy by ID (Minimal API)");

        group.MapPost("/", async (CreateUserPolicyCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/policies/{result.PolicyId}", result);
        }).WithName("CreatePolicyV2").WithSummary("Create user policy (Minimal API)");

        group.MapPut("/{id:long}", async (long id, UpdateUserPolicyCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command with { PolicyId = id }, ct);
            return Results.Ok(result);
        }).WithName("UpdatePolicyV2").WithSummary("Update user policy (Minimal API)");

        group.MapDelete("/{id:long}", async (long id, long deletedBy, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new DeleteUserPolicyCommand(id, deletedBy), ct);
            return Results.NoContent();
        }).WithName("DeletePolicyV2").WithSummary("Deactivate user policy (Minimal API)");

        return app;
    }

    public static IEndpointRouteBuilder MapWebsiteContactEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/contacts")
            .WithTags("WebsiteContact-MinimalAPI")
            .RequireAuthorization();

        group.MapGet("/user/{userSysId:long}", async (long userSysId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetContactsByUserSysIdQuery(userSysId), ct);
            return Results.Ok(result);
        }).WithName("GetContactsByUserV2");

        group.MapPost("/", async (CreateWebsiteContactCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/contacts/{result.ContactId}", result);
        }).WithName("CreateContactV2");

        return app;
    }
}
