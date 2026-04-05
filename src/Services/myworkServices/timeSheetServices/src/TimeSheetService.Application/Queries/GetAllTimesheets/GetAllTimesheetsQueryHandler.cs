using AutoMapper;
using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Interfaces;

namespace TimeSheetService.Application.Queries.GetAllTimesheets;

public class GetAllTimesheetsQueryHandler : IRequestHandler<GetAllTimesheetsQuery, IEnumerable<TimesheetEntryDto>>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;

    public GetAllTimesheetsQueryHandler(ITimesheetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TimesheetEntryDto>> Handle(GetAllTimesheetsQuery request, CancellationToken cancellationToken)
    {
        var entries = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TimesheetEntryDto>>(entries);
    }
}
