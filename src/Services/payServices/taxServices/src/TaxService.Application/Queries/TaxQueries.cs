using MediatR;
using TaxService.Application.Common;
using TaxService.Application.DTOs;

namespace TaxService.Application.Queries;

/// <summary>
/// Query to get tax marginal detail by ID
/// </summary>
public record GetTaxMarginalDetailByIdQuery(long Id) 
    : IRequest<Result<TaxMarginalDetailDto>>;

/// <summary>
/// Query handler for getting tax marginal detail by ID
/// </summary>
public class GetTaxMarginalDetailByIdQueryHandler 
    : IRequestHandler<GetTaxMarginalDetailByIdQuery, Result<TaxMarginalDetailDto>>
{
    public Task<Result<TaxMarginalDetailDto>> Handle(
        GetTaxMarginalDetailByIdQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Query to get tax marginal detail by employee and financial year
/// </summary>
public record GetTaxByEmployeeAndYearQuery(long EmployeeSystemId, int FinancialYear) 
    : IRequest<Result<TaxMarginalDetailDto>>;

/// <summary>
/// Query handler for getting tax by employee and year
/// </summary>
public class GetTaxByEmployeeAndYearQueryHandler 
    : IRequestHandler<GetTaxByEmployeeAndYearQuery, Result<TaxMarginalDetailDto>>
{
    public Task<Result<TaxMarginalDetailDto>> Handle(
        GetTaxByEmployeeAndYearQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Query to get all tax details for an employee
/// </summary>
public record GetEmployeeTaxDetailsQuery(long EmployeeSystemId) 
    : IRequest<Result<IEnumerable<TaxMarginalDetailDto>>>;

/// <summary>
/// Query handler for getting employee tax details
/// </summary>
public class GetEmployeeTaxDetailsQueryHandler 
    : IRequestHandler<GetEmployeeTaxDetailsQuery, Result<IEnumerable<TaxMarginalDetailDto>>>
{
    public Task<Result<IEnumerable<TaxMarginalDetailDto>>> Handle(
        GetEmployeeTaxDetailsQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Query to get conditional master by ID
/// </summary>
public record GetConditionalMasterByIdQuery(long Id) 
    : IRequest<Result<ConditionalMasterDto>>;

/// <summary>
/// Query handler for getting conditional master by ID
/// </summary>
public class GetConditionalMasterByIdQueryHandler 
    : IRequestHandler<GetConditionalMasterByIdQuery, Result<ConditionalMasterDto>>
{
    public Task<Result<ConditionalMasterDto>> Handle(
        GetConditionalMasterByIdQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Query to get conditional master by payee ID
/// </summary>
public record GetConditionalMasterByPayeeIdQuery(string PayeeId, int? FinancialYear = null) 
    : IRequest<Result<ConditionalMasterDto>>;

/// <summary>
/// Query handler for getting conditional master by payee ID
/// </summary>
public class GetConditionalMasterByPayeeIdQueryHandler 
    : IRequestHandler<GetConditionalMasterByPayeeIdQuery, Result<ConditionalMasterDto>>
{
    public Task<Result<ConditionalMasterDto>> Handle(
        GetConditionalMasterByPayeeIdQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Query to get all active conditional masters for a financial year
/// </summary>
public record GetActiveConditionalMastersQuery(int? FinancialYear = null) 
    : IRequest<Result<IEnumerable<ConditionalMasterDto>>>;

/// <summary>
/// Query handler for getting active conditional masters
/// </summary>
public class GetActiveConditionalMastersQueryHandler 
    : IRequestHandler<GetActiveConditionalMastersQuery, Result<IEnumerable<ConditionalMasterDto>>>
{
    public Task<Result<IEnumerable<ConditionalMasterDto>>> Handle(
        GetActiveConditionalMastersQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
