using AutoMapper;
using MediatR;
using ReceivingService.Application.DTOs;
using ReceivingService.Domain.Exceptions;
using ReceivingService.Domain.Interfaces;

namespace ReceivingService.Application.Commands.CloseReceiving;

public sealed class CloseReceivingCommandHandler
    : IRequestHandler<CloseReceivingCommand, ReceivingDto>
{
    private readonly IReceivingRepository _repository;
    private readonly IMapper _mapper;

    public CloseReceivingCommandHandler(IReceivingRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<ReceivingDto> Handle(
        CloseReceivingCommand request, CancellationToken cancellationToken)
    {
        var receiving = await _repository.GetByIdAsync(request.ReceivingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Receiving {request.ReceivingId} not found.");

        receiving.Close();
        _repository.Update(receiving);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReceivingDto>(receiving);
    }
}
