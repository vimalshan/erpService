using AutoMapper;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using FinanceService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Features.Payments.Commands.ProcessPayment;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentDto>
{
    private readonly IFinanceDbContext _context;
    private readonly IMapper _mapper;

    public ProcessPaymentCommandHandler(IFinanceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaymentDto> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var maxTrnNum = await _context.TravelAccounts
            .MaxAsync(a => (long?)a.TransactionNumber, cancellationToken) ?? 0;

        var account = TravelAccount.CreatePayment(
            maxTrnNum + 1,
            "001",
            request.PaymentAmount,
            (long)request.BatchNumber);

        _context.TravelAccounts.Add(account);

        var batch = await _context.TravelBatchMains
            .FirstOrDefaultAsync(b => b.BatchNumber == request.BatchNumber, cancellationToken);

        if (batch != null)
            batch.MarkPaymentInProgress();

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PaymentDto>(account);
    }
}
