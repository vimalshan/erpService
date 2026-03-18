using LoanDefinition.Application.DTOs;
using LoanDefinition.Application.Features.LoanTypes.Commands;
using LoanDefinition.Application.Features.LoanTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanDefinition.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoanTypesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<LoanTypeMasterDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllLoanTypesQuery());
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<LoanTypeMasterDto>> GetById(long id)
    {
        var result = await mediator.Send(new GetLoanTypeByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("category/{category}")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<LoanTypeMasterDto>>> GetByCategory(string category)
    {
        var result = await mediator.Send(new GetLoanTypesByCategoryQuery(category));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<LoanTypeMasterDto>> Create([FromBody] CreateLoanTypeCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.LoanType }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<LoanTypeMasterDto>> Update(long id, [FromBody] UpdateLoanTypeCommand command)
    {
        if (id != command.LoanType) return BadRequest("ID mismatch");
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await mediator.Send(new DeleteLoanTypeCommand(id));
        return result ? NoContent() : NotFound();
    }
}
