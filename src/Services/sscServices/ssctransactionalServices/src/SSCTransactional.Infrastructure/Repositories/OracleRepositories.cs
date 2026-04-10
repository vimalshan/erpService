using Microsoft.EntityFrameworkCore;
using SSCTransactional.Domain.Entities;
using SSCTransactional.Domain.Interfaces;
using SSCTransactional.Infrastructure.Persistence;

namespace SSCTransactional.Infrastructure.Repositories;

public class OracleInvoiceRepository : IOracleInvoiceRepository
{
    private readonly ApplicationDbContext _context;
    public OracleInvoiceRepository(ApplicationDbContext context) => _context = context;

    public async Task<OracleInvoice?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.OracleInvoices.FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<IEnumerable<OracleInvoice>> GetByDocIdAsync(long docId, CancellationToken ct = default)
        => await _context.OracleInvoices.Where(i => i.DocId == docId).ToListAsync(ct);

    public async Task AddAsync(OracleInvoice invoice, CancellationToken ct = default)
        => await _context.OracleInvoices.AddAsync(invoice, ct);

    public Task UpdateAsync(OracleInvoice invoice, CancellationToken ct = default)
    {
        _context.OracleInvoices.Update(invoice);
        return Task.CompletedTask;
    }

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.OracleInvoices.MaxAsync(i => (long?)i.Id, ct) ?? 0;
        return maxId + 1;
    }
}

public class OraclePaymentRepository : IOraclePaymentRepository
{
    private readonly ApplicationDbContext _context;
    public OraclePaymentRepository(ApplicationDbContext context) => _context = context;

    public async Task<OraclePayment?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.OraclePayments.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IEnumerable<OraclePayment>> GetByDocIdAsync(long docId, CancellationToken ct = default)
        => await _context.OraclePayments.Where(p => p.DocId == docId).ToListAsync(ct);

    public async Task AddAsync(OraclePayment payment, CancellationToken ct = default)
        => await _context.OraclePayments.AddAsync(payment, ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.OraclePayments.MaxAsync(p => (long?)p.Id, ct) ?? 0;
        return maxId + 1;
    }
}

public class OracleBankDetailRepository : IOracleBankDetailRepository
{
    private readonly ApplicationDbContext _context;
    public OracleBankDetailRepository(ApplicationDbContext context) => _context = context;

    public async Task<OracleBankDetail?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.OracleBankDetails.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<IEnumerable<OracleBankDetail>> GetByDocIdAsync(long docId, CancellationToken ct = default)
        => await _context.OracleBankDetails.Where(b => b.DocId == docId).ToListAsync(ct);

    public async Task AddAsync(OracleBankDetail bankDetail, CancellationToken ct = default)
        => await _context.OracleBankDetails.AddAsync(bankDetail, ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.OracleBankDetails.MaxAsync(b => (long?)b.Id, ct) ?? 0;
        return maxId + 1;
    }
}

public class OracleDueDetailRepository : IOracleDueDetailRepository
{
    private readonly ApplicationDbContext _context;
    public OracleDueDetailRepository(ApplicationDbContext context) => _context = context;

    public async Task<OracleDueDetail?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _context.OracleDueDetails.FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<IEnumerable<OracleDueDetail>> GetByDocIdAsync(long docId, CancellationToken ct = default)
        => await _context.OracleDueDetails.Where(d => d.DocId == docId).ToListAsync(ct);

    public async Task AddAsync(OracleDueDetail dueDetail, CancellationToken ct = default)
        => await _context.OracleDueDetails.AddAsync(dueDetail, ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var maxId = await _context.OracleDueDetails.MaxAsync(d => (long?)d.Id, ct) ?? 0;
        return maxId + 1;
    }
}

public class DocumentStatusRepository : IDocumentStatusRepository
{
    private readonly ApplicationDbContext _context;
    public DocumentStatusRepository(ApplicationDbContext context) => _context = context;

    public async Task<DocumentStatus?> GetByFlagAsync(string flag, CancellationToken ct = default)
        => await _context.DocumentStatuses.FirstOrDefaultAsync(s => s.Id == flag, ct);

    public async Task<IEnumerable<DocumentStatus>> GetAllAsync(CancellationToken ct = default)
        => await _context.DocumentStatuses.OrderBy(s => s.StageOrder).ToListAsync(ct);

    public async Task<IEnumerable<DocumentStatus>> GetByTypeAsync(string docType, CancellationToken ct = default)
        => await _context.DocumentStatuses.Where(s => s.DocType == docType).OrderBy(s => s.StageOrder).ToListAsync(ct);
}
