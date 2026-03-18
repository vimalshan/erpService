using MediatR;
using TdsService.Application.Common.Interfaces;
using TdsService.Domain.Entities;
using TdsService.Domain.Repositories;

namespace TdsService.Application.Files.Commands.UploadTdsFile;

public sealed class UploadTdsFileCommandHandler : IRequestHandler<UploadTdsFileCommand, long>
{
    private const string BlobContainer = "tds-files";

    private readonly ITdsFileRepository _repository;
    private readonly IBlobStorageService _blobStorage;

    public UploadTdsFileCommandHandler(
        ITdsFileRepository repository,
        IBlobStorageService blobStorage)
    {
        _repository = repository;
        _blobStorage = blobStorage;
    }

    public async Task<long> Handle(UploadTdsFileCommand request, CancellationToken cancellationToken)
    {
        var file = TdsFile.Create(
            request.FileId,
            request.FileName,
            request.PanNo,
            request.EmailStatus,
            request.FileType);

        // Upload binary content to Blob Storage if provided
        if (request.FileContent is not null)
        {
            var blobName = $"{request.FileId}/{request.FileName}";
            var uri = await _blobStorage.UploadAsync(
                BlobContainer,
                blobName,
                request.FileContent,
                request.ContentType ?? "application/octet-stream",
                cancellationToken);

            file.SetBlobUri(uri);
        }

        await _repository.AddAsync(file, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return file.Id;
    }
}
