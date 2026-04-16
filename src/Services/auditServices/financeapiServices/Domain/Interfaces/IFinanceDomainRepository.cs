using FinanceService.Domain.Entities;

namespace FinanceService.Domain.Interfaces;

public interface IFinanceDomainRepository
{
    Task<Invoice?> GetInvoiceByIdAsync(int id);
    Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
    Task<IEnumerable<Invoice>> GetInvoicesByCompanyAsync(int companyId);
    Task<Invoice> AddInvoiceAsync(Invoice invoice);
    Task UpdateInvoiceAsync(Invoice invoice);
    Task DeleteInvoiceAsync(int id);
    Task<Financial?> GetFinancialByIdAsync(int id);
    Task<IEnumerable<Financial>> GetFinancialsByCompanyAsync(int companyId, int? year);
    Task<Financial> AddFinancialAsync(Financial financial);
    Task UpdateFinancialAsync(Financial financial);
}
