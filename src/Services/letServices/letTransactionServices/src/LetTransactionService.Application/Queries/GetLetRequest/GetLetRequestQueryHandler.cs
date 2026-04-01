using LetTransactionService.Application.DTOs;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Queries.GetLetRequest;

public class GetLetRequestQueryHandler(ILetRequestRepository repository)
    : IRequestHandler<GetLetRequestQuery, LetMainDto?>
{
    public async Task<LetMainDto?> Handle(GetLetRequestQuery query, CancellationToken ct)
    {
        var letMain = await repository.GetByIdAsync(query.RequestNumber, ct);
        if (letMain is null) return null;

        return new LetMainDto(
            letMain.RequestNumber,
            letMain.FinancialYearSerialNo,
            letMain.EmployeeUserId,
            letMain.SupervisorUserId,
            letMain.RequestDate,
            letMain.SubEntries.Select(s => new LetSubDto(
                s.RequestNumber, s.SerialNumber, s.ModifiedDate, s.ModifiedUser,
                s.PreferredModeDev?.ToString() ?? string.Empty,
                s.ActionTaken, s.CourseId, s.TrainingProgramBhr,
                s.ImpactBenefitProcess, s.MeasureCompetency,
                s.MidYearReviewerName, s.MidYearReviewerDate, s.MidYearReviewerRemark,
                s.AnnualReviewerName, s.AnnualReviewerDate, s.AnnualReviewerRemark,
                s.CompetencyToDevelop, s.DomainKnowledgeDev, s.DomainKnowledgeDevDetail,
                s.ProcessDev, s.ProcessDevDetail,
                s.LetSubCode?.ToString() ?? string.Empty,
                s.ReviewType)));
    }
}
