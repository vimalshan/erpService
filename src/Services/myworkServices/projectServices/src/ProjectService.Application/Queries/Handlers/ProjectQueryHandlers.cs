using AutoMapper;
using MediatR;
using ProjectService.Application.DTOs;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Interfaces;

namespace ProjectService.Application.Queries.Handlers;

public class GetProjectByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProjectByIdQuery, ProjectMainDto?>
{
    public async Task<ProjectMainDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await unitOfWork.ProjectMains.GetByIdAsync(request.ProjId, cancellationToken);
        return project is null ? null : mapper.Map<ProjectMainDto>(project);
    }
}

public class GetAllProjectsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllProjectsQuery, IReadOnlyList<ProjectMainDto>>
{
    public async Task<IReadOnlyList<ProjectMainDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await unitOfWork.ProjectMains.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectMainDto>>(projects);
    }
}

public class GetProjectsByStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProjectsByStatusQuery, IReadOnlyList<ProjectMainDto>>
{
    public async Task<IReadOnlyList<ProjectMainDto>> Handle(GetProjectsByStatusQuery request, CancellationToken cancellationToken)
    {
        var projects = await unitOfWork.ProjectMains.GetProjectsByStatusAsync(request.Status, cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectMainDto>>(projects);
    }
}

public class GetProjectsByLeaderQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProjectsByLeaderQuery, IReadOnlyList<ProjectMainDto>>
{
    public async Task<IReadOnlyList<ProjectMainDto>> Handle(GetProjectsByLeaderQuery request, CancellationToken cancellationToken)
    {
        var projects = await unitOfWork.ProjectMains.GetProjectsByLeaderAsync(request.LeaderId, cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectMainDto>>(projects);
    }
}

public class GetProjectWithDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProjectWithDetailsQuery, ProjectMainDto?>
{
    public async Task<ProjectMainDto?> Handle(GetProjectWithDetailsQuery request, CancellationToken cancellationToken)
    {
        var project = await unitOfWork.ProjectMains.GetProjectWithDetailsAsync(request.ProjId, cancellationToken);
        return project is null ? null : mapper.Map<ProjectMainDto>(project);
    }
}

public class GetProjectMembersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProjectMembersQuery, IReadOnlyList<ProjectMemberDto>>
{
    public async Task<IReadOnlyList<ProjectMemberDto>> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await unitOfWork.ProjectMembers.GetByProjectAsync(request.ProjId, cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectMemberDto>>(members);
    }
}

public class GetAllProjectMastersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllProjectMastersQuery, IReadOnlyList<ProjectMasterDto>>
{
    public async Task<IReadOnlyList<ProjectMasterDto>> Handle(GetAllProjectMastersQuery request, CancellationToken cancellationToken)
    {
        var masters = await unitOfWork.ProjectMasters.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectMasterDto>>(masters);
    }
}

public class GetProjectMasterByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProjectMasterByIdQuery, ProjectMasterDto?>
{
    public async Task<ProjectMasterDto?> Handle(GetProjectMasterByIdQuery request, CancellationToken cancellationToken)
    {
        var master = await unitOfWork.ProjectMasters.GetByIdAsync(request.ProjectId, cancellationToken);
        return master is null ? null : mapper.Map<ProjectMasterDto>(master);
    }
}

public class GetAllProjectTypesQueryHandler(IRepository<ProjectTypeMaster> repo, IMapper mapper)
    : IRequestHandler<GetAllProjectTypesQuery, IReadOnlyList<ProjectTypeMasterDto>>
{
    public async Task<IReadOnlyList<ProjectTypeMasterDto>> Handle(GetAllProjectTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await repo.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectTypeMasterDto>>(types);
    }
}

public class GetProjectTypeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProjectTypeByIdQuery, ProjectTypeMasterDto?>
{
    public async Task<ProjectTypeMasterDto?> Handle(GetProjectTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var type = await unitOfWork.ProjectTypes.GetWithMappingsAsync(request.TypeId, cancellationToken);
        return type is null ? null : mapper.Map<ProjectTypeMasterDto>(type);
    }
}

public class GetAllLocationsQueryHandler(IRepository<ProjectLocation> repo, IMapper mapper)
    : IRequestHandler<GetAllLocationsQuery, IReadOnlyList<ProjectLocationDto>>
{
    public async Task<IReadOnlyList<ProjectLocationDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
    {
        var locations = await repo.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectLocationDto>>(locations);
    }
}

public class GetAllProcessesQueryHandler(IRepository<ProjectProcess> repo, IMapper mapper)
    : IRequestHandler<GetAllProcessesQuery, IReadOnlyList<ProjectProcessDto>>
{
    public async Task<IReadOnlyList<ProjectProcessDto>> Handle(GetAllProcessesQuery request, CancellationToken cancellationToken)
    {
        var processes = await repo.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectProcessDto>>(processes);
    }
}

public class GetAllDepartmentsQueryHandler(IRepository<ProjectDepartment> repo, IMapper mapper)
    : IRequestHandler<GetAllDepartmentsQuery, IReadOnlyList<ProjectDepartmentDto>>
{
    public async Task<IReadOnlyList<ProjectDepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await repo.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectDepartmentDto>>(departments);
    }
}

public class GetAllFunctionsQueryHandler(IRepository<ProjectFunction> repo, IMapper mapper)
    : IRequestHandler<GetAllFunctionsQuery, IReadOnlyList<ProjectFunctionDto>>
{
    public async Task<IReadOnlyList<ProjectFunctionDto>> Handle(GetAllFunctionsQuery request, CancellationToken cancellationToken)
    {
        var functions = await repo.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectFunctionDto>>(functions);
    }
}

public class GetAllCategoriesQueryHandler(IRepository<ProjectCategoryMaster> repo, IMapper mapper)
    : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<ProjectCategoryDto>>
{
    public async Task<IReadOnlyList<ProjectCategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await repo.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyList<ProjectCategoryDto>>(categories);
    }
}
