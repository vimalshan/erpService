using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Domain.Aggregates;
using SSCTransactional.Domain.Exceptions;
using SSCTransactional.Domain.Interfaces;

namespace SSCTransactional.Application.Commands.Correspondence;

public class CreateCorrespondenceCommandHandler : IRequestHandler<CreateCorrespondenceCommand, CorrespondenceDto>
{
    private readonly ICorrespondenceRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCorrespondenceCommandHandler(ICorrespondenceRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<CorrespondenceDto> Handle(CreateCorrespondenceCommand cmd, CancellationToken ct)
    {
        var id = await _repo.GetNextIdAsync(ct);
        var corr = CorrespondenceAggregate.Create(id, cmd.DocId, cmd.AllocationId,
            cmd.HoldCategory, cmd.HoldType, cmd.HoldRemarks, cmd.HoldBy, cmd.HoldNature);
        await _repo.AddAsync(corr, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(corr);
    }

    private static CorrespondenceDto MapToDto(CorrespondenceAggregate c) => new(
        c.Id, c.DocId, c.AllocationId, c.HoldCategory, c.HoldType,
        c.HoldDate, c.HoldRemarks, c.HoldBy, c.HoldStatus,
        c.ReleaseDate, c.ReleaseRemarks, c.ReleasedBy, c.HoldNature,
        c.Attachments.Select(a => new CorrespondenceAttachmentDto(a.Id, a.CorrespondenceId, a.CorrespondenceStatus, a.FilePath)).ToList());
}

public class ReleaseCorrespondenceCommandHandler : IRequestHandler<ReleaseCorrespondenceCommand, CorrespondenceDto>
{
    private readonly ICorrespondenceRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public ReleaseCorrespondenceCommandHandler(ICorrespondenceRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<CorrespondenceDto> Handle(ReleaseCorrespondenceCommand cmd, CancellationToken ct)
    {
        var corr = await _repo.GetByIdAsync(cmd.CorrespondenceId, ct)
            ?? throw new CorrespondenceNotFoundException(cmd.CorrespondenceId);
        corr.Release(cmd.ReleasedBy, cmd.ReleaseRemarks);
        await _repo.UpdateAsync(corr, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(corr);
    }

    private static CorrespondenceDto MapToDto(CorrespondenceAggregate c) => new(
        c.Id, c.DocId, c.AllocationId, c.HoldCategory, c.HoldType,
        c.HoldDate, c.HoldRemarks, c.HoldBy, c.HoldStatus,
        c.ReleaseDate, c.ReleaseRemarks, c.ReleasedBy, c.HoldNature,
        c.Attachments.Select(a => new CorrespondenceAttachmentDto(a.Id, a.CorrespondenceId, a.CorrespondenceStatus, a.FilePath)).ToList());
}
