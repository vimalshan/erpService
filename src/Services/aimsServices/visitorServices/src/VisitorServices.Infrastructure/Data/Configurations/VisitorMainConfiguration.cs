using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisitorServices.Domain.Aggregates;
using VisitorServices.Domain.Enums;

namespace VisitorServices.Infrastructure.Data.Configurations;

public class VisitorMainConfiguration : IEntityTypeConfiguration<VisitorAggregate>
{
    public void Configure(EntityTypeBuilder<VisitorAggregate> builder)
    {
        builder.ToTable("VISITOR_MAIN");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("VISITOR_ID").ValueGeneratedNever();

        builder.Property(v => v.Name)
            .HasColumnName("VISITOR_NAME")
            .HasMaxLength(255)
            .IsRequired();

        builder.OwnsOne(v => v.IdDocument, id =>
        {
            id.Property(d => d.IdType)
                .HasColumnName("VISITOR_IDTYPE")
                .HasMaxLength(1)
                .IsRequired()
                .HasConversion(
                    v => ((char)(int)v).ToString(),
                    s => (IdType)s[0]);

            id.Property(d => d.IdNumber)
                .HasColumnName("VISITOR_IDNUMBER")
                .HasMaxLength(50);
        });

        builder.OwnsOne(v => v.ContactInfo, ci =>
        {
            ci.Property(c => c.PhoneNumber)
                .HasColumnName("VISITOR_PHONENUMBER")
                .HasMaxLength(20);

            ci.Property(c => c.Email)
                .HasColumnName("VISITOR_EMAIL")
                .HasMaxLength(255);
        });

        builder.Property(v => v.Company)
            .HasColumnName("VISITOR_COMPANY")
            .HasMaxLength(255);

        builder.Property(v => v.Purpose)
            .HasColumnName("VISITOR_PURPOSE")
            .HasMaxLength(500);

        builder.Property(v => v.CheckInTime)
            .HasColumnName("VISITOR_CHECKINTIME")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(v => v.CheckOutTime)
            .HasColumnName("VISITOR_CHECKOUTTIME")
            .HasColumnType("datetime2(3)");

        builder.Property(v => v.Status)
            .HasColumnName("VISITOR_STATUS")
            .HasMaxLength(1)
            .IsRequired()
            .HasConversion(
                v => ((char)(int)v).ToString(),
                s => (VisitorStatus)s[0]);

        builder.Property(v => v.WhomToVisit)
            .HasColumnName("VISITOR_WHOMTOVISIT")
            .IsRequired();

        builder.Property(v => v.EnteredOn)
            .HasColumnName("VISITOR_ENTEREDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(v => v.EnteredBy)
            .HasColumnName("VISITOR_ENTEREDBY")
            .IsRequired();

        builder.Property(v => v.LastModifiedBy)
            .HasColumnName("VISITOR_LASTMODIFIEDBY")
            .IsRequired();

        builder.Property(v => v.LastModifiedOn)
            .HasColumnName("VISITOR_LASTMODIFIEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Ignore(v => v.DomainEvents);

        builder.HasIndex(v => v.CheckInTime).HasDatabaseName("IX_VISITOR_MAIN_CHECKINTIME");
        builder.HasIndex(v => v.Status).HasDatabaseName("IX_VISITOR_MAIN_STATUS");
    }
}
