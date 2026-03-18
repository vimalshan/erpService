using AlertsNotifications.Application.DTOs;
using MediatR;

namespace AlertsNotifications.Application.Features.Circulars.Commands;

public record CreateCircularCommand(
    long CircularId,
    string? CircularNo,
    int CircularYearId,
    long CircularType,
    long CircularOrgId,
    int CircularBuSpecific,
    int CircularUnitSpecific,
    int? CircularHrRoleId,
    int CircularVersionNo,
    long? CircularTemplateId,
    string? CircularPdfFileName,
    string? CircularRtf,
    long CircularSignatoryId,
    char CircularSparshFlag,
    DateTime? CircularPostDate,
    DateTime? CircularRemoveDate,
    string CircularDesc,
    string CircularSubject,
    string CircularToList,
    string? CircularCcList,
    char CircularStatus,
    char? CircularAttachEmpFlag,
    long CreatedBy
) : IRequest<CircularDto>;

public record UpdateCircularCommand(
    long CircularId,
    string? CircularNo,
    int CircularYearId,
    long CircularType,
    long CircularOrgId,
    int CircularBuSpecific,
    int CircularUnitSpecific,
    int? CircularHrRoleId,
    int CircularVersionNo,
    long? CircularTemplateId,
    string? CircularPdfFileName,
    string? CircularRtf,
    long CircularSignatoryId,
    char CircularSparshFlag,
    DateTime? CircularPostDate,
    DateTime? CircularRemoveDate,
    string CircularDesc,
    string CircularSubject,
    string CircularToList,
    string? CircularCcList,
    char CircularStatus,
    char? CircularAttachEmpFlag,
    long ModifiedBy
) : IRequest<Unit>;

public record ApproveCircularCommand(
    long CircularId,
    long ApprovedBy,
    string? Remarks
) : IRequest<Unit>;

public record DeleteCircularCommand(long CircularId) : IRequest<Unit>;
