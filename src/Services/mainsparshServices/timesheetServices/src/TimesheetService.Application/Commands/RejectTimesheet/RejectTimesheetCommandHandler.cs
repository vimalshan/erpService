using AutoMapper;
using MediatR;
using TimesheetService.Application.DTOs;
using TimesheetService.Domain.Exceptions;
using TimesheetService.Domain.Interfaces;

namespace TimesheetService.Application.Commands.RejectTimesheet;

public sealed class RejectTimesheetCommandHandler : IRequestHandler<RejectTimesheetCommand, TimesheetDto>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RejectTimesheetCommandHandler(ITimesheetRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper     = mapper;
        _mediator   = mediator;
    }

    public async Task<TimesheetDto> Handle(RejectTimesheetCommand request, CancellationToken cancellationToken)
    {
        var timesheet = await _repository.GetByIdAsync(request.TimesheetId, cancellationToken)
            ?? throw new TimesheetNotFoundException(request.TimesheetId);

        timesheet.Reject(request.ApproverId, request.RejectionReason);
        await _repository.UpdateAsync(timesheet, cancellationToken);

        foreach (var domainEvent in timesheet.DomainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        timesheet.ClearDomainEvents();
        return _mapper.Map<TimesheetDto>(timesheet);
    }
}
