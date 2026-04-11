using TransactionService.Domain.Common;
using TransactionService.Domain.Events;

namespace TransactionService.Domain.Entities;

/// <summary>
/// Maps to TICKET_AIRLINEINOVICE - Airline ticket invoicing
/// </summary>
public sealed class AirlineInvoice : BaseEntity
{
    private AirlineInvoice() { }

    public string AirTicketId { get; private set; } = default!;
    public string? BookCnfId { get; private set; }
    public string? TicketNumber { get; private set; }
    public string? PnrNumber { get; private set; }
    public string? AirlineVendorId { get; private set; }
    public DateTime? EntryDate { get; private set; }
    public string? EnteredBy { get; private set; }
    public string? InvoiceNumber { get; private set; }
    public DateTime? InvoiceDate { get; private set; }
    public string? InvoiceCost { get; private set; }
    public string? Cgst { get; private set; }
    public string? Sgst { get; private set; }
    public string? Igst { get; private set; }
    public string? DebitCredit { get; private set; }
    public string? RefAddCost { get; private set; }
    public string? RefChrTax { get; private set; }
    public string? VendorGstNumber { get; private set; }
    public string? VendorAttachment { get; private set; }

    public static AirlineInvoice Create(
        string airTicketId, string bookCnfId, string ticketNumber,
        string? pnrNumber, string airlineVendorId, string invoiceNumber,
        DateTime invoiceDate, string invoiceCost, string enteredBy,
        string? debitCredit = "D")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(airTicketId);

        var invoice = new AirlineInvoice
        {
            AirTicketId = airTicketId,
            BookCnfId = bookCnfId,
            TicketNumber = ticketNumber,
            PnrNumber = pnrNumber,
            AirlineVendorId = airlineVendorId,
            InvoiceNumber = invoiceNumber,
            InvoiceDate = invoiceDate,
            InvoiceCost = invoiceCost,
            EnteredBy = enteredBy,
            EntryDate = DateTime.UtcNow,
            DebitCredit = debitCredit
        };

        invoice.RaiseDomainEvent(new AirlineInvoiceCreatedEvent(
            Guid.NewGuid(), airTicketId, bookCnfId, invoiceNumber, DateTime.UtcNow));

        return invoice;
    }

    public void SetAttachment(string attachment) => VendorAttachment = attachment;

    public void SetGstDetails(string? cgst, string? sgst, string? igst, string? vendorGstNumber)
    {
        Cgst = cgst;
        Sgst = sgst;
        Igst = igst;
        VendorGstNumber = vendorGstNumber;
    }
}
