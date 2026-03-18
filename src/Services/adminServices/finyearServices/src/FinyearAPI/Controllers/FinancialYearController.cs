using Microsoft.AspNetCore.Mvc;
using FinyearAPI.Application.Services;
using FinyearAPI.Application.DTOs;
using FinyearAPI.Domain.Entities;
using FinyearAPI.Services;

namespace FinyearAPI.Controllers
{
    /// <summary>
    /// Financial Year API Controller
    /// Provides REST endpoints for financial year management
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FinancialYearController : ControllerBase
    {
        private readonly IFinancialYearService _financialYearService;
        private readonly ILogger<FinancialYearController> _logger;

        public FinancialYearController(IFinancialYearService financialYearService, ILogger<FinancialYearController> logger)
        {
            _financialYearService = financialYearService;
            _logger = logger;
        }

        /// <summary>
        /// Get all financial years
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<IEnumerable<FinancialYearMaster>>> GetAllFinancialYears()
        {
            _logger.LogInformation("GET: Retrieving all financial years");
            try
            {
                var result = await _financialYearService.GetAllFinancialYearsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving financial years - database may be unavailable");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new 
                { 
                    message = "Financial Year Service Unavailable",
                    details = "Unable to connect to database. Please ensure the database is available.",
                    error = ex.GetType().Name
                });
            }
        }

        /// <summary>
        /// Get financial year by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FinancialYearMaster>> GetFinancialYearById(long id)
        {
            _logger.LogInformation("GET: Retrieving financial year with ID: {Id}", id);
            var result = await _financialYearService.GetFinancialYearByIdAsync(id);
            if (result == null)
                return NotFound(new { message = $"Financial year with ID {id} not found" });
            return Ok(result);
        }

        /// <summary>
        /// Get current active financial year
        /// </summary>
        [HttpGet("current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FinancialYearMaster>> GetCurrentFinancialYear()
        {
            _logger.LogInformation("GET: Retrieving current financial year");
            var result = await _financialYearService.GetCurrentFinancialYearAsync();
            if (result == null)
                return NotFound(new { message = "No active financial year found" });
            return Ok(result);
        }

        /// <summary>
        /// Get financial year by name
        /// </summary>
        [HttpGet("by-name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FinancialYearMaster>> GetFinancialYearByName(string name)
        {
            _logger.LogInformation("GET: Retrieving financial year by name: {Name}", name);
            var result = await _financialYearService.GetFinancialYearByNameAsync(name);
            if (result == null)
                return NotFound(new { message = $"Financial year with name {name} not found" });
            return Ok(result);
        }

        /// <summary>
        /// Create a new financial year
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FinancialYearMaster>> CreateFinancialYear([FromBody] CreateFinancialYearDto dto)
        {
            _logger.LogInformation("POST: Creating financial year: {Name}", dto.FinancialYearName);
            try
            {
                var result = await _financialYearService.CreateFinancialYearAsync(dto);
                return CreatedAtAction(nameof(GetFinancialYearById), new { id = result.FinancialYearId }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating financial year");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the financial year" });
            }
        }

        /// <summary>
        /// Update an existing financial year
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FinancialYearMaster>> UpdateFinancialYear(long id, [FromBody] UpdateFinancialYearDto dto)
        {
            _logger.LogInformation("PUT: Updating financial year with ID: {Id}", id);
            try
            {
                var result = await _financialYearService.UpdateFinancialYearAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating financial year");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the financial year" });
            }
        }

        /// <summary>
        /// Delete a financial year
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFinancialYear(long id)
        {
            _logger.LogInformation("DELETE: Deleting financial year with ID: {Id}", id);
            try
            {
                var result = await _financialYearService.DeleteFinancialYearAsync(id);
                if (!result)
                    return NotFound(new { message = $"Financial year with ID {id} not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting financial year");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the financial year" });
            }
        }
    }
}
