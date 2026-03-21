using AutoMapper;
using FinanceService.Application.Common.Exceptions;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Application.Features.Invoices.Queries.GetInvoiceById;

public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDto>
{
    private readonly IFinanceDbContext _context;
    private readonly IMapper _mapper;

    public GetInvoiceByIdQueryHandler(IFinanceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _context.ApInvoices
            .Include(i => i.InvoiceLines)
            .FirstOrDefaultAsync(i => i.InvoiceId == request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.ApInvoice), request.InvoiceId);

        return _mapper.Map<InvoiceDto>(invoice);
    }
}
