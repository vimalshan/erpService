using MediatR;
using AdminService.Application.Commands;
using AdminService.Domain.Interfaces;

namespace AdminService.Application.Handlers;

/// <summary>
/// Handler for DeleteAdminUnitCommand
/// </summary>
public class DeleteAdminUnitCommandHandler : IRequestHandler<DeleteAdminUnitCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAdminUnitCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<bool> Handle(DeleteAdminUnitCommand request, CancellationToken cancellationToken)
    {
        var adminUnit = await _unitOfWork.AdminUnits.GetByIdAsync(request.Id, cancellationToken);
        if (adminUnit == null)
            return false;

        await _unitOfWork.AdminUnits.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
