using AutoMapper;
using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Interfaces;

namespace TimeSheetService.Application.Queries.GetTimesheetsByEmployee;

public class GetTimesheetsByEmployeeQueryHandler : IRequestHandler<GetTimesheetsByEmployeeQuery, IEnumerable<TimesheetEntryDto>>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;

    public GetTimesheetsByEmployeeQueryHandler(ITimesheetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TimesheetEntryDto>> Handle(GetTimesheetsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var entries = request.From.HasValue && request.To.HasValue
            ? await _repository.GetByDateRangeAsync(request.EmployeeSysId, request.From.Value, request.To.Value, cancellationToken)
            : await _repository.GetByEmployeeAsync(request.EmployeeSysId, cancellationToken);
        return _mapper.Map<IEnumerable<TimesheetEntryDto>>(entries);
    }
}
