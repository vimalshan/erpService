using HRDocumentService.Application.DTOs;
using MediatR;

namespace HRDocumentService.Application.Commands;

public sealed record CreateHRDocumentCommand(
    string DocType,
    long DocPayRefNo,
    long DocLocId,
    long DocUnitId,
    string DocRemarks,
    long DocUserId,
    string DocSource,
    string? DocRefNo = null,
    string? DocRefName = null
) : IRequest<HRDocumentDto>;
