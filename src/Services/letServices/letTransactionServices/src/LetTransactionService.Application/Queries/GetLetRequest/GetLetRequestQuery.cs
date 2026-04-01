using LetTransactionService.Application.DTOs;
using MediatR;

namespace LetTransactionService.Application.Queries.GetLetRequest;

public record GetLetRequestQuery(long RequestNumber) : IRequest<LetMainDto?>;
