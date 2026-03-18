using MediatR;
using TdsService.Application.Common.Exceptions;
using TdsService.Domain.Repositories;

namespace TdsService.Application.Vendors.Commands.UpdateTdsVendor;

public sealed class UpdateTdsVendorCommandHandler : IRequestHandler<UpdateTdsVendorCommand>
{
    private readonly ITdsVendorRepository _repository;

    public UpdateTdsVendorCommandHandler(ITdsVendorRepository repository)
        => _repository = repository;

    public async Task Handle(UpdateTdsVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await _repository.GetByIdAsync(request.VendorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.TdsVendor), request.VendorId);

        vendor.Update(request.VendorName, request.EmailAddress, request.PanNo);

        _repository.Update(vendor);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
