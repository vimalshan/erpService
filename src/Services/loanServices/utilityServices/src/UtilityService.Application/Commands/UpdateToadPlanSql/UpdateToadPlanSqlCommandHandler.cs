using MediatR;
using UtilityService.Domain.Interfaces;

namespace UtilityService.Application.Commands.UpdateToadPlanSql;

public class UpdateToadPlanSqlCommandHandler : IRequestHandler<UpdateToadPlanSqlCommand, bool>
{
    private readonly IToadPlanSqlRepository _repository;
    private readonly IPublisher _publisher;

    public UpdateToadPlanSqlCommandHandler(IToadPlanSqlRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<bool> Handle(UpdateToadPlanSqlCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;

        entity.Update(request.Username, request.Statement, request.Timestamp);
        await _repository.UpdateAsync(entity, cancellationToken);

        foreach (var domainEvent in entity.DomainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);

        entity.ClearDomainEvents();
        return true;
    }
}
