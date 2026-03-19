using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Events;

public sealed record LovMasterCreatedEvent(long LovId, string LovType, string LovName) : IDomainEvent;
public sealed record LovMasterUpdatedEvent(long LovId, string LovType, string LovName) : IDomainEvent;
public sealed record LovMasterDeletedEvent(long LovId) : IDomainEvent;

public sealed record LovTypeMasterCreatedEvent(string TypeCode, string TypeName) : IDomainEvent;
public sealed record LovTypeMasterUpdatedEvent(string TypeCode, string TypeName) : IDomainEvent;

public sealed record HoldTypeMasterCreatedEvent(long HoldId, string? HoldName) : IDomainEvent;
public sealed record HoldTypeMasterUpdatedEvent(long HoldId, string? HoldName) : IDomainEvent;

public sealed record LocationScanParamCreatedEvent(long ParamId, long LocationId) : IDomainEvent;
public sealed record ScannerMasterCreatedEvent(long DeviceId, string? DeviceName) : IDomainEvent;
public sealed record ScannerMasterUpdatedEvent(long DeviceId, string? DeviceName) : IDomainEvent;
