using Dapper;
using DeductionService.Application.CQRS.Queries.GetDeductionAmount;
using DeductionService.Application.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DeductionService.Infrastructure.Dapper;

/// <summary>
/// Dapper-based repository for read-optimized queries and stored procedure execution.
/// </summary>
public class DeductionDapperRepository(IConfiguration configuration) : IDeductionAmountService
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<DeductionAmountDto> GetDeductionAmountAsync(
        long empSysId, long itemCode, DateTime dateTaken, CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);

        var result = await conn.QueryFirstOrDefaultAsync<(decimal EmployeeShare, decimal EmployerShare)>(
            "SELECT EmployeeShare, EmployerShare FROM dbo.fn_GetCanteenDeductionAmount(@p_EmpSysID, @p_ItemCode, @p_DateTaken)",
            new { p_EmpSysID = empSysId, p_ItemCode = itemCode, p_DateTaken = dateTaken });

        return new DeductionAmountDto(result.EmployeeShare, result.EmployerShare, result.EmployeeShare + result.EmployerShare);
    }

    public async Task<IEnumerable<dynamic>> GetDeductionsByMonthRawAsync(string monthYear, CancellationToken ct = default)
    {
        using var conn = CreateConnection();
        await conn.OpenAsync(ct);

        return await conn.QueryAsync(
            "SELECT * FROM ADHOC_PAY_DED WHERE FORMAT(PY_TRN_DAT, 'yyyy-MM') = @MonthYear",
            new { MonthYear = monthYear });
    }
}
