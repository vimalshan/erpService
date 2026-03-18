using CardManagement.Domain.Entities;

namespace CardManagement.Domain.Interfaces;

public interface ICardSettlementRepository
{
    Task<CardSettlement?> GetByIdAsync(decimal sysId, CancellationToken ct = default);
    Task<IEnumerable<CardSettlement>> GetByCardNumberAsync(string cardNumber, CancellationToken ct = default);
    Task<IEnumerable<CardSettlement>> GetByCanteenUnitAsync(long canteenUnit, CancellationToken ct = default);
    Task AddAsync(CardSettlement entity, CancellationToken ct = default);
}
