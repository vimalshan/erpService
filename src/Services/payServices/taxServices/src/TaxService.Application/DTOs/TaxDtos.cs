using MediatR;
using TaxService.Domain.ValueObjects;

namespace TaxService.Application.DTOs;

/// <summary>
/// DTO for Tax Marginal Detail
/// </summary>
public record TaxMarginalDetailDto
{
    public long Id { get; init; }
    public long EmployeeSystemId { get; init; }
    public int FinancialYear { get; init; }
    public decimal GrossIncome { get; init; }
    public decimal StandardDeduction { get; init; }
    public decimal TaxableIncome { get; init; }
    public decimal CalculatedTax { get; init; }
    public string[] Exemptions { get; init; } = Array.Empty<string>();
    public string Remarks { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = null!;
}

/// <summary>
/// DTO for Conditional Master
/// </summary>
public record ConditionalMasterDto
{
    public long Id { get; init; }
    public string PayeeId { get; init; } = null!;
    public string PayeeName { get; init; } = null!;
    public string PayeeAddress { get; init; } = null!;
    public string PayeePAN { get; init; } = string.Empty;
    public string TaxRegime { get; init; } = "Old";
    public int FinancialYear { get; init; }
    public decimal TotalExemption { get; init; }
    public decimal TotalDeduction { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// DTO for Tax Exemption
/// </summary>
public record TaxExemptionDto
{
    public long Id { get; init; }
    public string Code { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Amount { get; init; }
}

/// <summary>
/// DTO for Tax Deduction
/// </summary>
public record TaxDeductionDto
{
    public long Id { get; init; }
    public string Code { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Amount { get; init; }
}

/// <summary>
/// DTO for creating Tax Marginal Detail
/// </summary>
public record CreateTaxMarginalDetailDto
{
    public long EmployeeSystemId { get; init; }
    public int FinancialYear { get; init; }
    public decimal GrossIncome { get; init; }
    public decimal StandardDeduction { get; init; }
}

/// <summary>
/// DTO for creating Conditional Master
/// </summary>
public record CreateConditionalMasterDto
{
    public string PayeeId { get; init; } = null!;
    public string PayeeName { get; init; } = null!;
    public string PayeeAddress { get; init; } = null!;
    public string PayeePAN { get; init; } = null!;
    public string TaxRegime { get; init; } = "Old";
    public int FinancialYear { get; init; }
}

/// <summary>
/// DTO for creating exemption
/// </summary>
public record CreateTaxExemptionDto
{
    public long ConditionalMasterId { get; init; }
    public string Code { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Amount { get; init; }
}

/// <summary>
/// DTO for creating deduction
/// </summary>
public record CreateTaxDeductionDto
{
    public long ConditionalMasterId { get; init; }
    public string Code { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Amount { get; init; }
}
