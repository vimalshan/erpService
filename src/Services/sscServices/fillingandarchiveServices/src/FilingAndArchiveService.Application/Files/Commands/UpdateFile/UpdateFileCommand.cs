using FilingAndArchiveService.Application.DTOs;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Commands.UpdateFile;

public record UpdateFileCommand(
    long FileId,
    string? Remarks,
    string? PodNo,
    string? CourierName,
    long UpdatedBy
) : IRequest<FileMasterDto>;
