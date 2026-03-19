using FilingAndArchiveService.Application.DTOs;
using FilingAndArchiveService.Application.Files.Commands.CreateFile;
using FilingAndArchiveService.Application.Files.Commands.DispatchFile;
using FilingAndArchiveService.Application.Files.Commands.UpdateFile;
using MediatR;

namespace FilingAndArchiveService.API.GraphQL;

public class FilingMutation
{
    public async Task<FileMasterDto> CreateFile(
        [Service] IMediator mediator,
        string fileOrgId,
        long fileYear,
        string fileNo,
        long createdBy,
        string? remarks,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new CreateFileCommand(fileOrgId, fileYear, fileNo, createdBy, remarks), cancellationToken);

    public async Task<FileMasterDto> UpdateFile(
        [Service] IMediator mediator,
        long fileId,
        string? remarks,
        string? podNo,
        string? courierName,
        long updatedBy,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new UpdateFileCommand(fileId, remarks, podNo, courierName, updatedBy), cancellationToken);

    public async Task<FileMasterDto> DispatchFile(
        [Service] IMediator mediator,
        long fileId,
        string podNo,
        string courierName,
        long dispatchedBy,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new DispatchFileCommand(fileId, podNo, courierName, dispatchedBy), cancellationToken);
}
