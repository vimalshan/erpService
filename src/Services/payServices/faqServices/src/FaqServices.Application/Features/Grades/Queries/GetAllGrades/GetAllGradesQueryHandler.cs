using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Grades.Queries.GetAllGrades;

public class GetAllGradesQueryHandler : IRequestHandler<GetAllGradesQuery, List<FaqGradeDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllGradesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<FaqGradeDto>> Handle(GetAllGradesQuery request, CancellationToken ct)
    {
        var grades = await _unitOfWork.FaqGrades.GetAllAsync(ct);
        return _mapper.Map<List<FaqGradeDto>>(grades);
    }
}
