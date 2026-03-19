namespace MobileExpenseManagement.Application.Commands;

using MediatR;
using MobileExpenseManagement.Application.DTOs;

/// <summary>
/// Command to create a new expense
/// </summary>
public class CreateExpenseCommand : IRequest<ExpenseDto>
{
    public decimal TripId { get; set; }
    public decimal CategoryId { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string Comment { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal? CurrencyId { get; set; }
    public decimal EnteredBy { get; set; }
}

/// <summary>
/// Command to update an existing expense
/// </summary>
public class UpdateExpenseCommand : IRequest<ExpenseDto>
{
    public decimal ExpenseId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal? CurrencyId { get; set; }
    public decimal ModifiedBy { get; set; }
}

/// <summary>
/// Command to delete an expense
/// </summary>
public class DeleteExpenseCommand : IRequest<bool>
{
    public decimal ExpenseId { get; set; }
    public decimal DeletedBy { get; set; }
}

/// <summary>
/// Command to attach a file to an expense
/// </summary>
public class AttachExpenseFileCommand : IRequest<ExpenseFileDto>
{
    public decimal ExpenseId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[] FileContent { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = string.Empty;
    public decimal UploadedBy { get; set; }
}

/// <summary>
/// Command to remove a file from an expense
/// </summary>
public class RemoveExpenseFileCommand : IRequest<bool>
{
    public decimal FileId { get; set; }
}

/// <summary>
/// Command to approve trip expenses
/// </summary>
public class ApproveExpensesCommand : IRequest<TripExpenseSummaryDto>
{
    public decimal TripId { get; set; }
    public decimal ApprovedBy { get; set; }
}
