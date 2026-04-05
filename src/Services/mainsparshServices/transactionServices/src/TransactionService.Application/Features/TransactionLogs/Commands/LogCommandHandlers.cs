using AutoMapper;
using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Features.TransactionLogs.Commands;

public class LogTransactionCommandHandler : IRequestHandler<LogTransactionCommand, TransactionLogDto>
{
    private readonly ITransactionLogRepository _repository;
    private readonly IMapper _mapper;

    public LogTransactionCommandHandler(ITransactionLogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TransactionLogDto> Handle(LogTransactionCommand request, CancellationToken cancellationToken)
    {
        var log = TransactionLog.Create(
            request.TransactionType,
            request.TransactionId,
            request.Action,
            request.ActionBy,
            request.ActionData,
            request.PreviousStatus,
            request.NewStatus,
            request.IpAddress);

        await _repository.AddAsync(log, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TransactionLogDto>(log);
    }
}
