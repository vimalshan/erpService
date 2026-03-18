using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StipendService.Application.DTOs;

namespace StipendService.Infrastructure.Dapper;

public class StipendDapperRepository
{
    private readonly string _connectionString;

    public StipendDapperRepository(IConfiguration configuration) =>
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    public async Task<IEnumerable<StipendMasterDto>> GetActiveStipendsByDateAsync(DateTime asOfDate, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                STIPEND_ID AS StipendId,
                RESEARCH_CATEGORY_ID AS ResearchCategoryId,
                SRF_RANK_ID AS SrfRankId,
                SRF_MONTHLY_STIPEND AS SrfMonthlyStipend,
                ADDITIONAL_ALLOWANCE AS AdditionalAllowance,
                EFFECTIVE_FROM AS EffectiveFrom,
                EFFECTIVE_TO AS EffectiveTo,
                STATUS AS Status,
                CREATED_BY AS CreatedBy,
                CREATED_ON AS CreatedOn,
                UPDATED_BY AS UpdatedBy,
                UPDATED_ON AS UpdatedOn
            FROM SRF_STIPEND_MASTER
            WHERE STATUS = 'A'
              AND EFFECTIVE_FROM <= @AsOfDate
              AND (EFFECTIVE_TO IS NULL OR EFFECTIVE_TO >= @AsOfDate)
            """;

        await using var conn = new SqlConnection(_connectionString);
        var result = await conn.QueryAsync<StipendMasterDto>(sql, new { AsOfDate = asOfDate });
        return result;
    }

    public async Task<decimal> CalculateSrfStipendAsync(long researchCategoryId, long rankId)
    {
        const string sql = """
            SELECT dbo.fn_CalculateSRFStipend(@ResearchCategoryId, @RankId)
            """;

        await using var conn = new SqlConnection(_connectionString);
        return await conn.ExecuteScalarAsync<decimal>(sql, new { ResearchCategoryId = researchCategoryId, RankId = rankId });
    }

    public async Task<ProcessMonthlyStipendResultDto> ProcessMonthlyStipendAsync(string monthYear, long processedBy)
    {
        const string sql = "EXEC dbo.usp_ProcessSRFMonthlyStipend @MonthYear, @ProcessedBy, @RowsProcessed OUTPUT";

        await using var conn = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@MonthYear", monthYear);
        parameters.Add("@ProcessedBy", processedBy);
        parameters.Add("@RowsProcessed", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync(sql, parameters);
        int rowsProcessed = parameters.Get<int>("@RowsProcessed");

        return new ProcessMonthlyStipendResultDto(monthYear, rowsProcessed, true,
            $"Processed {rowsProcessed} records via stored procedure.");
    }

    public async Task<CalculateDisbursementResultDto> CalculateAndDisburseAsync(string monthYear, long processedBy)
    {
        const string sql = "EXEC dbo.usp_CalculateAndDisburseSRFStipend @MonthYear, @ProcessedBy, @RowsCreated OUTPUT";

        await using var conn = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@MonthYear", monthYear);
        parameters.Add("@ProcessedBy", processedBy);
        parameters.Add("@RowsCreated", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync(sql, parameters);
        int rowsCreated = parameters.Get<int>("@RowsCreated");

        return new CalculateDisbursementResultDto(monthYear, rowsCreated, true,
            $"Created {rowsCreated} disbursement records via stored procedure.");
    }

    public async Task<IEnumerable<StipendDisbursementDto>> GetDisbursementsByMonthAsync(string monthYear)
    {
        const string sql = """
            SELECT
                DISBURSEMENT_ID AS DisbursementId,
                SRF_ID AS SrfId,
                STIPEND_ID AS StipendId,
                DISBURSEMENT_DATE AS DisbursementDate,
                DISBURSEMENT_AMOUNT AS DisbursementAmount,
                DISBURSEMENT_STATUS AS DisbursementStatus,
                MONTH_YEAR AS MonthYear,
                BANK_REFERENCE AS BankReference,
                REFERENCE_NO AS ReferenceNo,
                CREATED_BY AS CreatedBy,
                CREATED_ON AS CreatedOn
            FROM SRF_STIPEND_DISBURSEMENT
            WHERE MONTH_YEAR = @MonthYear
            ORDER BY DISBURSEMENT_DATE DESC
            """;

        await using var conn = new SqlConnection(_connectionString);
        return await conn.QueryAsync<StipendDisbursementDto>(sql, new { MonthYear = monthYear });
    }
}
