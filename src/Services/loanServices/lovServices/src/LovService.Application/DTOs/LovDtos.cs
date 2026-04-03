namespace LovService.Application.DTOs;

public record LovTypeMastDto(
    int LovTypeId,
    string LovTypeName,
    string LovCategory,
    int LovOrgId);

public record LovMasterDto(
    long LovId,
    int LovTypeId,
    string LovName,
    DateTime LovCreatedOn,
    long LovCreatedBy,
    long LovUpdatedBy,
    DateTime LovUpdatedOn,
    string? LovTypeName = null);

public record ProgramLovMastDto(
    string PrlovTypeCode,
    string PrlovCode,
    string PrlovName);
