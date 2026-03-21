using AutoMapper;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Queries;
using ExpenseService.Domain.Interfaces;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class GetExpenseByIdHandler : IRequestHandler<GetExpenseByIdQuery, TravelExpenseDto?>
{
    private readonly IExpenseRepository _repository;
    private readonly IMapper _mapper;

    public GetExpenseByIdHandler(IExpenseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TravelExpenseDto?> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var expense = await _repository.GetByIdAsync(request.RequestNumber, request.SerialNumber, cancellationToken);
        return expense == null ? null : _mapper.Map<TravelExpenseDto>(expense);
    }
}
