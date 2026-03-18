using WorkOrderService.Application.DTOs;

namespace WorkOrderService.API.GraphQL.Types;

public class WorkOrderType : ObjectType<WorkOrderDto>
{
    protected override void Configure(IObjectTypeDescriptor<WorkOrderDto> descriptor)
    {
        descriptor.Name("WorkOrder");
        descriptor.Field(f => f.WorkOrderId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.WorkOrderName).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.WorkOrderDescription).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.DueDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(f => f.AssignedTo).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.WorkOrderStatus).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.WorkOrderStatusCode).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.CompletionPercentage).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.Tasks).Type<NonNullType<ListType<NonNullType<ObjectType<WorkTaskDto>>>>>();
    }
}

public class WorkTaskType : ObjectType<WorkTaskDto>
{
    protected override void Configure(IObjectTypeDescriptor<WorkTaskDto> descriptor)
    {
        descriptor.Name("WorkTask");
        descriptor.Field(f => f.TaskId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.WorkOrderId).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.TaskName).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.AssignedTo).Type<NonNullType<LongType>>();
        descriptor.Field(f => f.EstimatedHours).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.ActualHours).Type<IntType>();
        descriptor.Field(f => f.TaskStatus).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.TaskStatusCode).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.CompletionRemarks).Type<StringType>();
    }
}
