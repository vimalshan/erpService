using FinanceService.Domain.Entities;
using FinanceService.Domain.Interfaces;
using FinanceService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Repositories;

public class EfFinanceDomainRepository : IFinanceDomainRepository
{
    private readonly FinanceDomainDbContext _ctx;
    public EfFinanceDomainRepository(FinanceDomainDbContext ctx) { _ctx = ctx; }

    public async Task<Invoice?> GetInvoiceByIdAsync(int id) =>
        await _ctx.Invoices.Include(i => i.AuditLogs).FirstOrDefaultAsync(i => i.InvoiceId == id);

    public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync() =>
        await _ctx.Invoices.OrderByDescending(i => i.CreatedDate).ToListAsync();

    public async Task<IEnumerable<Invoice>> GetInvoicesByCompanyAsync(int companyId) =>
        await _ctx.Invoices.Where(i => i.CompanyId == companyId).OrderByDescending(i => i.CreatedDate).ToListAsync();

    public async Task<Invoice> AddInvoiceAsync(Invoice invoice)
    {
        _ctx.Invoices.Add(invoice); await _ctx.SaveChangesAsync(); return invoice;
    }

    public async Task UpdateInvoiceAsync(Invoice invoice)
    {
        _ctx.Invoices.Update(invoice); await _ctx.SaveChangesAsync();
    }

    public async Task DeleteInvoiceAsync(int id)
    {
        var entity = await _ctx.Invoices.FindAsync(id);
        if (entity != null) { _ctx.Invoices.Remove(entity); await _ctx.SaveChangesAsync(); }
    }

    public async Task<Financial?> GetFinancialByIdAsync(int id) =>
        await _ctx.Financials.FindAsync(id);

    public async Task<IEnumerable<Financial>> GetFinancialsByCompanyAsync(int companyId, int? year)
    {
        var query = _ctx.Financials.Where(f => f.CompanyId == companyId);
        if (year.HasValue) query = query.Where(f => f.Year == year.Value);
        return await query.OrderByDescending(f => f.Year).ThenByDescending(f => f.Quarter).ThenByDescending(f => f.Month).ToListAsync();
    }

    public async Task<Financial> AddFinancialAsync(Financial financial)
    {
        _ctx.Financials.Add(financial); await _ctx.SaveChangesAsync(); return financial;
    }

    public async Task UpdateFinancialAsync(Financial financial)
    {
        _ctx.Financials.Update(financial); await _ctx.SaveChangesAsync();
    }
}
