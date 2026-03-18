using Microsoft.AspNetCore.Mvc;
using MediatR;
using ReferenceService.Application.Commands.LovType;
using ReferenceService.Application.Commands.LovValue;
using ReferenceService.Application.Queries.LovType;
using ReferenceService.Application.Queries.LovValue;
using ReferenceService.Application.DTOs;

namespace ReferenceService.API.Controllers;

/// <summary>
/// REST API controller for LOV Type operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LovTypesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LovTypesController> _logger;
    
    public LovTypesController(IMediator mediator, ILogger<LovTypesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    /// <summary>
    /// Get all LOV Types with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<LovTypeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllLovTypesQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(new ApiResponse<PaginatedResponse<LovTypeDto>>
        {
            Success = true,
            Message = "Success",
            Data = result,
            StatusCode = 200
        });
    }
    
    /// <summary>
    /// Get LOV Type by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<LovTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetLovTypeByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "LOV Type not found",
                StatusCode = 404
            });
        }
        
        return Ok(new ApiResponse<LovTypeDto>
        {
            Success = true,
            Message = "Success",
            Data = result,
            StatusCode = 200
        });
    }
    
    /// <summary>
    /// Create a new LOV Type.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreateLovTypeResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateLovTypeCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = result.Message,
                StatusCode = 400
            });
        }
        
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, new ApiResponse<CreateLovTypeResponse>
        {
            Success = true,
            Message = "LOV Type created successfully",
            Data = result,
            StatusCode = 201
        });
    }
    
    /// <summary>
    /// Update an existing LOV Type.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, UpdateLovTypeCommand command)
    {
        if (command.Id != id)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "ID mismatch",
                StatusCode = 400
            });
        
        var result = await _mediator.Send(command);
        
        if (!result.Success)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = result.Message,
                StatusCode = 400
            });
        }
        
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "LOV Type updated successfully",
            StatusCode = 200
        });
    }
    
    /// <summary>
    /// Deactivate a LOV Type.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id, [FromQuery] long modifiedBy)
    {
        var command = new DeactivateLovTypeCommand(id, modifiedBy);
        var result = await _mediator.Send(command);
        
        return Ok(new ApiResponse<object>
        {
            Success = result.Success,
            Message = result.Message,
            StatusCode = 200
        });
    }
}

/// <summary>
/// REST API controller for LOV Value operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LovValuesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LovValuesController> _logger;
    
    public LovValuesController(IMediator mediator, ILogger<LovValuesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    /// <summary>
    /// Get LOV Values for a specific type.
    /// </summary>
    [HttpGet("by-type/{typeId}")]
    [ProducesResponseType(typeof(ApiResponse<List<LovValueDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByType(int typeId)
    {
        var query = new GetLovValuesByTypeQuery(typeId);
        var result = await _mediator.Send(query);
        return Ok(new ApiResponse<List<LovValueDto>>
        {
            Success = true,
            Message = "Success",
            Data = result,
            StatusCode = 200
        });
    }
    
    /// <summary>
    /// Get LOV Value by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<LovValueDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetLovValueByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "LOV Value not found",
                StatusCode = 404
            });
        
        return Ok(new ApiResponse<LovValueDto>
        {
            Success = true,
            Data = result,
            StatusCode = 200
        });
    }
    
    /// <summary>
    /// Create a new LOV Value.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreateLovValueResponse>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateLovValueCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = result.Message,
                StatusCode = 400
            });
        
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, new ApiResponse<CreateLovValueResponse>
        {
            Success = true,
            Message = "LOV Value created successfully",
            Data = result,
            StatusCode = 201
        });
    }
}
