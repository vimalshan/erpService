namespace LocationServices.Application.DTOs;

public record LocationAppMapDto(
    decimal  LocationId,
    string   AppName,
    long?    SiteCategoryCode,
    string?  SelfAccess,
    string?  DeemedApproval,
    bool     IsActive,
    DateTime CreatedDate,
    string?  CreatedBy,
    DateTime? ModifiedDate,
    string?  ModifiedBy);

public record CreateLocationAppMapRequest(
    decimal  LocationId,
    string   AppName,
    long?    SiteCategoryCode,
    string?  SelfAccess,
    string?  DeemedApproval);

public record UpdateLocationAppMapRequest(
    long?   SiteCategoryCode,
    string? SelfAccess,
    string? DeemedApproval,
    bool    IsActive);

public record LocationAppMapSummary(
    decimal LocationId,
    string  AppName,
    bool    IsActive,
    long?   SiteCategoryCode);
