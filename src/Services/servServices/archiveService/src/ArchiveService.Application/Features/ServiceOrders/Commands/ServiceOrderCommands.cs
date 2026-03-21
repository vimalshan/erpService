using MediatR;

namespace ArchiveService.Application.Features.ServiceOrders.Commands;

public record CreateServiceOrderCommand(
    string SernoDell,
    string? Branch,
    string? SapLogin,
    DateTime? PostingDate,
    string? SapId,
    string? Sla,
    string? ProductId,
    string? ServiceTag,
    string? RelatedCase,
    string? Lob,
    string? CallStatus,
    string? CurrentRc,
    string? EngineerId,
    string? EngineerName,
    string? EngMobNo,
    string? OrgName,
    string? CustomerName,
    string? ContactNo,
    string? Address,
    string? AltContactNo,
    DateTime? DispatchDate,
    DateTime? CustEtaDate,
    DateTime? PartEtaDate,
    string? TechSupName,
    string? Dsp,
    string? ProblemDescription,
    string? LongDescription,
    string? ReasonCode,
    string? Activity,
    DateTime? OnsiteDate,
    DateTime? CompletedDate,
    string? Flag,
    string? EnteredBy) : IRequest<string>;

public record UpdateServiceOrderStatusCommand(
    string SernoDell,
    string? CallStatus,
    string? ReasonCode,
    string? ChangedBy) : IRequest<bool>;

public record DeleteServiceOrderCommand(string SernoDell) : IRequest<bool>;
