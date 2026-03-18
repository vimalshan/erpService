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
/// Command handler for creating tax marginal detail
/// </summary>
public class CreateTaxMarginalDetailCommandHandler 
    : IRequestHandler<CreateTaxMarginalDetailCommand, Result<TaxMarginalDetailDto>>
{
    public Task<Result<TaxMarginalDetailDto>> Handle(
        CreateTaxMarginalDetailCommand request,
        CancellationToken cancellationToken)
    {
        // Implementation will be done in infrastructure layer
        throw new NotImplementedException();
    }
}

/// <summary>
/// Command to calculate tax for a marginal detail
/// </summary>
public record CalculateTaxCommand(long TaxMarginalDetailId) 
    : IRequest<Result<TaxMarginalDetailDto>>;

/// <summary>
/// Command handler for calculating tax
/// </summary>
public class CalculateTaxCommandHandler 
    : IRequestHandler<CalculateTaxCommand, Result<TaxMarginalDetailDto>>
{
    public Task<Result<TaxMarginalDetailDto>> Handle(
        CalculateTaxCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Command to create conditional master
/// </summary>
public record CreateConditionalMasterCommand(CreateConditionalMasterDto Master, string UserId) 
    : IRequest<Result<ConditionalMasterDto>>;

/// <summary>
/// Command handler for creating conditional master
/// </summary>
public class CreateConditionalMasterCommandHandler 
    : IRequestHandler<CreateConditionalMasterCommand, Result<ConditionalMasterDto>>
{
    public Task<Result<ConditionalMasterDto>> Handle(
        CreateConditionalMasterCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Command to add exemption to conditional master
/// </summary>
public record AddExemptionCommand(CreateTaxExemptionDto Exemption, string UserId) 
    : IRequest<Result<ConditionalMasterDto>>;

/// <summary>
/// Command handler for adding exemption
/// </summary>
public class AddExemptionCommandHandler 
    : IRequestHandler<AddExemptionCommand, Result<ConditionalMasterDto>>
{
    public Task<Result<ConditionalMasterDto>> Handle(
        AddExemptionCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Command to add deduction to conditional master
/// </summary>
public record AddDeductionCommand(CreateTaxDeductionDto Deduction, string UserId) 
    : IRequest<Result<ConditionalMasterDto>>;

/// <summary>
/// Command handler for adding deduction
/// </summary>
public class AddDeductionCommandHandler 
    : IRequestHandler<AddDeductionCommand, Result<ConditionalMasterDto>>
{
    public Task<Result<ConditionalMasterDto>> Handle(
        AddDeductionCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
