using MediatR;
using ProxyModule.Application.DTOs;
using ProxyModule.Application.Interfaces;

namespace ProxyModule.Application.Queries.GetActiveProxyRights;

public sealed class GetActiveProxyRightsQueryHandler : IRequestHandler<GetActiveProxyRightsQuery, IEnumerable<ProxyRightDto>>
{
    private readonly IProxyRightReadRepository _readRepository;

    public GetActiveProxyRightsQueryHandler(IProxyRightReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<IEnumerable<ProxyRightDto>> Handle(GetActiveProxyRightsQuery request, CancellationToken cancellationToken)
    {
        return await _readRepository.GetActiveProxyRightsAsync(cancellationToken);
    }
}
