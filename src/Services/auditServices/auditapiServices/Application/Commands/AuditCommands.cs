using AuditService.Application.DTOs;
using MediatR;

namespace AuditService.Application.Commands;

public record CreateAuditCommand(CreateAuditDto Dto) : IRequest<AuditDto>;
public record UpdateAuditCommand(UpdateAuditDto Dto) : IRequest<AuditDto>;
public record DeleteAuditCommand(int Id) : IRequest<bool>;
public record ChangeAuditStatusCommand(int AuditId, string NewStatus) : IRequest<bool>;
