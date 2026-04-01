using LetTransactionService.Application.DTOs;
using MediatR;

namespace LetTransactionService.Application.Queries.GetReview;

public record GetReviewQuery(long ReviewSerialNumber) : IRequest<ReviewMainDto?>;
