using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Domain.Interfaces;

namespace SSCTransactional.Application.Queries.Oracle;

public record GetOracleInvoicesByDocIdQuery(long DocId) : IRequest<IEnumerable<OracleInvoiceDto>>;
public record GetOraclePaymentsByDocIdQuery(long DocId) : IRequest<IEnumerable<OraclePaymentDto>>;
public record GetOracleBankDetailsByDocIdQuery(long DocId) : IRequest<IEnumerable<OracleBankDetailDto>>;
public record GetOracleDueDetailsByDocIdQuery(long DocId) : IRequest<IEnumerable<OracleDueDetailDto>>;
public record GetDocumentStatusesQuery() : IRequest<IEnumerable<DocumentStatusDto>>;
public record GetDocumentStatusesByTypeQuery(string DocType) : IRequest<IEnumerable<DocumentStatusDto>>;

public class GetOracleInvoicesByDocIdQueryHandler : IRequestHandler<GetOracleInvoicesByDocIdQuery, IEnumerable<OracleInvoiceDto>>
{
    private readonly IOracleInvoiceRepository _repo;
    public GetOracleInvoicesByDocIdQueryHandler(IOracleInvoiceRepository repo) => _repo = repo;

    public async Task<IEnumerable<OracleInvoiceDto>> Handle(GetOracleInvoicesByDocIdQuery query, CancellationToken ct)
    {
        var list = await _repo.GetByDocIdAsync(query.DocId, ct);
        return list.Select(i => new OracleInvoiceDto(i.Id, i.DocId, i.VoucherNo, i.InvoiceType,
            i.VendorId, i.VendorSiteId, i.InvoiceNum, i.InvoiceDate, i.InvoiceAmount,
            i.InvoiceId, i.InvoiceStatus, i.PaymentMethodCode, i.AccountingDate));
    }
}

public class GetOraclePaymentsByDocIdQueryHandler : IRequestHandler<GetOraclePaymentsByDocIdQuery, IEnumerable<OraclePaymentDto>>
{
    private readonly IOraclePaymentRepository _repo;
    public GetOraclePaymentsByDocIdQueryHandler(IOraclePaymentRepository repo) => _repo = repo;

    public async Task<IEnumerable<OraclePaymentDto>> Handle(GetOraclePaymentsByDocIdQuery query, CancellationToken ct)
    {
        var list = await _repo.GetByDocIdAsync(query.DocId, ct);
        return list.Select(p => new OraclePaymentDto(p.Id, p.DocId, p.PaymentNum, p.InvoiceId,
            p.DueDate, p.GrossAmount, p.AmountRemaining, p.PaymentStatus, p.PaymentMethod,
            p.CheckId, p.BankStatus, p.CheckNumber, p.CheckDate, p.CheckAmount));
    }
}

public class GetOracleBankDetailsByDocIdQueryHandler : IRequestHandler<GetOracleBankDetailsByDocIdQuery, IEnumerable<OracleBankDetailDto>>
{
    private readonly IOracleBankDetailRepository _repo;
    public GetOracleBankDetailsByDocIdQueryHandler(IOracleBankDetailRepository repo) => _repo = repo;

    public async Task<IEnumerable<OracleBankDetailDto>> Handle(GetOracleBankDetailsByDocIdQuery query, CancellationToken ct)
    {
        var list = await _repo.GetByDocIdAsync(query.DocId, ct);
        return list.Select(b => new OracleBankDetailDto(b.Id, b.DocId, b.CheckId,
            b.Business, b.OrgId, b.Amount, b.Currency, b.PaymentNumber, b.StatusLookupCode));
    }
}

public class GetOracleDueDetailsByDocIdQueryHandler : IRequestHandler<GetOracleDueDetailsByDocIdQuery, IEnumerable<OracleDueDetailDto>>
{
    private readonly IOracleDueDetailRepository _repo;
    public GetOracleDueDetailsByDocIdQueryHandler(IOracleDueDetailRepository repo) => _repo = repo;

    public async Task<IEnumerable<OracleDueDetailDto>> Handle(GetOracleDueDetailsByDocIdQuery query, CancellationToken ct)
    {
        var list = await _repo.GetByDocIdAsync(query.DocId, ct);
        return list.Select(d => new OracleDueDetailDto(d.Id, d.DocId, d.OrgId, d.InvoiceId,
            d.VoucherNo, d.DocumentId, d.DueDate, d.PaymentNum, d.DueAmount));
    }
}

public class GetDocumentStatusesQueryHandler : IRequestHandler<GetDocumentStatusesQuery, IEnumerable<DocumentStatusDto>>
{
    private readonly IDocumentStatusRepository _repo;
    public GetDocumentStatusesQueryHandler(IDocumentStatusRepository repo) => _repo = repo;

    public async Task<IEnumerable<DocumentStatusDto>> Handle(GetDocumentStatusesQuery query, CancellationToken ct)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(s => new DocumentStatusDto(s.Id, s.DocType, s.CompletedRemark,
            s.PendingRemark, s.StageOrder, s.CategoryGroup, s.StageNo));
    }
}

public class GetDocumentStatusesByTypeQueryHandler : IRequestHandler<GetDocumentStatusesByTypeQuery, IEnumerable<DocumentStatusDto>>
{
    private readonly IDocumentStatusRepository _repo;
    public GetDocumentStatusesByTypeQueryHandler(IDocumentStatusRepository repo) => _repo = repo;

    public async Task<IEnumerable<DocumentStatusDto>> Handle(GetDocumentStatusesByTypeQuery query, CancellationToken ct)
    {
        var list = await _repo.GetByTypeAsync(query.DocType, ct);
        return list.Select(s => new DocumentStatusDto(s.Id, s.DocType, s.CompletedRemark,
            s.PendingRemark, s.StageOrder, s.CategoryGroup, s.StageNo));
    }
}
