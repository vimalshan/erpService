using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AccidentManagementService.DTOs;
using AccidentManagementService.Infrastructure.Persistence;

namespace AccidentManagementService.Controllers
{
    /// <summary>
    /// Controller for managing accident records
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class AccidentController : ControllerBase
    {
        private readonly ILogger<AccidentController> _logger;
        private readonly AccidentManagementDbContext _dbContext;

        public AccidentController(ILogger<AccidentController> logger, AccidentManagementDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all accidents
        /// </summary>
        /// <returns>A list of all accidents</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<DailyAccidentFIRDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<DailyAccidentFIRDto>>> GetAllAccidents()
        {
            try
            {
                _logger.LogInformation("Retrieving all accidents");
                var accidents = await _dbContext.DailyAccidentFIRs
                    .Select(a => new DailyAccidentFIRDto
                    {
                        AccidentNumber = a.AccidentNumber,
                        EmployeeNumber = a.EmployeeNumber,
                        EmployeeName = a.EmployeeName,
                        WorkerName = a.WorkerName,
                        ContractorId = a.ContractorId,
                        ContractorName = a.ContractorName,
                        EmployeeDepartment = a.EmployeeDepartment,
                        AccidentDateTime = a.AccidentDateTime,
                        AccidentLocation = a.AccidentLocation,
                        NatureOfInjury = a.NatureOfInjury,
                        BodyPartAffected = a.BodyPartAffected,
                        ShiftName = a.ShiftName,
                        MedicalCentreName = a.MedicalCentreName,
                        TreatmentGiven = a.TreatmentGiven,
                        MedicalCentreReceivingDate = a.MedicalCentreReceivingDate,
                        CompanyCode = a.CompanyCode,
                        InjuryCategoryCode = a.InjuryCategoryCode,
                        NatureOfInjuryCode = a.NatureOfInjuryCode,
                        PreventiveMeasures = a.PreventiveMeasures,
                        CauseOfIncident = a.CauseOfIncident,
                        Status = a.Status,
                        Remarks = a.Remarks
                    })
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} accidents", accidents.Count);
                return Ok(accidents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving accidents");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving accidents");
            }
        }

        /// <summary>
        /// Get a specific accident by number
        /// </summary>
        /// <param name="accidentNumber">The accident number</param>
        /// <returns>The accident details</returns>
        [HttpGet("{accidentNumber}")]
        [ProducesResponseType(typeof(DailyAccidentFIRDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DailyAccidentFIRDto>> GetAccidentByNumber(decimal accidentNumber)
        {
            try
            {
                _logger.LogInformation("Retrieving accident with number: {AccidentNumber}", accidentNumber);
                var accident = await _dbContext.DailyAccidentFIRs
                    .Where(a => a.AccidentNumber == accidentNumber)
                    .Select(a => new DailyAccidentFIRDto
                    {
                        AccidentNumber = a.AccidentNumber,
                        EmployeeNumber = a.EmployeeNumber,
                        EmployeeName = a.EmployeeName,
                        WorkerName = a.WorkerName,
                        ContractorId = a.ContractorId,
                        ContractorName = a.ContractorName,
                        EmployeeDepartment = a.EmployeeDepartment,
                        AccidentDateTime = a.AccidentDateTime,
                        AccidentLocation = a.AccidentLocation,
                        NatureOfInjury = a.NatureOfInjury,
                        BodyPartAffected = a.BodyPartAffected,
                        ShiftName = a.ShiftName,
                        MedicalCentreName = a.MedicalCentreName,
                        TreatmentGiven = a.TreatmentGiven,
                        MedicalCentreReceivingDate = a.MedicalCentreReceivingDate,
                        CompanyCode = a.CompanyCode,
                        InjuryCategoryCode = a.InjuryCategoryCode,
                        NatureOfInjuryCode = a.NatureOfInjuryCode,
                        PreventiveMeasures = a.PreventiveMeasures,
                        CauseOfIncident = a.CauseOfIncident,
                        Status = a.Status,
                        Remarks = a.Remarks
                    })
                    .FirstOrDefaultAsync();

                if (accident == null)
                {
                    _logger.LogWarning("Accident with number {AccidentNumber} not found", accidentNumber);
                    return NotFound($"Accident with number {accidentNumber} not found");
                }

                return Ok(accident);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving accident with number: {AccidentNumber}", accidentNumber);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the accident");
            }
        }

        /// <summary>
        /// Create a new accident record
        /// </summary>
        /// <param name="createAccidentDto">The accident details to create</param>
        /// <returns>The created accident</returns>
        [HttpPost]
        [ProducesResponseType(typeof(DailyAccidentFIRDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DailyAccidentFIRDto>> CreateAccident([FromBody] CreateDailyAccidentFIRDto createAccidentDto)
        {
            try
            {
                _logger.LogInformation("Creating new accident record");
                
                if (createAccidentDto == null)
                {
                    return BadRequest("Accident data is required");
                }

                // TODO: Implement command handler with MediatR
                return StatusCode(StatusCodes.Status201Created, "Accident created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating accident");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the accident");
            }
        }

        /// <summary>
        /// Update an existing accident record
        /// </summary>
        /// <param name="accidentNumber">The accident number to update</param>
        /// <param name="updateDto">The updated accident details</param>
        /// <returns>The updated accident</returns>
        [HttpPut("{accidentNumber}")]
        [ProducesResponseType(typeof(DailyAccidentFIRDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DailyAccidentFIRDto>> UpdateAccident(decimal accidentNumber, [FromBody] UpdateDailyAccidentFIRDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating accident with number: {AccidentNumber}", accidentNumber);
                
                if (updateDto == null)
                {
                    return BadRequest("Update data is required");
                }

                // TODO: Implement command handler with MediatR
                return Ok("Accident updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating accident with number: {AccidentNumber}", accidentNumber);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the accident");
            }
        }

        /// <summary>
        /// Delete an accident record
        /// </summary>
        /// <param name="accidentNumber">The accident number to delete</param>
        /// <returns>Success or failure message</returns>
        [HttpDelete("{accidentNumber}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAccident(decimal accidentNumber)
        {
            try
            {
                _logger.LogInformation("Deleting accident with number: {AccidentNumber}", accidentNumber);
                // TODO: Implement command handler
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting accident with number: {AccidentNumber}", accidentNumber);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the accident");
            }
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns>Service status</returns>
        [AllowAnonymous]
        [HttpGet("health/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult HealthCheck()
        {
            _logger.LogInformation("Health check endpoint called");
            return Ok(new { status = "healthy", service = "AccidentManagementService", timestamp = DateTime.UtcNow });
        }
    }
}
