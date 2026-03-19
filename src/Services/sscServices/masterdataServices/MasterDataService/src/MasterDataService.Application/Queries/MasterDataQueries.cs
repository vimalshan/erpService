using MediatR;
using MasterDataService.Application.DTOs;

namespace MasterDataService.Application.Queries;

// LOV Master
public record GetAllLovMastersQuery : IRequest<IReadOnlyList<LovMasterDto>>;
public record GetLovMasterByIdQuery(long LovId) : IRequest<LovMasterDto?>;
public record GetLovMastersByTypeQuery(string LovType) : IRequest<IReadOnlyList<LovMasterDto>>;

// LOV Type Master
public record GetAllLovTypeMastersQuery : IRequest<IReadOnlyList<LovTypeMasterDto>>;
public record GetLovTypeMasterByIdQuery(string TypeCode) : IRequest<LovTypeMasterDto?>;

// Hold Type Master
public record GetAllHoldTypeMastersQuery : IRequest<IReadOnlyList<HoldTypeMasterDto>>;
public record GetHoldTypeMasterByIdQuery(long HoldId) : IRequest<HoldTypeMasterDto?>;

// Location Scan Params
public record GetAllLocationScanParamsQuery : IRequest<IReadOnlyList<LocationScanParamDto>>;
public record GetLocationScanParamByIdQuery(long ParamId) : IRequest<LocationScanParamDto?>;

// Scanner Master
public record GetAllScannerMastersQuery : IRequest<IReadOnlyList<ScannerMasterDto>>;
public record GetScannerMasterByIdQuery(long DeviceId) : IRequest<ScannerMasterDto?>;
