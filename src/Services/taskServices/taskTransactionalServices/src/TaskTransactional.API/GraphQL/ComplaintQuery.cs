using TaskTransactional.Application.DTOs;
using TaskTransactional.Application.Queries;
using MediatR;

namespace TaskTransactional.API.GraphQL;

public class ComplaintQuery
{
    public async Task<IEnumerable<ComplaintMainDto>> GetComplaints([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllComplaintMainsQuery(), ct);

    public async Task<ComplaintMainDto?> GetComplaintByGroupId(string groupId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetComplaintMainByGroupIdQuery(groupId), ct);

    public async Task<IEnumerable<ComplaintMainDto>> GetComplaintsByUnitCode(string unitCode, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetComplaintMainsByUnitCodeQuery(unitCode), ct);

    public async Task<IEnumerable<ComplaintDetailDto>> GetTickets([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllTicketsQuery(), ct);

    public async Task<ComplaintDetailDto?> GetTicketByNum(decimal ticketNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetTicketByNumQuery(ticketNum), ct);

    public async Task<IEnumerable<ComplaintDetailDto>> GetTicketsByGroupId(decimal groupId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetTicketsByGroupIdQuery(groupId), ct);

    public async Task<IEnumerable<ComplaintActionDto>> GetActions([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllActionsQuery(), ct);

    public async Task<ComplaintActionDto?> GetActionByNum(decimal actionNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetActionByNumQuery(actionNum), ct);

    public async Task<IEnumerable<ComplaintHistoryDto>> GetHistoryByActionNum(decimal actionNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetHistoryByActionNumQuery(actionNum), ct);

    public async Task<IEnumerable<ComplaintEscalationDto>> GetEscalationsByTicketNum(decimal ticketNum, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetEscalationsByTicketNumQuery(ticketNum), ct);
}
