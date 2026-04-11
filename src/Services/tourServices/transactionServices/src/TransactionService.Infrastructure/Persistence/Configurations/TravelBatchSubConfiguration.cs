using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Persistence.Configurations;

public sealed class TravelBatchSubConfiguration : IEntityTypeConfiguration<TravelBatchSub>
{
    public void Configure(EntityTypeBuilder<TravelBatchSub> builder)
    {
        builder.ToTable("TRAVEL_BATCHSUB");
        builder.HasKey(x => x.BatchSubId);

        builder.Property(x => x.BatchSubId).HasColumnName("BATCHSUB_ID").HasMaxLength(255).ValueGeneratedNever();
        builder.Property(x => x.BatchId).HasColumnName("BATCHSUB_BATCHID").HasMaxLength(255);
        builder.Property(x => x.BookCnfId).HasColumnName("BATCHSUB_BOOKCNFID").HasMaxLength(255);
        builder.Property(x => x.BookNo).HasColumnName("BATCHSUB_BOOKNO").HasMaxLength(255);
        builder.Property(x => x.BasAmt).HasColumnName("BATCHSUB_BASAMT").HasMaxLength(255);
        builder.Property(x => x.AdjAmt).HasColumnName("BATCHSUB_ADJAMT").HasMaxLength(255);
        builder.Property(x => x.TotAmt).HasColumnName("BATCHSUB_TOTAMT").HasMaxLength(255);
        builder.Property(x => x.AppAmt).HasColumnName("BATCHSUB_APPAMT").HasMaxLength(255);
        builder.Property(x => x.SerTax).HasColumnName("BATCHSUB_SERTAX").HasMaxLength(255);
        builder.Property(x => x.CesTax).HasColumnName("BATCHSUB_CESTAX").HasMaxLength(255);
        builder.Property(x => x.AdlTax).HasColumnName("BATCHSUB_ADLTAX").HasMaxLength(255);
        builder.Property(x => x.TotPay).HasColumnName("BATCHSUB_TOTPAY").HasMaxLength(255);
        builder.Property(x => x.RefDet).HasColumnName("BATCHSUB_REFDET").HasMaxLength(255);
        builder.Property(x => x.VenRemarks).HasColumnName("BATCHSUB_VENREMARKS").HasMaxLength(255);
        builder.Property(x => x.CreditType).HasColumnName("BATCHSUB_CREDITTYPE").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AdmRemarks).HasColumnName("BATCHSUB_ADMREMARKS").HasMaxLength(255);
        builder.Property(x => x.TktReference).HasColumnName("BATCHSUB_TKTREFERENCE").HasMaxLength(255);
        builder.Property(x => x.TpId).HasColumnName("BATCHSUB_TPID").HasMaxLength(255);
        builder.Property(x => x.ForReqId).HasColumnName("BATCHSUB_FORREQID").HasMaxLength(255);
        builder.Property(x => x.HigCes).HasColumnName("BATCHSUB_HIGCES").HasMaxLength(255);
        builder.Property(x => x.RndOff).HasColumnName("BATCHSUB_RNDOFF").HasMaxLength(255);
        builder.Property(x => x.SurTax).HasColumnName("BATCHSUB_SURTAX").HasMaxLength(255);
        builder.Property(x => x.ChrTax).HasColumnName("BATCHSUB_CHRTAX").HasMaxLength(255);
        builder.Property(x => x.InvNum).HasColumnName("BATCHSUB_INVNUM").HasMaxLength(255);
        builder.Property(x => x.InvDate).HasColumnName("BATCHSUB_INVDATE");
        builder.Property(x => x.R12LocId).HasColumnName("BATCHSUB_R12LOCID").HasMaxLength(255);
        builder.Property(x => x.CgstBas).HasColumnName("BATCHSUB_CGSTBAS").HasMaxLength(255);
        builder.Property(x => x.SgstBas).HasColumnName("BATCHSUB_SGSTBAS").HasMaxLength(255);
        builder.Property(x => x.TravelClass).HasColumnName("BATCHSUB_TRVELCLASS").HasMaxLength(255);
        builder.Property(x => x.IgstBas).HasColumnName("BATCHSUB_IGSTBAS").HasMaxLength(255);
        builder.Property(x => x.CgstMgt).HasColumnName("BATCHSUB_CGSTMGT").HasMaxLength(255);
        builder.Property(x => x.SgstMgt).HasColumnName("BATCHSUB_SGSTMGT").HasMaxLength(255);
        builder.Property(x => x.IgstMgt).HasColumnName("BATCHSUB_IGSTMGT").HasMaxLength(255);
        builder.Property(x => x.R12Bu).HasColumnName("BATCHSUB_R12BU").HasMaxLength(255);
        builder.Property(x => x.TaxBasic).HasColumnName("BATCHSUB_TAXBASIC").HasMaxLength(255);
        builder.Property(x => x.VendorId).HasColumnName("BATCHSUB_VENDORID").HasMaxLength(255);

        builder.Ignore(x => x.DomainEvents);
    }
}
