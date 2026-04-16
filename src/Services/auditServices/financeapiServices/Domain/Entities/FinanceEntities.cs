using FinanceService.Domain.Events;

namespace FinanceService.Domain.Entities;

public class Invoice
{
    public int InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public int? ContractId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PlannedPaymentDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal Amount { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Status { get; set; } = "Pending";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Description { get; set; }
    public string? Terms { get; set; }
    public string? Notes { get; set; }
    public string? InvoicePath { get; set; }
    public string? PaymentMethod { get; set; }
    public string? PaymentReference { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? LateFee { get; set; }

    public ICollection<InvoiceAuditLog> AuditLogs { get; set; } = new List<InvoiceAuditLog>();

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();

    public static Invoice Create(string invoiceNumber, int companyId, int? contractId, DateTime invoiceDate,
        DateTime dueDate, decimal amount, decimal? taxAmount, decimal totalAmount, string currency, int? createdBy)
    {
        var invoice = new Invoice
        {
            InvoiceNumber = invoiceNumber, CompanyId = companyId, ContractId = contractId,
            InvoiceDate = invoiceDate, DueDate = dueDate, Amount = amount, TaxAmount = taxAmount ?? 0,
            TotalAmount = totalAmount, Currency = currency, Status = "Pending",
            IsActive = true, CreatedBy = createdBy, ModifiedBy = createdBy,
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        invoice._domainEvents.Add(new InvoiceCreatedEvent(0, invoiceNumber, companyId, totalAmount));
        return invoice;
    }

    public void MarkPaid(DateTime paidDate, string? paymentMethod, string? paymentReference, int? modifiedBy)
    {
        var oldStatus = Status;
        Status = "Paid"; PaidDate = paidDate; PaymentMethod = paymentMethod;
        PaymentReference = paymentReference; ModifiedDate = DateTime.UtcNow; ModifiedBy = modifiedBy;
        _domainEvents.Add(new InvoicePaidEvent(InvoiceId, InvoiceNumber, paidDate, paymentReference));
        _domainEvents.Add(new InvoiceStatusChangedEvent(InvoiceId, oldStatus, "Paid"));
    }

    public void ChangeStatus(string newStatus, int? modifiedBy)
    {
        var oldStatus = Status;
        Status = newStatus; ModifiedDate = DateTime.UtcNow; ModifiedBy = modifiedBy;
        _domainEvents.Add(new InvoiceStatusChangedEvent(InvoiceId, oldStatus, newStatus));
    }
}

public class InvoiceAuditLog
{
    public int InvoiceAuditLogId { get; set; }
    public int InvoiceId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? ChangedFields { get; set; }
    public string? Reason { get; set; }
    public DateTime ActionDate { get; set; } = DateTime.UtcNow;
    public int ActionBy { get; set; }
    public string? IPAddress { get; set; }
    public string? UserAgent { get; set; }

    public Invoice Invoice { get; set; } = null!;
}

public class Financial
{
    public int FinancialId { get; set; }
    public int CompanyId { get; set; }
    public int Year { get; set; }
    public int? Quarter { get; set; }
    public int? Month { get; set; }
    public decimal? Revenue { get; set; }
    public decimal? Expenses { get; set; }
    public decimal? Profit { get; set; }
    public decimal? OutstandingAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public decimal? OverdueAmount { get; set; }
    public string Currency { get; set; } = "USD";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Notes { get; set; }
    public string? DataSource { get; set; }
}
