using AutoMapper;
using MediatR;
using TimesheetService.Application.DTOs;
using TimesheetService.Domain.Exceptions;
using TimesheetService.Domain.Interfaces;

namespace TimesheetService.Application.Commands.SubmitTimesheet;

public sealed class SubmitTimesheetCommandHandler : IRequestHandler<SubmitTimesheetCommand, TimesheetDto>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public SubmitTimesheetCommandHandler(ITimesheetRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper     = mapper;
        _mediator   = mediator;
    }

    public async Task<TimesheetDto> Handle(SubmitTimesheetCommand request, CancellationToken cancellationToken)
    {
        var timesheet = await _repository.GetByIdAsync(request.TimesheetId, cancellationToken)
            ?? throw new TimesheetNotFoundException(request.TimesheetId);

        timesheet.Submit(request.UpdatedBy);
        await _repository.UpdateAsync(timesheet, cancellationToken);

        foreach (var domainEvent in timesheet.DomainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        timesheet.ClearDomainEvents();
        return _mapper.Map<TimesheetDto>(timesheet);
    }
}
