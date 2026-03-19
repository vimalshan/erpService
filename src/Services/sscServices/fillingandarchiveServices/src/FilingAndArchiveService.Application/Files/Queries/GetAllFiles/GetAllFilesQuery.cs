using FilingAndArchiveService.Application.DTOs;
using MediatR;

namespace FilingAndArchiveService.Application.Files.Queries.GetAllFiles;

public record GetAllFilesQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<FileMasterDto>>;
