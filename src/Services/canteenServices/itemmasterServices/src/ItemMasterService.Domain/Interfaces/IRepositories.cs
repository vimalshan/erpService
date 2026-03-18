using ItemMasterService.Domain.Entities;

namespace ItemMasterService.Domain.Interfaces;

public interface ICanteenItemRepository
{
    Task<CanteenItemMaster?> GetByIdAsync(long canteenUnitCode, long itemCode, CancellationToken ct = default);
    Task<IEnumerable<CanteenItemMaster>> GetAllAsync(long canteenUnitCode, CancellationToken ct = default);
    Task<bool> ExistsAsync(long canteenUnitCode, long itemCode, CancellationToken ct = default);
    Task AddAsync(CanteenItemMaster entity, CancellationToken ct = default);
    void Update(CanteenItemMaster entity);
    void Delete(CanteenItemMaster entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface ICanteenItemPriceRepository
{
    Task<CanteenItemPriceMaster?> GetActiveAsync(long canteenUnitCode, long itemCode, CancellationToken ct = default);
    Task<IEnumerable<CanteenItemPriceMaster>> GetHistoryAsync(long canteenUnitCode, long itemCode, CancellationToken ct = default);
    Task AddAsync(CanteenItemPriceMaster entity, CancellationToken ct = default);
    void Update(CanteenItemPriceMaster entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface ICanteenGradeItemPriceRepository
{
    Task<CanteenGradeItemPrice?> GetByUnitCodeAsync(long canteenUnitCode, CancellationToken ct = default);
    Task<IEnumerable<CanteenGradeItemPrice>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(CanteenGradeItemPrice entity, CancellationToken ct = default);
    void Update(CanteenGradeItemPrice entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
