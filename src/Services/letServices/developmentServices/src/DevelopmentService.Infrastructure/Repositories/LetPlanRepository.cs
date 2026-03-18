using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using DevelopmentService.Domain.Entities;
using DevelopmentService.Domain.Interfaces;
using DevelopmentService.Infrastructure.Data;

namespace DevelopmentService.Infrastructure.Repositories;

public class LetPlanRepository : ILetPlanRepository
{
    private readonly DevelopmentDbContext _context;
    private readonly string _connectionString;

    public LetPlanRepository(DevelopmentDbContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<LetPlan?> GetByIdAsync(long reqNum, CancellationToken ct = default)
        => await _context.LetPlans.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ReqNum == reqNum, ct);

    public async Task<IEnumerable<LetPlan>> GetAllAsync(string? userId, char? status, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var rows = await conn.QueryAsync<dynamic>(
            "usp_Development_GetPlans",
            new { p_UserID = userId, p_Status = status?.ToString() },
            commandType: System.Data.CommandType.StoredProcedure);

        var result = new List<LetPlan>();
        foreach (var r in rows) result.Add(MapRow(r));
        return result;
    }

    public async Task AddAsync(LetPlan plan, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        var outputParam = new DynamicParameters();
        outputParam.Add("@p_ReqNum", plan.ReqNum);
        outputParam.Add("@p_UserID", plan.UserId);
        outputParam.Add("@p_PinNum", plan.PinNum);
        outputParam.Add("@p_DevSource", plan.DevSource);
        outputParam.Add("@p_DevNeed", plan.DevNeed);
        outputParam.Add("@p_Priority", plan.Priority);
        outputParam.Add("@p_EntDate", plan.EntDate);
        outputParam.Add("@p_ReqNum_OUTPUT", dbType: System.Data.DbType.Int64,
            direction: System.Data.ParameterDirection.Output);

        await conn.ExecuteAsync(
            "usp_Development_CreateLearningPlan",
            outputParam,
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(LetPlan plan, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync(
            "usp_Development_ApprovePlan",
            new
            {
                p_ReqNum    = plan.ReqNum,
                p_AppStatus = plan.AppStatus.ToString(),
                p_BHRStatus = plan.BhrStatus?.ToString()
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(long reqNum, CancellationToken ct = default)
    {
        await _context.LetPlans
            .Where(p => p.ReqNum == reqNum)
            .ExecuteDeleteAsync(ct);
    }

    private static LetPlan MapRow(dynamic r)
    {
        var plan = LetPlan.Create(
            (long)r.DD_REQNUM,
            (string)(r.DD_USERID ?? ""),
            (long)(r.DD_PINNUM ?? 0L),
            (string)(r.DD_DEVSOURCE ?? ""),
            (string)(r.DD_DEVNEED ?? ""),
            (long)(r.DD_PRIORITY ?? 0L),
            (DateTime)(r.DD_ENTDATE ?? DateTime.MinValue));

        return plan;
    }
}
