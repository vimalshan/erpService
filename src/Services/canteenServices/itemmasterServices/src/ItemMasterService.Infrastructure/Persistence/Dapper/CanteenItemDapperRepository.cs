using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ItemMasterService.Application.DTOs;

namespace ItemMasterService.Infrastructure.Persistence.Dapper;

/// <summary>
/// Read-model queries using Dapper for high-performance reporting scenarios.
/// </summary>
public class CanteenItemDapperRepository
{
    private readonly string _connectionString;

    public CanteenItemDapperRepository(string connectionString) => _connectionString = connectionString;

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<CanteenItemMasterDto>> GetItemsByUnitCodeAsync(long canteenUnitCode)
    {
        const string sql = @"
            SELECT 
                CN_COM_COD AS CanteenUnitCode,
                CN_ITM_COD AS ItemCode,
                CN_ITM_DES AS ItemDescription,
                CN_ITM_TYP AS ItemType,
                CN_ITM_REF AS ItemReference,
                CN_ENT_DAT AS EnteredOn,
                CN_ENT_USR AS EnteredBy
            FROM CANTEEN_ITEM_MASTER
            WHERE CN_COM_COD = @CanteenUnitCode
            ORDER BY CN_ITM_COD";

        using var conn = CreateConnection();
        return await conn.QueryAsync<CanteenItemMasterDto>(sql, new { CanteenUnitCode = canteenUnitCode });
    }

    public async Task<IEnumerable<CanteenItemPriceMasterDto>> GetActivePricesAsync(long canteenUnitCode)
    {
        const string sql = @"
            SELECT 
                CN_COM_COD AS CanteenUnitCode,
                CN_ITM_COD AS ItemCode,
                CN_EMP_CON AS EmployeeContribution,
                CN_EPR_CON AS EmployerContribution,
                CN_EFF_DAT AS EffectiveDate,
                CN_CLS_DAT AS ClosureDate,
                CN_ENT_DAT AS EnteredOn,
                CN_ENT_USR AS EnteredBy
            FROM CANTEEN_ITEM_PRICE_MASTER
            WHERE CN_COM_COD = @CanteenUnitCode
              AND CN_CLS_DAT IS NULL
            ORDER BY CN_ITM_COD, CN_EFF_DAT DESC";

        using var conn = CreateConnection();
        return await conn.QueryAsync<CanteenItemPriceMasterDto>(sql, new { CanteenUnitCode = canteenUnitCode });
    }

    public async Task<IEnumerable<CanteenGradeItemPriceDto>> GetGradeItemPricesByGradeAsync(string gradeType)
    {
        const string sql = @"
            SELECT 
                CN_COM_COD AS CanteenUnitCode,
                CN_ITM_COD AS ItemCode,
                CN_EMP_CON AS EmployeeContribution,
                CN_EPR_CON AS EmployerContribution,
                CN_EFF_DAT AS EffectiveDate,
                CN_CLS_DAT AS ClosureDate,
                CN_ENT_DAT AS EnteredOn,
                CN_ENT_USR AS EnteredBy,
                CN_GRD_TYP AS GradeType
            FROM CANTEENGRADE_ITEM_PRICE
            WHERE CN_GRD_TYP = @GradeType";

        using var conn = CreateConnection();
        return await conn.QueryAsync<CanteenGradeItemPriceDto>(sql, new { GradeType = gradeType });
    }
}
