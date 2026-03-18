using AutoMapper;
using MediatR;
using TimesheetService.Application.DTOs;
using TimesheetService.Domain.Interfaces;

namespace TimesheetService.Application.Queries.GetTimesheetById;

public sealed class GetTimesheetByIdQueryHandler : IRequestHandler<GetTimesheetByIdQuery, TimesheetDto?>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;

    public GetTimesheetByIdQueryHandler(ITimesheetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<TimesheetDto?> Handle(GetTimesheetByIdQuery request, CancellationToken cancellationToken)
    {
        var timesheet = await _repository.GetByIdAsync(request.TimesheetId, cancellationToken);
        return timesheet is null ? null : _mapper.Map<TimesheetDto>(timesheet);
    }
}
