using AutoMapper;
using MediatR;
using ProxyModule.Application.DTOs;
using ProxyModule.Domain.Exceptions;
using ProxyModule.Domain.Interfaces;

namespace ProxyModule.Application.Commands.UpdateProxyRight;

public sealed class UpdateProxyRightCommandHandler : IRequestHandler<UpdateProxyRightCommand, ProxyRightDto>
{
    private readonly IProxyRightRepository _repository;
    private readonly IMapper _mapper;

    public UpdateProxyRightCommandHandler(IProxyRightRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProxyRightDto> Handle(UpdateProxyRightCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.ProxyId, cancellationToken)
            ?? throw new ProxyDomainException($"Proxy right with ID {request.ProxyId} not found.");

        entity.Update(
            request.ProxyStartDate,
            request.ProxyEndDate,
            request.ProxyType,
            request.Scope,
            request.Notes,
            request.UpdatedBy);

        await _repository.UpdateAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProxyRightDto>(entity);
    }
}
