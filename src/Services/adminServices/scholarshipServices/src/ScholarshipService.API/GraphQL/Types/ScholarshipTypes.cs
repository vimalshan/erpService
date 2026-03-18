using ScholarshipService.Application.DTOs;

namespace ScholarshipService.API.GraphQL.Types;

public class ScholarshipMainType : ObjectType<ScholarshipMainDto>
{
    protected override void Configure(IObjectTypeDescriptor<ScholarshipMainDto> descriptor)
    {
        descriptor.Description("A scholarship application record.");
        descriptor.Field(f => f.Id).Description("Unique scholarship ID.");
        descriptor.Field(f => f.ChildName).Description("Name of the employee's child.");
        descriptor.Field(f => f.CourseName).Description("Name of the course enrolled.");
        descriptor.Field(f => f.EntryStatus).Description("E=Entered, A=Approved, N=Not Eligible, B=Returned");
        descriptor.Field(f => f.LiveStatus).Description("Current live status (A=Active, S=Stopped).");
        descriptor.Field(f => f.Details).Description("Yearly scholarship detail records.");
    }
}

public class ScholarshipDetailType : ObjectType<ScholarshipDetailDto>
{
    protected override void Configure(IObjectTypeDescriptor<ScholarshipDetailDto> descriptor)
    {
        descriptor.Description("Yearly scholarship detail record.");
        descriptor.Field(f => f.MarksStatus).Description("S=Scheduled, P=Pending, A=Approved, R=Rejected");
        descriptor.Field(f => f.PayStatus).Description("S=Scheduled, A=HR Approved, P=Pending, C=Completed, O=Offline, B=Backdated");
    }
}

public class ScholarshipAmountType : ObjectType<ScholarshipAmountDto>
{
    protected override void Configure(IObjectTypeDescriptor<ScholarshipAmountDto> descriptor)
    {
        descriptor.Description("Eligible scholarship amount configuration.");
    }
}
