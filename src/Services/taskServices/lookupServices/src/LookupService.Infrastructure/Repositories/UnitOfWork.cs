using LookupService.Domain.Interfaces;
using LookupService.Infrastructure.Persistence;

namespace LookupService.Infrastructure.Repositories;

public class UnitOfWork(LookupDbContext db) : IUnitOfWork
{
    private ILovMasterRepository? _lovMasters;
    private ILovTypeMasterRepository? _lovTypeMasters;
    private IProcessMasterRepository? _processMasters;
    private IPanelMasterRepository? _panelMasters;
    private IUnitProcessMapRepository? _unitProcessMaps;
    private ILovUnitMapRepository? _lovUnitMaps;
    private IUnitLovAccessMasterRepository? _unitLovAccessMasters;

    public ILovMasterRepository LovMasters => _lovMasters ??= new LovMasterRepository(db);
    public ILovTypeMasterRepository LovTypeMasters => _lovTypeMasters ??= new LovTypeMasterRepository(db);
    public IProcessMasterRepository ProcessMasters => _processMasters ??= new ProcessMasterRepository(db);
    public IPanelMasterRepository PanelMasters => _panelMasters ??= new PanelMasterRepository(db);
    public IUnitProcessMapRepository UnitProcessMaps => _unitProcessMaps ??= new UnitProcessMapRepository(db);
    public ILovUnitMapRepository LovUnitMaps => _lovUnitMaps ??= new LovUnitMapRepository(db);
    public IUnitLovAccessMasterRepository UnitLovAccessMasters => _unitLovAccessMasters ??= new UnitLovAccessMasterRepository(db);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await db.SaveChangesAsync(ct);
    public void Dispose() => db.Dispose();
}
