namespace FeedbackService.Application.Queries.Handlers;

using MediatR;
using DTOs;
using AutoMapper;
using Commands.Handlers;

/// <summary>
/// Handler for GetAllFeedbackQuery
/// </summary>
public class GetAllFeedbackQueryHandler : IRequestHandler<GetAllFeedbackQuery, List<FeedbackDto>>
{
    private readonly IFeedbackRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetAllFeedbackQueryHandler class
    /// </summary>
    public GetAllFeedbackQueryHandler(IFeedbackRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetAllFeedbackQuery
    /// </summary>
    public async Task<List<FeedbackDto>> Handle(GetAllFeedbackQuery request, CancellationToken cancellationToken)
    {
        var feedbackList = await _repository.GetAllAsync(cancellationToken);

        // Apply filtering if provided
        if (!string.IsNullOrEmpty(request.StatusFilter))
        {
            feedbackList = feedbackList
                .Where(f => f.Status?.Value == request.StatusFilter)
                .ToList();
        }

        // Apply pagination
        var pagedFeedback = feedbackList
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return _mapper.Map<List<FeedbackDto>>(pagedFeedback);
    }
}
