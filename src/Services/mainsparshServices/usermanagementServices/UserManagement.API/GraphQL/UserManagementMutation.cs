using HotChocolate;
using MediatR;
using UserManagement.Application.DTOs;
using UserManagement.Application.Features.UserPolicy.Commands.CreateUserPolicy;
using UserManagement.Application.Features.UserPolicy.Commands.DeleteUserPolicy;
using UserManagement.Application.Features.UserPolicy.Commands.UpdateUserPolicy;
using UserManagement.Application.Features.WebsiteContact.Commands.CreateWebsiteContact;
using UserManagement.Application.Features.WebsiteContact.Commands.UpdateWebsiteContact;

namespace UserManagement.API.GraphQL;

/// <summary>HotChocolate GraphQL Mutation type</summary>
public class UserManagementMutation(IMediator mediator)
{
    [GraphQLDescription("Create a new user policy")]
    public async Task<UserPolicyDto> CreateUserPolicy(
        CreateUserPolicyCommand input,
        CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    [GraphQLDescription("Update an existing user policy")]
    public async Task<UserPolicyDto> UpdateUserPolicy(
        UpdateUserPolicyCommand input,
        CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    [GraphQLDescription("Deactivate a user policy")]
    public async Task<bool> DeleteUserPolicy(
        long policyId,
        long deletedBy,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteUserPolicyCommand(policyId, deletedBy), cancellationToken);
        return true;
    }

    [GraphQLDescription("Create website contact information")]
    public async Task<WebsiteContactDto> CreateWebsiteContact(
        CreateWebsiteContactCommand input,
        CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    [GraphQLDescription("Update website contact information")]
    public async Task<WebsiteContactDto> UpdateWebsiteContact(
        UpdateWebsiteContactCommand input,
        CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);
}
