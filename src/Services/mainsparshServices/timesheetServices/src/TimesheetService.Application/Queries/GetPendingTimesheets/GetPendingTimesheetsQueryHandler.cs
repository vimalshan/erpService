using AutoMapper;
using MediatR;
using TimesheetService.Application.DTOs;
using TimesheetService.Domain.Interfaces;

namespace TimesheetService.Application.Queries.GetPendingTimesheets;

public sealed class GetPendingTimesheetsQueryHandler : IRequestHandler<GetPendingTimesheetsQuery, IEnumerable<TimesheetSummaryDto>>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;

    public GetPendingTimesheetsQueryHandler(ITimesheetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<TimesheetSummaryDto>> Handle(GetPendingTimesheetsQuery request, CancellationToken cancellationToken)
    {
        var timesheets = await _repository.GetPendingTimesheetsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TimesheetSummaryDto>>(timesheets);
    }
}
