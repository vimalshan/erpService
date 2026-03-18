namespace LovService.Application.DTOs;

public record LovTypeDto(long LovTypeId, string LovTypeName);
public record CreateLovTypeRequest(long LovTypeId, string LovTypeName);
public record UpdateLovTypeRequest(string LovTypeName);
