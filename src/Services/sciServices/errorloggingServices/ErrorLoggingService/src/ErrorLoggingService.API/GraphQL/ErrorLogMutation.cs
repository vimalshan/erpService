using ErrorLoggingService.Application.Commands.LogError;
using MediatR;

namespace ErrorLoggingService.API.GraphQL;

public class ErrorLogMutation
{
    public async Task<int> LogError(
        string errorMessage,
        string storedProcedureName,
        int? errorReference,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new LogErrorCommand(errorMessage, storedProcedureName, errorReference),
            cancellationToken);
    }
}
