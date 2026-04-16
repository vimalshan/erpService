using FinanceService.Application.DTOs;
using FinanceService.Domain.Interfaces;
using MediatR;

namespace FinanceService.Application.Queries;

public record GetInvoiceByIdQuery(int InvoiceId) : IRequest<InvoiceDto?>;
public record GetAllInvoicesQuery() : IRequest<IEnumerable<InvoiceDto>>;
public record GetInvoicesByCompanyQuery(int CompanyId) : IRequest<IEnumerable<InvoiceDto>>;
public record GetFinancialsByCompanyQuery(int CompanyId, int? Year) : IRequest<IEnumerable<FinancialDto>>;

public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto?>
{
    private readonly IFinanceDomainRepository _repo;
    public GetInvoiceByIdQueryHandler(IFinanceDomainRepository repo) { _repo = repo; }

    public async Task<InvoiceDto?> Handle(GetInvoiceByIdQuery request, CancellationToken ct)
    {
        var i = await _repo.GetInvoiceByIdAsync(request.InvoiceId);
        if (i == null) return null;
        return new InvoiceDto(i.InvoiceId, i.InvoiceNumber, i.CompanyId, i.ContractId, i.InvoiceDate,
            i.DueDate, i.PlannedPaymentDate, i.PaidDate, i.Amount, i.TaxAmount, i.TotalAmount,
            i.Currency, i.Status, i.IsActive, i.CreatedDate, i.ModifiedDate, i.CreatedBy, i.ModifiedBy,
            i.Description, i.Terms, i.Notes, i.InvoicePath, i.PaymentMethod, i.PaymentReference,
            i.DiscountAmount, i.LateFee);
    }
}

public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, IEnumerable<InvoiceDto>>
{
    private readonly IFinanceDomainRepository _repo;
    public GetAllInvoicesQueryHandler(IFinanceDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<InvoiceDto>> Handle(GetAllInvoicesQuery request, CancellationToken ct)
    {
        var invoices = await _repo.GetAllInvoicesAsync();
        return invoices.Select(i => new InvoiceDto(i.InvoiceId, i.InvoiceNumber, i.CompanyId, i.ContractId,
            i.InvoiceDate, i.DueDate, i.PlannedPaymentDate, i.PaidDate, i.Amount, i.TaxAmount,
            i.TotalAmount, i.Currency, i.Status, i.IsActive, i.CreatedDate, i.ModifiedDate,
            i.CreatedBy, i.ModifiedBy, i.Description, i.Terms, i.Notes, i.InvoicePath,
            i.PaymentMethod, i.PaymentReference, i.DiscountAmount, i.LateFee));
    }
}

public class GetInvoicesByCompanyQueryHandler : IRequestHandler<GetInvoicesByCompanyQuery, IEnumerable<InvoiceDto>>
{
    private readonly IFinanceDomainRepository _repo;
    public GetInvoicesByCompanyQueryHandler(IFinanceDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<InvoiceDto>> Handle(GetInvoicesByCompanyQuery request, CancellationToken ct)
    {
        var invoices = await _repo.GetInvoicesByCompanyAsync(request.CompanyId);
        return invoices.Select(i => new InvoiceDto(i.InvoiceId, i.InvoiceNumber, i.CompanyId, i.ContractId,
            i.InvoiceDate, i.DueDate, i.PlannedPaymentDate, i.PaidDate, i.Amount, i.TaxAmount,
            i.TotalAmount, i.Currency, i.Status, i.IsActive, i.CreatedDate, i.ModifiedDate,
            i.CreatedBy, i.ModifiedBy, i.Description, i.Terms, i.Notes, i.InvoicePath,
            i.PaymentMethod, i.PaymentReference, i.DiscountAmount, i.LateFee));
    }
}

public class GetFinancialsByCompanyQueryHandler : IRequestHandler<GetFinancialsByCompanyQuery, IEnumerable<FinancialDto>>
{
    private readonly IFinanceDomainRepository _repo;
    public GetFinancialsByCompanyQueryHandler(IFinanceDomainRepository repo) { _repo = repo; }

    public async Task<IEnumerable<FinancialDto>> Handle(GetFinancialsByCompanyQuery request, CancellationToken ct)
    {
        var financials = await _repo.GetFinancialsByCompanyAsync(request.CompanyId, request.Year);
        return financials.Select(f => new FinancialDto(f.FinancialId, f.CompanyId, f.Year, f.Quarter,
            f.Month, f.Revenue, f.Expenses, f.Profit, f.OutstandingAmount, f.PaidAmount,
            f.OverdueAmount, f.Currency, f.IsActive, f.Notes, f.DataSource));
    }
}
