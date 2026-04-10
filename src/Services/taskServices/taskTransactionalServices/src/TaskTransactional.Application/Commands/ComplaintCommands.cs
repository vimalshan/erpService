using MediatR;

namespace TaskTransactional.Application.Commands;

// Complaint Main
public record CreateComplaintMainCommand(
    string UnitCode, string GroupId, string GroupName, decimal GroupSrc,
    string? GroupDesc = null, string? BehalfFlg = null, decimal? BehalfPin = null,
    decimal? RegPin = null, string? Shift = null, string? Mail = null) : IRequest<string>;

public record UpdateComplaintMainCommand(
    string GroupId, string GroupName, string? GroupDesc, string? Mail, string UpdatedBy) : IRequest<bool>;

public record DeleteComplaintMainCommand(string GroupId) : IRequest<bool>;

// Complaint Detail (Ticket)
public record CreateTicketCommand(
    decimal GroupId, decimal Type, decimal Location, decimal Department,
    decimal Process, string TargetDate,
    string? Subject = null, string? Description = null, string? Ncr = null) : IRequest<decimal>;

public record CloseTicketCommand(decimal TicketNum) : IRequest<bool>;

// Complaint Task
public record CreateComplaintTaskCommand(
    decimal TicketNum, string ScheduleFreq,
    string? ScheduleValue = null, string? ScheduleTime = null, string? ScheduleDay = null) : IRequest<decimal>;

public record CloseComplaintTaskCommand(decimal TaskNum, decimal UpdatedBy) : IRequest<bool>;

// Complaint Action
public record CreateActionCommand(decimal TaskNum) : IRequest<decimal>;

public record UpdatePrimaryActionCommand(
    decimal ActionNum, string? Resp, decimal ActBy, string? Solution) : IRequest<bool>;

public record UpdateSecondaryActionCommand(
    decimal ActionNum, string? Resp, decimal ActBy, string? Solution, decimal? EscHrs) : IRequest<bool>;

public record UpdateForwardActionCommand(
    decimal ActionNum, string? Remarks, string? Resp, decimal ActBy, string? Solution) : IRequest<bool>;

public record UpdateCorrectiveActionCommand(
    decimal ActionNum, string? ActReq, string? Remarks, string? Resp, decimal ActBy, string? Solution) : IRequest<bool>;

public record CloseActionCommand(decimal ActionNum) : IRequest<bool>;

public record ReopenActionCommand(decimal ActionNum, string? Remarks) : IRequest<bool>;

// Complaint History
public record CreateHistoryCommand(
    decimal ActionNum, string From, string To, string ActionType, string? Remarks = null) : IRequest<decimal>;

// Complaint Escalation
public record CreateEscalationCommand(
    decimal TicketNum, decimal LevelNum, decimal EscNoHrs, decimal UserPin) : IRequest<bool>;

public record CloseEscalationCommand(decimal TicketNum, decimal LevelNum, decimal UpdatedBy) : IRequest<bool>;
