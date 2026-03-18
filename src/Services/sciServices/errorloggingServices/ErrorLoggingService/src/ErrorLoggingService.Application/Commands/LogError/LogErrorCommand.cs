using MediatR;

namespace ErrorLoggingService.Application.Commands.LogError;

public record LogErrorCommand(
    string ErrorMessage,
    string StoredProcedureName,
    int? ErrorReference
) : IRequest<int>;
