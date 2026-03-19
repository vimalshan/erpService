using FilingAndArchiveService.Application.DTOs;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Commands.CreateFile;

public record CreateFileCommand(
    string FileOrgId,
    long FileYear,
    string FileNo,
    long CreatedBy,
    string? Remarks = null
) : IRequest<FileMasterDto>;
