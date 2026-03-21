using ExpenseService.Domain.Entities;
using ExpenseService.Domain.Interfaces;
using ExpenseService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Infrastructure.Repositories;

public class ConveyanceRepository : IConveyanceRepository
{
    private readonly ExpenseDbContext _context;

    public ConveyanceRepository(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<TravelConveyance?> GetByIdAsync(long serialNumber, long requestNumber, CancellationToken ct = default)
    {
        return await _context.TravelConveyances
            .FirstOrDefaultAsync(c => c.SerialNumber == serialNumber && c.RequestNumber == requestNumber, ct);
    }

    public async Task<IReadOnlyList<TravelConveyance>> GetByRequestNumberAsync(long requestNumber, CancellationToken ct = default)
    {
        return await _context.TravelConveyances
            .Where(c => c.RequestNumber == requestNumber)
            .ToListAsync(ct);
    }

    public async Task<TravelConveyance> AddAsync(TravelConveyance conveyance, CancellationToken ct = default)
    {
        _context.TravelConveyances.Add(conveyance);
        await _context.SaveChangesAsync(ct);
        return conveyance;
    }

    public async Task UpdateAsync(TravelConveyance conveyance, CancellationToken ct = default)
    {
        _context.TravelConveyances.Update(conveyance);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long serialNumber, long requestNumber, CancellationToken ct = default)
    {
        var entity = await GetByIdAsync(serialNumber, requestNumber, ct);
        if (entity != null)
        {
            _context.TravelConveyances.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }
}
