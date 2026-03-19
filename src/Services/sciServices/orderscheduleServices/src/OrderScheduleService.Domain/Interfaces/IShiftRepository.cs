namespace OrderScheduleService.Domain.Interfaces;

using OrderScheduleService.Domain.Entities;

public interface IShiftRepository
{
    Task<Shift?> GetByIdAsync(char shiftCode, decimal companyUnitId);
    Task<IEnumerable<Shift>> GetByCompanyAsync(decimal companyUnitId);
    Task<IEnumerable<Shift>> GetAllAsync();
    Task AddAsync(Shift shift);
    Task UpdateAsync(Shift shift);
    Task DeleteAsync(char shiftCode, decimal companyUnitId);
    Task SaveChangesAsync();
}
