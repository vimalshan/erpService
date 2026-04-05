using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Features.StoredProcedures.Commands;

public class ProcessMonthlyStipendSpCommandHandler : IRequestHandler<ProcessMonthlyStipendSpCommand, StoredProcResultDto>
{
    private readonly ITransactionDapperRepository _dapper;

    public ProcessMonthlyStipendSpCommandHandler(ITransactionDapperRepository dapper) => _dapper = dapper;

    public async Task<StoredProcResultDto> Handle(ProcessMonthlyStipendSpCommand request, CancellationToken cancellationToken)
    {
        var rowsProcessed = await _dapper.ProcessMonthlyStipendAsync(request.Month, request.Year, request.ProcessedBy, cancellationToken);
        return new StoredProcResultDto(true, $"Processed {rowsProcessed} stipend records for {request.Month}/{request.Year}.", null);
    }
}
