using VehicleTracking.Domain.Interfaces;
using VehicleTracking.Infrastructure.Persistence;
using VehicleTracking.Infrastructure.Repositories;

namespace VehicleTracking.Infrastructure;

public class UnitOfWork(VehicleTrackingDbContext context) : IUnitOfWork
{
    private IVehicleMasterRepository? _vehicleMasters;
    private IVehicleStageRepository? _vehicleStages;
    private IVehicleTransactionRepository? _vehicleTransactions;
    private IVehicleInvoiceRepository? _vehicleInvoices;
    private IStageMasterRepository? _stageMasters;
    private IPurposeMasterRepository? _purposeMasters;
    private IDecisionFlagRepository? _decisionFlags;
    private IWeightInfoRepository? _weightInfos;

    public IVehicleMasterRepository VehicleMasters => _vehicleMasters ??= new VehicleMasterRepository(context);
    public IVehicleStageRepository VehicleStages => _vehicleStages ??= new VehicleStageRepository(context);
    public IVehicleTransactionRepository VehicleTransactions => _vehicleTransactions ??= new VehicleTransactionRepository(context);
    public IVehicleInvoiceRepository VehicleInvoices => _vehicleInvoices ??= new VehicleInvoiceRepository(context);
    public IStageMasterRepository StageMasters => _stageMasters ??= new StageMasterRepository(context);
    public IPurposeMasterRepository PurposeMasters => _purposeMasters ??= new PurposeMasterRepository(context);
    public IDecisionFlagRepository DecisionFlags => _decisionFlags ??= new DecisionFlagRepository(context);
    public IWeightInfoRepository WeightInfos => _weightInfos ??= new WeightInfoRepository(context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}
