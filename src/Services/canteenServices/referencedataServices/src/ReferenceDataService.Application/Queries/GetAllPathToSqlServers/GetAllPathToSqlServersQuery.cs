using MediatR;
using ReferenceDataService.Application.DTOs;

namespace ReferenceDataService.Application.Queries.GetAllPathToSqlServers;

public record GetAllPathToSqlServersQuery : IRequest<IEnumerable<PathToSqlServerDto>>;
