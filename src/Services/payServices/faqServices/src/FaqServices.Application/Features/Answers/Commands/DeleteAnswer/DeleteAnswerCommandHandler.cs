using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Answers.Commands.DeleteAnswer;

public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAnswerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(DeleteAnswerCommand request, CancellationToken ct)
    {
        var answer = await _unitOfWork.FaqAnswers.GetByIdAsync(request.Id, ct);
        if (answer == null)
        {
            return false;
        }

        answer.MarkDeleted();
        _unitOfWork.FaqAnswers.Remove(answer);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
