using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TravelRequestService.Application.DTOs;
using TravelRequestService.Application.Interfaces;

namespace TravelRequestService.Infrastructure.Dapper;

public class DapperQueryService : IDapperQueryService
{
    private readonly string _connectionString;

    public DapperQueryService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("TravelDb")
            ?? throw new InvalidOperationException("Connection string 'TravelDb' not found.");
    }

    public async Task<IReadOnlyList<DashTourPlanDto>> GetDashTourPlansAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                TOURDATE AS TourDate,
                BUSINESS AS Business,
                UNIT AS Unit,
                EMPSYSID AS EmployeeSystemId,
                EMPNAME AS EmployeeName,
                GRADE AS Grade,
                TOURNO AS TourNumber,
                EXPAMT AS ExpenseAmount,
                NATURE AS Nature
            FROM DASH_TOURPLAN
            ORDER BY TOURDATE DESC
            """;

        await using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryAsync<DashTourPlanDto>(sql);
        return result.ToList().AsReadOnly();
    }

    public async Task<TravelRequestDto?> GetTravelRequestDetailsAsync(long travelReqId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT 
                tm.TR_PLN_NUM AS PlanNumber,
                tm.TR_COM_COD AS CompanyCode,
                tm.TR_USR_NUM AS UserNumber,
                tm.TR_APP_DAT AS AppliedDate,
                tm.TR_OBJ_DES AS ObjectiveDescription,
                tm.TR_BUD_AMT AS BudgetAmount,
                tm.TR_PLS_FLG AS Status,
                tm.TR_TVL_TYP AS TravelType,
                ISNULL(tar.TR_REM, 'No remarks') AS Remarks
            FROM TRAVEL_MAIN tm
            LEFT JOIN TRAVEL_APPRREMARKS tar ON tm.TR_PLN_NUM = tar.TR_REQNO
            WHERE tm.TR_PLN_NUM = @TravelReqId
            """;

        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<TravelRequestDto>(sql, new { TravelReqId = travelReqId });
    }
}
