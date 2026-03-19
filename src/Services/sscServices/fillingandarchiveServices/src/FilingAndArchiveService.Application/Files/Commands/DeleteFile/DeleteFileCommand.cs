using MediatR;

namespace FilingAndArchiveService.Application.Files.Commands.DeleteFile;

public record DeleteFileCommand(long FileId, long DeletedBy) : IRequest<bool>;
