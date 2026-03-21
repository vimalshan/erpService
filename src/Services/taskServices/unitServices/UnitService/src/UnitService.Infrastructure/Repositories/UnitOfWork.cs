using UnitService.Domain.Interfaces;
using UnitService.Infrastructure.Data;

namespace UnitService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly UnitDbContext _context;

    public UnitOfWork(UnitDbContext context,
        IEquipmentRepository equipment,
        ICategoryRepository categories,
        IEquipmentStatusRepository equipmentStatuses,
        IAccessRepository access,
        IBudgetRepository budgets,
        IMailIdRepository mailIds)
    {
        _context = context;
        Equipment = equipment;
        Categories = categories;
        EquipmentStatuses = equipmentStatuses;
        Access = access;
        Budgets = budgets;
        MailIds = mailIds;
    }

    public IEquipmentRepository Equipment { get; }
    public ICategoryRepository Categories { get; }
    public IEquipmentStatusRepository EquipmentStatuses { get; }
    public IAccessRepository Access { get; }
    public IBudgetRepository Budgets { get; }
    public IMailIdRepository MailIds { get; }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
