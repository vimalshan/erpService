using EximManagement.Application.DTOs;
using EximManagement.Application.Interfaces;
using EximManagement.Domain.Entities;
using MediatR;

namespace EximManagement.Application.Commands.DataFiles;

// ─── Commands ─────────────────────────────────────────────────────────────────

public record CreateDataFileCommand(
    long FileId, string FileType, string? FileName,
    long? UploadedBy, string? FileSource, string? Remarks,
    string? DataTypeCode, string? DataTypeMonth, string? DataXml
) : IRequest<EximDataFileDto>;

public record DeleteDataFileCommand(long FileId, string DeletedBy) : IRequest<bool>;

// ─── Handlers ─────────────────────────────────────────────────────────────────

public class CreateDataFileCommandHandler(
    IEximDataFileRepository repo,
    IUnitOfWork uow,
    IMessagePublisher publisher) : IRequestHandler<CreateDataFileCommand, EximDataFileDto>
{
    public async Task<EximDataFileDto> Handle(CreateDataFileCommand cmd, CancellationToken ct)
    {
        var file = EximDataFile.Create(
            cmd.FileId, cmd.FileType, cmd.FileName,
            cmd.UploadedBy, cmd.FileSource, cmd.Remarks,
            cmd.DataTypeCode, cmd.DataTypeMonth, cmd.DataXml);

        await repo.AddAsync(file, ct);
        await uow.SaveChangesAsync(ct);

        await publisher.PublishAsync(new { file.FileId, file.FileType, Timestamp = DateTime.UtcNow },
            "exim.file.uploaded", ct);

        return MapToDto(file);
    }

    private static EximDataFileDto MapToDto(EximDataFile f) => new()
    {
        FileId = f.FileId, FileType = f.FileType, FileName = f.FileName,
        OriginalCount = f.OriginalCount, FinalCount = f.FinalCount,
        FileUploadedBy = f.FileUploadedBy, FileUploadedOn = f.FileUploadedOn,
        Remarks = f.Remarks, FileSource = f.FileSource,
        DataTypeCode = f.DataTypeCode, DataTypeMonth = f.DataTypeMonth
    };
}

public class DeleteDataFileCommandHandler(
    IEximDataFileRepository repo,
    IUnitOfWork uow) : IRequestHandler<DeleteDataFileCommand, bool>
{
    public async Task<bool> Handle(DeleteDataFileCommand cmd, CancellationToken ct)
    {
        var file = await repo.GetByIdAsync(cmd.FileId, ct) 
            ?? throw new KeyNotFoundException($"File {cmd.FileId} not found.");
        file.SoftDelete(cmd.DeletedBy);
        await repo.UpdateAsync(file, ct);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
