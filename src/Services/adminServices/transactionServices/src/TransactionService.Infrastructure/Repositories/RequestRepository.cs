namespace TransactionService.Infrastructure.Repositories;

using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Persistence;

public sealed class RequestRepository : Repository<RequestMain>, IRequestRepository
{
    private readonly string _connectionString;

    public RequestRepository(TransactionDbContext context, string connectionString) : base(context)
    {
        _connectionString = connectionString;
    }

    public async Task<RequestMain?> GetByIdWithDetailsAsync(long requestId, CancellationToken ct = default)
    {
        return await _context.RequestMains
            .Include(r => r.Details)
            .FirstOrDefaultAsync(r => r.RequestId == requestId, ct);
    }

    public async Task<IEnumerable<RequestMain>> GetByLocationAsync(long locationId, CancellationToken ct = default)
    {
        return await _context.RequestMains
            .Include(r => r.Details)
            .Where(r => r.LocationId == locationId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<RequestMain>> GetByEmployeeAsync(long empSysId, CancellationToken ct = default)
    {
        return await _context.RequestMains
            .Include(r => r.Details)
            .Where(r => r.RequestedBy == empSysId)
            .ToListAsync(ct);
    }

    public async Task<long> GetNextRequestIdAsync(CancellationToken ct = default)
    {
        var max = await _context.RequestMains.MaxAsync(r => (long?)r.RequestId, ct);
        return (max ?? 0) + 1;
    }

    public async Task<long> GetNextRequestSubIdAsync(CancellationToken ct = default)
    {
        var max = await _context.RequestSubs.MaxAsync(r => (long?)r.RequestSubId, ct);
        return (max ?? 0) + 1;
    }

    public async Task<long> SubmitRequestSpAsync(
        long requestedBy, long locationId, string unitCode,
        IEnumerable<RequestSubParam> items, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var parameters = new DynamicParameters();
        parameters.Add("@p_RequestedBy", requestedBy, DbType.Int64);
        parameters.Add("@p_LocationID", locationId, DbType.Int64);
        parameters.Add("@p_UnitCode", unitCode, DbType.StringFixedLength, size: 3);
        parameters.Add("@p_NewRequestID", dbType: DbType.Int64, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "dbo.usp_StationeryRequestSubmit",
            parameters,
            commandType: CommandType.StoredProcedure);

        return parameters.Get<long>("@p_NewRequestID");
    }
}
