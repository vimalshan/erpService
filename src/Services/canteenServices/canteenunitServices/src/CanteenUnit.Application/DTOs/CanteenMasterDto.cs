namespace CanteenUnit.Application.DTOs;

public record CanteenMasterDto(
    decimal CnComCod,
    long CnCanNum,
    DateTime? CnCanFro,
    DateTime? CnCanTo,
    char? CnLivFlg,
    string? CnRemMrk);
