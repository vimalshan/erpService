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
/// Query to get tax marginal detail by employee and financial year
/// </summary>
public record GetTaxByEmployeeAndYearQuery(long EmployeeSystemId, int FinancialYear) 
    : IRequest<Result<TaxMarginalDetailDto>>;

/// <summary>
/// Query to get all tax details for an employee
/// </summary>
public record GetEmployeeTaxDetailsQuery(long EmployeeSystemId) 
    : IRequest<Result<IEnumerable<TaxMarginalDetailDto>>>;

/// <summary>
/// Query to get conditional master by ID
/// </summary>
public record GetConditionalMasterByIdQuery(long Id) 
    : IRequest<Result<ConditionalMasterDto>>;

/// <summary>
/// Query to get conditional master by payee ID
/// </summary>
public record GetConditionalMasterByPayeeIdQuery(string PayeeId, int? FinancialYear = null) 
    : IRequest<Result<ConditionalMasterDto>>;

/// <summary>
/// Query to get all active conditional masters for a financial year
/// </summary>
public record GetActiveConditionalMastersQuery(int? FinancialYear = null) 
    : IRequest<Result<IEnumerable<ConditionalMasterDto>>>;
