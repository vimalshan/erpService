using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Grades.Commands.DeleteGrade;

public class DeleteGradeCommandHandler : IRequestHandler<DeleteGradeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGradeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(DeleteGradeCommand request, CancellationToken ct)
    {
        var grade = await _unitOfWork.FaqGrades.GetByIdAsync(request.Id, ct);
        if (grade == null)
        {
            return false;
        }

        grade.MarkDeleted();
        _unitOfWork.FaqGrades.Remove(grade);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
