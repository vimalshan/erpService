using FilingAndArchiveService.Application.DTOs;
using FilingAndArchiveService.Domain.Interfaces;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Queries.GetAllFiles;

public class GetAllFilesQueryHandler : IRequestHandler<GetAllFilesQuery, IEnumerable<FileMasterDto>>
{
    private readonly IFileRepository _fileRepository;

    public GetAllFilesQueryHandler(IFileRepository fileRepository)
        => _fileRepository = fileRepository;

    public async Task<IEnumerable<FileMasterDto>> Handle(GetAllFilesQuery request, CancellationToken cancellationToken)
    {
        var files = await _fileRepository.GetAllAsync(cancellationToken);
        return files
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(f => f.ToDto());
    }
}
