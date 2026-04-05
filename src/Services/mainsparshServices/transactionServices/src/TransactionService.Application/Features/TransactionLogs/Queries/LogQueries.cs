using MediatR;
using TransactionService.Application.DTOs;

namespace TransactionService.Application.Features.TransactionLogs.Queries;

public record GetTransactionLogByIdQuery(long LogId) : IRequest<TransactionLogDto?>;
public record GetTransactionLogsByEntityQuery(string TransactionType, long TransactionId) : IRequest<IEnumerable<TransactionLogDto>>;
public record GetTransactionLogsByActionQuery(string Action) : IRequest<IEnumerable<TransactionLogDto>>;
public record GetAllTransactionLogsQuery() : IRequest<IEnumerable<TransactionLogDto>>;
