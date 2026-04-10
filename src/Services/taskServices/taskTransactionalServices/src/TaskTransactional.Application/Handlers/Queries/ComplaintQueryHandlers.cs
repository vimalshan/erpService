using AutoMapper;
using TaskTransactional.Application.DTOs;
using TaskTransactional.Application.Queries;
using TaskTransactional.Domain.Interfaces;
using MediatR;

namespace TaskTransactional.Application.Handlers.Queries;

// Complaint Main Query Handlers
public class GetAllComplaintMainsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllComplaintMainsQuery, IEnumerable<ComplaintMainDto>>
{
    public async Task<IEnumerable<ComplaintMainDto>> Handle(GetAllComplaintMainsQuery request, CancellationToken ct)
    {
        var entities = await uow.ComplaintMains.GetAllAsync(ct);
        return mapper.Map<IEnumerable<ComplaintMainDto>>(entities);
    }
}

public class GetComplaintMainByGroupIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetComplaintMainByGroupIdQuery, ComplaintMainDto?>
{
    public async Task<ComplaintMainDto?> Handle(GetComplaintMainByGroupIdQuery request, CancellationToken ct)
    {
        var entity = await uow.ComplaintMains.GetByGroupIdAsync(request.GroupId, ct);
        return entity is null ? null : mapper.Map<ComplaintMainDto>(entity);
    }
}

public class GetComplaintMainsByUnitCodeHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetComplaintMainsByUnitCodeQuery, IEnumerable<ComplaintMainDto>>
{
    public async Task<IEnumerable<ComplaintMainDto>> Handle(GetComplaintMainsByUnitCodeQuery request, CancellationToken ct)
    {
        var entities = await uow.ComplaintMains.GetByUnitCodeAsync(request.UnitCode, ct);
        return mapper.Map<IEnumerable<ComplaintMainDto>>(entities);
    }
}

// Ticket Query Handlers
public class GetAllTicketsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllTicketsQuery, IEnumerable<ComplaintDetailDto>>
{
    public async Task<IEnumerable<ComplaintDetailDto>> Handle(GetAllTicketsQuery request, CancellationToken ct)
    {
        var entities = await uow.ComplaintDetails.GetAllAsync(ct);
        return mapper.Map<IEnumerable<ComplaintDetailDto>>(entities);
    }
}

public class GetTicketByNumHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetTicketByNumQuery, ComplaintDetailDto?>
{
    public async Task<ComplaintDetailDto?> Handle(GetTicketByNumQuery request, CancellationToken ct)
    {
        var entity = await uow.ComplaintDetails.GetByTicketNumAsync(request.TicketNum, ct);
        return entity is null ? null : mapper.Map<ComplaintDetailDto>(entity);
    }
}

public class GetTicketsByGroupIdHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetTicketsByGroupIdQuery, IEnumerable<ComplaintDetailDto>>
{
    public async Task<IEnumerable<ComplaintDetailDto>> Handle(GetTicketsByGroupIdQuery request, CancellationToken ct)
    {
        var entities = await uow.ComplaintDetails.GetByGroupIdAsync(request.GroupId, ct);
        return mapper.Map<IEnumerable<ComplaintDetailDto>>(entities);
    }
}

// Task Query Handlers
public class GetTaskByNumHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetTaskByNumQuery, ComplaintTaskDto?>
{
    public async Task<ComplaintTaskDto?> Handle(GetTaskByNumQuery request, CancellationToken ct)
    {
        var entity = await uow.ComplaintTasks.GetByTaskNumAsync(request.TaskNum, ct);
        return entity is null ? null : mapper.Map<ComplaintTaskDto>(entity);
    }
}

public class GetTasksByTicketNumHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetTasksByTicketNumQuery, IEnumerable<ComplaintTaskDto>>
{
    public async Task<IEnumerable<ComplaintTaskDto>> Handle(GetTasksByTicketNumQuery request, CancellationToken ct)
    {
        var entities = await uow.ComplaintTasks.GetByTicketNumAsync(request.TicketNum, ct);
        return mapper.Map<IEnumerable<ComplaintTaskDto>>(entities);
    }
}

// Action Query Handlers
public class GetAllActionsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetAllActionsQuery, IEnumerable<ComplaintActionDto>>
{
    public async Task<IEnumerable<ComplaintActionDto>> Handle(GetAllActionsQuery request, CancellationToken ct)
    {
        var entities = await uow.ComplaintActions.GetAllAsync(ct);
        return mapper.Map<IEnumerable<ComplaintActionDto>>(entities);
    }
}

public class GetActionByNumHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetActionByNumQuery, ComplaintActionDto?>
{
    public async Task<ComplaintActionDto?> Handle(GetActionByNumQuery request, CancellationToken ct)
    {
        var entity = await uow.ComplaintActions.GetByActionNumAsync(request.ActionNum, ct);
        return entity is null ? null : mapper.Map<ComplaintActionDto>(entity);
    }
}

public class GetActionByTaskNumHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetActionByTaskNumQuery, ComplaintActionDto?>
{
    public async Task<ComplaintActionDto?> Handle(GetActionByTaskNumQuery request, CancellationToken ct)
    {
        var entity = await uow.ComplaintActions.GetByTaskNumAsync(request.TaskNum, ct);
        return entity is null ? null : mapper.Map<ComplaintActionDto>(entity);
    }
}

// History Query Handler
public class GetHistoryByActionNumHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetHistoryByActionNumQuery, IEnumerable<ComplaintHistoryDto>>
{
    public async Task<IEnumerable<ComplaintHistoryDto>> Handle(GetHistoryByActionNumQuery request, CancellationToken ct)
    {
        var entities = await uow.ComplaintHistories.GetByActionNumAsync(request.ActionNum, ct);
        return mapper.Map<IEnumerable<ComplaintHistoryDto>>(entities);
    }
}

// Escalation Query Handler
public class GetEscalationsByTicketNumHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetEscalationsByTicketNumQuery, IEnumerable<ComplaintEscalationDto>>
{
    public async Task<IEnumerable<ComplaintEscalationDto>> Handle(GetEscalationsByTicketNumQuery request, CancellationToken ct)
    {
        var entities = await uow.ComplaintEscalations.GetByTicketNumAsync(request.TicketNum, ct);
        return mapper.Map<IEnumerable<ComplaintEscalationDto>>(entities);
    }
}
