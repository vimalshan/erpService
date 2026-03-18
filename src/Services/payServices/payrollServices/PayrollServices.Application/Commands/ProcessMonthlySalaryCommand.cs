using MediatR;
using PayrollServices.Application.DTOs;

namespace PayrollServices.Application.Commands;

/// <summary>
/// Command to process monthly salary
/// </summary>
public class ProcessMonthlySalaryCommand : IRequest<ProcessMonthlySalaryResult>
{
    public string MonthYear { get; set; } = null!; // YYYY-MM format
    public long ProcessedBy { get; set; }
}

public class ProcessMonthlySalaryResult
{
    public long BatchId { get; set; }
    public string BatchMonth { get; set; } = null!;
    public int TransactionCount { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}
