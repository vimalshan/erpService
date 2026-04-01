using LetTransactionService.Application.DTOs;

namespace LetTransactionService.API.GraphQL.Types;

public class LetMainType : ObjectType<LetMainDto>
{
    protected override void Configure(IObjectTypeDescriptor<LetMainDto> descriptor)
    {
        descriptor.Description("A LET request header record.");

        descriptor.Field(f => f.RequestNumber).Description("Unique LET request identifier.");
        descriptor.Field(f => f.EmployeeUserId).Description("Employee who raised the request.");
        descriptor.Field(f => f.RequestDate).Description("Date the request was created.");
        descriptor.Field(f => f.SubEntries).Description("Line items of the LET request.");
    }
}

public class LetSubType : ObjectType<LetSubDto>
{
    protected override void Configure(IObjectTypeDescriptor<LetSubDto> descriptor)
    {
        descriptor.Description("A LET request line item.");
    }
}

public class LetSummaryType : ObjectType<LetSummaryDto>
{
    protected override void Configure(IObjectTypeDescriptor<LetSummaryDto> descriptor)
    {
        descriptor.Description("A summary of a LET request.");
    }
}

public class FeedbackMainType : ObjectType<FeedbackMainDto>
{
    protected override void Configure(IObjectTypeDescriptor<FeedbackMainDto> descriptor)
    {
        descriptor.Description("A course feedback header record.");

        descriptor.Field(f => f.FeedbackNumber).Description("Unique feedback identifier.");
        descriptor.Field(f => f.NominationNumber).Description("The nomination being evaluated.");
        descriptor.Field(f => f.FeedbackDetails).Description("Feedback detail items.");
    }
}

public class FeedbackSubType : ObjectType<FeedbackSubDto>
{
    protected override void Configure(IObjectTypeDescriptor<FeedbackSubDto> descriptor)
    {
        descriptor.Description("A feedback detail line item.");
    }
}

public class ReviewMainType : ObjectType<ReviewMainDto>
{
    protected override void Configure(IObjectTypeDescriptor<ReviewMainDto> descriptor)
    {
        descriptor.Description("A review header record.");

        descriptor.Field(f => f.ReviewSerialNumber).Description("Unique review identifier.");
        descriptor.Field(f => f.FeedbackNumber).Description("Feedback associated with this review.");
        descriptor.Field(f => f.ReviewDetails).Description("Review detail items.");
    }
}

public class ReviewSubType : ObjectType<ReviewSubDto>
{
    protected override void Configure(IObjectTypeDescriptor<ReviewSubDto> descriptor)
    {
        descriptor.Description("A review detail line item.");
    }
}

public class PendingReviewType : ObjectType<PendingReviewDto>
{
    protected override void Configure(IObjectTypeDescriptor<PendingReviewDto> descriptor)
    {
        descriptor.Description("A pending review summary.");
    }
}
