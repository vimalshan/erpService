using EmployeeService.Application.Queries.Employees;
using HotChocolate;
using MediatR;

namespace EmployeeService.API.GraphQL
{
    /// <summary>
    /// GraphQL Query Type - Define all available queries for employee data
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details or null if not found</returns>
        public async Task<EmployeeDto> GetEmployeeAsync(long id, [Service] IMediator mediatr)
        {
            var query = new GetEmployeeByIdQuery { EmployeeId = id };
            return await mediatr.Send(query);
        }

        /// <summary>
        /// Get employee by employee number
        /// </summary>
        /// <param name="employeeNumber">Employee number (e.g., EMP001)</param>
        /// <returns>Employee details or null if not found</returns>
        public async Task<EmployeeDto> GetEmployeeByNumberAsync(string employeeNumber, [Service] IMediator mediatr)
        {
            var query = new GetEmployeeByNumberQuery { EmployeeNumber = employeeNumber };
            return await mediatr.Send(query);
        }

        /// <summary>
        /// Get all active employees
        /// </summary>
        /// <returns>List of active employees</returns>
        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync([Service] IMediator mediatr)
        {
            var query = new GetAllActiveEmployeesQuery();
            return await mediatr.Send(query);
        }

        /// <summary>
        /// Search employees by name, email, or employee number
        /// </summary>
        /// <param name="searchTerm">Search term (name, email, or employee number)</param>
        /// <returns>List of matching employees</returns>
        public async Task<IEnumerable<EmployeeDto>> SearchEmployeesAsync(string searchTerm, [Service] IMediator mediatr)
        {
            var query = new SearchEmployeesQuery { SearchTerm = searchTerm };
            return await mediatr.Send(query);
        }

        /// <summary>
        /// Get employees by organizational unit
        /// </summary>
        /// <param name="unitId">Unit ID</param>
        /// <returns>List of employees in the unit</returns>
        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByUnitAsync(long unitId, [Service] IMediator mediatr)
        {
            var query = new GetEmployeesByUnitQuery { UnitId = unitId };
            return await mediatr.Send(query);
        }

        /// <summary>
        /// Get employees by grade
        /// </summary>
        /// <param name="gradeCode">Grade code (e.g., A-1, A-2, B-1)</param>
        /// <returns>List of employees with the grade</returns>
        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByGradeAsync(string gradeCode, [Service] IMediator mediatr)
        {
            var query = new GetEmployeesByGradeQuery { GradeCode = gradeCode };
            return await mediatr.Send(query);
        }

        /// <summary>
        /// Get employee count - total active employees
        /// </summary>
        /// <returns>Total number of active employees</returns>
        public async Task<int> GetEmployeeCountAsync([Service] IMediator mediatr)
        {
            var query = new GetAllActiveEmployeesQuery();
            var employees = await mediatr.Send(query);
            return employees?.Count ?? 0;
        }

        /// <summary>
        /// Get employee statistics
        /// </summary>
        /// <returns>Statistics about employees</returns>
        public async Task<EmployeeStatisticsDto> GetStatisticsAsync([Service] IMediator mediatr)
        {
            var query = new GetEmployeeStatisticsQuery();
            return await mediatr.Send(query);
        }

        /// <summary>
        /// Health check - verify GraphQL is working
        /// </summary>
        /// <returns>Status message</returns>
        public string HealthCheck => "GraphQL API is operational";
    }
}
