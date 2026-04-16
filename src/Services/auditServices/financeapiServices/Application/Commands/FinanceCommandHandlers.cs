using FinanceService.Application.DTOs;
using FinanceService.Domain.Entities;
using FinanceService.Domain.Interfaces;
using MediatR;

namespace FinanceService.Application.Commands;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, InvoiceDto>
{
    private readonly IFinanceDomainRepository _repo;
    private readonly IMediator _mediator;
    public CreateInvoiceCommandHandler(IFinanceDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<InvoiceDto> Handle(CreateInvoiceCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var entity = Invoice.Create(d.InvoiceNumber, d.CompanyId, d.ContractId, d.InvoiceDate, d.DueDate,
            d.Amount, d.TaxAmount, d.TotalAmount, d.Currency ?? "USD", d.CreatedBy);
        entity.Description = d.Description; entity.Terms = d.Terms; entity.Notes = d.Notes;
        entity.InvoicePath = d.InvoicePath; entity.DiscountAmount = d.DiscountAmount;

        var created = await _repo.AddInvoiceAsync(entity);
        foreach (var evt in created.DomainEvents) await _mediator.Publish(evt, ct);
        created.ClearDomainEvents();
        return MapToDto(created);
    }

    private static InvoiceDto MapToDto(Invoice i) => new(i.InvoiceId, i.InvoiceNumber, i.CompanyId, i.ContractId,
        i.InvoiceDate, i.DueDate, i.PlannedPaymentDate, i.PaidDate, i.Amount, i.TaxAmount, i.TotalAmount,
        i.Currency, i.Status, i.IsActive, i.CreatedDate, i.ModifiedDate, i.CreatedBy, i.ModifiedBy,
        i.Description, i.Terms, i.Notes, i.InvoicePath, i.PaymentMethod, i.PaymentReference,
        i.DiscountAmount, i.LateFee);
}

public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, InvoiceDto>
{
    private readonly IFinanceDomainRepository _repo;
    private readonly IMediator _mediator;
    public UpdateInvoiceCommandHandler(IFinanceDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<InvoiceDto> Handle(UpdateInvoiceCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var entity = await _repo.GetInvoiceByIdAsync(d.InvoiceId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Invoice {d.InvoiceId} not found");
        entity.InvoiceNumber = d.InvoiceNumber; entity.CompanyId = d.CompanyId; entity.ContractId = d.ContractId;
        entity.InvoiceDate = d.InvoiceDate; entity.DueDate = d.DueDate; entity.PlannedPaymentDate = d.PlannedPaymentDate;
        entity.Amount = d.Amount; entity.TaxAmount = d.TaxAmount; entity.TotalAmount = d.TotalAmount;
        entity.Currency = d.Currency; entity.Status = d.Status; entity.IsActive = d.IsActive;
        entity.Description = d.Description; entity.Terms = d.Terms; entity.Notes = d.Notes;
        entity.InvoicePath = d.InvoicePath; entity.PaymentMethod = d.PaymentMethod;
        entity.DiscountAmount = d.DiscountAmount; entity.LateFee = d.LateFee;
        entity.ModifiedDate = DateTime.UtcNow; entity.ModifiedBy = d.ModifiedBy;

        await _repo.UpdateInvoiceAsync(entity);
        foreach (var evt in entity.DomainEvents) await _mediator.Publish(evt, ct);
        entity.ClearDomainEvents();
        return new InvoiceDto(entity.InvoiceId, entity.InvoiceNumber, entity.CompanyId, entity.ContractId,
            entity.InvoiceDate, entity.DueDate, entity.PlannedPaymentDate, entity.PaidDate, entity.Amount,
            entity.TaxAmount, entity.TotalAmount, entity.Currency, entity.Status, entity.IsActive,
            entity.CreatedDate, entity.ModifiedDate, entity.CreatedBy, entity.ModifiedBy,
            entity.Description, entity.Terms, entity.Notes, entity.InvoicePath, entity.PaymentMethod,
            entity.PaymentReference, entity.DiscountAmount, entity.LateFee);
    }
}

public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, bool>
{
    private readonly IFinanceDomainRepository _repo;
    public DeleteInvoiceCommandHandler(IFinanceDomainRepository repo) { _repo = repo; }
    public async Task<bool> Handle(DeleteInvoiceCommand request, CancellationToken ct)
    {
        await _repo.DeleteInvoiceAsync(request.InvoiceId); return true;
    }
}

public class MarkInvoicePaidCommandHandler : IRequestHandler<MarkInvoicePaidCommand, InvoiceDto>
{
    private readonly IFinanceDomainRepository _repo;
    private readonly IMediator _mediator;
    public MarkInvoicePaidCommandHandler(IFinanceDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<InvoiceDto> Handle(MarkInvoicePaidCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetInvoiceByIdAsync(request.InvoiceId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Invoice {request.InvoiceId} not found");
        entity.MarkPaid(request.PaidDate, request.PaymentMethod, request.PaymentReference, request.ModifiedBy);
        await _repo.UpdateInvoiceAsync(entity);
        foreach (var evt in entity.DomainEvents) await _mediator.Publish(evt, ct);
        entity.ClearDomainEvents();
        return new InvoiceDto(entity.InvoiceId, entity.InvoiceNumber, entity.CompanyId, entity.ContractId,
            entity.InvoiceDate, entity.DueDate, entity.PlannedPaymentDate, entity.PaidDate, entity.Amount,
            entity.TaxAmount, entity.TotalAmount, entity.Currency, entity.Status, entity.IsActive,
            entity.CreatedDate, entity.ModifiedDate, entity.CreatedBy, entity.ModifiedBy,
            entity.Description, entity.Terms, entity.Notes, entity.InvoicePath, entity.PaymentMethod,
            entity.PaymentReference, entity.DiscountAmount, entity.LateFee);
    }
}

public class ChangeInvoiceStatusCommandHandler : IRequestHandler<ChangeInvoiceStatusCommand, InvoiceDto>
{
    private readonly IFinanceDomainRepository _repo;
    private readonly IMediator _mediator;
    public ChangeInvoiceStatusCommandHandler(IFinanceDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<InvoiceDto> Handle(ChangeInvoiceStatusCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetInvoiceByIdAsync(request.InvoiceId) ?? throw new System.Collections.Generic.KeyNotFoundException($"Invoice {request.InvoiceId} not found");
        entity.ChangeStatus(request.NewStatus, request.ModifiedBy);
        await _repo.UpdateInvoiceAsync(entity);
        foreach (var evt in entity.DomainEvents) await _mediator.Publish(evt, ct);
        entity.ClearDomainEvents();
        return new InvoiceDto(entity.InvoiceId, entity.InvoiceNumber, entity.CompanyId, entity.ContractId,
            entity.InvoiceDate, entity.DueDate, entity.PlannedPaymentDate, entity.PaidDate, entity.Amount,
            entity.TaxAmount, entity.TotalAmount, entity.Currency, entity.Status, entity.IsActive,
            entity.CreatedDate, entity.ModifiedDate, entity.CreatedBy, entity.ModifiedBy,
            entity.Description, entity.Terms, entity.Notes, entity.InvoicePath, entity.PaymentMethod,
            entity.PaymentReference, entity.DiscountAmount, entity.LateFee);
    }
}

public class CreateFinancialCommandHandler : IRequestHandler<CreateFinancialCommand, FinancialDto>
{
    private readonly IFinanceDomainRepository _repo;
    public CreateFinancialCommandHandler(IFinanceDomainRepository repo) { _repo = repo; }

    public async Task<FinancialDto> Handle(CreateFinancialCommand request, CancellationToken ct)
    {
        var d = request.Dto;
        var entity = new Financial
        {
            CompanyId = d.CompanyId, Year = d.Year, Quarter = d.Quarter, Month = d.Month,
            Revenue = d.Revenue, Expenses = d.Expenses, Profit = d.Profit,
            OutstandingAmount = d.OutstandingAmount, PaidAmount = d.PaidAmount, OverdueAmount = d.OverdueAmount,
            Currency = d.Currency ?? "USD", Notes = d.Notes, DataSource = d.DataSource,
            IsActive = true, CreatedBy = d.CreatedBy, ModifiedBy = d.CreatedBy,
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        var created = await _repo.AddFinancialAsync(entity);
        return new FinancialDto(created.FinancialId, created.CompanyId, created.Year, created.Quarter,
            created.Month, created.Revenue, created.Expenses, created.Profit, created.OutstandingAmount,
            created.PaidAmount, created.OverdueAmount, created.Currency, created.IsActive,
            created.Notes, created.DataSource);
    }
}
