using MediatR;

namespace AuditService.Application.Commands.Audits;

public record UpdateAuditCommand(
    long AuditId,
    string AuditName,
    string AuditDefLocation,
    DateTime AuditFrom,
    DateTime AuditTo,
    decimal UpdatedBy
) : IRequest<bool>;
