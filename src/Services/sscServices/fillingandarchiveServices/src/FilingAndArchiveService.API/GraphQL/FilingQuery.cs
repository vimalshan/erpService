using FilingAndArchiveService.Application.DTOs;
using FilingAndArchiveService.Application.Files.Queries.GetAllFiles;
using FilingAndArchiveService.Application.Files.Queries.GetFileById;
using FilingAndArchiveService.Infrastructure.Persistence.DapperQueries;
using MediatR;

namespace FilingAndArchiveService.API.GraphQL;

public class FilingQuery
{
    public async Task<IEnumerable<FileMasterDto>> GetFiles(
        [Service] IMediator mediator,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllFilesQuery(page, pageSize), cancellationToken);

    public async Task<FileMasterDto?> GetFileById(
        [Service] IMediator mediator,
        long fileId,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetFileByIdQuery(fileId), cancellationToken);

    public async Task<IEnumerable<FileMasterDto>> SearchFiles(
        [Service] FilingDapperRepository dapperRepo,
        string? orgId,
        string? fileNo,
        long? year,
        string? status,
        CancellationToken cancellationToken = default)
        => await dapperRepo.SearchFilesAsync(orgId, fileNo, year, status, cancellationToken);
}
