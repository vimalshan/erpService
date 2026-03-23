namespace AccessService.API.GraphQL;

using HotChocolate;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using AccessService.API.GraphQL.Types;
using AccessService.Application.CQRS.Queries;
using AccessService.Infrastructure.Repositories;

/// <summary>
/// GraphQL Query root type.
/// All resolvers are JWT-protected via [Authorize].
/// UserMap and UserRole queries use MediatR (CQRS pattern consistent with REST controllers).
/// Menu and SPARSH queries use IUnitOfWork directly (no CQRS handlers exist for these yet).
/// </summary>
public class Query
{
    // ─── UserMap Queries ────────────────────────────────────────────────────────

    [GraphQLDescription("Get a single user map by employee system ID.")]
    public async Task<UserMapType?> GetUserMap(
        long employeeSystemId,
        [Service] IMediator mediator)
    {
        var dto = await mediator.Send(new GetUserMapByEmployeeIdQuery { EmployeeSystemId = employeeSystemId });
        return dto is null ? null : UserMapType.FromDto(dto);
    }

    [GraphQLDescription("Get all user maps. Optionally filter to active-only records.")]
    public async Task<IEnumerable<UserMapType>> GetUserMaps(
        bool? activeOnly,
        [Service] IMediator mediator)
    {
        var dtos = await mediator.Send(new GetAllUserMapsQuery { ActiveOnly = activeOnly });
        return dtos.Select(UserMapType.FromDto);
    }

    // ─── UserRole Queries ────────────────────────────────────────────────────────

    [GraphQLDescription("Get a single user role by role ID.")]
    public async Task<UserRoleType?> GetUserRole(
        int roleId,
        [Service] IMediator mediator)
    {
        var dto = await mediator.Send(new GetUserRoleByIdQuery { RoleId = roleId });
        return dto is null ? null : UserRoleType.FromDto(dto);
    }

    [GraphQLDescription("Get all roles assigned to an employee. Optionally filter to active-only.")]
    public async Task<IEnumerable<UserRoleType>> GetUserRolesByEmployee(
        long employeeSystemId,
        bool? activeOnly,
        [Service] IMediator mediator)
    {
        var dtos = await mediator.Send(new GetUserRolesByEmployeeIdQuery
        {
            EmployeeSystemId = employeeSystemId,
            ActiveOnly = activeOnly
        });
        return dtos.Select(UserRoleType.FromDto);
    }

    [GraphQLDescription("Get all roles of a given type. roleType: S = SuperUser, U = UnitAccess, C = CalendarAccess.")]
    public async Task<IEnumerable<UserRoleType>> GetUserRolesByType(
        string roleType,
        [Service] IMediator mediator)
    {
        if (string.IsNullOrEmpty(roleType) || roleType.Length != 1)
            return Enumerable.Empty<UserRoleType>();

        var dtos = await mediator.Send(new GetUserRolesByTypeQuery { RoleType = char.ToUpper(roleType[0]) });
        return dtos.Select(UserRoleType.FromDto);
    }

    // ─── Menu Queries ─────────────────────────────────────────────────────────────

    [GraphQLDescription("Get all menus.")]
    public async Task<IEnumerable<MenuType>> GetMenus([Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var menus = await uow.Menus.GetAllAsync();
        return menus.Select(MenuType.FromEntity).ToList();
    }

    [GraphQLDescription("Get a single menu by menu ID.")]
    public async Task<MenuType?> GetMenu(int menuId, [Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var menu = await uow.Menus.GetByMenuIdAsync(menuId);
        return menu is null ? null : MenuType.FromEntity(menu);
    }

    [GraphQLDescription("Get all root-level menus (no parent).")]
    public async Task<IEnumerable<MenuType>> GetRootMenus([Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var menus = await uow.Menus.GetRootMenusAsync();
        return menus.Select(MenuType.FromEntity).ToList();
    }

    [GraphQLDescription("Get child menus for a given parent menu ID.")]
    public async Task<IEnumerable<MenuType>> GetMenuChildren(int parentMenuId, [Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var menus = await uow.Menus.GetMenusByParentIdAsync(parentMenuId);
        return menus.Select(MenuType.FromEntity).ToList();
    }

    // ─── SPARSH Menu Queries ───────────────────────────────────────────────────────

    [GraphQLDescription("Get all SPARSH menus.")]
    public async Task<IEnumerable<SparshMenuType>> GetSparshMenus([Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var menus = await uow.SPARSHMenus.GetAllAsync();
        return menus.Select(SparshMenuType.FromEntity).ToList();
    }

    [GraphQLDescription("Get a single SPARSH menu by menu ID.")]
    public async Task<SparshMenuType?> GetSparshMenu(long menuId, [Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var menu = await uow.SPARSHMenus.GetByMenuIdAsync(menuId);
        return menu is null ? null : SparshMenuType.FromEntity(menu);
    }

    [GraphQLDescription("Get a SPARSH menu by page name.")]
    public async Task<SparshMenuType?> GetSparshMenuByPage(string pageName, [Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var menu = await uow.SPARSHMenus.GetByPageNameAsync(pageName);
        return menu is null ? null : SparshMenuType.FromEntity(menu);
    }

    // ─── SPARSH Menu Access Queries ────────────────────────────────────────────────

    [GraphQLDescription("Get all SPARSH menu access records.")]
    public async Task<IEnumerable<SparshMenuAccessType>> GetSparshMenuAccesses([Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var accesses = await uow.SPARSHMenuAccess.GetAllAsync();
        return accesses.Select(SparshMenuAccessType.FromEntity).ToList();
    }

    [GraphQLDescription("Get a single SPARSH menu access record by access ID.")]
    public async Task<SparshMenuAccessType?> GetSparshMenuAccess(long accessId, [Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var access = await uow.SPARSHMenuAccess.GetByAccessIdAsync(accessId);
        return access is null ? null : SparshMenuAccessType.FromEntity(access);
    }

    [GraphQLDescription("Get SPARSH menu access records for a given unit.")]
    public async Task<IEnumerable<SparshMenuAccessType>> GetSparshMenuAccessByUnit(long unitId, [Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var accesses = await uow.SPARSHMenuAccess.GetAccessByUnitAsync(unitId);
        return accesses.Select(SparshMenuAccessType.FromEntity).ToList();
    }

    [GraphQLDescription("Get SPARSH menu access records for a given calendar.")]
    public async Task<IEnumerable<SparshMenuAccessType>> GetSparshMenuAccessByCalendar(long calendarId, [Service] IServiceScopeFactory scopeFactory)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var accesses = await uow.SPARSHMenuAccess.GetAccessByCalendarAsync(calendarId);
        return accesses.Select(SparshMenuAccessType.FromEntity).ToList();
    }
}
