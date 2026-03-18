using MediatR;
using TdsService.Application.DTOs;

namespace TdsService.Application.Files.Queries.GetTdsFileById;

public sealed record GetTdsFileByIdQuery(long FileId) : IRequest<TdsFileDto?>;
