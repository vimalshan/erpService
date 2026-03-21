using ArchiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchiveService.Infrastructure.Persistence.Configurations;

public class ArchivedServiceOrderConfiguration : IEntityTypeConfiguration<ArchivedServiceOrder>
{
    public void Configure(EntityTypeBuilder<ArchivedServiceOrder> builder)
    {
        builder.ToTable("OLD_SERVICE_ORDER_HDR");

        builder.HasKey(e => e.SernoDell);
        builder.Property(e => e.SernoDell).HasColumnName("SERNO_DELL").HasMaxLength(12).IsRequired();
        builder.Ignore(e => e.Id);

        builder.HasIndex(e => e.SapId).IsUnique().HasDatabaseName("IX_SERVICE_ORDER_HDR");

        builder.Property(e => e.Branch).HasColumnName("BRANCH").HasMaxLength(15);
        builder.Property(e => e.SapLogin).HasColumnName("SAP_LOGIN").HasMaxLength(15);
        builder.Property(e => e.PostingDate).HasColumnName("POSTING_DATE");
        builder.Property(e => e.SapId).HasColumnName("SAP_ID").HasMaxLength(12);
        builder.Property(e => e.Sla).HasColumnName("SLA").HasMaxLength(15);
        builder.Property(e => e.ProductId).HasColumnName("PRODUCT_ID").HasMaxLength(50);
        builder.Property(e => e.ServiceTag).HasColumnName("SERVICE_TAG").HasMaxLength(25);
        builder.Property(e => e.RelatedCase).HasColumnName("RELATED_CASE").HasMaxLength(25);
        builder.Property(e => e.Lob).HasColumnName("LOB").HasMaxLength(25);
        builder.Property(e => e.CallStatus).HasColumnName("CALL_STATUS").HasMaxLength(50);
        builder.Property(e => e.CurrentRc).HasColumnName("CURRENT_RC").HasMaxLength(25);

        builder.OwnsOne(e => e.Engineer, eng =>
        {
            eng.Property(p => p.EngineerId).HasColumnName("ENGINEER_ID").HasMaxLength(15);
            eng.Property(p => p.EngineerName).HasColumnName("ENGINEER_NAME").HasMaxLength(50);
            eng.Property(p => p.MobileNo).HasColumnName("ENGMOB_NO").HasMaxLength(15);
        });

        builder.Property(e => e.OrgName).HasColumnName("ORG_NAME").HasMaxLength(50);
        builder.Property(e => e.CustomerName).HasColumnName("CUSTOMER_NAME").HasMaxLength(25);

        builder.OwnsOne(e => e.Contact, c =>
        {
            c.Property(p => p.ContactNo).HasColumnName("CONTACT_NO").HasMaxLength(15);
            c.Property(p => p.AltContactNo).HasColumnName("ALT_CNTNO").HasMaxLength(15);
        });

        builder.OwnsOne(e => e.Address, a =>
        {
            a.Property(p => p.FullAddress).HasColumnName("ADDRESS").HasMaxLength(256);
        });

        builder.Property(e => e.DispatchDate).HasColumnName("DISPATCH_DATE");
        builder.Property(e => e.CustEtaDate).HasColumnName("CUSTETA_DATE");
        builder.Property(e => e.PartEtaDate).HasColumnName("PARTETA_DATE");
        builder.Property(e => e.TechSupName).HasColumnName("TECH_SUPNAME").HasMaxLength(50);
        builder.Property(e => e.Dsp).HasColumnName("DSP").HasMaxLength(100);
        builder.Property(e => e.ProblemDescription).HasColumnName("PRB_DESC").HasMaxLength(250);
        builder.Property(e => e.LongDescription).HasColumnName("LONG_DESC").HasMaxLength(4000);
        builder.Property(e => e.ReasonCode).HasColumnName("REASON_CODE").HasMaxLength(15);
        builder.Property(e => e.Activity).HasColumnName("ACTIVITY").HasMaxLength(100);
        builder.Property(e => e.OnsiteDate).HasColumnName("ONSITE_DT");
        builder.Property(e => e.CompletedDate).HasColumnName("CMPLTD_DT");
        builder.Property(e => e.Flag).HasColumnName("FLAG").HasMaxLength(5);
        builder.Property(e => e.EnteredOn).HasColumnName("ENTERED_ON");
        builder.Property(e => e.EnteredBy).HasColumnName("ENTERED_BY").HasMaxLength(15);
        builder.Property(e => e.ChangedOn).HasColumnName("CHANGED_ON");
        builder.Property(e => e.ChangedBy).HasColumnName("CHANGED_BY").HasMaxLength(15);

        builder.HasMany(e => e.Details)
            .WithOne()
            .HasForeignKey(d => d.SernoDell)
            .HasPrincipalKey(e => e.SernoDell);
    }
}
