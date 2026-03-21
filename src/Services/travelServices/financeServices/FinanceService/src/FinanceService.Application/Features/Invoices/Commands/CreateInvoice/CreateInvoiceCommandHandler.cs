using AutoMapper;
using FinanceService.Application.Common.Interfaces;
using FinanceService.Application.DTOs;
using FinanceService.Domain.Entities;
using FinanceService.Domain.Events;
using MediatR;

namespace FinanceService.Application.Features.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, InvoiceDto>
{
    private readonly IFinanceDbContext _context;
    private readonly IMapper _mapper;

    public CreateInvoiceCommandHandler(IFinanceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InvoiceDto> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = new ApInvoice
        {
            InvoiceNum = request.InvoiceNum,
            InvoiceTypeLookupCode = request.InvoiceTypeLookupCode,
            InvoiceDate = request.InvoiceDate,
            VendorId = request.VendorId,
            VendorSiteId = request.VendorSiteId,
            InvoiceAmount = request.InvoiceAmount,
            InvoiceCurrencyCode = request.InvoiceCurrencyCode,
            Description = request.Description,
            OrgId = request.OrgId,
            AgencyId = request.AgencyId,
            CreationDate = DateTime.UtcNow.ToString("o"),
            Status = "N"
        };

        long lineNumber = 1;
        foreach (var line in request.Lines)
        {
            invoice.InvoiceLines.Add(new ApInvoiceLine
            {
                LineNumber = lineNumber++,
                LineTypeLookupCode = line.LineTypeLookupCode,
                Amount = line.Amount,
                AccountingDate = DateTime.UtcNow,
                Description = line.Description,
                AccountCode = line.AccountCode,
                ProjectCode = line.ProjectCode,
                SgstAmt = line.SgstAmt,
                CgstAmt = line.CgstAmt,
                IgstAmt = line.IgstAmt,
                CreationDate = DateTime.UtcNow
            });
        }

        _context.ApInvoices.Add(invoice);
        await _context.SaveChangesAsync(cancellationToken);

        invoice.AddDomainEvent(new InvoiceCreatedEvent(invoice.InvoiceId, invoice.InvoiceNum));

        return _mapper.Map<InvoiceDto>(invoice);
    }
}
