using ScholarshipService.Application.DTOs;

namespace ScholarshipService.API.GraphQL.Subscriptions;

public class ScholarshipSubscription
{
    private static readonly string ScholarshipCreatedTopic = "scholarship_created";
    private static readonly string ScholarshipApprovedTopic = "scholarship_approved";
    private static readonly string ScholarshipStoppedTopic = "scholarship_stopped";

    [Subscribe]
    [Topic("scholarship_created")]
    [GraphQLDescription("Subscribe to new scholarship application events.")]
    public ScholarshipMainDto OnScholarshipCreated([EventMessage] ScholarshipMainDto scholarship)
        => scholarship;

    [Subscribe]
    [Topic("scholarship_approved")]
    [GraphQLDescription("Subscribe to scholarship approval events.")]
    public ScholarshipMainDto OnScholarshipApproved([EventMessage] ScholarshipMainDto scholarship)
        => scholarship;

    [Subscribe]
    [Topic("scholarship_stopped")]
    [GraphQLDescription("Subscribe to scholarship stopped events.")]
    public ScholarshipMainDto OnScholarshipStopped([EventMessage] ScholarshipMainDto scholarship)
        => scholarship;
}
