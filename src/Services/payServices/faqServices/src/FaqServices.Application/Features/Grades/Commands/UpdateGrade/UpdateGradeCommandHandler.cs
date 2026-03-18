using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Grades.Commands.UpdateGrade;

public class UpdateGradeCommandHandler : IRequestHandler<UpdateGradeCommand, FaqGradeDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateGradeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<FaqGradeDto> Handle(UpdateGradeCommand request, CancellationToken ct)
    {
        var grade = await _unitOfWork.FaqGrades.GetByIdAsync(request.Id, ct);
        if (grade == null)
        {
            throw new InvalidOperationException($"Grade with id {request.Id} not found");
        }

        grade.Update(request.GradeName, request.Description, request.SortOrder);
        _unitOfWork.FaqGrades.Update(grade);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<FaqGradeDto>(grade);
    }
}
