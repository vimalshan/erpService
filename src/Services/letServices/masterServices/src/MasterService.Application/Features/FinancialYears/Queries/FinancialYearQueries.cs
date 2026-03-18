using MediatR;
using MasterService.Application.DTOs;

namespace MasterService.Application.Features.FinancialYears.Queries;

public record GetActiveFinancialYearsQuery : IRequest<IEnumerable<FinancialYearDto>>;
