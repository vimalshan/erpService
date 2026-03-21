using AutoMapper;
using ExpenseService.Application.Commands;
using ExpenseService.Application.DTOs;
using ExpenseService.Domain.Entities;
using ExpenseService.Domain.Interfaces;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class CreateConveyanceHandler : IRequestHandler<CreateConveyanceCommand, ConveyanceDto>
{
    private readonly IConveyanceRepository _repository;
    private readonly IMapper _mapper;

    public CreateConveyanceHandler(IConveyanceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ConveyanceDto> Handle(CreateConveyanceCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByRequestNumberAsync(request.RequestNumber, cancellationToken);
        var nextSerial = existing.Count > 0 ? existing.Max(c => c.SerialNumber) + 1 : 1;

        var conveyance = new TravelConveyance
        {
            SerialNumber = nextSerial,
            RequestNumber = request.RequestNumber,
            Date = request.Date,
            Particulars = request.Particulars,
            Mode = request.Mode,
            Amount = request.Amount
        };

        var created = await _repository.AddAsync(conveyance, cancellationToken);
        return _mapper.Map<ConveyanceDto>(created);
    }
}
