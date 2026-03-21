using LookupService.Application.DTOs;
using MediatR;

namespace LookupService.Application.Queries;

// LOV Type Master
public record GetAllLovTypesQuery : IRequest<IEnumerable<LovTypeMasterDto>>;
public record GetLovTypeByCodeQuery(string TypeCode) : IRequest<LovTypeMasterDto?>;

// LOV Master
public record GetAllLovsQuery : IRequest<IEnumerable<LovMasterDto>>;
public record GetLovByIdQuery(long LovId) : IRequest<LovMasterDto?>;
public record GetLovsByTypeQuery(string LovType) : IRequest<IEnumerable<LovMasterDto>>;

// Process Master
public record GetAllProcessesQuery : IRequest<IEnumerable<ProcessMasterDto>>;
public record GetProcessByIdQuery(decimal ProcessId) : IRequest<ProcessMasterDto?>;

// Panel Master
public record GetAllPanelsQuery : IRequest<IEnumerable<PanelMasterDto>>;
public record GetPanelByIdQuery(decimal PanelId) : IRequest<PanelMasterDto?>;

// Unit Process Map
public record GetUnitProcessesByUnitCodeQuery(string UnitCode) : IRequest<IEnumerable<UnitProcessMapDto>>;

// LOV Unit Map
public record GetLovUnitMapsByLovIdQuery(long LovId) : IRequest<IEnumerable<LovUnitMapDto>>;

// Access Master
public record GetAllAccessMastersQuery : IRequest<IEnumerable<UnitLovAccessMasterDto>>;
public record GetAccessMasterByIdQuery(decimal AccessMastId) : IRequest<UnitLovAccessMasterDto?>;
