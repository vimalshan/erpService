using AutoMapper;
using MediatR;
using TimesheetService.Application.DTOs;
using TimesheetService.Domain.Entities;
using TimesheetService.Domain.Interfaces;

namespace TimesheetService.Application.Commands.CreateTimesheet;

public sealed class CreateTimesheetCommandHandler : IRequestHandler<CreateTimesheetCommand, TimesheetDto>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateTimesheetCommandHandler(ITimesheetRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper     = mapper;
        _mediator   = mediator;
    }

    public async Task<TimesheetDto> Handle(CreateTimesheetCommand request, CancellationToken cancellationToken)
    {
        var timesheet = Timesheet.Create(
            request.EmployeeId,
            request.TimesheetDate,
            request.WorkDate,
            request.StartTime,
            request.EndTime,
            request.TotalHours,
            request.ProjectId,
            request.TaskId,
            request.WorkDescription,
            request.CreatedBy);

        await _repository.AddAsync(timesheet, cancellationToken);

        foreach (var domainEvent in timesheet.DomainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        timesheet.ClearDomainEvents();

        return _mapper.Map<TimesheetDto>(timesheet);
    }
}
