using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ClubMembershipService.Application.DTOs;

namespace ClubMembershipService.Infrastructure.Dapper;

public class ClubDapperRepository
{
    private readonly string _connectionString;

    public ClubDapperRepository(string connectionString)
        => _connectionString = connectionString;

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<ClubDto>> GetAllClubsAsync()
    {
        const string sql = @"
            SELECT CLUB_ID as ClubId, CLUB_NAME as ClubName, CLUB_STATUS as Status,
                   CREATED_BY as CreatedBy, CREATED_ON as CreatedOn,
                   MODIFIED_BY as ModifiedBy, MODIFIED_ON as ModifiedOn
            FROM CLUB_MASTER
            ORDER BY CLUB_NAME";

        using var conn = CreateConnection();
        return await conn.QueryAsync<ClubDto>(sql);
    }

    public async Task<int> GetActiveClubCountAsync()
    {
        const string sql = "SELECT dbo.fn_GetActiveClubCount()";
        using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(sql);
    }

    public async Task<IEnumerable<MembershipDto>> GetMembershipsByClubAsync(long clubId)
    {
        const string sql = @"
            SELECT m.MEMBERSHIP_ID as MembershipId, m.CLUB_ID as ClubId,
                   c.CLUB_NAME as ClubName, m.MEMBER_ID as MemberId,
                   m.JOIN_DATE as JoinDate, m.MEMBERSHIP_FEE as MembershipFee,
                   m.MEMBERSHIP_STATUS as Status, m.CREATED_BY as CreatedBy,
                   m.CREATED_ON as CreatedOn, m.MODIFIED_BY as ModifiedBy, m.MODIFIED_ON as ModifiedOn
            FROM CLUB_MEMBERSHIP m
            INNER JOIN CLUB_MASTER c ON c.CLUB_ID = m.CLUB_ID
            WHERE m.CLUB_ID = @ClubId";

        using var conn = CreateConnection();
        return await conn.QueryAsync<MembershipDto>(sql, new { ClubId = clubId });
    }

    public async Task<long> CreateMembershipViaSpAsync(
        long clubId, long memberId, DateOnly joinDate, decimal? fee, long enrolledBy)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@p_ClubID", clubId);
        parameters.Add("@p_MemberID", memberId);
        parameters.Add("@p_JoinDate", joinDate.ToDateTime(TimeOnly.MinValue));
        parameters.Add("@p_MembershipFee", fee);
        parameters.Add("@p_EnrolledBy", enrolledBy);
        parameters.Add("@p_MembershipID", dbType: DbType.Int64, direction: ParameterDirection.Output);

        using var conn = CreateConnection();
        await conn.ExecuteAsync("dbo.usp_CreateClubMembership",
            parameters, commandType: CommandType.StoredProcedure);

        return parameters.Get<long>("@p_MembershipID");
    }

    public async Task<long> RecordActivityViaSpAsync(
        long clubId, string activityName, DateOnly activityDate, decimal? budget, long organizerId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@p_ClubID", clubId);
        parameters.Add("@p_ActivityName", activityName);
        parameters.Add("@p_ActivityDate", activityDate.ToDateTime(TimeOnly.MinValue));
        parameters.Add("@p_Budget", budget);
        parameters.Add("@p_OrganizerID", organizerId);
        parameters.Add("@p_ActivityID", dbType: DbType.Int64, direction: ParameterDirection.Output);

        using var conn = CreateConnection();
        await conn.ExecuteAsync("dbo.usp_RecordClubActivity",
            parameters, commandType: CommandType.StoredProcedure);

        return parameters.Get<long>("@p_ActivityID");
    }
}
