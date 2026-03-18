using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Entities;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Answers.Commands.CreateAnswer;

public class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, FaqAnswerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAnswerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<FaqAnswerDto> Handle(CreateAnswerCommand request, CancellationToken ct)
    {
        // Verify question exists
        var questionExists = await _unitOfWork.FaqQuestions.ExistsAsync(request.QuestionId, ct);
        if (!questionExists)
        {
            throw new InvalidOperationException($"Question with id {request.QuestionId} not found");
        }

        var answer = FaqAnswer.Create(
            request.QuestionId,
            request.AnswerText,
            request.AnswerTextAr,
            request.IsCorrect,
            request.SortOrder);

        await _unitOfWork.FaqAnswers.AddAsync(answer, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<FaqAnswerDto>(answer);
    }
}
