using LetTransactionService.Application.DTOs;
using MediatR;

namespace LetTransactionService.Application.Queries.GetPendingReviews;

public record GetPendingReviewsQuery() : IRequest<IEnumerable<PendingReviewDto>>;
