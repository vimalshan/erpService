using Dapper;
using Microsoft.Data.SqlClient;
using TourServices.Application.DTOs;

namespace TourServices.Infrastructure.DapperRepositories;

/// <summary>
/// Dapper-based read-optimised repository for complex tour queries.
/// </summary>
public sealed class TourPackageDapperRepository
{
    private readonly string _connectionString;

    public TourPackageDapperRepository(string connectionString) => _connectionString = connectionString;

    public async Task<IEnumerable<TourPackageDto>> GetTourSummaryAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT
                tp.TOUR_ID           AS TourId,
                tp.TOUR_NAME         AS TourName,
                tp.DESTINATION       AS Destination,
                tp.START_DATE        AS StartDate,
                tp.END_DATE          AS EndDate,
                tp.TOUR_PACKAGE_COST AS TourPackageCost,
                tp.MAX_PARTICIPANTS  AS MaxParticipants,
                tp.TOUR_STATUS       AS TourStatus,
                COUNT(CASE WHEN tr.REGISTRATION_STATUS = 'A' THEN 1 END) AS ActiveRegistrations,
                tp.CREATED_BY        AS CreatedBy,
                tp.CREATED_ON        AS CreatedOn,
                tp.MODIFIED_BY       AS ModifiedBy,
                tp.MODIFIED_ON       AS ModifiedOn
            FROM TOUR_PACKAGE tp
            LEFT JOIN TOUR_REGISTRATION tr ON tp.TOUR_ID = tr.TOUR_ID
            GROUP BY tp.TOUR_ID, tp.TOUR_NAME, tp.DESTINATION, tp.START_DATE,
                     tp.END_DATE, tp.TOUR_PACKAGE_COST, tp.MAX_PARTICIPANTS,
                     tp.TOUR_STATUS, tp.CREATED_BY, tp.CREATED_ON,
                     tp.MODIFIED_BY, tp.MODIFIED_ON
            ORDER BY tp.CREATED_ON DESC
            """;

        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<dynamic>(sql);

        return result.Select(r => new TourPackageDto(
            (long)r.TourId,
            (string)r.TourName,
            (string)r.Destination,
            DateOnly.FromDateTime((DateTime)r.StartDate),
            DateOnly.FromDateTime((DateTime)r.EndDate),
            (decimal)r.TourPackageCost,
            (int)r.MaxParticipants,
            (string)r.TourStatus,
            (int)r.ActiveRegistrations,
            (long)r.CreatedBy,
            (DateTime)r.CreatedOn,
            r.ModifiedBy is null ? null : (long?)r.ModifiedBy,
            r.ModifiedOn is null ? null : (DateTime?)r.ModifiedOn));
    }

    public async Task<decimal> GetCostPerPersonAsync(long tourId, int participantCount)
    {
        const string sql = """
            SELECT TOUR_PACKAGE_COST FROM TOUR_PACKAGE WHERE TOUR_ID = @TourId
            """;

        await using var connection = new SqlConnection(_connectionString);
        var cost = await connection.QueryFirstOrDefaultAsync<decimal>(sql, new { TourId = tourId });
        return participantCount > 0 ? Math.Round(cost / participantCount, 0) : 0;
    }
}
