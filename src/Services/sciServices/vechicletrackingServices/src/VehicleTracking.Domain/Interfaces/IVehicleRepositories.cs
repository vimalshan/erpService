using VehicleTracking.Domain.Entities;

namespace VehicleTracking.Domain.Interfaces;

public interface IVehicleMasterRepository : IRepository<VehicleMaster>
{
    Task<VehicleMaster?> GetByRegistrationAsync(string regNum1, string regNum4, CancellationToken ct = default);
}

public interface IVehicleStageRepository : IRepository<VehicleStage>
{
    Task<IEnumerable<VehicleStage>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default);
}

public interface IVehicleTransactionRepository : IRepository<VehicleTransaction>
{
    Task<IEnumerable<VehicleTransaction>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default);
    Task<IEnumerable<VehicleTransaction>> GetActiveTransactionsAsync(CancellationToken ct = default);
}

public interface IVehicleInvoiceRepository : IRepository<VehicleInvoice>
{
    Task<IEnumerable<VehicleInvoice>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default);
}

public interface IStageMasterRepository : IRepository<StageMaster>
{
}

public interface IPurposeMasterRepository : IRepository<PurposeMaster>
{
    Task<PurposeMaster?> GetWithStagesAsync(long purposeCode, CancellationToken ct = default);
}

public interface IDecisionFlagRepository : IRepository<DecisionFlag>
{
    Task<IEnumerable<DecisionFlag>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default);
}

public interface IWeightInfoRepository : IRepository<WeightInformation>
{
    Task<WeightInformation?> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default);
}
