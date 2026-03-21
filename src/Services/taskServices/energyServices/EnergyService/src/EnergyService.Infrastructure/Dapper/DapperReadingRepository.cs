using System.Data;
using Dapper;
using EnergyService.Domain.Entities;

namespace EnergyService.Infrastructure.Dapper;

public class DapperReadingRepository
{
    private readonly DapperContext _context;

    public DapperReadingRepository(DapperContext context) => _context = context;

    public async Task<IEnumerable<EcReading>> GetReadingsByProcessIdAsync(int processId)
    {
        const string sql = """
            SELECT EB_ID as EbId, EB_UNIT_CODE as EbUnitCode, EB_PROCESS_ID as EbProcessId,
                   EB_DATE as EbDate, EB_TARGET as EbTarget, EB_READING as EbReading,
                   EB_RESET_READING as EbResetReading, EB_ACTUAL_USAGE as EbActualUsage,
                   EB_TODATE as EbToDate, EB_REMARKS as EbRemarks,
                   LAST_MODIFIED_BY as LastModifiedBy, LAST_MODIFIED_ON as LastModifiedOn
            FROM EC_READING
            WHERE EB_PROCESS_ID = @ProcessId
            ORDER BY EB_DATE DESC
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<EcReading>(sql, new { ProcessId = processId });
    }

    public async Task InsertReadingAsync(string unitCode, int processId, long readingValue,
        long? targetValue, string? remarks, int modifiedBy)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "dbo.usp_ENERGY_InsertReading",
            new
            {
                p_UnitCode = unitCode,
                p_ProcessID = processId,
                p_ReadingValue = readingValue,
                p_TargetValue = targetValue,
                p_Remarks = remarks,
                p_ModifiedBy = modifiedBy
            },
            commandType: CommandType.StoredProcedure);
    }
}
