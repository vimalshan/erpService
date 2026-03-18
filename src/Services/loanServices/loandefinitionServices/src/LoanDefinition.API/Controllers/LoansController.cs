using LoanDefinition.Application.DTOs;
using LoanDefinition.Application.Features.Loans.Commands;
using LoanDefinition.Application.Features.Loans.Queries;
using LoanDefinition.Infrastructure.BlobStorage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanDefinition.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController(IMediator mediator, IBlobStorageService blobStorage) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<LoanMasterDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllLoansQuery());
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<LoanMasterDto>> GetById(long id)
    {
        var result = await mediator.Send(new GetLoanByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:long}/details")]
    [AllowAnonymous]
    public async Task<ActionResult<LoanMasterDetailDto>> GetDetails(long id)
    {
        var result = await mediator.Send(new GetLoanDetailQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("type/{typeId:long}")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<LoanMasterDto>>> GetByType(long typeId)
    {
        var result = await mediator.Send(new GetLoansByTypeQuery(typeId));
        return Ok(result);
    }

    [HttpGet("active")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<LoanMasterDto>>> GetActive()
    {
        var result = await mediator.Send(new GetActiveLoansQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<LoanMasterDto>> Create([FromBody] CreateLoanCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.LoanId }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<LoanMasterDto>> Update(long id, [FromBody] UpdateLoanCommand command)
    {
        if (id != command.LoanId) return BadRequest("ID mismatch");
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:long}/close")]
    public async Task<IActionResult> Close(long id, [FromBody] CloseLoanCommand command)
    {
        if (id != command.LoanId) return BadRequest("ID mismatch");
        var result = await mediator.Send(command);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await mediator.Send(new DeleteLoanCommand(id));
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{id:long}/policy")]
    public async Task<ActionResult<string>> UploadPolicy(long id, IFormFile file)
    {
        if (file.Length == 0) return BadRequest("Empty file");

        var blobName = $"loans/{id}/policy/{file.FileName}";
        await using var stream = file.OpenReadStream();
        var url = await blobStorage.UploadAsync("loan-policies", blobName, stream, file.ContentType);
        return Ok(new { Url = url });
    }
}
