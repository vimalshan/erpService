using FindingsAPI.Gateway.Application.DTOs;
using FindingsAPI.Gateway.Domain.Interfaces;
using MediatR;

namespace FindingsAPI.Gateway.Application.Queries;

public record GetFindingDomainByIdQuery(int FindingId) : IRequest<FindingDomainDto?>;
public record GetAllFindingsDomainQuery : IRequest<IEnumerable<FindingDomainDto>>;
public record GetFindingsByAuditQuery(int AuditId) : IRequest<IEnumerable<FindingDomainDto>>;
public record GetFindingsBySiteQuery(int SiteId) : IRequest<IEnumerable<FindingDomainDto>>;
public record GetFindingStatusesQuery : IRequest<IEnumerable<FindingStatusDto>>;
public record GetFindingCategoriesQuery : IRequest<IEnumerable<FindingCategoryDto>>;
public record GetFindingResponsesQuery(int FindingId) : IRequest<IEnumerable<FindingResponseDto>>;

public class GetFindingDomainByIdHandler : IRequestHandler<GetFindingDomainByIdQuery, FindingDomainDto?>
{
    private readonly IFindingsDomainRepository _repo;
    public GetFindingDomainByIdHandler(IFindingsDomainRepository repo) => _repo = repo;

    public async Task<FindingDomainDto?> Handle(GetFindingDomainByIdQuery request, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(request.FindingId);
        if (e == null) return null;
        return new FindingDomainDto
        {
            FindingId = e.FindingId, FindingNumber = e.FindingNumber, AuditId = e.AuditId,
            SiteId = e.SiteId, Title = e.Title, Description = e.Description,
            FindingType = e.FindingType, Severity = e.Severity, FindingStatusId = e.FindingStatusId,
            StatusName = e.FindingStatus?.StatusName, FindingCategoryId = e.FindingCategoryId,
            CategoryName = e.FindingCategory?.CategoryName, IdentifiedDate = e.IdentifiedDate,
            DueDate = e.DueDate, ClosedDate = e.ClosedDate, IsActive = e.IsActive,
            IdentifiedBy = e.IdentifiedBy, AssignedTo = e.AssignedTo, Evidence = e.Evidence,
            RootCause = e.RootCause, CorrectiveAction = e.CorrectiveAction,
            PreventiveAction = e.PreventiveAction, CompletionDate = e.CompletionDate,
            VerificationDate = e.VerificationDate, VerifiedBy = e.VerifiedBy
        };
    }
}

public class GetAllFindingsDomainHandler : IRequestHandler<GetAllFindingsDomainQuery, IEnumerable<FindingDomainDto>>
{
    private readonly IFindingsDomainRepository _repo;
    public GetAllFindingsDomainHandler(IFindingsDomainRepository repo) => _repo = repo;

    public async Task<IEnumerable<FindingDomainDto>> Handle(GetAllFindingsDomainQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetAllAsync();
        return entities.Select(e => new FindingDomainDto
        {
            FindingId = e.FindingId, FindingNumber = e.FindingNumber, AuditId = e.AuditId,
            SiteId = e.SiteId, Title = e.Title, Description = e.Description,
            FindingType = e.FindingType, Severity = e.Severity, FindingStatusId = e.FindingStatusId,
            StatusName = e.FindingStatus?.StatusName, FindingCategoryId = e.FindingCategoryId,
            CategoryName = e.FindingCategory?.CategoryName, IdentifiedDate = e.IdentifiedDate,
            DueDate = e.DueDate, ClosedDate = e.ClosedDate, IsActive = e.IsActive,
            IdentifiedBy = e.IdentifiedBy, AssignedTo = e.AssignedTo
        });
    }
}

public class GetFindingsByAuditHandler : IRequestHandler<GetFindingsByAuditQuery, IEnumerable<FindingDomainDto>>
{
    private readonly IFindingsDomainRepository _repo;
    public GetFindingsByAuditHandler(IFindingsDomainRepository repo) => _repo = repo;

