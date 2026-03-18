using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompetencyService.Application.Commands.EmpCompetencies;
using CompetencyService.Application.DTOs;
using CompetencyService.Application.Queries.EmpCompetencies;

namespace CompetencyService.API.Controllers;

[ApiController]
[Route("api/employees/{empSysId:decimal}/competencies")]
[Authorize]
public class EmpCompetenciesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmpSpecificCompetencyDto>), 200)]
    public async Task<IActionResult> GetByEmp(decimal empSysId, [FromQuery] decimal yearId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetEmpCompetenciesQuery(empSysId, yearId), ct);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmpSpecificCompetencyDto), 201)]
    public async Task<IActionResult> Assign(decimal empSysId, [FromBody] AssignEmpCompetencyCommand command, CancellationToken ct)
    {
        if (empSysId != command.EmpSysId) return BadRequest("EmpSysId mismatch.");
        var result = await mediator.Send(command, ct);
        return Ok(result);
    }
}
