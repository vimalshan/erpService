using EmployeeService.Application.Commands.Employees;
using EmployeeService.Application.Queries.Employees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace EmployeeService.API.Controllers
{
    /// <summary>
    /// Employee Management API Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [SwaggerTag("Employee Management operations")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediatr;

        public EmployeesController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,HR,Manager")]
        [SwaggerOperation(Summary = "Get employee by ID")]
        [SwaggerResponse(200, "Employee found", typeof(EmployeeDto))]
        [SwaggerResponse(404, "Employee not found")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeById(long id)
        {
            var query = new GetEmployeeByIdQuery { EmployeeId = id };
            var result = await _mediatr.Send(query);

            if (result == null)
                return NotFound("Employee not found.");

            return Ok(result);
        }

        /// <summary>
        /// Get employee by employee number
        /// </summary>
        /// <param name="employeeNumber">Employee number</param>
        /// <returns>Employee details</returns>
        [HttpGet("number/{employeeNumber}")]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Get employee by number")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeByNumber(string employeeNumber)
        {
            var query = new GetEmployeeByNumberQuery { EmployeeNumber = employeeNumber };
            var result = await _mediatr.Send(query);

            if (result == null)
                return NotFound("Employee not found.");

            return Ok(result);
        }

        /// <summary>
        /// Get all active employees
        /// </summary>
        /// <returns>List of active employees</returns>
        [HttpGet("active")]
        [Authorize(Roles = "Admin,HR,Manager")]
        [SwaggerOperation(Summary = "Get all active employees")]
        public async Task<ActionResult> GetActiveEmployees()
        {
            var query = new GetAllActiveEmployeesQuery();
            var result = await _mediatr.Send(query);

            return Ok(new
            {
                success = true,
                data = result,
                count = result.Count
            });
        }

        /// <summary>
        /// Get employees by unit
        /// </summary>
        /// <param name="unitId">Unit ID</param>
        /// <returns>List of employees in unit</returns>
        [HttpGet("unit/{unitId}")]
        [Authorize(Roles = "Admin,HR,Manager")]
        [SwaggerOperation(Summary = "Get employees by unit")]
        public async Task<ActionResult> GetEmployeesByUnit(long unitId)
        {
            var query = new GetEmployeesByUnitQuery { UnitId = unitId };
            var result = await _mediatr.Send(query);

            return Ok(new
            {
                success = true,
                data = result,
                count = result.Count
            });
        }

        /// <summary>
        /// Get employees by grade
        /// </summary>
        /// <param name="gradeCode">Grade code</param>
        /// <returns>List of employees in grade</returns>
        [HttpGet("grade/{gradeCode}")]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Get employees by grade")]
        public async Task<ActionResult> GetEmployeesByGrade(string gradeCode)
        {
            var query = new GetEmployeesByGradeQuery { GradeCode = gradeCode };
            var result = await _mediatr.Send(query);

            return Ok(new
            {
                success = true,
                data = result,
                count = result.Count
            });
        }

        /// <summary>
        /// Search employees
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching employees</returns>
        [HttpGet("search")]
        [Authorize(Roles = "Admin,HR,Manager")]
        [SwaggerOperation(Summary = "Search employees")]
        public async Task<ActionResult> SearchEmployees([FromQuery] string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return BadRequest("Search term is required.");

            var query = new SearchEmployeesQuery { SearchTerm = searchTerm };
            var result = await _mediatr.Send(query);

            return Ok(new
            {
                success = true,
                data = result,
                count = result.Count
            });
        }

        /// <summary>
        /// Get employee statistics
        /// </summary>
        /// <returns>Employee statistics</returns>
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Get employee statistics")]
        public async Task<ActionResult<EmployeeStatisticsDto>> GetStatistics()
        {
            var query = new GetEmployeeStatisticsQuery();
            var result = await _mediatr.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Get employee with all details
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Detailed employee information</returns>
        [HttpGet("{id}/details")]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Get employee with details")]
        public async Task<ActionResult<EmployeeDetailedDto>> GetEmployeeDetails(long id)
        {
            var query = new GetEmployeeWithDetailsQuery { EmployeeId = id };
            var result = await _mediatr.Send(query);

            if (result == null)
                return NotFound("Employee not found.");

            return Ok(result);
        }

        /// <summary>
        /// Create new employee
        /// </summary>
        /// <param name="command">Create employee command</param>
        /// <returns>Created employee response</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Create new employee")]
        [SwaggerResponse(201, "Employee created successfully", typeof(CreateEmployeeResponse))]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<CreateEmployeeResponse>> CreateEmployee(CreateEmployeeCommand command)
        {
            var result = await _mediatr.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetEmployeeById), new { id = result.EmployeeId }, result);
        }

        /// <summary>
        /// Update employee personal information
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="command">Update command</param>
        /// <returns>Success response</returns>
        [HttpPut("{id}/personal-info")]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Update employee personal information")]
        public async Task<ActionResult<BaseResponse>> UpdatePersonalInfo(long id, UpdateEmployeePersonalInfoCommand command)
        {
            command.EmployeeId = id;
            var result = await _mediatr.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Update employee contact information
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="command">Update command</param>
        /// <returns>Success response</returns>
        [HttpPut("{id}/contact")]
        [Authorize(Roles = "Admin,HR,Employee")]
        [SwaggerOperation(Summary = "Update employee contact information")]
        public async Task<ActionResult<BaseResponse>> UpdateContact(long id, UpdateEmployeeContactCommand command)
        {
            command.EmployeeId = id;
            var result = await _mediatr.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Update employee salary
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="command">Update salary command</param>
        /// <returns>Success response</returns>
        [HttpPut("{id}/salary")]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Update employee salary")]
        public async Task<ActionResult<BaseResponse>> UpdateSalary(long id, UpdateEmployeeSalaryCommand command)
        {
            command.EmployeeId = id;
            var result = await _mediatr.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Promote employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="command">Promotion command</param>
        /// <returns>Success response</returns>
        [HttpPut("{id}/promote")]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Promote employee")]
        public async Task<ActionResult<BaseResponse>> PromoteEmployee(long id, PromoteEmployeeCommand command)
        {
            command.EmployeeId = id;
            var result = await _mediatr.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Terminate employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="command">Termination command</param>
        /// <returns>Success response</returns>
        [HttpPut("{id}/terminate")]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Terminate employee")]
        public async Task<ActionResult<BaseResponse>> TerminateEmployee(long id, TerminateEmployeeCommand command)
        {
            command.EmployeeId = id;
            var result = await _mediatr.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Reactivate terminated employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="command">Reactivation command</param>
        /// <returns>Success response</returns>
        [HttpPut("{id}/reactivate")]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Reactivate employee")]
        public async Task<ActionResult<BaseResponse>> ReactivateEmployee(long id, ReactivateEmployeeCommand command)
        {
            command.EmployeeId = id;
            var result = await _mediatr.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Delete employee (soft delete)
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Success response</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,HR")]
        [SwaggerOperation(Summary = "Delete employee")]
        public async Task<ActionResult<BaseResponse>> DeleteEmployee(long id)
        {
            var command = new DeleteEmployeeCommand { EmployeeId = id };
            var result = await _mediatr.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
