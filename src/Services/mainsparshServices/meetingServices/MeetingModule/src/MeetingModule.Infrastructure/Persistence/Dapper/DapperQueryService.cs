using System.Data;
using Dapper;
using MeetingModule.Application.DTOs;
using Microsoft.Data.SqlClient;

namespace MeetingModule.Infrastructure.Persistence.Dapper;

public interface IDapperQueryService
{
    Task<IReadOnlyList<MeetingScheduleDto>> GetUpcomingMeetingsAsync(int top = 50, CancellationToken ct = default);
    Task<IReadOnlyList<MeetingTypeDto>> GetMeetingTypesWithCountsAsync(CancellationToken ct = default);
    Task<MeetingScheduleDto?> GetMeetingDetailAsync(long meetingId, CancellationToken ct = default);
}

public class DapperQueryService(string connectionString) : IDapperQueryService
{
    private IDbConnection CreateConnection() => new SqlConnection(connectionString);

    // Flat row without nested Polls — safe for Dapper construction
    private record MeetingRow(
        long MeetingId, long MeetTypeId, string? MeetTypeName, string MeetingTitle,
        DateTime MeetingDate, string? MeetingLocation, int? MeetingDuration, long OrganizerId,
        string MeetingStatus, string? Notes, long CreatedBy, DateTime CreatedOn);

    private static MeetingScheduleDto ToDto(MeetingRow r, List<PollDetailDto>? polls = null) =>
        new(r.MeetingId, r.MeetTypeId, r.MeetTypeName, r.MeetingTitle,
            r.MeetingDate, r.MeetingLocation, r.MeetingDuration, r.OrganizerId,
            r.MeetingStatus, r.Notes, r.CreatedBy, r.CreatedOn, polls);

    public async Task<IReadOnlyList<MeetingScheduleDto>> GetUpcomingMeetingsAsync(int top = 50, CancellationToken ct = default)
    {
        const string sql = """
            SELECT TOP(@Top)
                m.MEETING_ID AS MeetingId, m.MEETTYPE_ID AS MeetTypeId,
                t.MEETTYPE_NAME AS MeetTypeName, m.MEETING_TITLE AS MeetingTitle,
                m.MEETING_DATE AS MeetingDate, m.MEETING_LOCATION AS MeetingLocation,
                m.MEETING_DURATION AS MeetingDuration, m.ORGANIZER_ID AS OrganizerId,
                m.MEETING_STATUS AS MeetingStatus, m.NOTES AS Notes,
                m.CREATED_BY AS CreatedBy, m.CREATED_ON AS CreatedOn
            FROM SRF_MEETINGSCH m
            INNER JOIN MEETTYPE_MAST t ON t.MEETTYPE_ID = m.MEETTYPE_ID
            WHERE m.MEETING_STATUS = 'SCHEDULED' AND m.MEETING_DATE > GETDATE()
            ORDER BY m.MEETING_DATE
            """;

        using var connection = CreateConnection();
        var result = await connection.QueryAsync<MeetingRow>(sql, new { Top = top });
        return result.Select(r => ToDto(r)).ToList();
    }

    public async Task<IReadOnlyList<MeetingTypeDto>> GetMeetingTypesWithCountsAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT t.MEETTYPE_ID AS MeetTypeId, t.MEETTYPE_CODE AS MeetTypeCode,
                   t.MEETTYPE_NAME AS MeetTypeName, t.MEETTYPE_DESC AS MeetTypeDesc,
                   t.MEETTYPE_STATUS AS MeetTypeStatus,
                   t.CREATED_BY AS CreatedBy, t.CREATED_ON AS CreatedOn
            FROM MEETTYPE_MAST t
            ORDER BY t.MEETTYPE_NAME
            """;

        using var connection = CreateConnection();
        var result = await connection.QueryAsync<MeetingTypeDto>(sql);
        return result.ToList();
    }

    public async Task<MeetingScheduleDto?> GetMeetingDetailAsync(long meetingId, CancellationToken ct = default)
    {
        const string sql = """
            SELECT m.MEETING_ID AS MeetingId, m.MEETTYPE_ID AS MeetTypeId,
                   t.MEETTYPE_NAME AS MeetTypeName, m.MEETING_TITLE AS MeetingTitle,
                   m.MEETING_DATE AS MeetingDate, m.MEETING_LOCATION AS MeetingLocation,
                   m.MEETING_DURATION AS MeetingDuration, m.ORGANIZER_ID AS OrganizerId,
                   m.MEETING_STATUS AS MeetingStatus, m.NOTES AS Notes,
                   m.CREATED_BY AS CreatedBy, m.CREATED_ON AS CreatedOn
            FROM SRF_MEETINGSCH m
            INNER JOIN MEETTYPE_MAST t ON t.MEETTYPE_ID = m.MEETTYPE_ID
            WHERE m.MEETING_ID = @MeetingId;

            SELECT p.POLL_ID AS PollId, p.MEETING_ID AS MeetingId,
                   p.POLL_QUESTION AS PollQuestion, p.POLL_TYPE AS PollType,
                   p.POLL_STATUS AS PollStatus,
                   p.CREATED_BY AS CreatedBy, p.CREATED_ON AS CreatedOn
            FROM SRF_POLL_DETAIL p
            WHERE p.MEETING_ID = @MeetingId;
            """;

        using var connection = CreateConnection();
        using var multi = await connection.QueryMultipleAsync(sql, new { MeetingId = meetingId });

        var row = await multi.ReadFirstOrDefaultAsync<MeetingRow>();
        if (row is null) return null;

        var polls = (await multi.ReadAsync<PollDetailDto>()).ToList();
        return ToDto(row, polls);
    }
}
