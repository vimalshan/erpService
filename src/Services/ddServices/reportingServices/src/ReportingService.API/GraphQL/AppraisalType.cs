using HotChocolate.Types;
using ReportingService.Application.DTOs;

namespace ReportingService.API.GraphQL;

public class AppraisalType : ObjectType<AppraisalDto>
{
    protected override void Configure(IObjectTypeDescriptor<AppraisalDto> descriptor)
    {
        descriptor
            .Field(t => t.Id)
            .Type<NonNullType<LongType>>();

        descriptor
            .Field(t => t.RequestNumber)
            .Type<NonNullType<LongType>>();

        descriptor
            .Field(t => t.UserName)
            .Type<StringType>();

        descriptor
            .Field(t => t.UserId)
            .Type<StringType>();

        descriptor
            .Field(t => t.StatusDescription)
            .Type<StringType>();

        descriptor
            .Field(t => t.FinancialPeriod)
            .Type<StringType>();

        descriptor
            .Field(t => t.UnitCode)
            .Type<StringType>();

        descriptor
            .Field(t => t.GradeCode)
            .Type<StringType>();

        descriptor
            .Field(t => t.IsCompleted)
            .Type<NonNullType<BooleanType>>();

        descriptor
            .Field(t => t.CreatedAt)
            .Type<NonNullType<DateTimeType>>();

        descriptor
            .Field(t => t.UpdatedAt)
            .Type<DateTimeType>();
    }
}
