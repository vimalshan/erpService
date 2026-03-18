using MedicineManagement.Domain.Entities;
using MedicineManagement.Domain.Interfaces;
using MedicineManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicineManagement.Infrastructure.Repositories;

public class MedicineCreditRepository(MedicineManagementDbContext context) : IMedicineCreditRepository
{
    public async Task<MedicineCredit?> GetByIdAsync(string companyCode, CancellationToken ct = default)
        => await context.MedicineCredits.FirstOrDefaultAsync(c => c.CompanyCode == companyCode, ct);

    public async Task<IReadOnlyList<MedicineCredit>> GetByMedicineCodeAsync(string medicineCode, CancellationToken ct = default)
        => await context.MedicineCredits.Where(c => c.MedicineCode == medicineCode).ToListAsync(ct);

    public async Task<IReadOnlyList<MedicineCredit>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default)
        => await context.MedicineCredits.Where(c => c.TransactionDate >= from && c.TransactionDate <= to).ToListAsync(ct);

    public async Task<long> GetCurrentStockAsync(string medicineCode, CancellationToken ct = default)
    {
        var credits = await context.MedicineCredits
            .Where(c => c.MedicineCode == medicineCode && c.CancelFlag != 'Y')
            .ToListAsync(ct);

        long stock = 0;
        foreach (var c in credits)
        {
            stock += c.RecordType switch
            {
                'O' or 'P' => c.Quantity,
                'I' or 'E' => -c.Quantity,
                _ => 0
            };
        }
        return stock;
    }

    public async Task AddAsync(MedicineCredit entity, CancellationToken ct = default)
        => await context.MedicineCredits.AddAsync(entity, ct);
}
