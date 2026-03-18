using MediatR;

namespace RequestServices.Application.Commands.ApproveRequest;

public record ApproveRequestCommand(
    long   RequestId,
    long   SerialNumber,
    long   ApprovalNumber,
    string ApprovalRemark,
    string ApprovalUser
) : IRequest<bool>;
