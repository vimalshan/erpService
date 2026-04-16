using ContractService.Domain.Entities;

namespace ContractService.Domain.Interfaces;

public interface IContractDomainRepository
{
    Task<Contract?> GetByIdAsync(int id);
    Task<IEnumerable<Contract>> GetAllAsync();
    Task<IEnumerable<Contract>> GetByCompanyAsync(int companyId);
    Task<Contract> AddAsync(Contract contract);
    Task UpdateAsync(Contract contract);
    Task DeleteAsync(int id);
}
