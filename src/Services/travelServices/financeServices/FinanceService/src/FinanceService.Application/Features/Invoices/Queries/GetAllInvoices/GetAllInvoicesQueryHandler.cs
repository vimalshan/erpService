using AutoMapper;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Features.Invoices.Queries.GetAllInvoices;

public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, List<InvoiceDto>>
{
    private readonly IFinanceDbContext _context;
    private readonly IMapper _mapper;

    public GetAllInvoicesQueryHandler(IFinanceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<InvoiceDto>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
    {
        var invoices = await _context.ApInvoices
            .Include(i => i.InvoiceLines)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<InvoiceDto>>(invoices);
    }
}
