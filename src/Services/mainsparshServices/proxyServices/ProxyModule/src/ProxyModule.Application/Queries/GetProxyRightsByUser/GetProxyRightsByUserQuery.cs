using MediatR;
using ProxyModule.Application.DTOs;

namespace ProxyModule.Application.Queries.GetProxyRightsByUser;

public sealed record GetProxyRightsByUserQuery(long ProxyUserId) : IRequest<IEnumerable<ProxyRightDto>>;
