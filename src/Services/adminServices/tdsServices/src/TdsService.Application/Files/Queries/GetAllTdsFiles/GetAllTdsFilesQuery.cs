using MediatR;
using TdsService.Application.DTOs;

namespace TdsService.Application.Files.Queries.GetAllTdsFiles;

public sealed record GetAllTdsFilesQuery(int Page = 1, int PageSize = 20)
    : IRequest<PagedResult<TdsFileDto>>;
