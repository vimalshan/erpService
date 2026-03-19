using MediatR;
using MasterDataService.Application.DTOs;

namespace MasterDataService.Application.Commands;

// LOV Master
public record CreateLovMasterCommand(long LovId, string LovType, string LovName) : IRequest<LovMasterDto>;
public record UpdateLovMasterCommand(long LovId, string LovType, string LovName) : IRequest<LovMasterDto>;
public record DeleteLovMasterCommand(long LovId) : IRequest<bool>;

// LOV Type Master
public record CreateLovTypeMasterCommand(string TypeCode, string TypeName) : IRequest<LovTypeMasterDto>;
public record UpdateLovTypeMasterCommand(string TypeCode, string TypeName) : IRequest<LovTypeMasterDto>;
public record DeleteLovTypeMasterCommand(string TypeCode) : IRequest<bool>;

// Hold Type Master
public record CreateHoldTypeMasterCommand(long HoldId, string? HoldName, string? HoldCategory) : IRequest<HoldTypeMasterDto>;
public record UpdateHoldTypeMasterCommand(long HoldId, string? HoldName, string? HoldCategory) : IRequest<HoldTypeMasterDto>;
public record DeleteHoldTypeMasterCommand(long HoldId) : IRequest<bool>;

// Location Scan Params
public record CreateLocationScanParamCommand(long ParamId, long LocationId, DateTime EffectiveDate, DateTime? ClosingDate) : IRequest<LocationScanParamDto>;
public record UpdateLocationScanParamCommand(long ParamId, DateTime EffectiveDate, DateTime? ClosingDate) : IRequest<LocationScanParamDto>;
public record DeleteLocationScanParamCommand(long ParamId) : IRequest<bool>;

// Scanner Master
public record CreateScannerMasterCommand(long DeviceId, string? DeviceName, long DeviceLocationId, string? DevicePath) : IRequest<ScannerMasterDto>;
public record UpdateScannerMasterCommand(long DeviceId, string? DeviceName, long DeviceLocationId, string? DevicePath) : IRequest<ScannerMasterDto>;
public record DeleteScannerMasterCommand(long DeviceId) : IRequest<bool>;
