using MediatR;
using RecruitmentService.Application.DTOs;
using RecruitmentService.Domain.Entities;
using RecruitmentService.Domain.Exceptions;
using RecruitmentService.Domain.Interfaces;
using RecruitmentService.Application.Interfaces;

namespace RecruitmentService.Application.Commands.Prospects;

// ── Register ──────────────────────────────────────────────────────────────────

public record RegisterProspectCommand(RegisterProspectRequest Request) : IRequest<decimal>;

public class RegisterProspectCommandHandler : IRequestHandler<RegisterProspectCommand, decimal>
{
    private readonly IProspectRepository _repository;
    private readonly IUnitOfWork _uow;

    public RegisterProspectCommandHandler(IProspectRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<decimal> Handle(RegisterProspectCommand cmd, CancellationToken ct)
    {
        var req = cmd.Request;

        if (await _repository.EmailExistsAsync(req.EmailId, ct))
            throw new DuplicateEmailException(req.EmailId);

        var prospect = Prospect.Register(
            req.UserId, req.FirstName, req.MiddleName, req.LastName,
            req.EmailId, req.DateOfBirth, req.ProspectType);

        await _repository.AddAsync(prospect, ct);
        await _uow.SaveChangesAsync(ct);
        return prospect.WebUserId;
    }
}

// ── Deactivate ────────────────────────────────────────────────────────────────

public record DeactivateProspectCommand(decimal UserId) : IRequest;

public class DeactivateProspectCommandHandler : IRequestHandler<DeactivateProspectCommand>
{
    private readonly IProspectRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeactivateProspectCommandHandler(IProspectRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeactivateProspectCommand cmd, CancellationToken ct)
    {
        var prospect = await _repository.GetByIdAsync(cmd.UserId, ct)
            ?? throw new ProspectNotFoundException(cmd.UserId);

        prospect.Deactivate();
        _repository.Update(prospect);
        await _uow.SaveChangesAsync(ct);
    }
}
