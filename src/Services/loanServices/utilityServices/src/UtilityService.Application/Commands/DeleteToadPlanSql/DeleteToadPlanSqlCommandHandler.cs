using MediatR;
using UtilityService.Domain.Interfaces;

namespace UtilityService.Application.Commands.DeleteToadPlanSql;

public class DeleteToadPlanSqlCommandHandler : IRequestHandler<DeleteToadPlanSqlCommand, bool>
{
    private readonly IToadPlanSqlRepository _repository;
    private readonly IPublisher _publisher;

    public DeleteToadPlanSqlCommandHandler(IToadPlanSqlRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<bool> Handle(DeleteToadPlanSqlCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;

        entity.Delete();

        foreach (var domainEvent in entity.DomainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);

        entity.ClearDomainEvents();
        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
