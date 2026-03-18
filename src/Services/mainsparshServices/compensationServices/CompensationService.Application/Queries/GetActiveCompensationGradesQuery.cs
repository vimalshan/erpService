using MediatR;
using CompensationService.Application.DTOs;

namespace CompensationService.Application.Queries;

/// <summary>
/// Query to get active compensation grades
/// </summary>
public class GetActiveCompensationGradesQuery : IRequest<IEnumerable<CompensationGradeDto>>
{
}

/// <summary>
/// Handler for GetActiveCompensationGradesQuery
/// </summary>
public class GetActiveCompensationGradesQueryHandler : IRequestHandler<GetActiveCompensationGradesQuery, IEnumerable<CompensationGradeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetActiveCompensationGradesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompensationGradeDto>> Handle(GetActiveCompensationGradesQuery request, CancellationToken cancellationToken)
    {
        var grades = await _unitOfWork.CompensationGrades.GetActiveAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CompensationGradeDto>>(grades);
    }
}
