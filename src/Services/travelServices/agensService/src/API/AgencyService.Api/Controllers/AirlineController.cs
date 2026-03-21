using AgencyService.Application.Commands;
using AgencyService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgencyService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirlineController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AirlineController> _logger;
    
    public AirlineController(IMediator mediator, ILogger<AirlineController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    [HttpGet("{code}")]
    public async Task<IActionResult> GetAirline(string code)
    {
        var query = new GetAirlineByCodeQuery { Code = code };
        var result = await _mediator.Send(query);
        
        if (!result.Success)
            return NotFound(result);
        
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAirlines()
    {
        var query = new GetAllAirlinesQuery();
        var result = await _mediator.Send(query);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAirline([FromBody] CreateAirlineCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);
        
        return CreatedAtAction(nameof(GetAirline), new { code = result.Data }, result);
    }
}
