namespace FeedbackService.API.GraphQL;

using Application.DTOs;
using Application.Queries;
using MediatR;

/// <summary>
/// GraphQL Query type for feedback operations
/// </summary>
public class Query
{
    /// <summary>
    /// Gets feedback by ID
    /// </summary>
    public async Task<FeedbackDto?> GetFeedbackById(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new GetFeedbackByIdQuery { FeedbackId = id },
            cancellationToken);
    }

    /// <summary>
    /// Gets all feedback
    /// </summary>
    public async Task<List<FeedbackDto>> GetAllFeedback(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 10,
        string? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(
            new GetAllFeedbackQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                StatusFilter = statusFilter
            },
            cancellationToken);
    }

    /// <summary>
    /// Gets feedback by request number
    /// </summary>
    public async Task<List<FeedbackDto>> GetFeedbackByRequestNo(
        decimal requestNo,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new GetFeedbackByRequestNoQuery { RequestNo = requestNo },
            cancellationToken);
    }
}
