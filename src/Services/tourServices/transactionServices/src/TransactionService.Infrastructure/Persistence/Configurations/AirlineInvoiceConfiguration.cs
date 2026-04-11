using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class AirlineInvoiceConfiguration : IEntityTypeConfiguration<AirlineInvoice>
{
    public void Configure(EntityTypeBuilder<AirlineInvoice> builder)
    {
        builder.ToTable("TICKET_AIRLINEINOVICE");
        builder.HasKey(x => x.AirTicketId);

        builder.Property(x => x.AirTicketId).HasColumnName("AIRTICKET_ID").HasMaxLength(255).ValueGeneratedNever();
        builder.Property(x => x.BookCnfId).HasColumnName("AIRTICKET_BOOKCNFID").HasMaxLength(255);
        builder.Property(x => x.TicketNumber).HasColumnName("AIRTICKET_TICKETNUMBER").HasMaxLength(255);
        builder.Property(x => x.PnrNumber).HasColumnName("AIRTICKET_TICKETPNRNUM").HasMaxLength(255);
        builder.Property(x => x.AirlineVendorId).HasColumnName("AIRTICKET_AIRLINEVNDID").HasMaxLength(255);
        builder.Property(x => x.EntryDate).HasColumnName("AIRTICKET_ENTRYDATE");
        builder.Property(x => x.EnteredBy).HasColumnName("AIRTICKET_ENTREDBY").HasMaxLength(255);
        builder.Property(x => x.InvoiceNumber).HasColumnName("AIRTICKET_INVOICENUM").HasMaxLength(255);
        builder.Property(x => x.InvoiceDate).HasColumnName("AIRTICKET_INVOICEDATE");
        builder.Property(x => x.InvoiceCost).HasColumnName("AIRTICKET_INVOICECOST").HasMaxLength(255);
        builder.Property(x => x.Cgst).HasColumnName("AIRTICKET_CGST").HasMaxLength(255);
        builder.Property(x => x.Sgst).HasColumnName("AIRTICKET_SGST").HasMaxLength(255);
        builder.Property(x => x.Igst).HasColumnName("AIRTICKET_IGST").HasMaxLength(255);
        builder.Property(x => x.DebitCredit).HasColumnName("AIRTICKET_DEBITCREDIT").HasMaxLength(255);
        builder.Property(x => x.RefAddCost).HasColumnName("AIRTICKET_REFADDCOST").HasMaxLength(255);
        builder.Property(x => x.RefChrTax).HasColumnName("AIRTICKET_REFCHRTAX").HasMaxLength(255);
        builder.Property(x => x.VendorGstNumber).HasColumnName("AIRTICKET_VNDGSTNUM").HasMaxLength(255);
        builder.Property(x => x.VendorAttachment).HasColumnName("AIRTICKET_VNDATTACHMENT").HasMaxLength(255);

        builder.Ignore(x => x.DomainEvents);
    }
}
