using AutoMapper;
using MediatR;
using ProxyModule.Application.DTOs;
using ProxyModule.Application.Interfaces;

namespace ProxyModule.Application.Queries.GetProxyRightById;

public sealed class GetProxyRightByIdQueryHandler : IRequestHandler<GetProxyRightByIdQuery, ProxyRightDto?>
{
    private readonly IProxyRightReadRepository _readRepository;
    private readonly IMapper _mapper;

    public GetProxyRightByIdQueryHandler(IProxyRightReadRepository readRepository, IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<ProxyRightDto?> Handle(GetProxyRightByIdQuery request, CancellationToken cancellationToken)
    {
        return await _readRepository.GetByIdAsync(request.ProxyId, cancellationToken);
    }
}
