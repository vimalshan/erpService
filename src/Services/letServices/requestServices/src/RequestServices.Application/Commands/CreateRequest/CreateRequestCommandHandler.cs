using AutoMapper;
using MediatR;
using RequestServices.Application.DTOs;
using RequestServices.Application.Interfaces;
using RequestServices.Domain.Aggregates;
using RequestServices.Domain.Entities;
using RequestServices.Domain.Interfaces;

namespace RequestServices.Application.Commands.CreateRequest;

public class CreateRequestCommandHandler(
    IRequestRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IDomainEventDispatcher eventDispatcher)
    : IRequestHandler<CreateRequestCommand, RequestMainDto>
{
    public async Task<RequestMainDto> Handle(CreateRequestCommand cmd, CancellationToken ct)
    {
        var aggregate = RequestAggregate.Create(
            cmd.RequestId, cmd.EmployeeUser, cmd.RequestDate, cmd.SupervisorUser);

        var sub = RequestSub.Create(
            cmd.RequestId, cmd.RequestId,   // serial = requestId for first line
            cmd.RequestDate, 'P',
            cmd.TrainingNeed, cmd.CourseId,
            cmd.StartDate, cmd.EndDate,
            cmd.SupervisorUser, cmd.EmployeeUser,
            cmd.BusinessBenefit, cmd.ExpectedCompetency, cmd.CourseDescription);

        aggregate.AddSubRequest(sub);

        await repository.AddAsync(aggregate, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await eventDispatcher.DispatchAsync(aggregate.DomainEvents, ct);
        aggregate.ClearDomainEvents();

        // Build result from aggregate
        var mainDto = new RequestMainDto(
            aggregate.RequestId, aggregate.EmployeeUser,
            aggregate.RequestDate, aggregate.SupervisorUser,
            aggregate.SubRequests.Select(s => mapper.Map<RequestSubDto>(s)));

        return mainDto;
    }
}
