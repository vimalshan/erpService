namespace VehicleTracking.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IVehicleMasterRepository VehicleMasters { get; }
    IVehicleStageRepository VehicleStages { get; }
    IVehicleTransactionRepository VehicleTransactions { get; }
    IVehicleInvoiceRepository VehicleInvoices { get; }
    IStageMasterRepository StageMasters { get; }
    IPurposeMasterRepository PurposeMasters { get; }
    IDecisionFlagRepository DecisionFlags { get; }
    IWeightInfoRepository WeightInfos { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
