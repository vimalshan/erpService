using Microsoft.EntityFrameworkCore;
using CardManagement.Domain.Entities;
using CardManagement.Domain.Interfaces;

namespace CardManagement.Infrastructure.Persistence.Repositories;

public class GuestCardMasterRepository : IGuestCardMasterRepository
{
    private readonly ApplicationDbContext _context;

    public GuestCardMasterRepository(ApplicationDbContext context) => _context = context;

    public async Task<GuestCardMaster?> GetByIdAsync(long canteenUnit, CancellationToken ct = default)
        => await _context.GuestCardMasters.FirstOrDefaultAsync(x => x.CanteenUnit == canteenUnit, ct);

    public async Task<GuestCardMaster?> GetByCardNumberAsync(string cardNumber, CancellationToken ct = default)
        => await _context.GuestCardMasters.FirstOrDefaultAsync(x => x.CardNumber == cardNumber, ct);

    public async Task<IEnumerable<GuestCardMaster>> GetAllAsync(CancellationToken ct = default)
        => await _context.GuestCardMasters.ToListAsync(ct);

    public async Task<IEnumerable<GuestCardMaster>> GetByCanteenUnitAsync(long canteenUnit, CancellationToken ct = default)
        => await _context.GuestCardMasters.Where(x => x.CanteenUnit == canteenUnit).ToListAsync(ct);

    public async Task AddAsync(GuestCardMaster entity, CancellationToken ct = default)
        => await _context.GuestCardMasters.AddAsync(entity, ct);

    public void Update(GuestCardMaster entity)
        => _context.GuestCardMasters.Update(entity);

    public void Remove(GuestCardMaster entity)
        => _context.GuestCardMasters.Remove(entity);
}
