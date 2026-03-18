using AutoMapper;
using DeductionService.Application.DTOs;
using DeductionService.Domain.Interfaces;
using MediatR;

namespace DeductionService.Application.CQRS.Queries.GetDeductionHistory;

public record GetDeductionHistoryQuery(long EmployeeNumber) : IRequest<IEnumerable<AdhocPayDeductionHistoryDto>>;

public class GetDeductionHistoryQueryHandler(
    IAdhocPayDeductionRepository repository,
    IMapper mapper)
    : IRequestHandler<GetDeductionHistoryQuery, IEnumerable<AdhocPayDeductionHistoryDto>>
{
    public async Task<IEnumerable<AdhocPayDeductionHistoryDto>> Handle(
        GetDeductionHistoryQuery request, CancellationToken ct)
    {
        var history = await repository.GetHistoryByEmployeeAsync(request.EmployeeNumber, ct);
        return mapper.Map<IEnumerable<AdhocPayDeductionHistoryDto>>(history);
    }
}
