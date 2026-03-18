namespace LovService.Application.DTOs;

public record LovMasterDto(long LovId, long LovTypeId, string LovName, long LovUpdatedBy, DateTime LovUpdatedOn);
public record CreateLovMasterRequest(long LovId, long LovTypeId, string LovName, long UpdatedBy);
public record UpdateLovMasterRequest(string LovName, long UpdatedBy);
