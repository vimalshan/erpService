using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Questions.Queries.GetAllQuestions;

public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, IEnumerable<FaqQuestionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllQuestionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<FaqQuestionDto>> Handle(GetAllQuestionsQuery request, CancellationToken ct)
    {
        var questions = await _unitOfWork.FaqQuestions.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<FaqQuestionDto>>(questions);
    }
}
