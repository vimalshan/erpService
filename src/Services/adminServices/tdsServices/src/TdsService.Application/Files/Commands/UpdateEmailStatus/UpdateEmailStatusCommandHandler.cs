using MediatR;
using TdsService.Application.Common.Exceptions;
using TdsService.Application.Common.Interfaces;
using TdsService.Domain.Repositories;

namespace TdsService.Application.Files.Commands.UpdateEmailStatus;

public sealed class UpdateEmailStatusCommandHandler : IRequestHandler<UpdateEmailStatusCommand>
{
    private readonly ITdsFileRepository _repository;
    private readonly IMessagePublisher _publisher;

    public UpdateEmailStatusCommandHandler(
        ITdsFileRepository repository,
        IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task Handle(UpdateEmailStatusCommand request, CancellationToken cancellationToken)
    {
        var file = await _repository.GetByIdAsync(request.FileId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TdsFile), request.FileId);

        file.MarkEmailSent();

        _repository.Update(file);
        await _repository.SaveChangesAsync(cancellationToken);

        // Publish event for downstream consumers
        await _publisher.PublishAsync(
            "tds.exchange",
            "tds.email.sent",
            new { file.Id, file.PanNumber?.Value, SentAt = DateTimeOffset.UtcNow },
            cancellationToken);
    }
}
