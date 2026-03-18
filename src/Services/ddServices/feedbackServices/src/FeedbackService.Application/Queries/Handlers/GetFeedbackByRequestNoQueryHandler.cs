namespace FeedbackService.Application.Queries.Handlers;

using MediatR;
using DTOs;
using AutoMapper;
using Commands.Handlers;

/// <summary>
/// Handler for GetFeedbackByRequestNoQuery
/// </summary>
public class GetFeedbackByRequestNoQueryHandler : IRequestHandler<GetFeedbackByRequestNoQuery, List<FeedbackDto>>
{
    private readonly IFeedbackRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetFeedbackByRequestNoQueryHandler class
    /// </summary>
    public GetFeedbackByRequestNoQueryHandler(IFeedbackRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetFeedbackByRequestNoQuery
    /// </summary>
    public async Task<List<FeedbackDto>> Handle(GetFeedbackByRequestNoQuery request, CancellationToken cancellationToken)
    {
        var feedbackList = await _repository.GetByRequestNoAsync(request.RequestNo, cancellationToken);
        return _mapper.Map<List<FeedbackDto>>(feedbackList);
    }
}
