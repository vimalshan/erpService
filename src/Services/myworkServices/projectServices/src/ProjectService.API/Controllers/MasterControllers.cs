using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectService.Application.Commands;
using ProjectService.Application.DTOs;
using ProjectService.Application.Queries;

namespace ProjectService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectMastersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProjectMasterDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllProjectMastersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProjectMasterDto>> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProjectMasterByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectMasterDto>> Create([FromBody] CreateProjectMasterCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.ProjectId }, result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectTypesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProjectTypeMasterDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllProjectTypesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:decimal}")]
    public async Task<ActionResult<ProjectTypeMasterDto>> GetById(decimal id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProjectTypeByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LookupsController(IMediator mediator) : ControllerBase
{
    [HttpGet("locations")]
    public async Task<ActionResult<IReadOnlyList<ProjectLocationDto>>> GetLocations(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetAllLocationsQuery(), cancellationToken));

    [HttpGet("processes")]
    public async Task<ActionResult<IReadOnlyList<ProjectProcessDto>>> GetProcesses(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetAllProcessesQuery(), cancellationToken));

    [HttpGet("departments")]
    public async Task<ActionResult<IReadOnlyList<ProjectDepartmentDto>>> GetDepartments(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetAllDepartmentsQuery(), cancellationToken));

    [HttpGet("functions")]
    public async Task<ActionResult<IReadOnlyList<ProjectFunctionDto>>> GetFunctions(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetAllFunctionsQuery(), cancellationToken));

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<ProjectCategoryDto>>> GetCategories(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new GetAllCategoriesQuery(), cancellationToken));
}
