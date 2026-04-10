using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Domain.Entities;
using SSCTransactional.Domain.Exceptions;
using SSCTransactional.Domain.Interfaces;

namespace SSCTransactional.Application.Commands.Rescan;

public class CreateRescanCommandHandler : IRequestHandler<CreateRescanCommand, RescanDto>
{
    private readonly IRescanRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRescanCommandHandler(IRescanRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<RescanDto> Handle(CreateRescanCommand cmd, CancellationToken ct)
    {
        var id = await _repo.GetNextIdAsync(ct);
        var rescan = RescanDetail.Create(id, cmd.DocId, cmd.AllocationId, cmd.RescanTo, cmd.Remarks);
        await _repo.AddAsync(rescan, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(rescan);
    }

    private static RescanDto MapToDto(RescanDetail r) => new(
        r.Id, r.DocId, r.AllocationId, r.Status, r.RescanDate, r.RescanTo,
        r.RescanRemarks, r.CompletedOn, r.CompletedBy, r.CompletionRemarks, r.FilePath);
}

public class CompleteRescanCommandHandler : IRequestHandler<CompleteRescanCommand, RescanDto>
{
    private readonly IRescanRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteRescanCommandHandler(IRescanRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<RescanDto> Handle(CompleteRescanCommand cmd, CancellationToken ct)
    {
        var rescan = await _repo.GetByIdAsync(cmd.RescanId, ct)
            ?? throw new RescanNotFoundException(cmd.RescanId);
        rescan.Complete(cmd.CompletedBy, cmd.CompletionRemarks, cmd.FilePath);
        await _repo.UpdateAsync(rescan, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return MapToDto(rescan);
    }

    private static RescanDto MapToDto(RescanDetail r) => new(
        r.Id, r.DocId, r.AllocationId, r.Status, r.RescanDate, r.RescanTo,
        r.RescanRemarks, r.CompletedOn, r.CompletedBy, r.CompletionRemarks, r.FilePath);
}
