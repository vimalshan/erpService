namespace TaskTransactional.Application.DTOs;

public record ComplaintMainDto(
    string CmUnitCode, string CmGroupId, string CmGroupName, string? CmGroupDesc,
    decimal CmGroupSrc, string? CmBehalfFlg, decimal? CmBehalfPin, decimal? CmRegPin,
    string? CmShift, string? CmMail, string? CmSubmit, DateTime? CmRegDate,
    string? CmUpdatedBy, DateTime? CmUpdatedOn);

public record ComplaintDetailDto(
    decimal CdTicketNum, decimal CdGroupId, decimal CdType, decimal CdLocation,
    decimal CdDepartment, decimal CdProcess, string? CdSubject, string? CdDescription,
    string? CdNcr, string? CdPicturePath, string? CdFilePath,
    string CdTargetDate, DateTime? CdClosureDate);

public record ComplaintTaskDto(
    decimal CtTaskNum, decimal CtTicketNum, string CtScheduleFreq,
    string? CtScheduleValue, string? CtScheduleTime, string? CtScheduleDay,
    DateTime? CtEffDate, DateTime? CtClsDate, decimal? CtUpdatedBy, DateTime? CtUpdatedOn);

public record ComplaintActionDto(
    decimal CaActionNum, decimal CaTaskNum,
    string? CaPrmResp, decimal? CaPrmActBy, DateTime? CaPrmActDate, string? CaPrmSolution,
    decimal? CaSecEscHrs, string? CaSecResp, decimal? CaSecActBy, DateTime? CaSecActDate, string? CaSecSolution,
    string? CaFwdRemarks, string? CaFwdResp, decimal? CaFwdActBy, DateTime? CaFwdActDate, string? CaFwdSolution,
    decimal? CaCurEscLevel, string? CaCorrActReq, string? CaCorrRemarks, string? CaCorrResp,
    decimal? CaCorrActBy, DateTime? CaCorrActDate, string? CaCorrSolution,
    string? CaReopenFlg, string? CaReopenRemarks, DateTime? CaTrgDat, DateTime? CaClsDat, decimal? CaUpdatedBy);

public record ComplaintHistoryDto(
    decimal ChHistoryNum, decimal ChActionNum, decimal ChSerialNum,
    string? ChFrom, string? ChTo, DateTime ChActionDate, string ChActionType,
    string? ChRemarks, decimal? ChUpdatedBy, DateTime? ChUpdatedOn, string? ChFilePath);

public record ComplaintEscalationDto(
    decimal CeTicketNum, decimal CeLevelNum, decimal CeEscNoHrs, decimal CeUserPin,
    DateTime CeEffDate, DateTime? CeClsDate, string? CeExclude,
    decimal? CeUpdatedBy, DateTime? CeUpdatedOn);
