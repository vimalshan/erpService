using MediatR;
using OtherService.Domain.Interfaces;

namespace OtherService.Application.CQRS.Commands.DeleteLogDdCatDevDetail;

public sealed class DeleteLogDdCatDevDetailCommandHandler
    : IRequestHandler<DeleteLogDdCatDevDetailCommand, bool>
{
    private readonly ILogDdCatDevDetailRepository _repository;

    public DeleteLogDdCatDevDetailCommandHandler(ILogDdCatDevDetailRepository repository)
        => _repository = repository;

    public async Task<bool> Handle(
        DeleteLogDdCatDevDetailCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByKeyAsync(request.AppId, request.AppNum, cancellationToken);
        if (entity is null) return false;

        _repository.Delete(entity);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
