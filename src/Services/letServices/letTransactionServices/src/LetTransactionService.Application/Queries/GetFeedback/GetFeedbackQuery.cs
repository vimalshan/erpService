using LetTransactionService.Application.DTOs;
using MediatR;

namespace LetTransactionService.Application.Queries.GetFeedback;

public record GetFeedbackQuery(long FeedbackNumber) : IRequest<FeedbackMainDto?>;
