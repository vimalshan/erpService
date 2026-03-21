using Dapper;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Interfaces;
using ExpenseService.Infrastructure.Data;

namespace ExpenseService.Infrastructure.Repositories;

public class DapperExpenseQuery : IDapperExpenseQuery
{
    private readonly DapperContext _context;

    public DapperExpenseQuery(DapperContext context)
    {
        _context = context;
    }

    public async Task<ExpenseSummaryDto?> GetExpenseSummaryAsync(long requestNumber)
    {
        const string sql = """
            SELECT 
                ISNULL(SUM(te.TR_ELG_AMT), 0) AS TotalExpenses,
                ISNULL(SUM(te.TR_VAR_AMT), 0) AS TotalVariance,
                ISNULL(SUM(te.TR_ACT_SLF), 0) AS EmployeeShare,
                ISNULL(SUM(te.TR_ELG_AMT) - SUM(te.TR_ACT_SLF), 0) AS CompanyShare,
                ISNULL(ds.DA_ADMAMT, 0) + ISNULL(ds.DA_SLFAMT, 0) AS TotalDAAmount
            FROM TRAVEL_EXPENSE te
            LEFT JOIN DA_SUMMARY ds ON te.TR_REQ_NUM = ds.DA_REQID
            WHERE te.TR_REQ_NUM = @RequestNumber
            GROUP BY ds.DA_ADMAMT, ds.DA_SLFAMT
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ExpenseSummaryDto>(sql, new { RequestNumber = requestNumber });
    }

    public async Task<decimal?> GetDARateAsync(string gradeCode, string arrangementType, DateTime fromDate, DateTime toDate)
    {
        const string sql = """
            SELECT TOP 1 RL_BUD_AMT
            FROM RULE_DA
            WHERE RL_BND_COD = @GradeCode
              AND RL_ADM_SLF = @ArrangementType
              AND RL_EFF_DAT <= @FromDate
              AND (RL_CLS_DAT IS NULL OR RL_CLS_DAT >= @ToDate)
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<decimal?>(sql,
            new { GradeCode = gradeCode, ArrangementType = arrangementType, FromDate = fromDate, ToDate = toDate });
    }

    public async Task<SettlementResultDto> SettleExpensesAsync(long requestNumber)
    {
        const string sql = """
            SELECT 
                ISNULL(SUM(TR_ELG_AMT), 0) AS TotalActual,
                ISNULL(SUM(TR_BUD_AMT), 0) AS TotalBudget
            FROM TRAVEL_EXPENSE
            WHERE TR_REQ_NUM = @RequestNumber
            """;

        using var connection = _context.CreateConnection();
        var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { RequestNumber = requestNumber });

        decimal totalActual = result?.TotalActual ?? 0;
        decimal totalBudget = result?.TotalBudget ?? 0;

        var settlementAmount = totalActual >= totalBudget ? totalBudget : totalActual;
        var refundAmount = totalActual < totalBudget ? totalBudget - totalActual : 0;

        return new SettlementResultDto
        {
            SettlementAmount = settlementAmount,
            RefundAmount = refundAmount,
            Status = "Settled"
        };
    }
}
