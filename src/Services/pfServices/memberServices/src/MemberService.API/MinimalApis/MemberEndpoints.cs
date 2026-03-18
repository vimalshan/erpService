using MediatR;
using MemberService.Application.Commands.AddNominee;
using MemberService.Application.Commands.CloseMember;
using MemberService.Application.Commands.CreateMember;
using MemberService.Application.Queries.GetMember;
using MemberService.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MemberService.API.MinimalApis;

public static class MemberEndpoints
{
    public static IEndpointRouteBuilder MapMemberEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/members")
            .WithTags("Members (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async ([FromQuery] string? trustCode, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.Send(new GetAllMembersQuery(trustCode), ct)))
            .WithName("GetAllMembersV2")
            .WithSummary("Get all active members");

        group.MapGet("/{memberNo:long}", async (long memberNo, IMediator mediator, CancellationToken ct) =>
        {
            var member = await mediator.Send(new GetMemberQuery(memberNo), ct);
            return member is null ? Results.NotFound() : Results.Ok(member);
        })
        .WithName("GetMemberV2")
        .WithSummary("Get member profile by member number");

        group.MapPost("/", async ([FromBody] CreateMemberCommand cmd, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return Results.Created($"/api/v2/members/{result.MemberNo}", result);
        })
        .WithName("CreateMemberV2")
        .WithSummary("Enroll new member");

        group.MapPost("/{memberNo:long}/close",
            async (long memberNo, [FromBody] CloseMemberRequest req, IMediator mediator, CancellationToken ct) =>
            {
                await mediator.Send(new CloseMemberCommand(memberNo, req.LeaveReason, req.LeaveDate, 1), ct);
                return Results.NoContent();
            })
        .WithName("CloseMemberV2")
        .WithSummary("Close member account")
        .RequireAuthorization("AdminOrManager");

        group.MapPost("/{memberNo:long}/nominees",
            async (long memberNo, [FromBody] AddNomineeCommand cmd, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(cmd with { MemberNo = memberNo }, ct);
                return Results.Created($"/api/v2/members/{memberNo}", result);
            })
        .WithName("AddNomineeV2")
        .WithSummary("Add nominee to member");

        return app;
    }
}
