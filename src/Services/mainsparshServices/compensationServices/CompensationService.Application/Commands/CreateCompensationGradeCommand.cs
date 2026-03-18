using MediatR;
using CompensationService.Application.DTOs;

namespace CompensationService.Application.Commands;

/// <summary>
/// Command to create a new compensation grade
/// </summary>
public class CreateCompensationGradeCommand : IRequest<CompensationGradeDto>
{
    public string GradeCode { get; set; } = null!;
    public string GradeName { get; set; } = null!;
    public int GradeLevel { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal HraPercentage { get; set; }
    public decimal DaPercentage { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public long CreatedBy { get; set; }
}

/// <summary>
/// Handler for CreateCompensationGradeCommand
/// </summary>
public class CreateCompensationGradeCommandHandler : IRequestHandler<CreateCompensationGradeCommand, CompensationGradeDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompensationGradeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CompensationGradeDto> Handle(CreateCompensationGradeCommand request, CancellationToken cancellationToken)
    {
        // Check if grade code already exists
        var existing = await _unitOfWork.CompensationGrades.GetByCodeAsync(request.GradeCode, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException($"Grade code '{request.GradeCode}' already exists.");

        // Create new grade
        var grade = CompensationService.Domain.Entities.CompensationGrade.Create(
            request.GradeCode,
            request.GradeName,
            request.GradeLevel,
            request.BaseSalary,
            request.HraPercentage,
            request.DaPercentage,
            request.EffectiveFrom,
            request.CreatedBy);

        await _unitOfWork.CompensationGrades.AddAsync(grade, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CompensationGradeDto>(grade);
    }
}
