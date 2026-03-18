using AutoMapper;
using MediatR;
using UtilityService.Application.DTOs;
using UtilityService.Domain.Entities;
using UtilityService.Domain.Interfaces;

namespace UtilityService.Application.Commands.CreateToadPlanSql;

public class CreateToadPlanSqlCommandHandler : IRequestHandler<CreateToadPlanSqlCommand, ToadPlanSqlDto>
{
    private readonly IToadPlanSqlRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;

    public CreateToadPlanSqlCommandHandler(
        IToadPlanSqlRepository repository,
        IMapper mapper,
        IPublisher publisher)
    {
        _repository = repository;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<ToadPlanSqlDto> Handle(CreateToadPlanSqlCommand request, CancellationToken cancellationToken)
    {
        var entity = ToadPlanSql.Create(request.Username, request.StatementId, request.Statement, request.Timestamp);

        await _repository.AddAsync(entity, cancellationToken);

        foreach (var domainEvent in entity.DomainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);

        entity.ClearDomainEvents();

        return _mapper.Map<ToadPlanSqlDto>(entity);
    }
}
