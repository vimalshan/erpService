using MeetingModule.Infrastructure.Persistence.Dapper;

namespace MeetingModule.API.MinimalApis;

public static class MeetingMinimalApiEndpoints
{
    public static void MapMeetingMinimalApis(this WebApplication app)
    {
        var group = app.MapGroup("/api/v2/meetings").WithTags("Meetings (Minimal API)");

        group.MapGet("/upcoming", async (IDapperQueryService dapper, int? top) =>
        {
            var result = await dapper.GetUpcomingMeetingsAsync(top ?? 50);
            return Results.Ok(result);
        })
        .WithName("GetUpcomingMeetings");

        group.MapGet("/{id:long}/detail", async (IDapperQueryService dapper, long id) =>
        {
            var result = await dapper.GetMeetingDetailAsync(id);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetMeetingDetail");

        group.MapGet("/types", async (IDapperQueryService dapper) =>
        {
            var result = await dapper.GetMeetingTypesWithCountsAsync();
            return Results.Ok(result);
        })
        .WithName("GetMeetingTypesMinimal");
    }
}
