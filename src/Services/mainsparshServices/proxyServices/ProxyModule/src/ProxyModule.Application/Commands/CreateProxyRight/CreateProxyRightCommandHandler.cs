using AutoMapper;
using MediatR;
using ProxyModule.Application.DTOs;
using ProxyModule.Domain.Entities;
using ProxyModule.Domain.Interfaces;

namespace ProxyModule.Application.Commands.CreateProxyRight;

public sealed class CreateProxyRightCommandHandler : IRequestHandler<CreateProxyRightCommand, ProxyRightDto>
{
    private readonly IProxyRightRepository _repository;
    private readonly IMapper _mapper;

    public CreateProxyRightCommandHandler(IProxyRightRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProxyRightDto> Handle(CreateProxyRightCommand request, CancellationToken cancellationToken)
    {
        var entity = ProxyRight.Create(
            request.ProxyUserId,
            request.DelegatedUserId,
            request.ProxyStartDate,
            request.ProxyEndDate,
            request.ProxyType,
            request.Scope,
            request.Notes,
            request.CreatedBy);

        var created = await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProxyRightDto>(created);
    }
}
