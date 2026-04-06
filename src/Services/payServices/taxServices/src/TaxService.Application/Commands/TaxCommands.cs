using MediatR;
using TaxService.Application.Common;
using TaxService.Application.DTOs;

namespace TaxService.Application.Commands;

/// <summary>
/// Command to create a new tax marginal detail
/// </summary>
public record CreateTaxMarginalDetailCommand(CreateTaxMarginalDetailDto Detail, string UserId) 
    : IRequest<Result<TaxMarginalDetailDto>>;

/// <summary>
/// Command to calculate tax for a marginal detail
/// </summary>
public record CalculateTaxCommand(long TaxMarginalDetailId) 
    : IRequest<Result<TaxMarginalDetailDto>>;

/// <summary>
/// Command to create conditional master
/// </summary>
public record CreateConditionalMasterCommand(CreateConditionalMasterDto Master, string UserId) 
    : IRequest<Result<ConditionalMasterDto>>;

/// <summary>
/// Command to add exemption to conditional master
/// </summary>
public record AddExemptionCommand(CreateTaxExemptionDto Exemption, string UserId) 
    : IRequest<Result<ConditionalMasterDto>>;

/// <summary>
/// Command to add deduction to conditional master
/// </summary>
public record AddDeductionCommand(CreateTaxDeductionDto Deduction, string UserId) 
    : IRequest<Result<ConditionalMasterDto>>;
