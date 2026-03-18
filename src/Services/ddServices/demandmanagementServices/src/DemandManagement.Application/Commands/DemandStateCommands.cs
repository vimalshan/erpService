using MediatR;

namespace DemandManagement.Application.Commands;

public record ApproveDemandCommand(long DemandId, long ApprovedBy, string Remarks) : IRequest<bool>;
public record RejectDemandCommand(long DemandId, long RejectedBy, string Remarks) : IRequest<bool>;
public record CompleteDemandCommand(long DemandId, long CompletedBy, string Remarks) : IRequest<bool>;
