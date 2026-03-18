using MediatR;
using CompensationService.Application.DTOs;

namespace CompensationService.Application.Queries;

/// <summary>
/// Query to get compensation grade by ID
/// </summary>
public class GetCompensationGradeByIdQuery : IRequest<CompensationGradeDto>
{
    public long GradeId { get; set; }
}

/// <summary>
/// Handler for GetCompensationGradeByIdQuery
/// </summary>
public class GetCompensationGradeByIdQueryHandler : IRequestHandler<GetCompensationGradeByIdQuery, CompensationGradeDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompensationGradeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CompensationGradeDto> Handle(GetCompensationGradeByIdQuery request, CancellationToken cancellationToken)
    {
        var grade = await _unitOfWork.CompensationGrades.GetByIdAsync(request.GradeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Compensation grade with ID {request.GradeId} not found.");

        return _mapper.Map<CompensationGradeDto>(grade);
    }
}
