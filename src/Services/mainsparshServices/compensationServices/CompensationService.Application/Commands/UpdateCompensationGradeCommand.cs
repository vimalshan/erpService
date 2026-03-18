using MediatR;
using CompensationService.Application.DTOs;

namespace CompensationService.Application.Commands;

/// <summary>
/// Command to update a compensation grade
/// </summary>
public class UpdateCompensationGradeCommand : IRequest<CompensationGradeDto>
{
    public long GradeId { get; set; }
    public string GradeName { get; set; } = null!;
    public decimal BaseSalary { get; set; }
    public decimal HraPercentage { get; set; }
    public decimal DaPercentage { get; set; }
    public long UpdatedBy { get; set; }
}

/// <summary>
/// Handler for UpdateCompensationGradeCommand
/// </summary>
public class UpdateCompensationGradeCommandHandler : IRequestHandler<UpdateCompensationGradeCommand, CompensationGradeDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCompensationGradeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CompensationGradeDto> Handle(UpdateCompensationGradeCommand request, CancellationToken cancellationToken)
    {
        var grade = await _unitOfWork.CompensationGrades.GetByIdAsync(request.GradeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Compensation grade with ID {request.GradeId} not found.");

        grade.Update(request.GradeName, request.BaseSalary, request.HraPercentage, request.DaPercentage, request.UpdatedBy);
        await _unitOfWork.CompensationGrades.UpdateAsync(grade, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CompensationGradeDto>(grade);
    }
}
