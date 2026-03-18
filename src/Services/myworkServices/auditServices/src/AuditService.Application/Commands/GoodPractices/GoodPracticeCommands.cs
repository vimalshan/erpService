using AuditService.Application.DTOs;
using MediatR;

namespace AuditService.Application.Commands.GoodPractices;

public record CreateGoodPracticeCommand(
    long PracticeId,
    string PracticeTitle,
    string PracticeDescription,
    string PracticeBenefits,
    string PracticeRemarks,
    long PracticeProcess,
    long PracticeEmpSysId,
    long PracticeUnit,
    long CreatedBy
) : IRequest<GoodPracticeDto>;

public record RateGoodPracticeCommand(
    long PracticeId,
    long RatingId,
    long RatedBy,
    int Rating
) : IRequest<bool>;
