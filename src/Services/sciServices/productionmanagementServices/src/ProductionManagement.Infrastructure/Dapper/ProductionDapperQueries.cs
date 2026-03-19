using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductionManagement.Application.DTOs;

namespace ProductionManagement.Infrastructure.Dapper;

public interface IProductionDapperQueries
{
    Task<IEnumerable<ProductionPlantDto>> GetAllPlantsAsync();
    Task<ProductionPlantDto?> GetPlantByIdAsync(int plantId);
    Task<int> RegisterProductionPlantAsync(int companyUnitId, string plantName, string location, int createdBy);
    Task<IEnumerable<ProductionPlanDto>> GetPlansByPlantIdAsync(int plantId);
}

public class ProductionDapperQueries : IProductionDapperQueries
{
    private readonly string _connectionString;

    public ProductionDapperQueries(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<IEnumerable<ProductionPlantDto>> GetAllPlantsAsync()
    {
        const string sql = @"
            SELECT PRODUCTION_PLANT_ID AS ProductionPlantId,
                   COMPANY_UNIT_ID AS CompanyUnitId,
                   PLANT_NAME AS PlantName,
                   LOCATION AS Location,
                   SCI_USER_ID_CREATED AS CreatedBy,
                   CREATION_DATE AS CreationDate,
                   SCI_USER_ID_MODIFIED AS ModifiedBy,
                   MODIFIED_DATE AS ModifiedDate
            FROM PRODUCTION_PLANT";

        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ProductionPlantDto>(sql);
    }

    public async Task<ProductionPlantDto?> GetPlantByIdAsync(int plantId)
    {
        const string sql = @"
            SELECT PRODUCTION_PLANT_ID AS ProductionPlantId,
                   COMPANY_UNIT_ID AS CompanyUnitId,
                   PLANT_NAME AS PlantName,
                   LOCATION AS Location,
                   SCI_USER_ID_CREATED AS CreatedBy,
                   CREATION_DATE AS CreationDate,
                   SCI_USER_ID_MODIFIED AS ModifiedBy,
                   MODIFIED_DATE AS ModifiedDate
            FROM PRODUCTION_PLANT
            WHERE PRODUCTION_PLANT_ID = @PlantId";

        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<ProductionPlantDto>(sql, new { PlantId = plantId });
    }

    public async Task<int> RegisterProductionPlantAsync(int companyUnitId, string plantName, string location, int createdBy)
    {
        using var connection = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@p_CompanyUnitID", companyUnitId);
        parameters.Add("@p_PlantName", plantName);
        parameters.Add("@p_Location", location);
        parameters.Add("@p_CreatedBy", createdBy);
        parameters.Add("@p_PlantID", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

        await connection.ExecuteAsync("dbo.usp_RegisterProductionPlant", parameters, commandType: System.Data.CommandType.StoredProcedure);
        return parameters.Get<int>("@p_PlantID");
    }

    public async Task<IEnumerable<ProductionPlanDto>> GetPlansByPlantIdAsync(int plantId)
    {
        const string sql = @"
            SELECT PRODUCTION_PLANT_ID AS ProductionPlantId,
                   SCI_ITEM_ID AS SciItemId,
                   QTY_PERDAY AS QtyPerDay,
                   PLAN_START_DATE AS PlanStartDate,
                   PLAN_CLOSURE_DATE AS PlanClosureDate,
                   SCI_USER_ID_MODIFIED AS ModifiedBy,
                   MODIFIED_DATE AS ModifiedDate
            FROM PRODUCTION_PLAN
            WHERE PRODUCTION_PLANT_ID = @PlantId";

        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<ProductionPlanDto>(sql, new { PlantId = plantId });
    }
}
