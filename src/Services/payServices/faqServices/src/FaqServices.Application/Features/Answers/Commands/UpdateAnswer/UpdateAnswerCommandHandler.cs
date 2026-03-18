using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Answers.Commands.UpdateAnswer;

public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, FaqAnswerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAnswerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<FaqAnswerDto> Handle(UpdateAnswerCommand request, CancellationToken ct)
    {
        var answer = await _unitOfWork.FaqAnswers.GetByIdAsync(request.Id, ct);
        if (answer == null)
        {
            throw new InvalidOperationException($"Answer with id {request.Id} not found");
        }

        answer.Update(request.AnswerText, request.AnswerTextAr, request.IsCorrect, request.SortOrder);
        _unitOfWork.FaqAnswers.Update(answer);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<FaqAnswerDto>(answer);
    }
}
