namespace NotificationService.Application.DTOs;

public record NotificationDto(
    int NotificationId, string Title, string Message, int CategoryId, int? CompanyId,
    int? SiteId, int? ServiceId, string Priority, string Status, DateTime CreatedDate,
    DateTime ModifiedDate, int? CreatedBy, int? ModifiedBy, DateTime? ExpiryDate, bool IsActive,
    string? TargetAudience, bool ActionRequired, string? ActionUrl, string? AttachmentPath,
    string? RelatedEntityType, int? RelatedEntityId);

public record CreateNotificationDto(
    string Title, string Message, int CategoryId, int? CompanyId, int? SiteId, int? ServiceId,
    string Priority, string? TargetAudience, DateTime? ExpiryDate, bool ActionRequired,
    string? ActionUrl, string? AttachmentPath, string? RelatedEntityType, int? RelatedEntityId, int? CreatedBy);

public record UpdateNotificationDto(
    int NotificationId, string Title, string Message, int CategoryId, int? CompanyId,
    int? SiteId, int? ServiceId, string Priority, string Status, DateTime? ExpiryDate,
    bool IsActive, string? TargetAudience, bool ActionRequired, string? ActionUrl,
    string? AttachmentPath, string? RelatedEntityType, int? RelatedEntityId, int? ModifiedBy);

public record NotificationCategoryDto(
    int CategoryId, string CategoryName, string CategoryCode, string? Description, bool IsActive,
    string? Color, string? Icon, int? Priority, int? DisplayOrder);

public record CreateNotificationCategoryDto(
    string CategoryName, string CategoryCode, string? Description, string? Color, string? Icon,
    int? Priority, int? DisplayOrder, int? CreatedBy);
