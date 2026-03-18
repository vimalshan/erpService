using Dapper;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Interfaces;
using LoanManagement.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class DisbursementRepository : IDisbursementRepository
{
    private readonly LoanManagementDbContext _context;
    private readonly string _connectionString;

    public DisbursementRepository(LoanManagementDbContext context)
    {
        _context = context;
        _connectionString = context.Database.GetConnectionString()!;
    }

    public async Task<LoanDisbursementSchedule?> GetByIdAsync(long disbId, CancellationToken cancellationToken = default)
        => await _context.LoanDisbursementSchedules.FindAsync([disbId], cancellationToken);

    public async Task<IEnumerable<LoanDisbursementSchedule>> GetByLoanIdAsync(decimal loanId, CancellationToken cancellationToken = default)
        => await _context.LoanDisbursementSchedules
            .Where(d => d.DisbLoanId == loanId)
            .OrderBy(d => d.DisbDate)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(LoanDisbursementSchedule disbursement, CancellationToken cancellationToken = default)
        => await _context.LoanDisbursementSchedules.AddAsync(disbursement, cancellationToken);

    public Task UpdateAsync(LoanDisbursementSchedule disbursement, CancellationToken cancellationToken = default)
    {
        _context.LoanDisbursementSchedules.Update(disbursement);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_connectionString);
        var max = await connection.ExecuteScalarAsync<long?>("SELECT MAX(DISB_ID) FROM LOAN_DISBSCH");
        return (max ?? 0) + 1;
    }
}
