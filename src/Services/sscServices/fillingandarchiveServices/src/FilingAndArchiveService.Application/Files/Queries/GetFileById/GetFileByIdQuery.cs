using FilingAndArchiveService.Application.DTOs;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Queries.GetFileById;

public record GetFileByIdQuery(long FileId) : IRequest<FileMasterDto?>;
