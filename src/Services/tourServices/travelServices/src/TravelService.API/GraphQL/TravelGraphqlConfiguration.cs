using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using TravelService.Application.DTOs;

namespace TravelService.API.GraphQL;

public class TravelQueryType : ObjectType<TravelQuery>
{
    protected override void Configure(IObjectTypeDescriptor<TravelQuery> descriptor)
    {
        descriptor
            .Field(c => c.GetTourPlan(default!, default!, default!))
            .Name("getTourPlan")
            .Argument("id", a => a.Type<NonNullType<StringType>>())
            .Type<ObjectType<TourPlanDto>>();

        descriptor
            .Field(c => c.GetTourPlans(default!, 1, 20, default!))
            .Name("getTourPlans")
            .Argument("page", a => a.Type<IntType>().DefaultValue(1))
            .Argument("pageSize", a => a.Type<IntType>().DefaultValue(20))
            .Type<ListType<ObjectType<TourPlanDto>>>();

        descriptor
            .Field(c => c.GetBatch(default!, default!, default!))
            .Name("getBatch")
            .Argument("id", a => a.Type<NonNullType<StringType>>())
            .Type<ObjectType<BatchMainDto>>();
    }
}

public class TravelMutationType : ObjectType<TravelMutation>
{
    protected override void Configure(IObjectTypeDescriptor<TravelMutation> descriptor)
    {
        descriptor
            .Field(c => c.ApproveTourPlan(default!, default!, default!, default!))
            .Name("approveTourPlan")
            .Argument("tourPlanId", a => a.Type<NonNullType<StringType>>())
            .Argument("approvedBy", a => a.Type<NonNullType<StringType>>())
            .Type<NonNullType<StringType>>();
    }
}
