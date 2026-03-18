using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OrganizationStructureService.Infrastructure.Dapper;

public interface IDapperQueryService
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null);
}

public class DapperQueryService : IDapperQueryService
{
    private readonly string _connectionString;

    public DapperQueryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("HrDb")
            ?? throw new InvalidOperationException("Connection string 'HrDb' not configured.");
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<T>(sql, param);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
    {
        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
    }
}

public class OrganizationDapperQueries
{
    private readonly IDapperQueryService _dapper;
    public OrganizationDapperQueries(IDapperQueryService dapper) => _dapper = dapper;

    public async Task<IEnumerable<dynamic>> GetUnitHierarchyAsync(decimal businessId)
    {
        const string sql = @"
            SELECT 
                b.BUSINESS_ID, b.BUSINESS_NAME, b.BUSINESS_SHTNAME, b.BUSINESS_LIVFLAG,
                u.UNIT_ID, u.UNIT_NAME, u.UNIT_SHTNAME, u.UNIT_CODE, u.UNIT_LIVFLAG,
                d.DEPARTMENT_ID, d.DEPARTMENT_NAME, d.DEPARTMENT_LIVFLAG
            FROM BUSINESS_MASTER b
            LEFT JOIN UNIT_MASTER u ON u.UNIT_BUSINESSID = b.BUSINESS_ID
            LEFT JOIN UNIT_DEPARTMENT_MAP udm ON udm.UNIT_ID = u.UNIT_ID
            LEFT JOIN DEPARTMENT_MASTER d ON d.DEPARTMENT_ID = udm.UNIT_DEPARTMENT_ID
            WHERE b.BUSINESS_ID = @BusinessId
            ORDER BY b.BUSINESS_NAME, u.UNIT_NAME";

        return await _dapper.QueryAsync<dynamic>(sql, new { BusinessId = businessId });
    }

    public async Task<IEnumerable<dynamic>> GetActivePositionsByGradeAsync(decimal gradeId)
    {
        const string sql = @"
            SELECT 
                p.POSITION_ID, p.POSITION_NAME, p.POSITION_DESIGNATION,
                p.POS_UNIT_CODE, p.POS_EFFECTIVE_DATE, p.CTC,
                g.GRADE_NAME, g.GRADE_CODE, g.GRADE_DESIGNATION
            FROM POSITION_MASTER p
            INNER JOIN GRADE_MASTER g ON g.GRADE_ID = p.POS_GRADE_ID
            WHERE p.POS_GRADE_ID = @GradeId 
              AND p.DELETED_FLAG = 'N' 
              AND (p.POS_CLOSED_DATE IS NULL OR p.POS_CLOSED_DATE > GETDATE())
            ORDER BY p.POS_EFFECTIVE_DATE DESC";

        return await _dapper.QueryAsync<dynamic>(sql, new { GradeId = gradeId });
    }

    public async Task<IEnumerable<dynamic>> GetSitesByUnitAsync(string unitCode)
    {
        const string sql = @"
            SELECT 
                s.SITE_ID, s.SITE_NAME, s.SITE_SHORT_NAME, s.SITE_LIVFLAG,
                s.SITE_ADDRESS_LINE_1, s.SITE_CITY_CODE, s.SITE_PHONE_1,
                usm.UNIT_CODE, usm.UNIT_ID
            FROM SITE_MASTER s
            INNER JOIN UNIT_SITE_MAP usm ON usm.SITE_ID = s.SITE_ID
            WHERE usm.UNIT_CODE = @UnitCode AND s.SITE_LIVFLAG = 'Y'
            ORDER BY s.SITE_NAME";

        return await _dapper.QueryAsync<dynamic>(sql, new { UnitCode = unitCode });
    }

    public async Task<IEnumerable<dynamic>> GetGradesByUnitAsync(string unitCode)
    {
        const string sql = @"
            SELECT 
                g.GRADE_ID, g.GRADE_NAME, g.GRADE_CODE, g.GRADE_DESIGNATION,
                ugm.UNIT_CODE, ugm.GRADE_DISPLAY, ugm.UNIT_MAPLIVESTATUS
            FROM GRADE_MASTER g
            INNER JOIN UNIT_GRADE_MAP ugm ON ugm.GRADE_ID = g.GRADE_ID
            WHERE ugm.UNIT_CODE = @UnitCode AND (ugm.UNIT_MAPLIVESTATUS IS NULL OR ugm.UNIT_MAPLIVESTATUS = 'Y')
            ORDER BY g.GRADE_PRIORITY";

        return await _dapper.QueryAsync<dynamic>(sql, new { UnitCode = unitCode });
    }
}
