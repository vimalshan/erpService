using Dapper;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Infrastructure.Dapper;

public class DapperEmployeeRepository
{
    private readonly DapperContext _context;

    public DapperEmployeeRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllActiveAsync()
    {
        const string sql = """
            SELECT employee_id AS EmployeeId, user_id AS UserId, first_name AS FirstName,
                   last_name AS LastName, employee_code AS EmployeeCode, hire_date AS HireDate,
                   job_title AS JobTitle, department AS Department, warehouse_id AS WarehouseId,
                   phone AS Phone, email AS Email, is_active AS IsActive,
                   created_date AS CreatedDate, modified_date AS ModifiedDate
            FROM Employee
            WHERE is_active = 1
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<EmployeeDto>(sql);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int employeeId)
    {
        const string sql = """
            SELECT employee_id AS EmployeeId, user_id AS UserId, first_name AS FirstName,
                   last_name AS LastName, employee_code AS EmployeeCode, hire_date AS HireDate,
                   job_title AS JobTitle, department AS Department, warehouse_id AS WarehouseId,
                   phone AS Phone, email AS Email, is_active AS IsActive,
                   created_date AS CreatedDate, modified_date AS ModifiedDate
            FROM Employee
            WHERE employee_id = @EmployeeId
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<EmployeeDto>(sql, new { EmployeeId = employeeId });
    }

    public async Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(string department)
    {
        const string sql = """
            SELECT employee_id AS EmployeeId, user_id AS UserId, first_name AS FirstName,
                   last_name AS LastName, employee_code AS EmployeeCode, hire_date AS HireDate,
                   job_title AS JobTitle, department AS Department, warehouse_id AS WarehouseId,
                   phone AS Phone, email AS Email, is_active AS IsActive,
                   created_date AS CreatedDate, modified_date AS ModifiedDate
            FROM Employee
            WHERE department = @Department AND is_active = 1
            """;

        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<EmployeeDto>(sql, new { Department = department });
    }
}
