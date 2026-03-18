using Dapper;
using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Interfaces;
using LoanManagement.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly LoanManagementDbContext _context;
    private readonly string _connectionString;

    public LoanRepository(LoanManagementDbContext context)
    {
        _context = context;
        _connectionString = context.Database.GetConnectionString()!;
    }

    public async Task<LoanMain?> GetByIdAsync(decimal loanId, CancellationToken cancellationToken = default)
    {
        return await _context.LoanMain
            .Include(l => l.Disbursements)
            .Include(l => l.Interests)
            .Include(l => l.Repayments)
            .FirstOrDefaultAsync(l => l.LoanId == loanId, cancellationToken);
    }

    public async Task<LoanMain?> GetByKeyAsync(string loanKey, CancellationToken cancellationToken = default)
    {
        return await _context.LoanMain
            .Include(l => l.Disbursements)
            .Include(l => l.Interests)
            .Include(l => l.Repayments)
            .FirstOrDefaultAsync(l => l.LoanKey == loanKey.ToUpperInvariant(), cancellationToken);
    }

    public async Task<IEnumerable<LoanMain>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Use Dapper for efficient bulk read
        using var connection = new SqlConnection(_connectionString);
        var sql = "SELECT LOAN_ID AS LoanId, LOAN_KEY AS LoanKey, LOAN_ORGID AS LoanOrgId, " +
                  "LOAN_ORGCURR AS LoanOrgCurr, LOAN_CURR AS LoanCurr, LOAN_DATE AS LoanDate, " +
                  "LOAN_TYPEID AS LoanTypeId, LOAN_BANKID AS LoanBankId, " +
                  "LOAN_CREATEDBY AS LoanCreatedBy, LOAN_CREATEDON AS LoanCreatedOn, " +
                  "LOAN_MODIFIEDBY AS LoanModifiedBy, LOAN_MODIFIEDON AS LoanModifiedOn, " +
                  "LOAN_AMOUNT AS LoanAmount, LOAN_STATUS AS LoanStatus FROM LOAN_MAIN";

        var results = await connection.QueryAsync<LoanMain>(sql);
        return results;
    }

    public async Task<IEnumerable<LoanMain>> GetByOrganizationAsync(decimal orgId, CancellationToken cancellationToken = default)
    {
        return await _context.LoanMain
            .Where(l => l.LoanOrgId == orgId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(LoanMain loan, CancellationToken cancellationToken = default)
    {
        await _context.LoanMain.AddAsync(loan, cancellationToken);
    }

    public Task UpdateAsync(LoanMain loan, CancellationToken cancellationToken = default)
    {
        _context.LoanMain.Update(loan);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(decimal loanId, CancellationToken cancellationToken = default)
    {
        return await _context.LoanMain.AnyAsync(l => l.LoanId == loanId, cancellationToken);
    }

    public async Task<decimal> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        using var connection = new SqlConnection(_connectionString);
        var max = await connection.ExecuteScalarAsync<decimal?>("SELECT MAX(LOAN_ID) FROM LOAN_MAIN");
        return (max ?? 0) + 1;
    }
}
