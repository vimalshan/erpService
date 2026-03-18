using ErrorLoggingService.Application.DTOs;
using ErrorLoggingService.Domain.Repositories;
using MediatR;

namespace ErrorLoggingService.Application.Queries.GetErrorLogs;

public sealed class GetErrorLogsQueryHandler : IRequestHandler<GetErrorLogsQuery, IEnumerable<ErrorLogDto>>
{
    private readonly IErrorLogRepository _repository;

    public GetErrorLogsQueryHandler(IErrorLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ErrorLogDto>> Handle(GetErrorLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repository.GetByDateRangeAsync(request.StartDate, request.EndDate, cancellationToken);

        return logs.Select(l => new ErrorLogDto(
            l.Id,
            l.ErrorMessage,
            l.StoredProcedureName,
            l.ErrorReference,
            l.ErrorDate));
    }
}
