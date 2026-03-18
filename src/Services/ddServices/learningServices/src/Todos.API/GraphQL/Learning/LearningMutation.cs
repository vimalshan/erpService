using MediatR;
using Todos.Application.Commands;
using Todos.Application.DTOs;

namespace Todos.API.GraphQL.Learning;

/// <summary>
/// GraphQL Mutation type for Learning module
/// </summary>
public class LearningMutation
{
    private readonly IMediator _mediator;

    public LearningMutation(IMediator mediator)
    {
        _mediator = mediator;
    }

    [GraphQLName("createLearningRecord")]
    public async Task<LearningRecordDto?> CreateLearningRecord(
        decimal letId,
        decimal requestNumber,
        string? employeeId,
        string? specificNeed,
        decimal modifiedBy,
        CancellationToken cancellationToken)
    {
        var command = new CreateLearningRecordCommand
        {
            LetId = letId,
            RequestNumber = requestNumber,
            EmployeeId = employeeId,
            SpecificNeed = specificNeed,
            ModifiedBy = modifiedBy
        };

        var result = await _mediator.Send(command, cancellationToken);
        return result.Data;
    }

    [GraphQLName("updateLearningRecord")]
    public async Task<LearningRecordDto?> UpdateLearningRecord(
        Guid id,
        string? specificNeed,
        string? indicator,
        string? developmentArea,
        string? expectedPostTraining,
        string? bhrStatus,
        decimal modifiedBy,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLearningRecordCommand
        {
            Id = id,
            SpecificNeed = specificNeed,
            Indicator = indicator,
            DevelopmentArea = developmentArea,
            ExpectedPostTraining = expectedPostTraining,
            BhrStatus = bhrStatus,
            ModifiedBy = modifiedBy
        };

        var result = await _mediator.Send(command, cancellationToken);
        return result.Data;
    }

    [GraphQLName("deleteLearningRecord")]
    public async Task<bool> DeleteLearningRecord(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteLearningRecordCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);
        return result.Success;
    }

    [GraphQLName("submitFeedback")]
    public async Task<LearningFeedbackDto?> SubmitFeedback(
        Guid feedbackId,
        string? trainingProgram,
        string? feedbackStatus,
        string? appraiseeComments,
        string? appraiserComments,
        string? reviewerComments,
        decimal modifiedBy,
        CancellationToken cancellationToken)
    {
        var command = new SubmitLearningFeedbackCommand
        {
            FeedbackId = feedbackId,
            TrainingProgram = trainingProgram,
            FeedbackStatus = feedbackStatus,
            AppraiseeComments = appraiseeComments,
            AppraiserComments = appraiserComments,
            ReviewerComments = reviewerComments,
            ModifiedBy = modifiedBy
        };

        var result = await _mediator.Send(command, cancellationToken);
        return result.Data;
    }

    [GraphQLName("identifyLearningNeed")]
    public async Task<LearningRecordDto?> IdentifyLearningNeed(
        Guid learningRecordId,
        string developmentArea,
        string indicator,
        CancellationToken cancellationToken)
    {
        var command = new IdentifyLearningNeedCommand
        {
            LearningRecordId = learningRecordId,
            DevelopmentArea = developmentArea,
            Indicator = indicator
        };

        var result = await _mediator.Send(command, cancellationToken);
        return result.Data;
    }
}
