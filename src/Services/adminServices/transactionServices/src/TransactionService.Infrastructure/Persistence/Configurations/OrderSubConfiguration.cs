namespace TransactionService.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

public sealed class OrderSubConfiguration : IEntityTypeConfiguration<OrderSub>
{
    public void Configure(EntityTypeBuilder<OrderSub> builder)
    {
        builder.ToTable("SP_ORDER_SUB");
        builder.HasKey(o => o.OrderSubId);
        builder.Property(o => o.OrderSubId).HasColumnName("OS_ORDERSUB_ID").ValueGeneratedNever();
        builder.Property(o => o.OrderMainId).HasColumnName("OS_ORDERMAIN_ID");
        builder.Property(o => o.RequestSubId).HasColumnName("OS_REQUESTSUB_ID");
        builder.Property(o => o.OrderedQty).HasColumnName("OS_ORDERED_QTY");
        builder.Property(o => o.ReceivedOn).HasColumnName("OS_RECEIVEDON");
        builder.Property(o => o.ReceivedBy).HasColumnName("OS_RECEIVED_BY");
        builder.Property(o => o.OrderPrice).HasColumnName("OS_ORDERPRICE");
        builder.Property(o => o.ActualPrice).HasColumnName("OS_ACTUALPRICE");
        builder.Property(o => o.ReceivedDate).HasColumnName("OS_RECEIVEDDATE");
        builder.Property(o => o.DeliveryDate).HasColumnName("OS_DELIVERYDATE");
        builder.Property(o => o.ReceiptEntryBy).HasColumnName("OS_RECEIPTENTRYBY");
        builder.Property(o => o.ReceiptEntryOn).HasColumnName("OS_RECEIPTENTRYON");

        builder.Ignore(o => o.DomainEvents);
    }
}
