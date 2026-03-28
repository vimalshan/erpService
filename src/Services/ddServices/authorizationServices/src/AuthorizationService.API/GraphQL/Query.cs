using HotChocolate.Types;
using AuthorizationService.Application.DTOs;
using AuthorizationService.Application.Queries;
using MediatR;

namespace AuthorizationService.API.GraphQL;

public class Query
{
    public async Task<RightDto?> GetRightAsync([Service] IMediator mediator, long id)
    {
        return await mediator.Send(new GetRightByIdQuery { Id = id });
    }

    public async Task<IEnumerable<RightDto>> GetRightsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllRightsQuery());
    }

    public async Task<IEnumerable<UserRightDto>> GetUserRightsAsync([Service] IMediator mediator, string userId)
    {
        return await mediator.Send(new GetUserRightsByUserIdQuery { UserId = userId });
    }

    public async Task<IEnumerable<UserRightDto>> GetAllUserRightsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllUserRightsQuery());
    }

    public async Task<IEnumerable<TrackerRightDto>> GetTrackerRightsAsync([Service] IMediator mediator, string userId)
    {
        return await mediator.Send(new GetTrackerRightsByUserIdQuery { UserId = userId });
    }

    public async Task<IEnumerable<TrackerRightDto>> GetAllTrackerRightsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllTrackerRightsQuery());
    }

    public async Task<IEnumerable<SpecialInputDto>> GetAllSpecialInputsAsync([Service] IMediator mediator)
    {
        return await mediator.Send(new GetAllSpecialInputsQuery());
    }
}

public class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor.Name("Query");

        descriptor
            .Field(q => q.GetRightAsync(default!, default))
            .Name("getRight")
            .Type<RightType>();

        descriptor
            .Field(q => q.GetRightsAsync(default!))
            .Name("getRights")
            .Type<NonNullType<ListType<RightType>>>();

        descriptor
            .Field(q => q.GetUserRightsAsync(default!, default!))
            .Name("getUserRights")
            .Type<NonNullType<ListType<UserRightType>>>();

        descriptor
            .Field(q => q.GetAllUserRightsAsync(default!))
            .Name("getAllUserRights")
            .Type<NonNullType<ListType<UserRightType>>>();

        descriptor
            .Field(q => q.GetTrackerRightsAsync(default!, default!))
            .Name("getTrackerRights")
            .Type<NonNullType<ListType<TrackerRightType>>>();

        descriptor
            .Field(q => q.GetAllTrackerRightsAsync(default!))
            .Name("getAllTrackerRights")
            .Type<NonNullType<ListType<TrackerRightType>>>();

        descriptor
            .Field(q => q.GetAllSpecialInputsAsync(default!))
            .Name("getAllSpecialInputs")
            .Type<NonNullType<ListType<SpecialInputType>>>();
    }
}
