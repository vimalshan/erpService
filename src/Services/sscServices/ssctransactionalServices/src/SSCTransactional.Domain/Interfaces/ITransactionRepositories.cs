using SSCTransactional.Domain.Entities;

namespace SSCTransactional.Domain.Interfaces;

public interface IDocumentApprovalRepository
{
    Task<DocumentApproval?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<DocumentApproval>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task AddAsync(DocumentApproval approval, CancellationToken ct = default);
    Task UpdateAsync(DocumentApproval approval, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IRescanRepository
{
    Task<RescanDetail?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<RescanDetail>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task<IEnumerable<RescanDetail>> GetPendingAsync(CancellationToken ct = default);
    Task AddAsync(RescanDetail rescan, CancellationToken ct = default);
    Task UpdateAsync(RescanDetail rescan, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IRevokeRepository
{
    Task<RevokeDetail?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<RevokeDetail>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task AddAsync(RevokeDetail revoke, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IDocumentApproverRepository
{
    Task<DocumentApprover?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<DocumentApprover>> GetByBusinessUnitAsync(string buId, CancellationToken ct = default);
    Task AddAsync(DocumentApprover approver, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IOracleInvoiceRepository
{
    Task<OracleInvoice?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<OracleInvoice>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task AddAsync(OracleInvoice invoice, CancellationToken ct = default);
    Task UpdateAsync(OracleInvoice invoice, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IOraclePaymentRepository
{
    Task<OraclePayment?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<OraclePayment>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task AddAsync(OraclePayment payment, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IOracleBankDetailRepository
{
    Task<OracleBankDetail?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<OracleBankDetail>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task AddAsync(OracleBankDetail bankDetail, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IOracleDueDetailRepository
{
    Task<OracleDueDetail?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<OracleDueDetail>> GetByDocIdAsync(long docId, CancellationToken ct = default);
    Task AddAsync(OracleDueDetail dueDetail, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}

public interface IDocumentStatusRepository
{
    Task<DocumentStatus?> GetByFlagAsync(string flag, CancellationToken ct = default);
    Task<IEnumerable<DocumentStatus>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<DocumentStatus>> GetByTypeAsync(string docType, CancellationToken ct = default);
}
