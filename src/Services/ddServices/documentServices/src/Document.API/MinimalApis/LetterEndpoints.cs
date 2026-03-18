using MediatR;
using Document.Application.DTOs;
using Document.Application.Features.LetterLog.Commands;

namespace Document.API.MinimalApis;

public static class LetterEndpoints
{
    public static IEndpointRouteBuilder MapLetterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/letters")
            .WithTags("Letters")
            .RequireAuthorization();

        group.MapPost("/log", async (LogLetterOpenRequest req, IMediator mediator, HttpContext ctx, CancellationToken ct) =>
        {
            var ip = ctx.Connection.RemoteIpAddress?.ToString() ?? req.IpAddress;
            var result = await mediator.Send(new LogLetterOpenCommand(
                req.LogSysId, ip, req.EmployeeSysId, req.LetterType, req.FinancialYearId), ct);
            return Results.Created($"/api/letters/log/{result.LogSysId}", result);
        })
        .WithName("LogLetterOpen")
        .Produces<LetterLogHistoryDto>(StatusCodes.Status201Created);

        group.MapGet("/types", () => Results.Ok(new[] { "APR", "AN1", "AN2" }))
            .WithName("GetLetterTypes")
            .AllowAnonymous();

        return app;
    }
}
