namespace AccessService.API.GraphQL;

using HotChocolate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AccessService.API.GraphQL.Inputs;
using AccessService.API.GraphQL.Payloads;
using AccessService.Application.CQRS.Commands;

/// <summary>
/// GraphQL Mutation root type.
/// All mutations are JWT-protected.
/// Commands are dispatched via MediatR (consistent with REST controller pattern).
/// </summary>
public class Mutation
{
    // ─── UserMap Mutations ───────────────────────────────────────────────────────

    [GraphQLDescription("Create a new user map for an employee.")]
    public async Task<CreateUserMapPayload> CreateUserMap(
        CreateUserMapInput input,
        [Service] IMediator mediator)
    {
        try
        {
            var id = await mediator.Send(new CreateUserMapCommand
            {
                EmployeeSystemId = input.EmployeeSystemId
            });
            return CreateUserMapPayload.Ok(id);
        }
        catch (DbUpdateException ex) when (
            ex.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true ||
            ex.InnerException?.Message.Contains("PRIMARY KEY", StringComparison.OrdinalIgnoreCase) == true)
        {
            return CreateUserMapPayload.Fail($"A user map already exists for employee {input.EmployeeSystemId}.");
        }
    }

    [GraphQLDescription("Activate a user map and set its effective date.")]
    public async Task<MutationPayload> ActivateUserMap(
        ActivateUserMapInput input,
        [Service] IMediator mediator)
    {
        await mediator.Send(new ActivateUserMapCommand
        {
            EmployeeSystemId = input.EmployeeSystemId,
            EffectiveDate    = input.EffectiveDate
        });
        return MutationPayload.Ok($"UserMap for employee {input.EmployeeSystemId} activated.");
    }

    [GraphQLDescription("Deactivate a user map by setting its closure date.")]
    public async Task<MutationPayload> DeactivateUserMap(
        DeactivateUserMapInput input,
        [Service] IMediator mediator)
    {
        await mediator.Send(new DeactivateUserMapCommand
        {
            EmployeeSystemId = input.EmployeeSystemId,
            ClosureDate      = input.ClosureDate
        });
        return MutationPayload.Ok($"UserMap for employee {input.EmployeeSystemId} deactivated.");
    }

    // ─── UserRole Mutations ──────────────────────────────────────────────────────

    [GraphQLDescription("Assign a role to an employee. roleType: S = SuperUser, U = UnitAccess, C = CalendarAccess.")]
    public async Task<AssignUserRolePayload> AssignUserRole(
        AssignUserRoleInput input,
        [Service] IMediator mediator)
    {
        if (string.IsNullOrEmpty(input.RoleType) || input.RoleType.Length != 1)
            return AssignUserRolePayload.Fail("roleType must be a single character: S, U, or C.");

        if (!string.IsNullOrEmpty(input.MenuAccess) && input.MenuAccess.Length != 1)
            return AssignUserRolePayload.Fail("menuAccess must be a single character if provided.");

        var roleId = await mediator.Send(new AssignUserRoleCommand
        {
            EmployeeSystemId = input.EmployeeSystemId,
            RoleType         = char.ToUpper(input.RoleType[0]),
            MenuAccess       = input.MenuAccess?.Length == 1 ? char.ToUpper(input.MenuAccess[0]) : null,
            OrganizationId   = input.OrganizationId,
            UnitId           = input.UnitId,
            CalendarId       = input.CalendarId
        });
        return AssignUserRolePayload.Ok(roleId);
    }

    [GraphQLDescription("Update an existing role assignment.")]
    public async Task<MutationPayload> UpdateUserRole(
        UpdateUserRoleInput input,
        [Service] IMediator mediator)
    {
        if (!string.IsNullOrEmpty(input.MenuAccess) && input.MenuAccess.Length != 1)
            throw new GraphQLException("menuAccess must be a single character if provided.");

        await mediator.Send(new UpdateUserRoleCommand
        {
            RoleId         = input.RoleId,
            MenuAccess     = input.MenuAccess?.Length == 1 ? char.ToUpper(input.MenuAccess[0]) : null,
            OrganizationId = input.OrganizationId,
            UnitId         = input.UnitId,
            CalendarId     = input.CalendarId
        });
        return MutationPayload.Ok($"Role {input.RoleId} updated.");
    }

    [GraphQLDescription("Revoke a role by setting its closure date.")]
    public async Task<MutationPayload> RevokeUserRole(
        RevokeUserRoleInput input,
        [Service] IMediator mediator)
    {
        await mediator.Send(new RevokeUserRoleCommand
        {
            RoleId      = input.RoleId,
            ClosureDate = input.ClosureDate
        });
        return MutationPayload.Ok($"Role {input.RoleId} revoked.");
    }
}
