using MediatR;
using CompensationService.Application.DTOs;

namespace CompensationService.Application.Queries;

/// <summary>
/// Query to get all compensation grades
/// </summary>
public class GetAllCompensationGradesQuery : IRequest<IEnumerable<CompensationGradeDto>>
{
}

/// <summary>
/// Handler for GetAllCompensationGradesQuery
/// </summary>
public class GetAllCompensationGradesQueryHandler : IRequestHandler<GetAllCompensationGradesQuery, IEnumerable<CompensationGradeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCompensationGradesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompensationGradeDto>> Handle(GetAllCompensationGradesQuery request, CancellationToken cancellationToken)
    {
        var grades = await _unitOfWork.CompensationGrades.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CompensationGradeDto>>(grades);
    }
}
