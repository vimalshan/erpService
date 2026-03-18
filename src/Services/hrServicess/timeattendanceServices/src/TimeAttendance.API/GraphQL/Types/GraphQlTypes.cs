using TimeAttendance.Application.DTOs;

namespace TimeAttendance.API.GraphQL.Types;

public class AbsenteeismDetailType : ObjectType<AbsenteeismDetailDto>
{
    protected override void Configure(IObjectTypeDescriptor<AbsenteeismDetailDto> descriptor)
    {
        descriptor.Description("Represents an absenteeism detail record (ABSENTEEISM_DET).");
        descriptor.Field(f => f.Id).Description("Primary key.");
        descriptor.Field(f => f.UnitId).Description("Organizational unit ID.");
        descriptor.Field(f => f.Year).Description("Calendar year.");
        descriptor.Field(f => f.Month).Description("Calendar month (1-12).");
        descriptor.Field(f => f.TotalManDays).Description("Total man days in the period.");
        descriptor.Field(f => f.AbsentManDays).Description("Number of absent man days.");
        descriptor.Field(f => f.PresentManDays).Description("Number of present man days.");
        descriptor.Field(f => f.AbsenteeismRate).Description("Absenteeism rate as a percentage.");
        descriptor.Field(f => f.GradeCategory).Description("Employee grade category.");
        descriptor.Field(f => f.Gender).Description("Gender (M/F/O).");
    }
}

public class AbsenteeismMisType : ObjectType<AbsenteeismMisDto>
{
    protected override void Configure(IObjectTypeDescriptor<AbsenteeismMisDto> descriptor)
    {
        descriptor.Description("Represents an absenteeism MIS record (ABSMIS).");
        descriptor.Field(f => f.Id).Description("Primary key.");
        descriptor.Field(f => f.UnitId).Description("Unit ID.");
        descriptor.Field(f => f.Month).Description("Month in YYYYMM format.");
        descriptor.Field(f => f.PlannedLeave).Description("Planned leave days.");
        descriptor.Field(f => f.PaidDays).Description("Paid days.");
        descriptor.Field(f => f.LeaveWithoutPay).Description("Days of leave without pay.");
    }
}
