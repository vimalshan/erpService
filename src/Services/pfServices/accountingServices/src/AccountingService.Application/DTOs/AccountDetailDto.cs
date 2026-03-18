namespace AccountingService.Application.DTOs;

public record AccountDetailDto(
    long AcSysId,
    string AcTrustCode,
    string AcTranCode,
    long AcTranNo,
    long AcDocNo,
    long AcFinYer,
    DateTime AcDocDat,
    string AcMainCode,
    string AcSubCode,
    string AcDcType,
    decimal AcTranAmt,
    string AcRefTranCode,
    long AcRefTranNo,
    string? AcRemarks
);
