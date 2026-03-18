using MediatR;

namespace TdsService.Application.Files.Commands.UploadTdsFile;

public sealed record UploadTdsFileCommand(
    long FileId,
    string FileName,
    string? PanNo,
    string? EmailStatus,
    string? FileType,
    Stream? FileContent,
    string? ContentType) : IRequest<long>;
