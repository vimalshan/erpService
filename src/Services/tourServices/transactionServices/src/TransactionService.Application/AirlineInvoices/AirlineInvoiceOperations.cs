using FluentValidation;
using MediatR;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.AirlineInvoices;

public sealed record CreateAirlineInvoiceCommand : IRequest<AirlineInvoiceDto>
{
    public string AirTicketId { get; init; } = default!;
    public string BookCnfId { get; init; } = default!;
    public string TicketNumber { get; init; } = default!;
    public string? PnrNumber { get; init; }
    public string AirlineVendorId { get; init; } = default!;
    public string InvoiceNumber { get; init; } = default!;
    public DateTime InvoiceDate { get; init; }
    public string InvoiceCost { get; init; } = default!;
    public string EnteredBy { get; init; } = default!;
    public string? DebitCredit { get; init; }
    public string? Cgst { get; init; }
    public string? Sgst { get; init; }
    public string? Igst { get; init; }
    public string? VendorGstNumber { get; init; }
}

public sealed class CreateAirlineInvoiceCommandValidator : AbstractValidator<CreateAirlineInvoiceCommand>
{
    public CreateAirlineInvoiceCommandValidator()
    {
        RuleFor(x => x.AirTicketId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.BookCnfId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TicketNumber).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AirlineVendorId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.InvoiceNumber).NotEmpty().MaximumLength(255);
        RuleFor(x => x.InvoiceCost).NotEmpty();
        RuleFor(x => x.EnteredBy).NotEmpty();
    }
}

public sealed class CreateAirlineInvoiceCommandHandler(
    IAirlineInvoiceRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateAirlineInvoiceCommand, AirlineInvoiceDto>
{
    public async Task<AirlineInvoiceDto> Handle(
        CreateAirlineInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var invoice = AirlineInvoice.Create(
            request.AirTicketId, request.BookCnfId, request.TicketNumber,
            request.PnrNumber, request.AirlineVendorId, request.InvoiceNumber,
            request.InvoiceDate, request.InvoiceCost, request.EnteredBy,
            request.DebitCredit);

        if (request.Cgst != null || request.Sgst != null || request.Igst != null)
            invoice.SetGstDetails(request.Cgst, request.Sgst, request.Igst, request.VendorGstNumber);

        await repository.AddAsync(invoice, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(invoice);
    }

    private static AirlineInvoiceDto MapToDto(AirlineInvoice inv) => new()
    {
        AirTicketId = inv.AirTicketId,
        BookCnfId = inv.BookCnfId,
        TicketNumber = inv.TicketNumber,
        PnrNumber = inv.PnrNumber,
        AirlineVendorId = inv.AirlineVendorId,
        EntryDate = inv.EntryDate,
        InvoiceNumber = inv.InvoiceNumber,
        InvoiceDate = inv.InvoiceDate,
        InvoiceCost = inv.InvoiceCost,
        Cgst = inv.Cgst,
        Sgst = inv.Sgst,
        Igst = inv.Igst,
        DebitCredit = inv.DebitCredit,
        VendorAttachment = inv.VendorAttachment
    };
}

// Queries

public sealed record GetAirlineInvoiceByIdQuery(string AirTicketId) : IRequest<AirlineInvoiceDto>;

public sealed class GetAirlineInvoiceByIdQueryHandler(
    IAirlineInvoiceRepository repository) : IRequestHandler<GetAirlineInvoiceByIdQuery, AirlineInvoiceDto>
{
    public async Task<AirlineInvoiceDto> Handle(GetAirlineInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var inv = await repository.GetByIdAsync(request.AirTicketId, cancellationToken)
            ?? throw new AirlineInvoiceNotFoundException(request.AirTicketId);

        return new AirlineInvoiceDto
        {
            AirTicketId = inv.AirTicketId,
            BookCnfId = inv.BookCnfId,
            TicketNumber = inv.TicketNumber,
            PnrNumber = inv.PnrNumber,
            AirlineVendorId = inv.AirlineVendorId,
            EntryDate = inv.EntryDate,
            InvoiceNumber = inv.InvoiceNumber,
            InvoiceDate = inv.InvoiceDate,
            InvoiceCost = inv.InvoiceCost,
            Cgst = inv.Cgst,
            Sgst = inv.Sgst,
            Igst = inv.Igst,
            DebitCredit = inv.DebitCredit,
            VendorAttachment = inv.VendorAttachment
        };
    }
}

public sealed record GetAirlineInvoicesByBookingQuery(string BookCnfId) : IRequest<IEnumerable<AirlineInvoiceDto>>;

public sealed class GetAirlineInvoicesByBookingQueryHandler(
    IAirlineInvoiceRepository repository) : IRequestHandler<GetAirlineInvoicesByBookingQuery, IEnumerable<AirlineInvoiceDto>>
{
    public async Task<IEnumerable<AirlineInvoiceDto>> Handle(GetAirlineInvoicesByBookingQuery request, CancellationToken cancellationToken)
    {
        var invoices = await repository.GetByBookingConfirmationIdAsync(request.BookCnfId, cancellationToken);

        return invoices.Select(inv => new AirlineInvoiceDto
        {
            AirTicketId = inv.AirTicketId,
            BookCnfId = inv.BookCnfId,
            TicketNumber = inv.TicketNumber,
            PnrNumber = inv.PnrNumber,
            AirlineVendorId = inv.AirlineVendorId,
            EntryDate = inv.EntryDate,
            InvoiceNumber = inv.InvoiceNumber,
            InvoiceDate = inv.InvoiceDate,
            InvoiceCost = inv.InvoiceCost,
            DebitCredit = inv.DebitCredit
        });
    }
}
