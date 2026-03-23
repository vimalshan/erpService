using TdsService.Application.Files.Commands.UpdateEmailStatus;
using TdsService.Application.Files.Commands.UploadTdsFile;
using TdsService.Application.Vendors.Commands.CreateTdsVendor;
using TdsService.Application.Vendors.Commands.DeleteTdsVendor;
using TdsService.Application.Vendors.Commands.UpdateTdsVendor;
using MediatR;

namespace TdsService.API.GraphQL;

public sealed class TdsMutation
{
    public async Task<long> CreateVendor(
        [Service] IMediator mediator,
        long vendorId, string vendorName, string? emailAddress, string? panNo,
        CancellationToken ct = default)
        => await mediator.Send(new CreateTdsVendorCommand(vendorId, vendorName, emailAddress, panNo), ct);

    public async Task<bool> UpdateVendor(
        [Service] IMediator mediator,
        long vendorId, string vendorName, string? emailAddress, string? panNo,
        CancellationToken ct = default)
    {
        await mediator.Send(new UpdateTdsVendorCommand(vendorId, vendorName, emailAddress, panNo), ct);
        return true;
    }

    public async Task<bool> DeleteVendor(
        [Service] IMediator mediator,
        long vendorId,
        CancellationToken ct = default)
    {
        await mediator.Send(new DeleteTdsVendorCommand(vendorId), ct);
        return true;
    }

    public async Task<long> UploadFile(
        [Service] IMediator mediator,
        long fileId, string fileName, string? panNo, string? emailStatus, string? fileType,
        CancellationToken ct = default)
        => await mediator.Send(
            new UploadTdsFileCommand(fileId, fileName, panNo, emailStatus, fileType, null, null), ct);

    public async Task<bool> MarkEmailSent(
        [Service] IMediator mediator,
        long fileId,
        CancellationToken ct = default)
    {
        await mediator.Send(new UpdateEmailStatusCommand(fileId), ct);
        return true;
    }
}
