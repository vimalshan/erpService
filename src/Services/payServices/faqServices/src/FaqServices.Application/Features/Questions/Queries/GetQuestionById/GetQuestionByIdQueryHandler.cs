using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Questions.Queries.GetQuestionById;

public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, FaqQuestionDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetQuestionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<FaqQuestionDto?> Handle(GetQuestionByIdQuery request, CancellationToken ct)
    {
        var question = await _unitOfWork.FaqQuestions.GetByIdWithAnswersAsync(request.Id, ct);
        return question != null ? _mapper.Map<FaqQuestionDto>(question) : null;
    }
}
