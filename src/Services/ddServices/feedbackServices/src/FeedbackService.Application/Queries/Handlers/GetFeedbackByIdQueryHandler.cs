namespace FeedbackService.Application.Queries.Handlers;

using MediatR;
using DTOs;
using AutoMapper;
using Commands.Handlers;

/// <summary>
/// Handler for GetFeedbackByIdQuery
/// </summary>
public class GetFeedbackByIdQueryHandler : IRequestHandler<GetFeedbackByIdQuery, FeedbackDto?>
{
    private readonly IFeedbackRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetFeedbackByIdQueryHandler class
    /// </summary>
    public GetFeedbackByIdQueryHandler(IFeedbackRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetFeedbackByIdQuery
    /// </summary>
    public async Task<FeedbackDto?> Handle(GetFeedbackByIdQuery request, CancellationToken cancellationToken)
    {
        var feedback = await _repository.GetByIdAsync(request.FeedbackId, cancellationToken);
        return feedback == null ? null : _mapper.Map<FeedbackDto>(feedback);
    }
}
