using EximManagement.Application.DTOs;
using EximManagement.Application.Interfaces;
using MediatR;

namespace EximManagement.Application.Queries.DataFiles;

// ─── Queries ──────────────────────────────────────────────────────────────────

public record GetDataFileByIdQuery(long FileId) : IRequest<EximDataFileDto?>;

public record GetAllDataFilesQuery(string? FileType = null) : IRequest<IEnumerable<EximDataFileDto>>;

public record GetEximDataQuery(DateTime StartDate, DateTime EndDate, string FileType, int Page, int PageSize)
    : IRequest<PagedResult<object>>;

// ─── Handlers ─────────────────────────────────────────────────────────────────

public class GetDataFileByIdQueryHandler(IEximDataFileRepository repo)
    : IRequestHandler<GetDataFileByIdQuery, EximDataFileDto?>
{
    public async Task<EximDataFileDto?> Handle(GetDataFileByIdQuery query, CancellationToken ct)
    {
        var file = await repo.GetByIdAsync(query.FileId, ct);
        if (file is null) return null;
        return new EximDataFileDto
        {
            FileId = file.FileId, FileType = file.FileType, FileName = file.FileName,
            OriginalCount = file.OriginalCount, FinalCount = file.FinalCount,
            FileUploadedBy = file.FileUploadedBy, FileUploadedOn = file.FileUploadedOn,
            Remarks = file.Remarks, FileSource = file.FileSource,
            DataTypeCode = file.DataTypeCode, DataTypeMonth = file.DataTypeMonth
        };
    }
}

public class GetAllDataFilesQueryHandler(IEximDataFileRepository repo)
    : IRequestHandler<GetAllDataFilesQuery, IEnumerable<EximDataFileDto>>
{
    public async Task<IEnumerable<EximDataFileDto>> Handle(GetAllDataFilesQuery query, CancellationToken ct)
    {
        var files = string.IsNullOrWhiteSpace(query.FileType)
            ? await repo.GetAllAsync(ct)
            : await repo.GetByTypeAsync(query.FileType, ct);

        return files.Select(f => new EximDataFileDto
        {
            FileId = f.FileId, FileType = f.FileType, FileName = f.FileName,
            OriginalCount = f.OriginalCount, FinalCount = f.FinalCount,
            FileUploadedBy = f.FileUploadedBy, FileUploadedOn = f.FileUploadedOn,
            Remarks = f.Remarks, FileSource = f.FileSource,
            DataTypeCode = f.DataTypeCode, DataTypeMonth = f.DataTypeMonth
        });
    }
}
