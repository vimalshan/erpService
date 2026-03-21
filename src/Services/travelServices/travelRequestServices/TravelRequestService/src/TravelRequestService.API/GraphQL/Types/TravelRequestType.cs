using TravelRequestService.Application.DTOs;

namespace TravelRequestService.API.GraphQL.Types;

public class TravelRequestType : ObjectType<TravelRequestDto>
{
    protected override void Configure(IObjectTypeDescriptor<TravelRequestDto> descriptor)
    {
        descriptor.Name("TravelRequest");

        descriptor.Field(t => t.PlanNumber).Type<NonNullType<LongType>>();
        descriptor.Field(t => t.CompanyCode).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.UserNumber).Type<LongType>();
        descriptor.Field(t => t.AppliedDate).Type<DateTimeType>();
        descriptor.Field(t => t.ObjectiveDescription).Type<StringType>();
        descriptor.Field(t => t.Status).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.TravelType).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.BudgetAmount).Type<DecimalType>();
        descriptor.Field(t => t.SubDetails).Type<ListType<NonNullType<ObjectType<TravelSubDto>>>>();
        descriptor.Field(t => t.Agendas).Type<ListType<NonNullType<ObjectType<TravelAgendaDto>>>>();
        descriptor.Field(t => t.Advances).Type<ListType<NonNullType<ObjectType<TravelAdvanceDto>>>>();
        descriptor.Field(t => t.ApprovalRemarks).Type<ListType<NonNullType<ObjectType<TravelApprovalRemarkDto>>>>();
    }
}
