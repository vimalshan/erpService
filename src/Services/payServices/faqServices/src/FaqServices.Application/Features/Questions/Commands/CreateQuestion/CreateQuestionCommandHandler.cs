using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Entities;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, FaqQuestionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateQuestionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<FaqQuestionDto> Handle(CreateQuestionCommand request, CancellationToken ct)
    {
        // Verify grade exists
        var gradeExists = await _unitOfWork.FaqGrades.ExistsAsync(request.GradeId, ct);
        if (!gradeExists)
        {
            throw new InvalidOperationException($"Grade with id {request.GradeId} not found");
        }

        var question = FaqQuestion.Create(
            request.GradeId,
            request.QuestionText,
            request.QuestionTextAr,
            request.SortOrder);

        await _unitOfWork.FaqQuestions.AddAsync(question, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<FaqQuestionDto>(question);
    }
}
