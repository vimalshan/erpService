using TaskTransactional.Application.DTOs;
using MediatR;

namespace TaskTransactional.Application.Queries;

// Complaint Main
public record GetAllComplaintMainsQuery : IRequest<IEnumerable<ComplaintMainDto>>;
public record GetComplaintMainByGroupIdQuery(string GroupId) : IRequest<ComplaintMainDto?>;
public record GetComplaintMainsByUnitCodeQuery(string UnitCode) : IRequest<IEnumerable<ComplaintMainDto>>;

// Complaint Detail (Ticket)
public record GetAllTicketsQuery : IRequest<IEnumerable<ComplaintDetailDto>>;
public record GetTicketByNumQuery(decimal TicketNum) : IRequest<ComplaintDetailDto?>;
public record GetTicketsByGroupIdQuery(decimal GroupId) : IRequest<IEnumerable<ComplaintDetailDto>>;

// Complaint Task
public record GetTaskByNumQuery(decimal TaskNum) : IRequest<ComplaintTaskDto?>;
public record GetTasksByTicketNumQuery(decimal TicketNum) : IRequest<IEnumerable<ComplaintTaskDto>>;

// Complaint Action
public record GetAllActionsQuery : IRequest<IEnumerable<ComplaintActionDto>>;
public record GetActionByNumQuery(decimal ActionNum) : IRequest<ComplaintActionDto?>;
public record GetActionByTaskNumQuery(decimal TaskNum) : IRequest<ComplaintActionDto?>;

// Complaint History
public record GetHistoryByActionNumQuery(decimal ActionNum) : IRequest<IEnumerable<ComplaintHistoryDto>>;

// Complaint Escalation
public record GetEscalationsByTicketNumQuery(decimal TicketNum) : IRequest<IEnumerable<ComplaintEscalationDto>>;
