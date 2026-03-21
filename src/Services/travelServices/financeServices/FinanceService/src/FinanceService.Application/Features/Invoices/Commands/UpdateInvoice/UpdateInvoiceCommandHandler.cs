using AutoMapper;
using FinanceService.Application.Common.Exceptions;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using MediatR;

namespace FinanceService.Application.Features.Invoices.Commands.UpdateInvoice;

public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, InvoiceDto>
{
    private readonly IFinanceDbContext _context;
    private readonly IMapper _mapper;

    public UpdateInvoiceCommandHandler(IFinanceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceDto> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _context.ApInvoices.FindAsync(new object[] { request.InvoiceId }, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.ApInvoice), request.InvoiceId);

        if (request.InvoiceNum != null) invoice.InvoiceNum = request.InvoiceNum;
        if (request.InvoiceTypeLookupCode != null) invoice.InvoiceTypeLookupCode = request.InvoiceTypeLookupCode;
        if (request.InvoiceAmount != null) invoice.InvoiceAmount = request.InvoiceAmount;
        if (request.Description != null) invoice.Description = request.Description;
        if (request.Status != null) invoice.Status = request.Status;
        invoice.LastUpdateDate = DateTime.UtcNow.ToString("o");

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<InvoiceDto>(invoice);
    }
}
