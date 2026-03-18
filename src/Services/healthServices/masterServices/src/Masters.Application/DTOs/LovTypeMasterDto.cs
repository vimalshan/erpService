namespace Masters.Application.DTOs;

public record LovTypeMasterDto(
    string LovTypeCode,
    string LovTypeName
);

public record CreateLovTypeMasterDto(
    string LovTypeCode,
    string LovTypeName
);

public record UpdateLovTypeMasterDto(
    string LovTypeName
);
