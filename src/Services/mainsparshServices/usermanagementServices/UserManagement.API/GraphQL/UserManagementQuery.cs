using HotChocolate;
using MediatR;
using UserManagement.Application.DTOs;
using UserManagement.Application.Features.UserPolicy.Queries.GetAllUserPolicies;
using UserManagement.Application.Features.UserPolicy.Queries.GetUserPolicyById;
using UserManagement.Application.Features.UserProfileHist.Queries;
using UserManagement.Application.Features.WebsiteContact.Queries;

namespace UserManagement.API.GraphQL;

/// <summary>HotChocolate GraphQL Query type — accessible at /graphql</summary>
public class UserManagementQuery
{
    [GraphQLDescription("Get all user policies")]
    public async Task<IEnumerable<UserPolicyDto>> GetUserPolicies(
        string? policyType,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetAllUserPoliciesQuery(policyType), cancellationToken);

    [GraphQLDescription("Get a user policy by its ID")]
    public async Task<UserPolicyDto> GetUserPolicy(
        long policyId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetUserPolicyByIdQuery(policyId), cancellationToken);

    [GraphQLDescription("Get contact details by contact ID")]
    public async Task<WebsiteContactDto> GetWebsiteContact(
        long contactId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetWebsiteContactByIdQuery(contactId), cancellationToken);

    [GraphQLDescription("Get all contacts for a user")]
    public async Task<IEnumerable<WebsiteContactDto>> GetUserContacts(
        long userSysId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetContactsByUserSysIdQuery(userSysId), cancellationToken);

    [GraphQLDescription("Get profile change history for a user")]
    public async Task<IEnumerable<UserProfileHistDto>> GetProfileHistory(
        long userSysId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new GetProfileHistoryByUserQuery(userSysId), cancellationToken);
}
