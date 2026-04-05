using MediatR;
using TimeSheetService.Domain.Events;
using TimeSheetService.Domain.Interfaces;

namespace TimeSheetService.Application.Commands.DeleteTimesheet;

public class DeleteTimesheetCommandHandler : IRequestHandler<DeleteTimesheetCommand, bool>
{
    private readonly ITimesheetRepository _repository;

    public DeleteTimesheetCommandHandler(ITimesheetRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteTimesheetCommand request, CancellationToken cancellationToken)
    {
        var entry = await _repository.GetByIdAsync(request.TimeId, cancellationToken);
        if (entry is null) return false;

        entry.AddDomainEvent(new TimesheetDeletedEvent(entry.Id, entry.EmployeeSysId));
        await _repository.DeleteAsync(request.TimeId, cancellationToken);
        return true;
    }
}
