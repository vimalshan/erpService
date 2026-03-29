namespace HealthTransaction.Application.DTOs;

public record DynamicHealthDetailDto(
    decimal HlthNum,
    string ChkupCod,
    string ComCode,
    decimal CtrlSrcId,
    string? DynVal,
    decimal EmpNum,
    DateTime? SysDate);

public record SaveDynamicHealthDetailDto(
    decimal HlthNum,
    string ChkupCod,
    string ComCode,
    decimal CtrlSrcId,
    string? DynVal,
    decimal EmpNum);
