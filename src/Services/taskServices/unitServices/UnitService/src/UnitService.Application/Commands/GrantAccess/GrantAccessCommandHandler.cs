using MediatR;
using UnitService.Domain.Entities;
using UnitService.Domain.Interfaces;

namespace UnitService.Application.Commands.GrantAccess;

public class GrantAccessCommandHandler : IRequestHandler<GrantAccessCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public GrantAccessCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(GrantAccessCommand request, CancellationToken cancellationToken)
    {
        var nextId = await _unitOfWork.Access.GetNextIdAsync(cancellationToken);

        var access = AccessMaster.Create(
            nextId,
            request.UnitCode,
            request.EmployeeSysId,
            request.AccessType,
            request.Module,
            request.ModifiedBy);

        await _unitOfWork.Access.AddAsync(access, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return nextId;
    }
}
