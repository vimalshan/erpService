using AutoMapper;
using MediatR;
using TimesheetService.Application.DTOs;
using TimesheetService.Domain.Exceptions;
using TimesheetService.Domain.Interfaces;

namespace TimesheetService.Application.Commands.ApproveTimesheet;

public sealed class ApproveTimesheetCommandHandler : IRequestHandler<ApproveTimesheetCommand, TimesheetDto>
{
    private readonly ITimesheetRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ApproveTimesheetCommandHandler(ITimesheetRepository repository, IMapper mapper, IMediator mediator)
    {
        _repository = repository;
        _mapper     = mapper;
        _mediator   = mediator;
    }

    public async Task<TimesheetDto> Handle(ApproveTimesheetCommand request, CancellationToken cancellationToken)
    {
        var timesheet = await _repository.GetByIdAsync(request.TimesheetId, cancellationToken)
            ?? throw new TimesheetNotFoundException(request.TimesheetId);

        timesheet.Approve(request.ApproverId);
        await _repository.UpdateAsync(timesheet, cancellationToken);

        foreach (var domainEvent in timesheet.DomainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);

        timesheet.ClearDomainEvents();
        return _mapper.Map<TimesheetDto>(timesheet);
    }
}
