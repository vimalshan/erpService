using Dapper;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Interfaces;
using LoanManagement.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class RepaymentRepository : IRepaymentRepository
{
    private readonly LoanManagementDbContext _context;
    private readonly string _connectionString;

    public RepaymentRepository(LoanManagementDbContext context)
    {
        _context = context;
        _connectionString = context.Database.GetConnectionString()!;
    }

    public async Task<LoanRepaymentSchedule?> GetByIdAsync(long repayId, CancellationToken cancellationToken = default)
        => await _context.LoanRepaymentSchedules.FindAsync([repayId], cancellationToken);

    public async Task<IEnumerable<LoanRepaymentSchedule>> GetByLoanIdAsync(decimal loanId, CancellationToken cancellationToken = default)
        => await _context.LoanRepaymentSchedules
            .Where(r => r.RepayLoanId == loanId)
            .OrderBy(r => r.RepayDate)
            .ToListAsync(cancellationToken);

    public async Task AddRangeAsync(IEnumerable<LoanRepaymentSchedule> repayments, CancellationToken cancellationToken = default)
        => await _context.LoanRepaymentSchedules.AddRangeAsync(repayments, cancellationToken);

    public Task UpdateAsync(LoanRepaymentSchedule repayment, CancellationToken cancellationToken = default)
    {
        _context.LoanRepaymentSchedules.Update(repayment);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_connectionString);
        var max = await connection.ExecuteScalarAsync<long?>("SELECT MAX(REPAY_ID) FROM LOAN_REPAYSCH");
        return (max ?? 0) + 1;
    }
}
