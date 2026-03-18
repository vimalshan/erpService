using MediatR;

namespace RequestServices.Application.Commands.CancelRequest;

public record CancelRequestCommand(
    long   RequestId,
    long   SerialNumber,
    string Remark
) : IRequest<bool>;
