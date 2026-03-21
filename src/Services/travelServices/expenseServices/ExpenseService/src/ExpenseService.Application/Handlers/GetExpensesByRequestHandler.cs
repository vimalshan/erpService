using AutoMapper;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Queries;
using ExpenseService.Domain.Interfaces;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class GetExpensesByRequestHandler : IRequestHandler<GetExpensesByRequestQuery, IReadOnlyList<TravelExpenseDto>>
{
    private readonly IExpenseRepository _repository;
    private readonly IMapper _mapper;

    public GetExpensesByRequestHandler(IExpenseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TravelExpenseDto>> Handle(GetExpensesByRequestQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _repository.GetByRequestNumberAsync(request.RequestNumber, cancellationToken);
        return _mapper.Map<IReadOnlyList<TravelExpenseDto>>(expenses);
    }
}
