using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DevelopmentService.Domain.Entities;
using DevelopmentService.Domain.Interfaces;

namespace DevelopmentService.Infrastructure.Repositories;

public class LetBhrPlanRepository : ILetBhrPlanRepository
{
    private readonly string _connectionString;

    public LetBhrPlanRepository(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<LetBhrPlan?> GetByIdAsync(long reqNum, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var row = await conn.QueryFirstOrDefaultAsync<dynamic>(
            "SELECT * FROM DD_LETBHRPLAN WHERE DD_REQNUM = @ReqNum",
            new { ReqNum = reqNum });

        if (row is null) return null;

        return LetBhrPlan.Create(
            (long)row.DD_REQNUM,
            (string)(row.DD_USERID ?? ""),
            (string)(row.DD_TRAININGPROGRAM ?? ""),
            (decimal)(row.DD_TRAININGCODE ?? 0m),
            (decimal)(row.DD_PRIORITY ?? 0m),
            (char)(row.DD_BHRACCEPT ?? 'A'));
    }

    public async Task AddAsync(LetBhrPlan plan, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(
            "usp_Development_CreateBHRPlan",
            new
            {
                p_ReqNum          = plan.ReqNum,
                p_UserID          = plan.UserId,
                p_TrainingProgram = plan.TrainingProgram,
                p_TrainingCode    = plan.TrainingCode,
                p_Priority        = plan.Priority,
                p_BHRAccept       = plan.BhrAccept.ToString()
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
