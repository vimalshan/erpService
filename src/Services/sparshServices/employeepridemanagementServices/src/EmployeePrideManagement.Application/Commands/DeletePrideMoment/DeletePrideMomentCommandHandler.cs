using EmployeePrideManagement.Domain.Exceptions;
using EmployeePrideManagement.Domain.Interfaces;
using MediatR;

namespace EmployeePrideManagement.Application.Commands.DeletePrideMoment;

public class DeletePrideMomentCommandHandler : IRequestHandler<DeletePrideMomentCommand, bool>
{
    private readonly IPrideMomentRepository _repository;
    private readonly IMessagePublisher _messagePublisher;

    public DeletePrideMomentCommandHandler(
        IPrideMomentRepository repository,
        IMessagePublisher messagePublisher)
    {
        _repository = repository;
        _messagePublisher = messagePublisher;
    }

    public async Task<bool> Handle(DeletePrideMomentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.MomentPrideId, cancellationToken)
            ?? throw new PrideMomentNotFoundException(request.MomentPrideId);

        await _repository.DeleteAsync(request.MomentPrideId, cancellationToken);

        await _messagePublisher.PublishAsync("pride-moment-deleted", new
        {
            entity.MomentPrideId,
            entity.Title,
            DeletedOn = DateTime.UtcNow
        }, cancellationToken);

        return true;
    }
}
