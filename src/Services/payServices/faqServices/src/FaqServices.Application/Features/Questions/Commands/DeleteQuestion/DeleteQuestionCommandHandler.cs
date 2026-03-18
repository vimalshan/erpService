using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken ct)
    {
        var question = await _unitOfWork.FaqQuestions.GetByIdAsync(request.Id, ct);
        if (question == null)
        {
            return false;
        }

        question.MarkDeleted();
        _unitOfWork.FaqQuestions.Remove(question);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
