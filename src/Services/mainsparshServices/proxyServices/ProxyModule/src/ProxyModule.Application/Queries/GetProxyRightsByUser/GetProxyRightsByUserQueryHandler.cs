using MediatR;
using ProxyModule.Application.DTOs;
using ProxyModule.Application.Interfaces;

namespace ProxyModule.Application.Queries.GetProxyRightsByUser;

public sealed class GetProxyRightsByUserQueryHandler : IRequestHandler<GetProxyRightsByUserQuery, IEnumerable<ProxyRightDto>>
{
    private readonly IProxyRightReadRepository _readRepository;

    public GetProxyRightsByUserQueryHandler(IProxyRightReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<IEnumerable<ProxyRightDto>> Handle(GetProxyRightsByUserQuery request, CancellationToken cancellationToken)
    {
        return await _readRepository.GetByProxyUserIdAsync(request.ProxyUserId, cancellationToken);
    }
}
