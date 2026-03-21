using MediatR;
using TaskServices.Domain.Repositories;
using TaskServices.Domain.ValueObjects;

namespace TaskServices.Application.Features.TaskMails.Commands;

public class UpdateTaskMailCommandHandler : IRequestHandler<UpdateTaskMailCommand, Unit>
{
    private readonly ITaskMailRepository _repository;

    public UpdateTaskMailCommandHandler(ITaskMailRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateTaskMailCommand request, CancellationToken cancellationToken)
    {
        var taskMail = await _repository.GetByIdAsync(request.MID, cancellationToken)
            ?? throw new KeyNotFoundException($"TaskMail with MID {request.MID} not found.");

        taskMail.Reassign(new SystemUserId(request.SYSID));
        await _repository.UpdateAsync(taskMail, cancellationToken);
        return Unit.Value;
    }
}
