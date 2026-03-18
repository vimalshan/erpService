using Dapper;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Interfaces;
using LoanManagement.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class InterestRepository : IInterestRepository
{
    private readonly LoanManagementDbContext _context;
    private readonly string _connectionString;

    public InterestRepository(LoanManagementDbContext context)
    {
        _context = context;
        _connectionString = context.Database.GetConnectionString()!;
    }

    public async Task<LoanInterest?> GetByIdAsync(long intId, CancellationToken cancellationToken = default)
        => await _context.LoanInterests.FindAsync([intId], cancellationToken);

    public async Task<IEnumerable<LoanInterest>> GetByLoanIdAsync(decimal loanId, CancellationToken cancellationToken = default)
        => await _context.LoanInterests
            .Where(i => i.IntLoanId == loanId)
            .OrderByDescending(i => i.IntEffDate)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(LoanInterest interest, CancellationToken cancellationToken = default)
        => await _context.LoanInterests.AddAsync(interest, cancellationToken);

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_connectionString);
        var max = await connection.ExecuteScalarAsync<long?>("SELECT MAX(INT_ID) FROM LOAN_INTEREST");
        return (max ?? 0) + 1;
    }
}
