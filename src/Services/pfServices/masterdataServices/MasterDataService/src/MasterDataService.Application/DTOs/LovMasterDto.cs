namespace MasterDataService.Application.DTOs;

public record LovMasterDto(
    decimal LovId,
    string LovCode,
    string LovDescription,
    string LovValue,
    string LovCategory,
    string LovStatus);

public record CreateLovMasterDto(
    string LovCode,
    string LovDescription,
    string LovValue,
    string LovCategory);

public record UpdateLovMasterDto(
    decimal LovId,
    string LovCode,
    string LovDescription,
    string LovValue,
    string LovCategory,
    string LovStatus);
