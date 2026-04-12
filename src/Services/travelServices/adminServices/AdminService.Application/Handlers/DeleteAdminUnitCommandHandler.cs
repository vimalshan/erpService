using MediatR;
using AdminService.Application.Commands;
using AdminService.Domain.Events;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Handlers;

/// <summary>
/// Handler for DeleteAdminUnitCommand
/// </summary>
public class DeleteAdminUnitCommandHandler : IRequestHandler<DeleteAdminUnitCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public DeleteAdminUnitCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<bool> Handle(DeleteAdminUnitCommand request, CancellationToken cancellationToken)
    {
        var adminUnit = await _unitOfWork.AdminUnits.GetByIdAsync(request.Id, cancellationToken);
        if (adminUnit == null)
            return false;

        var adminCode = adminUnit.AdminCode;

        await _unitOfWork.AdminUnits.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new AdminUnitDeletedEvent(adminCode, DateTime.UtcNow), cancellationToken);

        return true;
    }
}
