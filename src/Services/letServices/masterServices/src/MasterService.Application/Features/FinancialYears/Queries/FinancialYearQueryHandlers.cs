using AutoMapper;
using MediatR;
using MasterService.Application.DTOs;
using MasterService.Domain.Interfaces;

namespace MasterService.Application.Features.FinancialYears.Queries;

public sealed class GetActiveFinancialYearsQueryHandler(IFinancialYearRepository repository, IMapper mapper)
    : IRequestHandler<GetActiveFinancialYearsQuery, IEnumerable<FinancialYearDto>>
{
    public async Task<IEnumerable<FinancialYearDto>> Handle(GetActiveFinancialYearsQuery request, CancellationToken cancellationToken)
    {
        var list = await repository.GetActiveAsync(cancellationToken);
        return mapper.Map<IEnumerable<FinancialYearDto>>(list);
    }
}