    public async Task<IEnumerable<FindingDomainDto>> Handle(GetFindingsByAuditQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetByAuditAsync(request.AuditId);
        return entities.Select(e => new FindingDomainDto
        {
            FindingId = e.FindingId, FindingNumber = e.FindingNumber, AuditId = e.AuditId,
            Title = e.Title, FindingType = e.FindingType, Severity = e.Severity,
            FindingStatusId = e.FindingStatusId, StatusName = e.FindingStatus?.StatusName,
            DueDate = e.DueDate, IsActive = e.IsActive, AssignedTo = e.AssignedTo
        });
    }
}

public class GetFindingsBySiteHandler : IRequestHandler<GetFindingsBySiteQuery, IEnumerable<FindingDomainDto>>
{
    private readonly IFindingsDomainRepository _repo;
    public GetFindingsBySiteHandler(IFindingsDomainRepository repo) => _repo = repo;

    public async Task<IEnumerable<FindingDomainDto>> Handle(GetFindingsBySiteQuery request, CancellationToken ct)
    {
        var entities = await _repo.GetBySiteAsync(request.SiteId);
        return entities.Select(e => new FindingDomainDto
        {
            FindingId = e.FindingId, FindingNumber = e.FindingNumber, AuditId = e.AuditId,
            Title = e.Title, FindingType = e.FindingType, Severity = e.Severity,
            FindingStatusId = e.FindingStatusId, DueDate = e.DueDate, IsActive = e.IsActive
        });
    }
}

public class GetFindingStatusesHandler : IRequestHandler<GetFindingStatusesQuery, IEnumerable<FindingStatusDto>>
{
    private readonly IFindingsDomainRepository _repo;
    public GetFindingStatusesHandler(IFindingsDomainRepository repo) => _repo = repo;

    public async Task<IEnumerable<FindingStatusDto>> Handle(GetFindingStatusesQuery request, CancellationToken ct)
    {
        var statuses = await _repo.GetStatusesAsync();
        return statuses.Select(s => new FindingStatusDto
        {
            FindingStatusId = s.FindingStatusId, StatusName = s.StatusName, StatusCode = s.StatusCode,
            Description = s.Description, Color = s.Color, DisplayOrder = s.DisplayOrder,
            IsClosedStatus = s.IsClosedStatus
        });
    }
}

public class GetFindingCategoriesHandler : IRequestHandler<GetFindingCategoriesQuery, IEnumerable<FindingCategoryDto>>
{
    private readonly IFindingsDomainRepository _repo;
    public GetFindingCategoriesHandler(IFindingsDomainRepository repo) => _repo = repo;

    public async Task<IEnumerable<FindingCategoryDto>> Handle(GetFindingCategoriesQuery request, CancellationToken ct)
    {
        var categories = await _repo.GetCategoriesAsync();
        return categories.Select(c => new FindingCategoryDto
        {
            FindingCategoryId = c.FindingCategoryId, CategoryName = c.CategoryName,
            CategoryCode = c.CategoryCode, Description = c.Description,
            ParentCategoryId = c.ParentCategoryId, Color = c.Color, DisplayOrder = c.DisplayOrder
        });
    }
}

public class GetFindingResponsesHandler : IRequestHandler<GetFindingResponsesQuery, IEnumerable<FindingResponseDto>>
{
    private readonly IFindingsDomainRepository _repo;
    public GetFindingResponsesHandler(IFindingsDomainRepository repo) => _repo = repo;

    public async Task<IEnumerable<FindingResponseDto>> Handle(GetFindingResponsesQuery request, CancellationToken ct)
    {
        var responses = await _repo.GetResponsesByFindingAsync(request.FindingId);
        return responses.Select(r => new FindingResponseDto
        {
            FindingResponseId = r.FindingResponseId, FindingId = r.FindingId,
            ResponseText = r.ResponseText, ResponseType = r.ResponseType,
            ResponseDate = r.ResponseDate, RespondedBy = r.RespondedBy,
            IsSubmittedToDNV = r.IsSubmittedToDNV, Status = r.Status,
            AttachmentPath = r.AttachmentPath
        });
    }
}
