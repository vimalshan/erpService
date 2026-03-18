using MediatR;
using TdsService.Application.DTOs;
using TdsService.Domain.Repositories;
using TdsService.Domain.ValueObjects;

namespace TdsService.Application.Files.Queries.GetAllTdsFiles;

public sealed class GetAllTdsFilesQueryHandler
    : IRequestHandler<GetAllTdsFilesQuery, PagedResult<TdsFileDto>>
{
    private readonly ITdsFileRepository _repository;

    public GetAllTdsFilesQueryHandler(ITdsFileRepository repository)
        => _repository = repository;

    public async Task<PagedResult<TdsFileDto>> Handle(
        GetAllTdsFilesQuery request,
        CancellationToken cancellationToken)
    {
        var all = await _repository.GetAllAsync(cancellationToken);
        var totalCount = all.Count;

        var paged = all
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(f => new TdsFileDto(
                f.Id,
                f.FileName,
                f.PanNumber?.Value,
                f.EmailStatus.ToDbValue(),
                f.FileType?.Value,
                f.BlobStorageUri,
                f.CreatedAt,
                f.UpdatedAt))
            .ToList();

        return new PagedResult<TdsFileDto>(paged, totalCount, request.Page, request.PageSize);
    }
}
