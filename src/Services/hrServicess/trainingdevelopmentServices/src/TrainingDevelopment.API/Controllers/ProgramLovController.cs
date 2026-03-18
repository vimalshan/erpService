using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingDevelopment.Application.DTOs;
using TrainingDevelopment.Application.Features.ProgramLov.Queries;

namespace TrainingDevelopment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ProgramLovController : ControllerBase
{
    private readonly ISender _sender;

    public ProgramLovController(ISender sender) => _sender = sender;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProgramLovDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetProgramLovListQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{typeCode}")]
    [ProducesResponseType(typeof(ProgramLovDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByTypeCode(string typeCode, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetProgramLovByTypeCodeQuery(typeCode), cancellationToken);
        return Ok(result);
    }
}
