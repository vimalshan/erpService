using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Grades.Queries.GetGradeById;

public class GetGradeByIdQueryHandler : IRequestHandler<GetGradeByIdQuery, FaqGradeDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGradeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<FaqGradeDto?> Handle(GetGradeByIdQuery request, CancellationToken ct)
    {
        var grade = await _unitOfWork.FaqGrades.GetByIdAsync(request.Id, ct);
        return grade != null ? _mapper.Map<FaqGradeDto>(grade) : null;
    }
}
