namespace OtherService.Application.DTOs;

public sealed record UpdateLogDdCatDevDetailDto(
    decimal? ReqNum,
    decimal? QtnNum,
    decimal? AnsSrl,
    DateTime? EntDat,
    string? Desc,
    string? Need
);
