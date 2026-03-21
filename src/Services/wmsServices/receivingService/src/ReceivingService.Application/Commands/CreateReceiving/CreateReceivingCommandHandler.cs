using AutoMapper;
using MediatR;
using ReceivingService.Application.DTOs;
using ReceivingService.Domain.Interfaces;

namespace ReceivingService.Application.Commands.CreateReceiving;

public sealed class CreateReceivingCommandHandler
    : IRequestHandler<CreateReceivingCommand, ReceivingDto>
{
    private readonly IReceivingRepository _repository;
    private readonly IMapper _mapper;

    public CreateReceivingCommandHandler(IReceivingRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<ReceivingDto> Handle(
        CreateReceivingCommand request, CancellationToken cancellationToken)
    {
        var receiving = Domain.Entities.Receiving.Create(
            request.ReceivingNumber,
            request.PoId,
            request.WarehouseId,
            request.Notes,
            request.CreatedBy);

        foreach (var line in request.Lines)
        {
            receiving.AddLine(
                line.PoLineId,
                line.ProductId,
                line.BinId,
                line.QuantityReceived,
                line.LotNumber,
                line.ExpiryDate,
                line.Notes);
        }

        await _repository.AddAsync(receiving, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReceivingDto>(receiving);
    }
}
