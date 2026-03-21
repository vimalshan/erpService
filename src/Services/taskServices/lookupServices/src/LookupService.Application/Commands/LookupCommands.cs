using MediatR;

namespace LookupService.Application.Commands;

// LOV Type Master
public record CreateLovTypeCommand(string LovTypeCode, string? LovTypeName) : IRequest<string>;
public record UpdateLovTypeCommand(string LovTypeCode, string? LovTypeName) : IRequest<bool>;
public record DeleteLovTypeCommand(string LovTypeCode) : IRequest<bool>;

// LOV Master
public record CreateLovCommand(string LovType, string LovName) : IRequest<long>;
public record UpdateLovCommand(long LovId, string LovName) : IRequest<bool>;
public record DeleteLovCommand(long LovId) : IRequest<bool>;

// LOV Unit Map
public record MapLovToUnitCommand(long LovId, string UnitCode, string Flag = "Y") : IRequest<decimal>;

// Process Master
public record CreateProcessCommand(decimal ProcessId, string ProcessName, string LiveFlag = "Y") : IRequest<decimal>;
public record UpdateProcessCommand(decimal ProcessId, string ProcessName, string LiveFlag) : IRequest<bool>;
public record DeleteProcessCommand(decimal ProcessId) : IRequest<bool>;

// Unit Process Map
public record MapUnitProcessCommand(string UnitCode, decimal ProcessId) : IRequest<decimal>;

// Panel Master
public record CreatePanelCommand(decimal PanelId, string PanelName) : IRequest<decimal>;
public record UpdatePanelCommand(decimal PanelId, string PanelName) : IRequest<bool>;

// Access Master
public record CreateAccessMasterCommand(
    decimal UnitLovMapId, decimal DepartmentId, decimal ProcessId) : IRequest<decimal>;
