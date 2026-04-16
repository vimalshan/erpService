namespace FinanceService.Application.DTOs;

public record InvoiceDto(
    int InvoiceId, string InvoiceNumber, int CompanyId, int? ContractId, DateTime InvoiceDate,
    DateTime DueDate, DateTime? PlannedPaymentDate, DateTime? PaidDate, decimal Amount, decimal? TaxAmount,
    decimal TotalAmount, string Currency, string Status, bool IsActive, DateTime CreatedDate,
    DateTime ModifiedDate, int? CreatedBy, int? ModifiedBy, string? Description, string? Terms,
    string? Notes, string? InvoicePath, string? PaymentMethod, string? PaymentReference,
    decimal? DiscountAmount, decimal? LateFee);

public record CreateInvoiceDto(
    string InvoiceNumber, int CompanyId, int? ContractId, DateTime InvoiceDate, DateTime DueDate,
    decimal Amount, decimal? TaxAmount, decimal TotalAmount, string? Currency, string? Description,
    string? Terms, string? Notes, string? InvoicePath, decimal? DiscountAmount, int? CreatedBy);

public record UpdateInvoiceDto(
    int InvoiceId, string InvoiceNumber, int CompanyId, int? ContractId, DateTime InvoiceDate,
    DateTime DueDate, DateTime? PlannedPaymentDate, decimal Amount, decimal? TaxAmount,
    decimal TotalAmount, string Currency, string Status, bool IsActive, string? Description,
    string? Terms, string? Notes, string? InvoicePath, string? PaymentMethod,
    decimal? DiscountAmount, decimal? LateFee, int? ModifiedBy);

public record FinancialDto(
    int FinancialId, int CompanyId, int Year, int? Quarter, int? Month,
    decimal? Revenue, decimal? Expenses, decimal? Profit, decimal? OutstandingAmount,
    decimal? PaidAmount, decimal? OverdueAmount, string Currency, bool IsActive,
    string? Notes, string? DataSource);

public record CreateFinancialDto(
    int CompanyId, int Year, int? Quarter, int? Month, decimal? Revenue, decimal? Expenses,
    decimal? Profit, decimal? OutstandingAmount, decimal? PaidAmount, decimal? OverdueAmount,
    string? Currency, string? Notes, string? DataSource, int? CreatedBy);
