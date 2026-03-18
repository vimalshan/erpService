using ErrorLoggingService.Application.DTOs;
using ErrorLoggingService.Application.Queries.GetErrorLogs;
using MediatR;

namespace ErrorLoggingService.API.GraphQL;

public class ErrorLogQuery
{
    public async Task<IEnumerable<ErrorLogDto>> GetErrorLogs(
        DateTime startDate,
        DateTime endDate,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetErrorLogsQuery(startDate, endDate), cancellationToken);
    }
}
