using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Answers.Queries.GetAnswersByQuestionId;

public class GetAnswersByQuestionIdQueryHandler : IRequestHandler<GetAnswersByQuestionIdQuery, IEnumerable<FaqAnswerDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAnswersByQuestionIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<FaqAnswerDto>> Handle(GetAnswersByQuestionIdQuery request, CancellationToken ct)
    {
        var answers = await _unitOfWork.FaqAnswers.GetByQuestionIdAsync(request.QuestionId, ct);
        return _mapper.Map<IEnumerable<FaqAnswerDto>>(answers);
    }
}
