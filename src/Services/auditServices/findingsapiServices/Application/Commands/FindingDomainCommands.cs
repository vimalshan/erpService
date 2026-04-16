using FindingsAPI.Gateway.Application.DTOs;
using FindingsAPI.Gateway.Domain.Entities;
using FindingsAPI.Gateway.Domain.Interfaces;
using MediatR;

namespace FindingsAPI.Gateway.Application.Commands;

// Commands
public record CreateFindingDomainCommand(CreateFindingDomainDto Dto) : IRequest<FindingDomainDto>;
public record UpdateFindingDomainCommand(UpdateFindingDomainDto Dto) : IRequest<FindingDomainDto>;
public record DeleteFindingDomainCommand(int FindingId) : IRequest<bool>;
public record ChangeStatusCommand(int FindingId, int NewStatusId, int? ModifiedBy) : IRequest<FindingDomainDto>;
public record CloseFindingDomainCommand(int FindingId, int? ClosedBy) : IRequest<FindingDomainDto>;
public record AssignFindingCommand(int FindingId, int? AssignedTo, int? ModifiedBy) : IRequest<FindingDomainDto>;
public record VerifyFindingCommand(int FindingId, int? VerifiedBy) : IRequest<FindingDomainDto>;
public record AddFindingResponseCommand(CreateFindingResponseDto Dto) : IRequest<FindingResponseDto>;

// Handlers
public class CreateFindingDomainHandler : IRequestHandler<CreateFindingDomainCommand, FindingDomainDto>
{
    private readonly IFindingsDomainRepository _repo;
    private readonly IMediator _mediator;

    public CreateFindingDomainHandler(IFindingsDomainRepository repo, IMediator mediator)
    {
        _repo = repo;
        _mediator = mediator;
    }

    public async Task<FindingDomainDto> Handle(CreateFindingDomainCommand request, CancellationToken ct)
    {
        var entity = FindingEntity.Create(
            request.Dto.AuditId, request.Dto.Title, request.Dto.Description,
            request.Dto.FindingType, request.Dto.Severity, request.Dto.FindingStatusId,
            request.Dto.FindingCategoryId, request.Dto.SiteId, request.Dto.IdentifiedBy);

        var saved = await _repo.AddAsync(entity);
        foreach (var e in saved.DomainEvents) await _mediator.Publish(e, ct);
        saved.ClearDomainEvents();

        return MapToDto(saved);
    }

    private static FindingDomainDto MapToDto(FindingEntity e) => new()
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

public class UpdateFindingDomainHandler : IRequestHandler<UpdateFindingDomainCommand, FindingDomainDto>
{
    private readonly IFindingsDomainRepository _repo;
    public UpdateFindingDomainHandler(IFindingsDomainRepository repo) => _repo = repo;

    public async Task<FindingDomainDto> Handle(UpdateFindingDomainCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Dto.FindingId)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Finding {request.Dto.FindingId} not found");
        entity.Title = request.Dto.Title;
        entity.Description = request.Dto.Description;
        entity.Severity = request.Dto.Severity;
        entity.FindingCategoryId = request.Dto.FindingCategoryId;
        entity.DueDate = request.Dto.DueDate;
        entity.Evidence = request.Dto.Evidence;
        entity.RootCause = request.Dto.RootCause;
        entity.CorrectiveAction = request.Dto.CorrectiveAction;
        entity.PreventiveAction = request.Dto.PreventiveAction;
        entity.ModifiedBy = request.Dto.ModifiedBy;
        entity.ModifiedDate = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
        return new FindingDomainDto
        {
            FindingId = entity.FindingId, FindingNumber = entity.FindingNumber, AuditId = entity.AuditId,
            Title = entity.Title, Description = entity.Description, FindingType = entity.FindingType,
            Severity = entity.Severity, FindingStatusId = entity.FindingStatusId,
            FindingCategoryId = entity.FindingCategoryId, DueDate = entity.DueDate,
            IsActive = entity.IsActive, IdentifiedBy = entity.IdentifiedBy, AssignedTo = entity.AssignedTo,
            Evidence = entity.Evidence, RootCause = entity.RootCause,
            CorrectiveAction = entity.CorrectiveAction, PreventiveAction = entity.PreventiveAction
        };
    }
}

public class DeleteFindingDomainHandler : IRequestHandler<DeleteFindingDomainCommand, bool>
{
    private readonly IFindingsDomainRepository _repo;
    public DeleteFindingDomainHandler(IFindingsDomainRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteFindingDomainCommand request, CancellationToken ct)
    {
        await _repo.DeleteAsync(request.FindingId);
        return true;
    }
}

