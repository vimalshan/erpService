using Dapper;
using TrainingDevelopment.Application.DTOs;

namespace TrainingDevelopment.Infrastructure.Dapper;

/// <summary>
/// Dapper-based read-optimised queries for training reports.
/// </summary>
public class TrainingDetailDapperRepository
{
    private readonly DapperContext _context;

    public TrainingDetailDapperRepository(DapperContext context) => _context = context;

    public async Task<IEnumerable<TrainingDetailDto>> GetTrainingReportAsync(
        decimal? financialYear = null,
        string? status = null,
        CancellationToken cancellationToken = default)
    {
        using var connection = _context.CreateConnection();

        var sql = """
            SELECT
                TR_ID AS Id,
                TR_FINYEAR AS FinancialYear,
                TR_EMPSYSID AS EmployeeSysId,
                TR_NEED AS TrainingNeed,
                TR_GAPS AS GapArea,
                TR_MODE AS Mode,
                CASE TR_MODE WHEN 1 THEN 'On-The-Job' WHEN 2 THEN 'Classroom' ELSE 'N/A' END AS ModeDisplay,
                TR_PROGRAMID AS ProgramId,
                TR_PROGRAMDESC AS ProgramDescription,
                TR_PLANFROM AS PlannedFrom,
                TR_PLANTO AS PlannedTo,
                TR_STATUS AS Status,
                CASE TR_STATUS WHEN 'P' THEN 'Pending' WHEN 'C' THEN 'Completed' WHEN 'D' THEN 'Dropped' ELSE '' END AS StatusDisplay,
                TR_ACTFROM AS ActualFrom,
                TR_ACTTO AS ActualTo,
                TR_INSTITUTEID AS InstituteId,
                TR_INSTITUTEDESC AS InstituteDescription,
                TR_TRAINERID AS TrainerId,
                TR_TRAINERDESC AS TrainerDescription,
                TR_PLACEID AS PlaceId,
                TR_PLACE AS Place,
                TR_COST AS Cost,
                TR_DROPREMARKS AS DroppedRemarks,
                TR_LASTMODIFIEDBY AS LastModifiedBy,
                TR_LASTMODIFIEDON AS LastModifiedOn
            FROM TRAINING_DET
            WHERE (@FinancialYear IS NULL OR TR_FINYEAR = @FinancialYear)
              AND (@Status IS NULL OR TR_STATUS = @Status)
            ORDER BY TR_ID
            """;

        var results = await connection.QueryAsync<TrainingDetailDto>(
            sql,
            new { FinancialYear = financialYear, Status = status });

        return results;
    }

    public async Task<IEnumerable<dynamic>> GetTrainingSummaryByStatusAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT 
                TR_STATUS AS Status,
                CASE TR_STATUS WHEN 'P' THEN 'Pending' WHEN 'C' THEN 'Completed' WHEN 'D' THEN 'Dropped' ELSE '' END AS StatusDisplay,
                COUNT(*) AS Count,
                SUM(ISNULL(TR_COST, 0)) AS TotalCost
            FROM TRAINING_DET
            GROUP BY TR_STATUS
            """;
        return await connection.QueryAsync(sql);
    }
}
