namespace Masters.Application.DTOs;

public record LovMasterDto(
    long LovId,
    string LovType,
    string LovName
);

public record CreateLovMasterDto(
    long LovId,
    string LovType,
    string LovName
);

public record UpdateLovMasterDto(
    string LovName
);
