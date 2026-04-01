using LetTransactionService.Domain.Exceptions;
using LetTransactionService.Domain.Interfaces;
using MediatR;

namespace LetTransactionService.Application.Commands.AddLetSub;

public class AddLetSubCommandHandler(ILetRequestRepository repository)
    : IRequestHandler<AddLetSubCommand, bool>
{
    public async Task<bool> Handle(AddLetSubCommand cmd, CancellationToken ct)
    {
        var letMain = await repository.GetByIdAsync(cmd.RequestNumber, ct)
            ?? throw new LetNotFoundException("LetMain", cmd.RequestNumber);

        letMain.AddSubEntry(
            cmd.SerialNumber, cmd.PreferredModeDev, cmd.ActionTaken,
            cmd.CourseId, cmd.TrainingProgramBhr, cmd.ImpactBenefitProcess,
            cmd.MeasureCompetency, cmd.CompetencyToDevelop, cmd.DomainKnowledgeDev,
            cmd.DomainKnowledgeDevDetail, cmd.ProcessDev, cmd.ProcessDevDetail,
            cmd.LetSubCode, cmd.ReviewType);

        await repository.UpdateAsync(letMain, ct);
        return true;
    }
}
