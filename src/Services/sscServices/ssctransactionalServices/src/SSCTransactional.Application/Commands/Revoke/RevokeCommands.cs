using MediatR;
using SSCTransactional.Application.DTOs;
using SSCTransactional.Domain.Entities;
using SSCTransactional.Domain.Interfaces;

namespace SSCTransactional.Application.Commands.Revoke;

public record CreateRevokeCommand(long DocId, string Remarks, string Status, long RevokedBy) : IRequest<RevokeDto>;

public class CreateRevokeCommandHandler : IRequestHandler<CreateRevokeCommand, RevokeDto>
{
    private readonly IRevokeRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRevokeCommandHandler(IRevokeRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<RevokeDto> Handle(CreateRevokeCommand cmd, CancellationToken ct)
    {
        var id = await _repo.GetNextIdAsync(ct);
        var revoke = RevokeDetail.Create(id, cmd.DocId, cmd.Remarks, cmd.Status, cmd.RevokedBy);
        await _repo.AddAsync(revoke, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return new RevokeDto(revoke.Id, revoke.DocId, revoke.RevokeRemarks,
            revoke.RevokeStatus, revoke.RevokedBy, revoke.RevokedOn);
    }
}
