namespace ComplaintService.Application.DTOs;

public record ComplaintGroupDto(
    string UnitCode,
    string GroupId,
    string GroupName,
    string? GroupDesc,
    decimal GroupSrc,
    decimal? RegPin,
    string? Shift,
    string? Mail,
    DateTime? RegDate
);

public record CreateComplaintGroupRequest(
    string UnitCode,
    string GroupId,
    string GroupName,
    decimal GroupSrc,
    decimal RegPin,
    string? GroupDesc = null,
    string? Shift = null,
    string? Mail = null
);
