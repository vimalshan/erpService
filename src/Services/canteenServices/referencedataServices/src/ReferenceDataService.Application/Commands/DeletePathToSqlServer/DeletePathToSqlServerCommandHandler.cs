using MediatR;
using ReferenceDataService.Domain.Interfaces;

namespace ReferenceDataService.Application.Commands.DeletePathToSqlServer;

public class DeletePathToSqlServerCommandHandler : IRequestHandler<DeletePathToSqlServerCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePathToSqlServerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeletePathToSqlServerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.PathToSqlServers.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return false;

        _unitOfWork.PathToSqlServers.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
