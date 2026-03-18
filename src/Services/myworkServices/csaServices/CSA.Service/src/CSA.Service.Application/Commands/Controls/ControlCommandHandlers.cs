using AutoMapper;
using CSA.Service.Application.DTOs;
using CSA.Service.Domain.Aggregates;
using CSA.Service.Domain.Entities;
using CSA.Service.Domain.Events;
using CSA.Service.Domain.Interfaces;
using MediatR;

namespace CSA.Service.Application.Commands.Controls;

public class CreateControlCommandHandler(
    IControlRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IMediator mediator)
    : IRequestHandler<CreateControlCommand, ControlDto>
{
    public async Task<ControlDto> Handle(CreateControlCommand request, CancellationToken ct)
    {
        var control = mapper.Map<Control>(request.Dto);
        control.CreatedBy = request.UserId;
        control.CreatedOn = DateTime.UtcNow;

        var aggregate = ControlAggregate.Create(control);
        var created = await repository.AddAsync(aggregate.Control, ct);
        await unitOfWork.SaveChangesAsync(ct);

        foreach (var domainEvent in created.DomainEvents)
            await mediator.Publish(domainEvent, ct);
        created.ClearDomainEvents();

        return mapper.Map<ControlDto>(created);
    }
}

public class UpdateControlCommandHandler(
    IControlRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IMediator mediator)
    : IRequestHandler<UpdateControlCommand, ControlDto>
{
    public async Task<ControlDto> Handle(UpdateControlCommand request, CancellationToken ct)
    {
        var control = await repository.GetByIdAsync(request.Dto.ControlId, ct)
            ?? throw new KeyNotFoundException($"Control {request.Dto.ControlId} not found.");

        mapper.Map(request.Dto, control);
        control.ModifiedBy = request.UserId;
        control.ModifiedOn = DateTime.UtcNow;
        control.AddDomainEvent(new ControlUpdatedEvent(control.ControlId, control.Title));

        await repository.UpdateAsync(control, ct);
        await unitOfWork.SaveChangesAsync(ct);

        foreach (var domainEvent in control.DomainEvents)
            await mediator.Publish(domainEvent, ct);
        control.ClearDomainEvents();

        return mapper.Map<ControlDto>(control);
    }
}

public class DeleteControlCommandHandler(
    IControlRepository repository,
    IUnitOfWork unitOfWork,
    IMediator mediator)
    : IRequestHandler<DeleteControlCommand, bool>
{
    public async Task<bool> Handle(DeleteControlCommand request, CancellationToken ct)
    {
        await repository.DeleteAsync(request.ControlId, ct);
        await unitOfWork.SaveChangesAsync(ct);
        await mediator.Publish(new ControlDeletedEvent(request.ControlId), ct);
        return true;
    }
}
