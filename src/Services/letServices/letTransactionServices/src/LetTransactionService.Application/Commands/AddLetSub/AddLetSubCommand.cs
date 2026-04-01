using MediatR;

namespace LetTransactionService.Application.Commands.AddLetSub;

public record AddLetSubCommand(
    long RequestNumber,
    int SerialNumber,
    char? PreferredModeDev,
    string? ActionTaken,
    int? CourseId,
    string? TrainingProgramBhr,
    string? ImpactBenefitProcess,
    string? MeasureCompetency,
    int? CompetencyToDevelop,
    string? DomainKnowledgeDev,
    string? DomainKnowledgeDevDetail,
    string? ProcessDev,
    string? ProcessDevDetail,
    char? LetSubCode,
    string? ReviewType
) : IRequest<bool>;
