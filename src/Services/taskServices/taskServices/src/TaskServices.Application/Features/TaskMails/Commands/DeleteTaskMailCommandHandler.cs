using MediatR;
using TaskServices.Domain.Repositories;

namespace TaskServices.Application.Features.TaskMails.Commands;

public class DeleteTaskMailCommandHandler : IRequestHandler<DeleteTaskMailCommand, Unit>
{
    private readonly ITaskMailRepository _repository;

    public DeleteTaskMailCommandHandler(ITaskMailRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteTaskMailCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.ExistsAsync(request.MID, cancellationToken))
            throw new KeyNotFoundException($"TaskMail with MID {request.MID} not found.");

        await _repository.DeleteAsync(request.MID, cancellationToken);
        return Unit.Value;
    }
}
