using MediatR;
using ProjectService.Application.DTOs;

namespace ProjectService.Application.Queries;

public record GetProjectByIdQuery(long ProjId) : IRequest<ProjectMainDto?>;

public record GetAllProjectsQuery : IRequest<IReadOnlyList<ProjectMainDto>>;

public record GetProjectsByStatusQuery(char Status) : IRequest<IReadOnlyList<ProjectMainDto>>;

public record GetProjectsByLeaderQuery(long LeaderId) : IRequest<IReadOnlyList<ProjectMainDto>>;

public record GetProjectsByTypeQuery(long TypeId) : IRequest<IReadOnlyList<ProjectMainDto>>;

public record GetProjectWithDetailsQuery(long ProjId) : IRequest<ProjectMainDto?>;

public record GetProjectMembersQuery(long ProjId) : IRequest<IReadOnlyList<ProjectMemberDto>>;

public record GetAllProjectMastersQuery : IRequest<IReadOnlyList<ProjectMasterDto>>;

public record GetProjectMasterByIdQuery(long ProjectId) : IRequest<ProjectMasterDto?>;

public record GetAllProjectTypesQuery : IRequest<IReadOnlyList<ProjectTypeMasterDto>>;

public record GetProjectTypeByIdQuery(decimal TypeId) : IRequest<ProjectTypeMasterDto?>;

public record GetAllLocationsQuery : IRequest<IReadOnlyList<ProjectLocationDto>>;

public record GetAllProcessesQuery : IRequest<IReadOnlyList<ProjectProcessDto>>;

public record GetAllDepartmentsQuery : IRequest<IReadOnlyList<ProjectDepartmentDto>>;

public record GetAllFunctionsQuery : IRequest<IReadOnlyList<ProjectFunctionDto>>;

public record GetAllCategoriesQuery : IRequest<IReadOnlyList<ProjectCategoryDto>>;

public record GetAllTypeCategoriesQuery : IRequest<IReadOnlyList<ProjectTypeCategoryDto>>;
