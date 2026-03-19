namespace MasterDataService.Application.DTOs;

public record LovMasterDto(long LovId, string LovType, string LovName);
public record CreateLovMasterDto(long LovId, string LovType, string LovName);
public record UpdateLovMasterDto(string LovType, string LovName);

public record LovTypeMasterDto(string TypeCode, string TypeName);
public record CreateLovTypeMasterDto(string TypeCode, string TypeName);
public record UpdateLovTypeMasterDto(string TypeName);

public record HoldTypeMasterDto(long HoldId, string? HoldName, string? HoldCategory);
public record CreateHoldTypeMasterDto(long HoldId, string? HoldName, string? HoldCategory);
public record UpdateHoldTypeMasterDto(string? HoldName, string? HoldCategory);

public record LocationScanParamDto(long ParamId, long LocationId, DateTime EffectiveDate, DateTime? ClosingDate);
public record CreateLocationScanParamDto(long ParamId, long LocationId, DateTime EffectiveDate, DateTime? ClosingDate);
public record UpdateLocationScanParamDto(DateTime EffectiveDate, DateTime? ClosingDate);

public record ScannerMasterDto(long DeviceId, string? DeviceName, long DeviceLocationId, string? DevicePath);
public record CreateScannerMasterDto(long DeviceId, string? DeviceName, long DeviceLocationId, string? DevicePath);
public record UpdateScannerMasterDto(string? DeviceName, long DeviceLocationId, string? DevicePath);
