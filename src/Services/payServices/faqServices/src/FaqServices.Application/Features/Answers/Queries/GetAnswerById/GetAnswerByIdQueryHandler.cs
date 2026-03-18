using AutoMapper;
using FaqServices.Application.Common.DTOs;
using FaqServices.Domain.Interfaces;
using MediatR;

namespace FaqServices.Application.Features.Answers.Queries.GetAnswerById;

public class GetAnswerByIdQueryHandler : IRequestHandler<GetAnswerByIdQuery, FaqAnswerDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAnswerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<FaqAnswerDto?> Handle(GetAnswerByIdQuery request, CancellationToken ct)
    {
        var answer = await _unitOfWork.FaqAnswers.GetByIdAsync(request.Id, ct);
        return answer != null ? _mapper.Map<FaqAnswerDto>(answer) : null;
    }
}
