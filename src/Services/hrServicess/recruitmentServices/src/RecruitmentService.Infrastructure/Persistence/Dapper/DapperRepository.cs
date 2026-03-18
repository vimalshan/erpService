using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RecruitmentService.Infrastructure.Persistence.Dapper;

public class DapperRepository
{
    private readonly string _connectionString;

    public DapperRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
    {
        await using var conn = CreateConnection();
        return await conn.QueryAsync<T>(sql, param);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
    {
        await using var conn = CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null)
    {
        await using var conn = CreateConnection();
        return await conn.ExecuteAsync(sql, param);
    }

    /// <summary>Search open vacancies with filters via raw SQL for performance-sensitive queries.</summary>
    public async Task<IEnumerable<dynamic>> SearchVacanciesAsync(
        string? unit = null, string? designation = null, decimal? locationId = null)
    {
        const string sql = """
            SELECT VACANCY_ID, VACANCY_NAME, VACANCY_DESIGNATION, VACANCY_UNIT,
                   VACANCY_LOCATION, VACANCY_NOS, VACANCY_CTCFROM, VACANCY_CTCTO,
                   VACANCY_LASTDATE, VACANCY_POSTDATE
            FROM   VACANCY_MAIN
            WHERE  VACANCY_LIVESTATUS = 'Y'
              AND  (@Unit        IS NULL OR VACANCY_UNIT        = @Unit)
              AND  (@Designation IS NULL OR VACANCY_DESIGNATION LIKE '%' + @Designation + '%')
              AND  (@LocationId  IS NULL OR VACANCY_LOCATION    = @LocationId)
            ORDER BY VACANCY_POSTDATE DESC
            """;

        return await QueryAsync<dynamic>(sql, new { Unit = unit, Designation = designation, LocationId = locationId });
    }

    /// <summary>Get application count per vacancy for dashboard reporting.</summary>
    public async Task<IEnumerable<dynamic>> GetApplicationCountByVacancyAsync()
    {
        const string sql = """
            SELECT   AH.APP_VACANCYID AS VacancyId,
                     VM.VACANCY_NAME  AS VacancyName,
                     COUNT(*)         AS ApplicationCount,
                     SUM(CASE WHEN AH.APP_STATUS = '05' THEN 1 ELSE 0 END) AS SelectedCount
            FROM     APPLICATION_HISTORY AH
            JOIN     VACANCY_MAIN VM ON VM.VACANCY_ID = AH.APP_VACANCYID
            GROUP BY AH.APP_VACANCYID, VM.VACANCY_NAME
            ORDER BY ApplicationCount DESC
            """;

        return await QueryAsync<dynamic>(sql);
    }
}
