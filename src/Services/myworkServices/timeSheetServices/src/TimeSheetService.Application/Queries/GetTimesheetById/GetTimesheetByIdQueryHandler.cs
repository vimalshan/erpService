using AutoMapper;
using MediatR;
using TimeSheetService.Application.DTOs;
using TimeSheetService.Domain.Interfaces;

namespace TimeSheetService.Application.Queries.GetTimesheetById;

public class GetTimesheetByIdQueryHandler : IRequestHandler<GetTimesheetByIdQuery, TimesheetEntryDto?>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;

    public GetTimesheetByIdQueryHandler(ITimesheetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TimesheetEntryDto?> Handle(GetTimesheetByIdQuery request, CancellationToken cancellationToken)
    {
        var entry = await _repository.GetByIdAsync(request.TimeId, cancellationToken);
        return entry is null ? null : _mapper.Map<TimesheetEntryDto>(entry);
    }
}
