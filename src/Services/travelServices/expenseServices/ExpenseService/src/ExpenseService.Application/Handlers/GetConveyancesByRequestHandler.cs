using AutoMapper;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Queries;
using ExpenseService.Domain.Interfaces;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class GetConveyancesByRequestHandler : IRequestHandler<GetConveyancesByRequestQuery, IReadOnlyList<ConveyanceDto>>
{
    private readonly IConveyanceRepository _repository;
    private readonly IMapper _mapper;

    public GetConveyancesByRequestHandler(IConveyanceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ConveyanceDto>> Handle(GetConveyancesByRequestQuery request, CancellationToken cancellationToken)
    {
        var conveyances = await _repository.GetByRequestNumberAsync(request.RequestNumber, cancellationToken);
        return _mapper.Map<IReadOnlyList<ConveyanceDto>>(conveyances);
    }
}
