using EmployeePrideManagement.Application.Commands.CreatePrideMoment;
using EmployeePrideManagement.Application.Commands.DeletePrideMoment;
using EmployeePrideManagement.Application.Commands.UpdatePrideMoment;
using EmployeePrideManagement.Application.DTOs;
using EmployeePrideManagement.Application.Queries.GetAllPrideMoments;
using EmployeePrideManagement.Application.Queries.GetPrideMomentById;
using EmployeePrideManagement.Application.Queries.GetPrideMomentsByEmployee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePrideManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PrideMomentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PrideMomentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<PrideMomentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<PrideMomentDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllPrideMomentsQuery(pageNumber, pageSize));
        return Ok(result);
    }

    [HttpGet("{id:decimal}")]
    [ProducesResponseType(typeof(PrideMomentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrideMomentDto>> GetById(decimal id)
    {
        var result = await _mediator.Send(new GetPrideMomentByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("employee/{employeeSysId:decimal}")]
    [ProducesResponseType(typeof(IEnumerable<PrideMomentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrideMomentDto>>> GetByEmployee(decimal employeeSysId)
    {
        var result = await _mediator.Send(new GetPrideMomentsByEmployeeQuery(employeeSysId));
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PrideMomentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PrideMomentDto>> Create([FromBody] CreatePrideMomentDto dto)
    {
        var command = new CreatePrideMomentCommand(
            dto.Title, dto.Body, dto.EmployeeSysId,
            dto.Footer, dto.Location, dto.ImagePath, dto.ModifiedBy);

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.MomentPrideId }, result);
    }

    [HttpPut("{id:decimal}")]
    [ProducesResponseType(typeof(PrideMomentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrideMomentDto>> Update(decimal id, [FromBody] UpdatePrideMomentDto dto)
    {
        var command = new UpdatePrideMomentCommand(
            id, dto.Title, dto.Body, dto.Footer,
            dto.Location, dto.ImagePath, dto.ModifiedBy);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:decimal}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(decimal id)
    {
        await _mediator.Send(new DeletePrideMomentCommand(id));
        return NoContent();
    }
}
