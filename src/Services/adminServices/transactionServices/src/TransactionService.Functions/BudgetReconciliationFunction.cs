using System.Data;
using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace TransactionService.Functions;

public sealed class BudgetReconciliationFunction
{
    private readonly ILogger<BudgetReconciliationFunction> _logger;

    public BudgetReconciliationFunction(ILogger<BudgetReconciliationFunction> logger)
    {
        _logger = logger;
    }

    [Function("BudgetReconciliation")]
    public async Task Run(
        [TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo) // Daily at 2 AM
    {
        _logger.LogInformation("Budget reconciliation started at {Time}", DateTime.UtcNow);

        var connectionString = Environment.GetEnvironmentVariable("TransactionDb");
        if (string.IsNullOrEmpty(connectionString))
        {
            _logger.LogError("TransactionDb connection string not configured");
            return;
        }

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // Check all department budgets and flag overspending
        var overBudgetDepts = await connection.QueryAsync<dynamic>(
            @"SELECT db.DB_LOCATION_ID, db.DB_DEPT_ID, db.DB_FINYEAR_ID, db.DB_BUDGETAMOUNT,
                     ISNULL(SUM(rs.RS_APPROVEDQTY * sm.SM_PRICE_PERUNIT), 0) AS SpentAmount
              FROM SP_DEPT_BUDGET db
              LEFT JOIN SP_REQUEST_SUB rs ON rs.RS_DEPTID = db.DB_DEPT_ID AND rs.RS_STATUS IN ('A','P')
              LEFT JOIN STATIONARY_MASTER sm ON rs.RS_STATIONARYID = sm.SM_STATIONARYID
              GROUP BY db.DB_LOCATION_ID, db.DB_DEPT_ID, db.DB_FINYEAR_ID, db.DB_BUDGETAMOUNT
              HAVING ISNULL(SUM(rs.RS_APPROVEDQTY * sm.SM_PRICE_PERUNIT), 0) > db.DB_BUDGETAMOUNT");

        foreach (var dept in overBudgetDepts)
        {
            _logger.LogWarning(
                "BUDGET EXCEEDED: Location {LocationId}, Dept {DeptId}, FY {FinYearId} - Budget: {Budget}, Spent: {Spent}",
                (object)dept.DB_LOCATION_ID, (object)dept.DB_DEPT_ID, (object)dept.DB_FINYEAR_ID,
                (object)dept.DB_BUDGETAMOUNT, (object)dept.SpentAmount);
        }

        _logger.LogInformation(
            "Budget reconciliation completed. {Count} departments over budget.",
            overBudgetDepts.Count());
    }
}
