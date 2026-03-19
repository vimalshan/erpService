namespace EmployeePrideManagement.Domain.Interfaces;

public interface IDapperPrideMomentRepository
{
    Task<T?> GetByIdAsync<T>(decimal id) where T : class;
    Task<IEnumerable<T>> GetByEmployeeIdAsync<T>(decimal employeeSysId) where T : class;
    Task<(IEnumerable<T> Items, int TotalCount)> GetAllPagedAsync<T>(int pageNumber, int pageSize) where T : class;
}
