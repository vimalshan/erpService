using FilingAndArchiveService.Application.DTOs;
using FilingAndArchiveService.Domain.Interfaces;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Queries.GetFileById;

public class GetFileByIdQueryHandler : IRequestHandler<GetFileByIdQuery, FileMasterDto?>
{
    private readonly IFileRepository _fileRepository;

    public GetFileByIdQueryHandler(IFileRepository fileRepository)
        => _fileRepository = fileRepository;

    public async Task<FileMasterDto?> Handle(GetFileByIdQuery request, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetByIdAsync(request.FileId, cancellationToken);
        return file?.ToDto();
    }
}
