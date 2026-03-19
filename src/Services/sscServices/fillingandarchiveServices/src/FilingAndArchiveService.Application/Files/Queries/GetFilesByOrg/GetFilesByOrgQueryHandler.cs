using FilingAndArchiveService.Application.DTOs;
using FilingAndArchiveService.Domain.Interfaces;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Queries.GetFilesByOrg;

public class GetFilesByOrgQueryHandler : IRequestHandler<GetFilesByOrgQuery, IEnumerable<FileMasterDto>>
{
    private readonly IFileRepository _fileRepository;

    public GetFilesByOrgQueryHandler(IFileRepository fileRepository)
        => _fileRepository = fileRepository;

    public async Task<IEnumerable<FileMasterDto>> Handle(GetFilesByOrgQuery request, CancellationToken cancellationToken)
    {
        var files = await _fileRepository.GetByOrgAsync(request.OrgId, cancellationToken);

        if (request.Year.HasValue)
            files = files.Where(f => f.FileYear == request.Year.Value);

        return files.Select(f => f.ToDto());
    }
}
