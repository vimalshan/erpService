using Dapper;
using Microsoft.Data.SqlClient;
using EligibilityService.Application.DTOs;

namespace EligibilityService.Infrastructure.Repositories.Dapper;

/// <summary>
/// Lightweight Dapper repository for read-heavy / reporting queries.
/// Mirrors the dbo.fn_IsEmployeeEligibleForMeal SQL function.
/// </summary>
public class EligibilityDapperRepository
{
    private readonly string _connectionString;

    public EligibilityDapperRepository(string connectionString)
        => _connectionString = connectionString;

    public async Task<EligibilityCheckResultDto> CheckMealEligibilityAsync(
        long empSysId, long itemCode, string shiftCode, long canteenUnit)
    {
        const string sql = """
            SELECT dbo.fn_IsEmployeeEligibleForMeal(@EmpSysId, @ItemCode, @ShiftCode, @CanteenUnit) AS IsEligible,
                   (SELECT TOP 1 CN_ELG_LMT FROM CAN_ELIGIBILITY_MASTER
                    WHERE CN_COM_COD = @CanteenUnit AND CN_SFT_COD = @ShiftCode AND CN_ITM_COD = @ItemCode) AS EligibleLimit
            """;

        await using var conn = new SqlConnection(_connectionString);
        var result = await conn.QueryFirstOrDefaultAsync<dynamic>(sql, new
        {
            EmpSysId = empSysId,
            ItemCode = itemCode,
            ShiftCode = shiftCode,
            CanteenUnit = canteenUnit
        });

        return result is null
            ? new EligibilityCheckResultDto(false, null)
            : new EligibilityCheckResultDto((bool)result.IsEligible, (int?)result.EligibleLimit);
    }

    public async Task<IEnumerable<EligibilityMasterDto>> GetEligibilityPagedAsync(
        long canteenUnit, int page, int pageSize)
    {
        const string sql = """
            SELECT CN_COM_COD   AS CanteenUnit,
                   CN_SFT_COD   AS ShiftCode,
                   CN_ITM_COD   AS ItemCode,
                   CN_ELG_LMT   AS EligibleLimit,
                   CN_ENT_USR   AS EnteredUser,
                   CN_ENT_DAT   AS EnteredOn,
                   CN_TIM_UNT   AS TimeOfficeUnit
            FROM   CAN_ELIGIBILITY_MASTER
            WHERE  CN_COM_COD = @CanteenUnit
            ORDER BY CN_ITM_COD
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        await using var conn = new SqlConnection(_connectionString);
        var rows = await conn.QueryAsync<dynamic>(sql, new
        {
            CanteenUnit = canteenUnit,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });

        return rows.Select(r => new EligibilityMasterDto(
            (long)r.CanteenUnit,
            (string)r.ShiftCode,
            (decimal)r.ItemCode,
            (int?)r.EligibleLimit,
            (long?)r.EnteredUser,
            (DateTime?)r.EnteredOn,
            (string?)r.TimeOfficeUnit));
    }
}
