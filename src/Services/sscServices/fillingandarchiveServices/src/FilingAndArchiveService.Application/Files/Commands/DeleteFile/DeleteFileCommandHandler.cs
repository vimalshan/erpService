using FilingAndArchiveService.Application.Common.Interfaces;
using FilingAndArchiveService.Domain.Interfaces;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Commands.DeleteFile;

public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand, bool>
{
    private readonly IFileRepository _fileRepository;
    private readonly IApplicationDbContext _context;

    public DeleteFileCommandHandler(IFileRepository fileRepository, IApplicationDbContext context)
    {
        _fileRepository = fileRepository;
        _context = context;
    }

    public async Task<bool> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        if (!await _fileRepository.ExistsAsync(request.FileId, cancellationToken))
            throw new Domain.Exceptions.FileNotFoundException(request.FileId);

        await _fileRepository.DeleteAsync(request.FileId, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
