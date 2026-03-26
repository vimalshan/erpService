using MediatR;

namespace ReferenceDataService.Application.Commands.DeletePathToSqlServer;

public record DeletePathToSqlServerCommand(int Id) : IRequest<bool>;
