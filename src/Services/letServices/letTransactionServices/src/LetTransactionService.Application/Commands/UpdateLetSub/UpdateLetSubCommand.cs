using MediatR;

namespace LetTransactionService.Application.Commands.UpdateLetSub;

public record UpdateLetSubCommand(
    long RequestNumber,
    int SerialNumber,
    string? MidYearReviewerName,
    string? MidYearReviewerDate,
    string? MidYearReviewerRemark,
    string? AnnualReviewerName,
    string? AnnualReviewerDate,
    string? AnnualReviewerRemark
) : IRequest<bool>;
