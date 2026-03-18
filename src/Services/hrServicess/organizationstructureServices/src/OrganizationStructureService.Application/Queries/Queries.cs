using MediatR;
using OrganizationStructureService.Application.DTOs;

namespace OrganizationStructureService.Application.Queries;

// Business Queries
public record GetBusinessByIdQuery(decimal BusinessId) : IRequest<BusinessDto?>;
public record GetAllBusinessesQuery() : IRequest<IReadOnlyList<BusinessDto>>;
public record GetActiveBusinessesQuery() : IRequest<IReadOnlyList<BusinessDto>>;

// Unit Queries
public record GetUnitByIdQuery(decimal UnitId) : IRequest<UnitDto?>;
public record GetAllUnitsQuery() : IRequest<IReadOnlyList<UnitDto>>;
public record GetUnitsByBusinessIdQuery(decimal BusinessId) : IRequest<IReadOnlyList<UnitDto>>;
public record GetActiveUnitsQuery() : IRequest<IReadOnlyList<UnitDto>>;

// Department Queries
public record GetDepartmentByIdQuery(decimal DepartmentId) : IRequest<DepartmentDto?>;
public record GetAllDepartmentsQuery() : IRequest<IReadOnlyList<DepartmentDto>>;

// Division Queries
public record GetDivisionByIdQuery(decimal DivisionId) : IRequest<DivisionDto?>;
public record GetAllDivisionsQuery() : IRequest<IReadOnlyList<DivisionDto>>;

// Grade Queries
public record GetGradeByIdQuery(decimal GradeId) : IRequest<GradeDto?>;
public record GetAllGradesQuery() : IRequest<IReadOnlyList<GradeDto>>;
public record GetActiveGradesQuery() : IRequest<IReadOnlyList<GradeDto>>;

// Position Queries
public record GetPositionByIdQuery(decimal PositionId) : IRequest<PositionDto?>;
public record GetAllPositionsQuery() : IRequest<IReadOnlyList<PositionDto>>;
public record GetPositionsByUnitCodeQuery(string UnitCode) : IRequest<IReadOnlyList<PositionDto>>;

// Site Queries
public record GetSiteByIdQuery(decimal SiteId) : IRequest<SiteDto?>;
public record GetAllSitesQuery() : IRequest<IReadOnlyList<SiteDto>>;
