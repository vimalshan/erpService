using AutoMapper;
using MediatR;
using ScholarshipService.Application.DTOs;
using ScholarshipService.Domain.Repositories;

namespace ScholarshipService.Application.Queries.GetScholarshipAmounts;

public record GetScholarshipAmountsQuery : IRequest<IEnumerable<ScholarshipAmountDto>>;

public class GetScholarshipAmountsQueryHandler(
    IScholarshipAmountRepository repository,
    IMapper mapper)
    : IRequestHandler<GetScholarshipAmountsQuery, IEnumerable<ScholarshipAmountDto>>
{
    public async Task<IEnumerable<ScholarshipAmountDto>> Handle(GetScholarshipAmountsQuery request, CancellationToken cancellationToken)
    {
        var amounts = await repository.GetAllAsync(cancellationToken);
        return amounts.Select(mapper.Map<ScholarshipAmountDto>);
    }
}
