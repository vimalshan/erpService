using Microsoft.EntityFrameworkCore;
using SSCTransactional.Domain.Entities;
using SSCTransactional.Domain.Interfaces;
using SSCTransactional.Infrastructure.Persistence;

namespace SSCTransactional.Infrastructure.Repositories;

public class DocumentApprovalRepository : IDocumentApprovalRepository
{
    private readonly ApplicationDbContext _context;
    public DocumentApprovalRepository(ApplicationDbContext context) => _context = context;

    public async Task<DocumentApproval?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.DocumentApprovals.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IEnumerable<DocumentApproval>> GetByDocIdAsync(long docId, CancellationToken ct = default)
        => await _context.DocumentApprovals.Where(a => a.DocId == docId).ToListAsync(ct);

    public async Task AddAsync(DocumentApproval approval, CancellationToken ct = default)
        => await _context.DocumentApprovals.AddAsync(approval, ct);

    public Task UpdateAsync(DocumentApproval approval, CancellationToken ct = default)
    {
        _context.DocumentApprovals.Update(approval);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.DocumentApprovals.MaxAsync(a => (long?)a.Id, ct) ?? 0;
        return maxId + 1;
    }
}

public class RescanRepository : IRescanRepository
{
    private readonly ApplicationDbContext _context;
    public RescanRepository(ApplicationDbContext context) => _context = context;

    public async Task<RescanDetail?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.RescanDetails.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<IEnumerable<RescanDetail>> GetByDocIdAsync(long docId, CancellationToken ct = default)
        => await _context.RescanDetails.Where(r => r.DocId == docId).ToListAsync(ct);

    public async Task<IEnumerable<RescanDetail>> GetPendingAsync(CancellationToken ct = default)
        => await _context.RescanDetails.Where(r => r.Status == "N").ToListAsync(ct);

    public async Task AddAsync(RescanDetail rescan, CancellationToken ct = default)
        => await _context.RescanDetails.AddAsync(rescan, ct);

    public Task UpdateAsync(RescanDetail rescan, CancellationToken ct = default)
    {
        _context.RescanDetails.Update(rescan);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.RescanDetails.MaxAsync(r => (long?)r.Id, ct) ?? 0;
        return maxId + 1;
    }
}

public class RevokeRepository : IRevokeRepository
{
    private readonly ApplicationDbContext _context;
    public RevokeRepository(ApplicationDbContext context) => _context = context;

    public async Task<RevokeDetail?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.RevokeDetails.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<IEnumerable<RevokeDetail>> GetByDocIdAsync(long docId, CancellationToken ct = default)
        => await _context.RevokeDetails.Where(r => r.DocId == docId).ToListAsync(ct);

    public async Task AddAsync(RevokeDetail revoke, CancellationToken ct = default)
        => await _context.RevokeDetails.AddAsync(revoke, ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.RevokeDetails.MaxAsync(r => (long?)r.Id, ct) ?? 0;
        return maxId + 1;
    }
}

public class DocumentApproverRepository : IDocumentApproverRepository
{
    private readonly ApplicationDbContext _context;
    public DocumentApproverRepository(ApplicationDbContext context) => _context = context;

    public async Task<DocumentApprover?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.DocumentApprovers.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IEnumerable<DocumentApprover>> GetByBusinessUnitAsync(string buId, CancellationToken ct = default)
        => await _context.DocumentApprovers.Where(a => a.BusinessUnit == buId).ToListAsync(ct);

    public async Task AddAsync(DocumentApprover approver, CancellationToken ct = default)
        => await _context.DocumentApprovers.AddAsync(approver, ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.DocumentApprovers.MaxAsync(a => (long?)a.Id, ct) ?? 0;
        return maxId + 1;
    }
}
