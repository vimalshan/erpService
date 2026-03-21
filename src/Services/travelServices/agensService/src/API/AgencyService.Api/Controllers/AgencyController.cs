using AgencyService.Application.Commands;
using AgencyService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgencyService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgencyController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AgencyController> _logger;
    
    public AgencyController(IMediator mediator, ILogger<AgencyController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    [HttpGet("{agencyCode}")]
    public async Task<IActionResult> GetAgency(long agencyCode)
    {
        var query = new GetAgencyByCodeQuery { AgencyCode = agencyCode };
        var result = await _mediator.Send(query);
        
        if (!result.Success)
            return NotFound(result);
        
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAgencies()
    {
        var query = new GetAllAgenciesQuery();
        var result = await _mediator.Send(query);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
    
    [HttpGet("type/{type}")]
    public async Task<IActionResult> GetAgenciesByType(string type)
    {
        var query = new GetAgenciesByTypeQuery { Type = type };
        var result = await _mediator.Send(query);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAgency([FromBody] CreateAgencyCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);
        
        return CreatedAtAction(nameof(GetAgency), new { agencyCode = result.Data }, result);
    }
    
    [HttpPut("{agencyCode}")]
    public async Task<IActionResult> UpdateAgency(long agencyCode, [FromBody] UpdateAgencyCommand command)
    {
        command.AgencyCode = agencyCode;
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
    
    [HttpDelete("{agencyCode}")]
    public async Task<IActionResult> DeleteAgency(long agencyCode)
    {
        var command = new DeleteAgencyCommand { AgencyCode = agencyCode };
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
}
