using ErrorLoggingService.Application.DTOs;
using MediatR;

namespace ErrorLoggingService.Application.Queries.GetErrorLogs;

public record GetErrorLogsQuery(
    DateTime StartDate,
    DateTime EndDate
) : IRequest<IEnumerable<ErrorLogDto>>;
