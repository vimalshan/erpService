namespace FeedbackService.API.GraphQL;

using Application.Commands;
using Application.DTOs;
using MediatR;

/// <summary>
/// GraphQL Mutation type for feedback operations
/// </summary>
public class Mutation
{
    /// <summary>
    /// Creates a new feedback
    /// </summary>
    public async Task<FeedbackDto> CreateFeedback(
        decimal feedbackId,
        decimal requestNo,
        decimal approverSystemId,
        string? remarks,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new CreateFeedbackCommand
            {
                FeedbackId = feedbackId,
                RequestNo = requestNo,
                ApproverSystemId = approverSystemId,
                Remarks = remarks
            },
            cancellationToken);
    }

    /// <summary>
    /// Adds an item to feedback
    /// </summary>
    public async Task<FeedbackDto> AddFeedbackItem(
        decimal feedbackId,
        decimal questionNo,
        decimal? answerNo,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new AddFeedbackItemCommand
            {
                FeedbackId = feedbackId,
                QuestionNo = questionNo,
                AnswerNo = answerNo
            },
            cancellationToken);
    }

    /// <summary>
    /// Submits feedback
    /// </summary>
    public async Task<FeedbackDto> SubmitFeedback(
        decimal feedbackId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(
            new SubmitFeedbackCommand { FeedbackId = feedbackId },
            cancellationToken);
    }
}
