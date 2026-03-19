using FilingAndArchiveService.Application.DTOs;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Commands.DispatchFile;

public record DispatchFileCommand(
    long FileId,
    string PodNo,
    string CourierName,
    long DispatchedBy
) : IRequest<FileMasterDto>;
