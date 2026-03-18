using Dapper;
using InventoryManagement.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Infrastructure.Dapper;

/// <summary>
/// Dapper-based read repository for high-performance product queries.
/// </summary>
public sealed class DapperProductReadRepository
{
    private readonly string _connectionString;

    public DapperProductReadRepository(IConfiguration config)
        => _connectionString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string not configured.");

    public async Task<IEnumerable<MainProductMaster>> GetAllProductsAsync()
    {
        const string sql = @"
            SELECT pm.PRODUCT_ID AS ProductId, pm.PRODUCT_NAME AS ProductName,
                   pm.PRODUCT_DESCRIPTION AS ProductDescription, pm.UNIT_ID AS UnitId,
                   pm.PRODUCT_TYPE_ID AS ProductTypeId, pm.COMPANY_UNIT_ID AS CompanyUnitId
            FROM MAIN_PRODUCT_MASTER pm WITH (NOLOCK)
            ORDER BY pm.PRODUCT_ID";

        using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<MainProductMaster>(sql);
    }

    public async Task<int> RegisterProductViaStoredProcAsync(
        string productName, string? description, int unitId, int productTypeId, int companyUnitId, int createdBy)
    {
        using var conn = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@p_ProductName", productName);
        parameters.Add("@p_ProductDescription", description);
        parameters.Add("@p_UnitID", unitId);
        parameters.Add("@p_ProductTypeID", productTypeId);
        parameters.Add("@p_CompanyUnitID", companyUnitId);
        parameters.Add("@p_CreatedBy", createdBy);
        parameters.Add("@p_ProductID", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync("dbo.usp_RegisterProduct",
            parameters, commandType: System.Data.CommandType.StoredProcedure);

        return parameters.Get<int>("@p_ProductID");
    }
}
