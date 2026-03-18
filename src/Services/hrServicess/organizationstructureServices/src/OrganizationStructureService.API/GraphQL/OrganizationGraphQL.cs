using HotChocolate;
using HotChocolate.Types;
using MediatR;
using OrganizationStructureService.Application.DTOs;
using OrganizationStructureService.Application.Queries;

namespace OrganizationStructureService.API.GraphQL;

public class Query
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<BusinessDto>> GetBusinesses(
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetAllBusinessesQuery(), ct);

    public async Task<BusinessDto?> GetBusiness(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetBusinessByIdQuery(id), ct);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<UnitDto>> GetUnits(
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetAllUnitsQuery(), ct);

    public async Task<UnitDto?> GetUnit(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetUnitByIdQuery(id), ct);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<GradeDto>> GetGrades(
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetAllGradesQuery(), ct);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<PositionDto>> GetPositions(
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetAllPositionsQuery(), ct);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<SiteDto>> GetSites(
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetAllSitesQuery(), ct);

    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<DepartmentDto>> GetDepartments(
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetAllDepartmentsQuery(), ct);
}

public class Mutation
{
    public async Task<BusinessDto> CreateBusiness(
        decimal businessId,
        string businessName,
        string businessShortName,
        string businessCode,
        decimal companyId,
        string companyCode,
        decimal updatedBy,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new Application.Commands.CreateBusinessCommand(
            businessId, businessName, businessShortName, businessCode, companyId, companyCode, updatedBy), ct);

    public async Task<UnitDto> CreateUnit(
        decimal unitId,
        string unitName,
        string unitShortName,
        string unitCode,
        decimal businessId,
        string businessCode,
        decimal orgId,
        string? reportFlag,
        decimal updatedBy,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new Application.Commands.CreateUnitCommand(
            unitId, unitName, unitShortName, unitCode, businessId, businessCode, orgId, reportFlag, updatedBy), ct);
}
