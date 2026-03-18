using HotChocolate.Execution.Configuration;
using Todos.Application.DTOs;

namespace Todos.API.GraphQL.Learning;

/// <summary>
/// GraphQL Subscription type for Learning module
/// </summary>
public class LearningSubscription
{
    [Subscribe]
    [GraphQLName("learningRecordCreated")]
    public LearningRecordDto LearningRecordCreated([EventMessage] LearningRecordDto learningRecord)
    {
        return learningRecord;
    }

    [Subscribe]
    [GraphQLName("feedbackSubmitted")]
    public LearningFeedbackDto FeedbackSubmitted([EventMessage] LearningFeedbackDto feedback)
    {
        return feedback;
    }
}
