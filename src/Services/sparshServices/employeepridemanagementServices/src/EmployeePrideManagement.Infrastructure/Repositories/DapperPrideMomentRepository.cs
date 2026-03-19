using Dapper;
using EmployeePrideManagement.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeePrideManagement.Infrastructure.Repositories;

public class DapperPrideMomentRepository : IDapperPrideMomentRepository
{
    private readonly string _connectionString;

    public DapperPrideMomentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<T?> GetByIdAsync<T>(decimal id) where T : class
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = @"
            SELECT MOMENTPRIDE_ID AS MomentPrideId,
                   MOMENTPRIDE_TITLE AS Title,
                   MOMENTPRIDE_BODY AS Body,
                   MOMENTPRIDE_EMPSYSID AS EmployeeSysId,
                   MOMENTPRIDE_FOOTER AS Footer,
                   MOMENTPRIDE_LOCATION AS Location,
                   MOMENTPRIDE_IMAGE AS Image,
                   MOMENTPRIDE_MODIFIEDBY AS ModifiedBy,
                   MOMENTPRIDE_MODIFIEDON AS ModifiedOn
            FROM MOMENT_PRIDE
            WHERE MOMENTPRIDE_ID = @Id";

        return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
    }

    public async Task<IEnumerable<T>> GetByEmployeeIdAsync<T>(decimal employeeSysId) where T : class
    {
        using var connection = new SqlConnection(_connectionString);
        const string sql = @"
            SELECT MOMENTPRIDE_ID AS MomentPrideId,
                   MOMENTPRIDE_TITLE AS Title,
                   MOMENTPRIDE_BODY AS Body,
                   MOMENTPRIDE_EMPSYSID AS EmployeeSysId,
                   MOMENTPRIDE_FOOTER AS Footer,
                   MOMENTPRIDE_LOCATION AS Location,
                   MOMENTPRIDE_IMAGE AS Image,
                   MOMENTPRIDE_MODIFIEDBY AS ModifiedBy,
                   MOMENTPRIDE_MODIFIEDON AS ModifiedOn
            FROM MOMENT_PRIDE
            WHERE MOMENTPRIDE_EMPSYSID = @EmployeeSysId
            ORDER BY MOMENTPRIDE_MODIFIEDON DESC";

        return await connection.QueryAsync<T>(sql, new { EmployeeSysId = employeeSysId });
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> GetAllPagedAsync<T>(int pageNumber, int pageSize) where T : class
    {
        using var connection = new SqlConnection(_connectionString);

        const string countSql = "SELECT COUNT(*) FROM MOMENT_PRIDE";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql);

        const string sql = @"
            SELECT MOMENTPRIDE_ID AS MomentPrideId,
                   MOMENTPRIDE_TITLE AS Title,
                   MOMENTPRIDE_BODY AS Body,
                   MOMENTPRIDE_EMPSYSID AS EmployeeSysId,
                   MOMENTPRIDE_FOOTER AS Footer,
                   MOMENTPRIDE_LOCATION AS Location,
                   MOMENTPRIDE_IMAGE AS Image,
                   MOMENTPRIDE_MODIFIEDBY AS ModifiedBy,
                   MOMENTPRIDE_MODIFIEDON AS ModifiedOn
            FROM MOMENT_PRIDE
            ORDER BY MOMENTPRIDE_MODIFIEDON DESC
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY";

        var items = await connection.QueryAsync<T>(sql, new
        {
            Offset = (pageNumber - 1) * pageSize,
            PageSize = pageSize
        });

        return (items, totalCount);
    }
}
