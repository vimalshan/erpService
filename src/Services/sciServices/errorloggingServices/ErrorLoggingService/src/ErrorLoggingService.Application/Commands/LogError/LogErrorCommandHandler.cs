using ErrorLoggingService.Domain.Entities;
using ErrorLoggingService.Domain.Repositories;
using MediatR;

namespace ErrorLoggingService.Application.Commands.LogError;

public sealed class LogErrorCommandHandler : IRequestHandler<LogErrorCommand, int>
{
    private readonly IErrorLogRepository _repository;

    public LogErrorCommandHandler(IErrorLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(LogErrorCommand request, CancellationToken cancellationToken)
    {
        var errorLog = ErrorLog.Create(
            request.ErrorMessage,
            request.StoredProcedureName,
            request.ErrorReference);

        await _repository.AddAsync(errorLog, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return errorLog.Id;
    }
}