public class ChangeStatusHandler : IRequestHandler<ChangeStatusCommand, FindingDomainDto>
{
    private readonly IFindingsDomainRepository _repo;
    private readonly IMediator _mediator;
    public ChangeStatusHandler(IFindingsDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<FindingDomainDto> Handle(ChangeStatusCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.FindingId)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Finding {request.FindingId} not found");
        entity.ChangeStatus(request.NewStatusId, request.ModifiedBy);
        await _repo.UpdateAsync(entity);
        foreach (var e in entity.DomainEvents) await _mediator.Publish(e, ct);
        entity.ClearDomainEvents();
        return new FindingDomainDto
        {
            FindingId = entity.FindingId, FindingNumber = entity.FindingNumber,
            Title = entity.Title, FindingStatusId = entity.FindingStatusId, IsActive = entity.IsActive
        };
    }
}

public class CloseFindingDomainHandler : IRequestHandler<CloseFindingDomainCommand, FindingDomainDto>
{
    private readonly IFindingsDomainRepository _repo;
    private readonly IMediator _mediator;
    public CloseFindingDomainHandler(IFindingsDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<FindingDomainDto> Handle(CloseFindingDomainCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.FindingId)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Finding {request.FindingId} not found");
        entity.Close(request.ClosedBy);
        await _repo.UpdateAsync(entity);
        foreach (var e in entity.DomainEvents) await _mediator.Publish(e, ct);
        entity.ClearDomainEvents();
        return new FindingDomainDto
        {
            FindingId = entity.FindingId, FindingNumber = entity.FindingNumber,
            Title = entity.Title, ClosedDate = entity.ClosedDate, IsActive = entity.IsActive
        };
    }
}

public class AssignFindingHandler : IRequestHandler<AssignFindingCommand, FindingDomainDto>
{
    private readonly IFindingsDomainRepository _repo;
    private readonly IMediator _mediator;
    public AssignFindingHandler(IFindingsDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<FindingDomainDto> Handle(AssignFindingCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.FindingId)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Finding {request.FindingId} not found");
        entity.Assign(request.AssignedTo, request.ModifiedBy);
        await _repo.UpdateAsync(entity);
        foreach (var e in entity.DomainEvents) await _mediator.Publish(e, ct);
        entity.ClearDomainEvents();
        return new FindingDomainDto
        {
            FindingId = entity.FindingId, FindingNumber = entity.FindingNumber,
            Title = entity.Title, AssignedTo = entity.AssignedTo
        };
    }
}

public class VerifyFindingHandler : IRequestHandler<VerifyFindingCommand, FindingDomainDto>
{
    private readonly IFindingsDomainRepository _repo;
    private readonly IMediator _mediator;
    public VerifyFindingHandler(IFindingsDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<FindingDomainDto> Handle(VerifyFindingCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.FindingId)
            ?? throw new System.Collections.Generic.KeyNotFoundException($"Finding {request.FindingId} not found");
        entity.Verify(request.VerifiedBy);
        await _repo.UpdateAsync(entity);
        foreach (var e in entity.DomainEvents) await _mediator.Publish(e, ct);
        entity.ClearDomainEvents();
        return new FindingDomainDto
        {
            FindingId = entity.FindingId, FindingNumber = entity.FindingNumber,
            Title = entity.Title, VerifiedBy = entity.VerifiedBy, VerificationDate = entity.VerificationDate
        };
    }
}

public class AddFindingResponseHandler : IRequestHandler<AddFindingResponseCommand, FindingResponseDto>
{
    private readonly IFindingsDomainRepository _repo;
    private readonly IMediator _mediator;
    public AddFindingResponseHandler(IFindingsDomainRepository repo, IMediator mediator) { _repo = repo; _mediator = mediator; }

    public async Task<FindingResponseDto> Handle(AddFindingResponseCommand request, CancellationToken ct)
    {
        var response = new FindingResponseEntity
        {
            FindingId = request.Dto.FindingId,
            ResponseText = request.Dto.ResponseText,
            ResponseType = request.Dto.ResponseType,
            RespondedBy = request.Dto.RespondedBy,
            AttachmentPath = request.Dto.AttachmentPath,
            ResponseDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsActive = true,
            Status = "Draft"
        };
        var saved = await _repo.AddResponseAsync(response);
        await _mediator.Publish(new Domain.Events.FindingResponseAddedEvent(saved.FindingResponseId, saved.FindingId, saved.ResponseType), ct);
        return new FindingResponseDto
        {
            FindingResponseId = saved.FindingResponseId, FindingId = saved.FindingId,
            ResponseText = saved.ResponseText, ResponseType = saved.ResponseType,
            ResponseDate = saved.ResponseDate, RespondedBy = saved.RespondedBy,
            IsSubmittedToDNV = saved.IsSubmittedToDNV, Status = saved.Status,
            AttachmentPath = saved.AttachmentPath
        };
    }
}
