using AuditService.Domain.Interfaces;
using MediatR;

namespace AuditService.Application.Commands.Audits;

public sealed class UpdateAuditCommandHandler : IRequestHandler<UpdateAuditCommand, bool>
{
    private readonly IAuditRepository _auditRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAuditCommandHandler(IAuditRepository auditRepository, IUnitOfWork unitOfWork)
    {
        _auditRepository = auditRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateAuditCommand request, CancellationToken cancellationToken)
    {
        var audit = await _auditRepository.GetByIdAsync(request.AuditId, cancellationToken);
        if (audit is null) return false;

        audit.Update(request.AuditName, request.AuditDefLocation, request.AuditFrom, request.AuditTo, request.UpdatedBy);
        await _auditRepository.UpdateAsync(audit, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
