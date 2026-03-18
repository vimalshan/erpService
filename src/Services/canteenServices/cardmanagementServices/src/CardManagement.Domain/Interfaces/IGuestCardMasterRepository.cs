using CardManagement.Domain.Entities;

namespace CardManagement.Domain.Interfaces;

public interface IGuestCardMasterRepository
{
    Task<GuestCardMaster?> GetByIdAsync(long canteenUnit, CancellationToken ct = default);
    Task<GuestCardMaster?> GetByCardNumberAsync(string cardNumber, CancellationToken ct = default);
    Task<IEnumerable<GuestCardMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<GuestCardMaster>> GetByCanteenUnitAsync(long canteenUnit, CancellationToken ct = default);
    Task AddAsync(GuestCardMaster entity, CancellationToken ct = default);
    void Update(GuestCardMaster entity);
    void Remove(GuestCardMaster entity);
}
