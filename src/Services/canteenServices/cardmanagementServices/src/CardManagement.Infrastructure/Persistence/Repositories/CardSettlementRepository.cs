using Microsoft.EntityFrameworkCore;
using CardManagement.Domain.Entities;
using CardManagement.Domain.Interfaces;

namespace CardManagement.Infrastructure.Persistence.Repositories;

public class CardSettlementRepository : ICardSettlementRepository
{
    private readonly ApplicationDbContext _context;

    public CardSettlementRepository(ApplicationDbContext context) => _context = context;

    public async Task<CardSettlement?> GetByIdAsync(decimal sysId, CancellationToken ct = default)
        => await _context.CardSettlements.FirstOrDefaultAsync(x => x.SysId == sysId, ct);

    public async Task<IEnumerable<CardSettlement>> GetByCardNumberAsync(string cardNumber, CancellationToken ct = default)
        => await _context.CardSettlements.Where(x => x.CardNumber == cardNumber).ToListAsync(ct);

    public async Task<IEnumerable<CardSettlement>> GetByCanteenUnitAsync(long canteenUnit, CancellationToken ct = default)
        => await _context.CardSettlements.Where(x => x.CanteenUnit == canteenUnit).ToListAsync(ct);

    public async Task AddAsync(CardSettlement entity, CancellationToken ct = default)
        => await _context.CardSettlements.AddAsync(entity, ct);
}
