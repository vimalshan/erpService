using AutoMapper;
using ExpenseService.Application.Commands;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Interfaces;
using ExpenseService.Domain.Entities;
using ExpenseService.Domain.Events;
using ExpenseService.Domain.Interfaces;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class CalculateDAHandler : IRequestHandler<CalculateDACommand, DaSummaryDto>
{
    private readonly IDaSummaryRepository _summaryRepo;
    private readonly IDapperExpenseQuery _dapperQuery;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CalculateDAHandler(
        IDaSummaryRepository summaryRepo,
        IDapperExpenseQuery dapperQuery,
        IMapper mapper,
        IMediator mediator)
    {
        _summaryRepo = summaryRepo;
        _dapperQuery = dapperQuery;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<DaSummaryDto> Handle(CalculateDACommand request, CancellationToken cancellationToken)
    {
        var noOfDays = (request.ToDate - request.FromDate).Days + 1;

        var dailyRate = await _dapperQuery.GetDARateAsync(
            request.GradeCode, request.ArrangementType, request.FromDate, request.ToDate);

        if (dailyRate == null)
            throw new InvalidOperationException("No DA rate found for specified criteria.");

        var totalAmount = noOfDays * dailyRate.Value;
        var isAdmin = request.ArrangementType == "A";

        var summary = new DaSummary
        {
            RequestId = request.RequestNumber,
            AdminHours = isAdmin ? noOfDays * 24 : 0,
            AdminDays = isAdmin ? noOfDays : 0,
            AdminRate = isAdmin ? dailyRate.Value : 0,
            AdminAmount = isAdmin ? totalAmount : 0,
            SelfHours = !isAdmin ? noOfDays * 24 : 0,
            SelfDays = !isAdmin ? noOfDays : 0,
            SelfRate = !isAdmin ? dailyRate.Value : 0,
            SelfAmount = !isAdmin ? totalAmount : 0
        };

        var created = await _summaryRepo.AddAsync(summary, cancellationToken);

        await _mediator.Publish(new DACalculatedEvent(
            request.RequestNumber, totalAmount), cancellationToken);

        return _mapper.Map<DaSummaryDto>(created);
    }
}
