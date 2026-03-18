using Microsoft.EntityFrameworkCore;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Interfaces;
using DocumentService.Infrastructure.Data;

namespace DocumentService.Infrastructure.Repositories;

public sealed class LoanDocumentRepository : ILoanDocumentRepository
{
    private readonly DocumentDbContext _context;

    public LoanDocumentRepository(DocumentDbContext context)
    {
        _context = context;
    }

    public async Task<LoanDocument?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await _context.LoanDocuments.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<LoanDocument>> GetByLoanIdAsync(long loanId, CancellationToken cancellationToken = default) =>
        await _context.LoanDocuments
            .Where(d => d.LoanId == loanId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<LoanDocument>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.LoanDocuments.ToListAsync(cancellationToken);

    public async Task AddAsync(LoanDocument document, CancellationToken cancellationToken = default)
    {
        await _context.LoanDocuments.AddAsync(document, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(LoanDocument document, CancellationToken cancellationToken = default)
    {
        _context.LoanDocuments.Update(document);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var document = await GetByIdAsync(id, cancellationToken);
        if (document is not null)
        {
            _context.LoanDocuments.Remove(document);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
