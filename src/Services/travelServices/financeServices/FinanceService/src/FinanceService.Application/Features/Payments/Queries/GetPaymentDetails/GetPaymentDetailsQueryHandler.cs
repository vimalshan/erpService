using AutoMapper;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Features.Payments.Queries.GetPaymentDetails;

public class GetPaymentDetailsQueryHandler : IRequestHandler<GetPaymentDetailsQuery, List<PaymentDto>>
{
    private readonly IFinanceDbContext _context;
    private readonly IMapper _mapper;

    public GetPaymentDetailsQueryHandler(IFinanceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PaymentDto>> Handle(GetPaymentDetailsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.TravelAccounts.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(request.UnitCode))
            query = query.Where(a => a.UnitCode == request.UnitCode);

        var accounts = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<PaymentDto>>(accounts);
    }
}
