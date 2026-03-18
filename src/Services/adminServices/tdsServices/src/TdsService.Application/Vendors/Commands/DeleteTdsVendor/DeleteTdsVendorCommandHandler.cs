using MediatR;
using TdsService.Application.Common.Exceptions;
using TdsService.Domain.Repositories;

namespace TdsService.Application.Vendors.Commands.DeleteTdsVendor;

public sealed class DeleteTdsVendorCommandHandler : IRequestHandler<DeleteTdsVendorCommand>
{
    private readonly ITdsVendorRepository _repository;

    public DeleteTdsVendorCommandHandler(ITdsVendorRepository repository)
        => _repository = repository;

    public async Task Handle(DeleteTdsVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _repository.GetByIdAsync(request.VendorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TdsVendor), request.VendorId);

        _repository.Remove(vendor);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
