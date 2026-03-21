using AutoMapper;
using MediatR;
using ReceivingService.Application.DTOs;
using ReceivingService.Domain.Interfaces;

namespace ReceivingService.Application.Commands.CancelReceiving;

public sealed class CancelReceivingCommandHandler
    : IRequestHandler<CancelReceivingCommand, ReceivingDto>
{
    private readonly IReceivingRepository _repository;
    private readonly IMapper _mapper;

    public CancelReceivingCommandHandler(IReceivingRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<ReceivingDto> Handle(
        CancelReceivingCommand request, CancellationToken cancellationToken)
    {
        var receiving = await _repository.GetByIdAsync(request.ReceivingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Receiving {request.ReceivingId} not found.");

        receiving.Cancel();
        _repository.Update(receiving);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReceivingDto>(receiving);
    }
}
