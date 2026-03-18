using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, FaqQuestionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<FaqQuestionDto> Handle(UpdateQuestionCommand request, CancellationToken ct)
    {
        var question = await _unitOfWork.FaqQuestions.GetByIdAsync(request.Id, ct);
        if (question == null)
        {
            throw new InvalidOperationException($"Question with id {request.Id} not found");
        }

        question.Update(request.QuestionText, request.QuestionTextAr, request.SortOrder);
        _unitOfWork.FaqQuestions.Update(question);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<FaqQuestionDto>(question);
    }
}
