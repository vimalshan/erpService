namespace UnitService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEquipmentRepository Equipment { get; }
    ICategoryRepository Categories { get; }
    IEquipmentStatusRepository EquipmentStatuses { get; }
    IAccessRepository Access { get; }
    IBudgetRepository Budgets { get; }
    IMailIdRepository MailIds { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
