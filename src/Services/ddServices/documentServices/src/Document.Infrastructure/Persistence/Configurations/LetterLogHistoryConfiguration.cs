using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Document.Domain.Entities;

namespace Document.Infrastructure.Persistence.Configurations;

public class LetterLogHistoryConfiguration : IEntityTypeConfiguration<LetterLogHistory>
{
    public void Configure(EntityTypeBuilder<LetterLogHistory> builder)
    {
        builder.ToTable("DDLETTER_LOGHISTORY");
        builder.HasNoKey();
        builder.Property(l => l.LogSysId).HasColumnName("DDLETTER_LOGSYSID").HasColumnType("decimal(38,0)");
        builder.Property(l => l.IpAddress).HasColumnName("DDLETTER_IPADDRESS").HasMaxLength(100).IsRequired();
        builder.Property(l => l.OpenedOn).HasColumnName("DDLETTER_OPENEDON");
        builder.Property(l => l.FinancialYearId).HasColumnName("DDLETTER_FINYEARID").HasColumnType("decimal(38,0)");
        builder.Property(l => l.EmployeeSysId).HasColumnName("DDLETTER_EMPSYSID").HasColumnType("decimal(38,0)");
        builder.Property(l => l.LetterType).HasColumnName("DDLETTER_TYPE").HasMaxLength(3);
        builder.Ignore(l => l.DomainEvents);
    }
}
