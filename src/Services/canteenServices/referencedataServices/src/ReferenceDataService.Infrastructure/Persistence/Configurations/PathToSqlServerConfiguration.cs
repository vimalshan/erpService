using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Infrastructure.Persistence.Configurations;

public class PathToSqlServerConfiguration : IEntityTypeConfiguration<PathToSqlServer>
{
    public void Configure(EntityTypeBuilder<PathToSqlServer> builder)
    {
        builder.ToTable("PATHTOSQLSERVER");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CompanyCode)
            .HasColumnName("COM_COD")
            .HasColumnType("char(3)");

        builder.Property(e => e.ServerName)
            .HasColumnName("SERVER_NAME")
            .HasColumnType("varchar(20)");

        builder.Property(e => e.DatabaseName)
            .HasColumnName("DATABASE_NAME")
            .HasColumnType("varchar(20)");

        builder.Property(e => e.UserId)
            .HasColumnName("USER_ID")
            .HasColumnType("varchar(10)");

        builder.Property(e => e.DbPassword)
            .HasColumnName("DBPASSWORD")
            .HasColumnType("varchar(10)");

        builder.Ignore(e => e.DomainEvents);
    }
}
