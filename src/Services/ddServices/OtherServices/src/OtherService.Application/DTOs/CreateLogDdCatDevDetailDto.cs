namespace OtherService.Application.DTOs;

public sealed record CreateLogDdCatDevDetailDto(
    decimal? ReqNum,
    decimal? QtnNum,
    decimal? AnsSrl,
    string AppId,
    decimal AppNum,
    DateTime? EntDat,
    string? Desc,
    string? Need
);
