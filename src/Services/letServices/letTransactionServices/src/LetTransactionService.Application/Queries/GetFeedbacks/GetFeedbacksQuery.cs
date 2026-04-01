using LetTransactionService.Application.DTOs;
using MediatR;

namespace LetTransactionService.Application.Queries.GetFeedbacks;

public record GetFeedbacksQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<FeedbackSummaryDto>>;
