using TaskServices.Application.DTOs;

namespace TaskServices.API.GraphQL;

public class TaskMailType : ObjectType<TaskMailDto>
{
    protected override void Configure(IObjectTypeDescriptor<TaskMailDto> descriptor)
    {
        descriptor.Name("TaskMail");
        descriptor.Field(t => t.MID).Type<NonNullType<DecimalType>>().Description("Mail/Task ID");
        descriptor.Field(t => t.SYSID).Type<NonNullType<DecimalType>>().Description("System User ID");
    }
}
