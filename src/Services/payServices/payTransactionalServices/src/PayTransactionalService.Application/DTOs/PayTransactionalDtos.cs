namespace PayTransactionalService.Application.DTOs;

public record PayTransactionDto
{
    public long Id { get; init; }
    public long EmployeeSystemId { get; init; }
    public string MonthYear { get; init; } = null!;
    public decimal GrossAmount { get; init; }
    public decimal Deductions { get; init; }
    public decimal NetAmount { get; init; }
    public long? BatchId { get; init; }
    public string Status { get; init; } = null!;
    public string? Remarks { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = null!;
}

public record CreatePayTransactionDto
{
    public long EmployeeSystemId { get; init; }
    public string MonthYear { get; init; } = null!;
    public decimal GrossAmount { get; init; }
    public decimal Deductions { get; init; }
}

public record PayArrearDto
{
    public long Id { get; init; }
    public long EmployeeSystemId { get; init; }
    public decimal Amount { get; init; }
    public string Type { get; init; } = null!;
    public string? Code { get; init; }
    public string? Description { get; init; }
    public DateTime PayDate { get; init; }
    public string MonthYear { get; init; } = null!;
    public bool IsProcessed { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = null!;
}

public record CreatePayArrearDto
{
    public long EmployeeSystemId { get; init; }
    public decimal Amount { get; init; }
    public string Type { get; init; } = null!; // A or D
    public string MonthYear { get; init; } = null!;
    public string? Code { get; init; }
    public string? Description { get; init; }
}

public record PayAdjustmentDto
{
    public long Id { get; init; }
    public long EmployeeSystemId { get; init; }
    public string AdjustmentType { get; init; } = null!;
    public decimal Amount { get; init; }
    public string MonthYear { get; init; } = null!;
    public DateTime EffectiveDate { get; init; }
    public string? Reason { get; init; }
    public string Status { get; init; } = null!;
    public long? ApprovedBy { get; init; }
    public DateTime? ApprovedAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = null!;
}

public record CreatePayAdjustmentDto
{
    public long EmployeeSystemId { get; init; }
    public string AdjustmentType { get; init; } = null!;
    public decimal Amount { get; init; }
    public string MonthYear { get; init; } = null!;
    public DateTime EffectiveDate { get; init; }
    public string? Reason { get; init; }
}

public record PayrollBatchDto
{
    public long Id { get; init; }
    public string MonthYear { get; init; } = null!;
    public string Status { get; init; } = null!;
    public int TransactionCount { get; init; }
    public DateTime? CompletedAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = null!;
}

public record ProcessMonthlySalaryDto
{
    public string MonthYear { get; init; } = null!;
}
