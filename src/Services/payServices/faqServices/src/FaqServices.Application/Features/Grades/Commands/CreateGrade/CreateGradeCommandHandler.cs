using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Entities;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Grades.Commands.CreateGrade;

public class CreateGradeCommandHandler : IRequestHandler<CreateGradeCommand, FaqGradeDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateGradeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<FaqGradeDto> Handle(CreateGradeCommand request, CancellationToken ct)
    {
        var grade = FaqGrade.Create(request.GradeName, request.Description, request.SortOrder);
        await _unitOfWork.FaqGrades.AddAsync(grade, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<FaqGradeDto>(grade);
    }
}
