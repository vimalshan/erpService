using Dapper;
using LocationServices.Domain.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LocationServices.Infrastructure.Repositories;

public sealed class DapperLocationAppMapReadRepository : ILocationAppMapReadRepository
{
    private readonly string _connectionString;

    public DapperLocationAppMapReadRepository(IConfiguration config)
        => _connectionString = config.GetConnectionString("LocationDb")!;

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<IEnumerable<LocationAppMapReadModel>> GetAllAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT LOCATION_ID           AS LocationId,
                   APP_NAME              AS AppName,
                   SITE_CATEGORY_CODE    AS SiteCategoryCode,
                   SELF_ACCESS           AS SelfAccess,
                   DEEMED_APPROVAL       AS DeemedApproval,
                   IS_ACTIVE            AS IsActive,
                   CREATED_DATE         AS CreatedDate,
                   CREATED_BY           AS CreatedBy,
                   MODIFIED_DATE        AS ModifiedDate,
                   MODIFIED_BY          AS ModifiedBy
            FROM   dbo.LOCATION_APP_MAP
            ORDER BY LOCATION_ID, APP_NAME
            """;
        await using var conn = CreateConnection();
        return await conn.QueryAsync<LocationAppMapReadModel>(sql);
    }

    public async Task<IEnumerable<LocationAppMapReadModel>> GetActiveMappingsAsync(CancellationToken ct = default)
    {
        const string sql = """
            SELECT LOCATION_ID           AS LocationId,
                   APP_NAME              AS AppName,
                   SITE_CATEGORY_CODE    AS SiteCategoryCode,
                   SELF_ACCESS           AS SelfAccess,
                   DEEMED_APPROVAL       AS DeemedApproval,
                   IS_ACTIVE            AS IsActive,
                   CREATED_DATE         AS CreatedDate,
                   CREATED_BY           AS CreatedBy,
                   MODIFIED_DATE        AS ModifiedDate,
                   MODIFIED_BY          AS ModifiedBy
            FROM   dbo.LOCATION_APP_MAP
            WHERE  IS_ACTIVE = 1
            ORDER BY LOCATION_ID, APP_NAME
            """;
        await using var conn = CreateConnection();
        return await conn.QueryAsync<LocationAppMapReadModel>(sql);
    }

    public async Task<IEnumerable<LocationAppMapReadModel>> GetByLocationIdAsync(decimal locationId, CancellationToken ct = default)
    {
        const string sql = """
            SELECT LOCATION_ID           AS LocationId,
                   APP_NAME              AS AppName,
                   SITE_CATEGORY_CODE    AS SiteCategoryCode,
                   SELF_ACCESS           AS SelfAccess,
                   DEEMED_APPROVAL       AS DeemedApproval,
                   IS_ACTIVE            AS IsActive,
                   CREATED_DATE         AS CreatedDate,
                   CREATED_BY           AS CreatedBy,
                   MODIFIED_DATE        AS ModifiedDate,
                   MODIFIED_BY          AS ModifiedBy
            FROM   dbo.LOCATION_APP_MAP
            WHERE  LOCATION_ID = @LocationId
            ORDER BY APP_NAME
            """;
        await using var conn = CreateConnection();
        return await conn.QueryAsync<LocationAppMapReadModel>(sql, new { LocationId = locationId });
    }

    public async Task<LocationAppMapReadModel?> GetMappingAsync(decimal locationId, string appName, CancellationToken ct = default)
    {
        const string sql = """
            SELECT LOCATION_ID           AS LocationId,
                   APP_NAME              AS AppName,
                   SITE_CATEGORY_CODE    AS SiteCategoryCode,
                   SELF_ACCESS           AS SelfAccess,
                   DEEMED_APPROVAL       AS DeemedApproval,
                   IS_ACTIVE            AS IsActive,
                   CREATED_DATE         AS CreatedDate,
                   CREATED_BY           AS CreatedBy,
                   MODIFIED_DATE        AS ModifiedDate,
                   MODIFIED_BY          AS ModifiedBy
            FROM   dbo.LOCATION_APP_MAP
            WHERE  LOCATION_ID = @LocationId AND APP_NAME = @AppName
            """;
        await using var conn = CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<LocationAppMapReadModel>(sql,
            new { LocationId = locationId, AppName = appName });
    }

    public async Task<int> GetTotalCountAsync(CancellationToken ct = default)
    {
        const string sql = "SELECT COUNT(1) FROM dbo.LOCATION_APP_MAP";
        await using var conn = CreateConnection();
        return await conn.ExecuteScalarAsync<int>(sql);
    }
}
