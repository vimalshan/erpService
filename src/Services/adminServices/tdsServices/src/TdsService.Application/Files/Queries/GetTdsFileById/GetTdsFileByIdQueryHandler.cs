using MediatR;
using TdsService.Application.DTOs;
using TdsService.Domain.Repositories;
using TdsService.Domain.ValueObjects;

namespace TdsService.Application.Files.Queries.GetTdsFileById;

public sealed class GetTdsFileByIdQueryHandler
    : IRequestHandler<GetTdsFileByIdQuery, TdsFileDto?>
{
    private readonly ITdsFileRepository _repository;

    public GetTdsFileByIdQueryHandler(ITdsFileRepository repository)
        => _repository = repository;

    public async Task<TdsFileDto?> Handle(
        GetTdsFileByIdQuery request,
        CancellationToken cancellationToken)
    {
        var file = await _repository.GetByIdAsync(request.FileId, cancellationToken);
        if (file is null) return null;

        return new TdsFileDto(
            file.Id,
            file.FileName,
            file.PanNumber?.Value,
            file.EmailStatus.ToDbValue(),
            file.FileType?.Value,
            file.BlobStorageUri,
            file.CreatedAt,
            file.UpdatedAt);
    }
}
