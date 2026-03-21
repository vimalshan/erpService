using AgencyService.Application.Commands;
using AgencyService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgencyService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendorController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<VendorController> _logger;
    
    public VendorController(IMediator mediator, ILogger<VendorController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    [HttpGet("{vendorId}")]
    public async Task<IActionResult> GetVendor(long vendorId)
    {
        var query = new GetVendorByIdQuery { VendorId = vendorId };
        var result = await _mediator.Send(query);
        
        if (!result.Success)
            return NotFound(result);
        
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllVendors()
    {
        var query = new GetAllVendorsQuery();
        var result = await _mediator.Send(query);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
    
    [HttpGet("category/{categoryType}")]
    public async Task<IActionResult> GetVendorsByCategory(string categoryType)
    {
        var query = new GetVendorsByCategoryQuery { CategoryType = categoryType };
        var result = await _mediator.Send(query);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateVendor([FromBody] CreateVendorCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);
        
        return CreatedAtAction(nameof(GetVendor), new { vendorId = result.Data }, result);
    }
    
    [HttpPut("{vendorId}")]
    public async Task<IActionResult> UpdateVendor(long vendorId, [FromBody] UpdateVendorCommand command)
    {
        command.VendorId = vendorId;
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
    
    [HttpDelete("{vendorId}")]
    public async Task<IActionResult> DeleteVendor(long vendorId)
    {
        var command = new DeleteVendorCommand { VendorId = vendorId };
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
}
