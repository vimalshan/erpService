using AutoMapper;
using MediatR;
using TimesheetService.Application.DTOs;
using TimesheetService.Domain.Interfaces;

namespace TimesheetService.Application.Queries.GetTimesheetsByEmployee;

public sealed class GetTimesheetsByEmployeeQueryHandler : IRequestHandler<GetTimesheetsByEmployeeQuery, IEnumerable<TimesheetSummaryDto>>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;

    public GetTimesheetsByEmployeeQueryHandler(ITimesheetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<TimesheetSummaryDto>> Handle(GetTimesheetsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var timesheets = await _repository.GetByEmployeeIdAsync(request.EmployeeId, request.From, request.To, cancellationToken);
        return _mapper.Map<IEnumerable<TimesheetSummaryDto>>(timesheets);
    }
}
