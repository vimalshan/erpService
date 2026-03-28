using HotChocolate.Types;
using AuthorizationService.Application.DTOs;

namespace AuthorizationService.API.GraphQL;

public class RightType : ObjectType<RightDto>
{
    protected override void Configure(IObjectTypeDescriptor<RightDto> descriptor)
    {
        descriptor
            .Field(t => t.Id)
            .Type<NonNullType<LongType>>();

        descriptor
            .Field(t => t.RightCode)
            .Type<NonNullType<DecimalType>>();

        descriptor
            .Field(t => t.RightDescription)
            .Type<StringType>();

        descriptor
            .Field(t => t.CreatedAt)
            .Type<NonNullType<DateTimeType>>();

        descriptor
            .Field(t => t.UpdatedAt)
            .Type<DateTimeType>();
    }
}

public class UserRightType : ObjectType<UserRightDto>
{
    protected override void Configure(IObjectTypeDescriptor<UserRightDto> descriptor)
    {
        descriptor
            .Field(t => t.Id)
            .Type<NonNullType<LongType>>();

        descriptor
            .Field(t => t.UserId)
            .Type<StringType>();

        descriptor
            .Field(t => t.PinNumber)
            .Type<DecimalType>();

        descriptor
            .Field(t => t.RightCode)
            .Type<DecimalType>();

        descriptor
            .Field(t => t.BusinessCode)
            .Type<StringType>();

        descriptor
            .Field(t => t.UnitCode)
            .Type<StringType>();

        descriptor
            .Field(t => t.RightMode)
            .Type<DecimalType>();
    }
}

public class TrackerRightType : ObjectType<TrackerRightDto>
{
    protected override void Configure(IObjectTypeDescriptor<TrackerRightDto> descriptor)
    {
        descriptor
            .Field(t => t.Id)
            .Type<NonNullType<LongType>>();

        descriptor
            .Field(t => t.UserId)
            .Type<StringType>();

        descriptor
            .Field(t => t.PinNumber)
            .Type<DecimalType>();

        descriptor
            .Field(t => t.TrackerMode)
            .Type<StringType>();

        descriptor
            .Field(t => t.BusinessCode)
            .Type<StringType>();

        descriptor
            .Field(t => t.UnitCode)
            .Type<StringType>();

        descriptor
            .Field(t => t.TrackerRights)
            .Type<StringType>();

        descriptor
            .Field(t => t.VtcRights)
            .Type<StringType>();

        descriptor
            .Field(t => t.RepresentingUnit)
            .Type<StringType>();

        descriptor
            .Field(t => t.LetRight)
            .Type<StringType>();

        descriptor
            .Field(t => t.CarRight)
            .Type<StringType>();

        descriptor
            .Field(t => t.HasTrackerAccess)
            .Type<NonNullType<BooleanType>>();

        descriptor
            .Field(t => t.HasVtcAccess)
            .Type<NonNullType<BooleanType>>();
    }
}

public class SpecialInputType : ObjectType<SpecialInputDto>
{
    protected override void Configure(IObjectTypeDescriptor<SpecialInputDto> descriptor)
    {
        descriptor
            .Field(t => t.Id)
            .Type<NonNullType<LongType>>();

        descriptor
            .Field(t => t.SpecialInputId)
            .Type<NonNullType<DecimalType>>();

        descriptor
            .Field(t => t.YearId)
            .Type<NonNullType<DecimalType>>();

        descriptor
            .Field(t => t.RoleType)
            .Type<StringType>();

        descriptor
            .Field(t => t.Inputs)
            .Type<StringType>();

        descriptor
            .Field(t => t.Status)
            .Type<NonNullType<StringType>>();

        descriptor
            .Field(t => t.IsSubmitted)
            .Type<NonNullType<BooleanType>>();
    }
}
