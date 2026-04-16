using ContractService.Domain.Entities;
using ContractService.Domain.Interfaces;
using ContractService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContractService.Infrastructure.Repositories;

public class EfContractDomainRepository : IContractDomainRepository
{
    private readonly ContractDomainDbContext _ctx;
    public EfContractDomainRepository(ContractDomainDbContext ctx) { _ctx = ctx; }

    public async Task<Contract?> GetByIdAsync(int id) =>
        await _ctx.Contracts.Include(c => c.ContractServices).Include(c => c.ContractSites)
            .FirstOrDefaultAsync(c => c.ContractId == id);

    public async Task<IEnumerable<Contract>> GetAllAsync() =>
        await _ctx.Contracts.Include(c => c.ContractServices).Include(c => c.ContractSites)
            .OrderByDescending(c => c.CreatedDate).ToListAsync();

    public async Task<IEnumerable<Contract>> GetByCompanyAsync(int companyId) =>
        await _ctx.Contracts.Include(c => c.ContractServices).Include(c => c.ContractSites)
            .Where(c => c.CompanyId == companyId).OrderByDescending(c => c.CreatedDate).ToListAsync();

    public async Task<Contract> AddAsync(Contract contract)
    {
        _ctx.Contracts.Add(contract);
        await _ctx.SaveChangesAsync();
        return contract;
    }

    public async Task UpdateAsync(Contract contract)
    {
        _ctx.Contracts.Update(contract);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _ctx.Contracts.FindAsync(id);
        if (entity != null) { _ctx.Contracts.Remove(entity); await _ctx.SaveChangesAsync(); }
    }
}
