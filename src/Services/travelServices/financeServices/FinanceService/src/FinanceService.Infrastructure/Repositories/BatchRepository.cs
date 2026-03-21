using Dapper;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;

namespace FinanceService.Infrastructure.Repositories;

public class BatchRepository
{
    private readonly IDapperContext _dapperContext;

    public BatchRepository(IDapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<IEnumerable<BatchDto>> GetAllBatchesAsync(string? unitCode = null)
    {
        using var connection = _dapperContext.CreateConnection();
        var sql = @"
            SELECT TM_UNT_COD AS UnitCode, TM_BAT_NUM AS BatchNumber, TM_BAT_DAT AS BatchDate,
                   TM_INV_NUM AS InvoiceNumber, TM_BAT_STS AS BatchStatus,
                   TM_ADM_REM AS AdminRemarks, TM_FIN_REM AS FinanceRemarks,
                   TM_AGN_COD AS AgencyCode, TM_TOTAPPRAMT AS TotalApprovedAmount,
                   TM_TOTAL AS Total
            FROM TRAVEL_BATCH_MAIN
            WHERE (@UnitCode IS NULL OR TM_UNT_COD = @UnitCode)";
        return await connection.QueryAsync<BatchDto>(sql, new { UnitCode = unitCode });
    }

    public async Task<BatchDto?> GetBatchDetailsAsync(string unitCode, decimal batchNumber)
    {
        using var connection = _dapperContext.CreateConnection();
        const string sql = "EXEC dbo.usp_GetBatchDetails @p_UnitCode = @UnitCode, @p_BatchNum = @BatchNumber";
        return await connection.QueryFirstOrDefaultAsync<BatchDto>(sql, new { UnitCode = unitCode, BatchNumber = batchNumber });
    }
}
