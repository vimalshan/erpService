using MemberService.Domain.Entities;
using MemberService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemberService.Infrastructure.Data.Configurations;

public class MemberNomineeConfiguration : IEntityTypeConfiguration<MemberNominee>
{
    public void Configure(EntityTypeBuilder<MemberNominee> builder)
    {
        builder.ToTable("MEMBER_NOMINEE");
        builder.HasKey(n => new { n.MemberNo, n.SerialNo, n.FundType });

        builder.Property(n => n.MemberNo).HasColumnName("NOMINEE_MEMBER_NO").HasColumnType("BIGINT").IsRequired();
        builder.Property(n => n.SerialNo).HasColumnName("NOMINEE_SERIAL_NO").IsRequired();
        builder.Property(n => n.FundType).HasColumnName("NOMINEE_FUND_TYPE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(n => n.NomineeName).HasColumnName("NOMINEE_NAME").HasMaxLength(65).IsRequired();
        builder.Property(n => n.RelationshipCode).HasColumnName("NOMINEE_RELATIONSHIP_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(n => n.Percentage).HasColumnName("NOMINEE_PERCENTAGE").IsRequired();
        builder.Property(n => n.DateOfBirth).HasColumnName("NOMINEE_DOB").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(n => n.AddressLine1).HasColumnName("NOMINEE_ADDRESS_LINE_1").HasMaxLength(200);
        builder.Property(n => n.AddressLine2).HasColumnName("NOMINEE_ADDRESS_LINE_2").HasMaxLength(200);
        builder.Property(n => n.AddressLine3).HasColumnName("NOMINEE_ADDRESS_LINE_3").HasMaxLength(200);
        builder.Property(n => n.PhoneNo).HasColumnName("NOMINEE_PHONE_NO").HasMaxLength(20);
        builder.Property(n => n.Email).HasColumnName("NOMINEE_EMAIL").HasMaxLength(100);
        builder.Property(n => n.EffectiveDate).HasColumnName("NOMINEE_EFF_DATE").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(n => n.ClosureDate).HasColumnName("NOMINEE_CLS_DATE").HasColumnType("DATETIME2(3)");
        builder.Property(n => n.IsMinor).HasColumnName("NOMINEE_MINOR_FLAG")
            .HasConversion(b => b ? "Y" : "N", s => s == "Y");
        builder.Property(n => n.TrustCode).HasColumnName("NOMINEE_TRUST_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(n => n.Status)
            .HasColumnName("NOMINEE_STATUS")
            .HasColumnType("CHAR(1)")
            .IsRequired()
            .HasConversion(s => ((char)s).ToString(), s => (NomineeStatus)s[0]);

        builder.Ignore(n => n.DomainEvents);
        builder.Ignore(n => n.Guardian);

        builder.HasIndex(n => new { n.MemberNo, n.EffectiveDate, n.Status }).HasDatabaseName("IDX_MEMBER_NOMINEE_MEMBER");
    }
}

public class MemberPayrollConfiguration : IEntityTypeConfiguration<MemberPayroll>
{
    public void Configure(EntityTypeBuilder<MemberPayroll> builder)
    {
        builder.ToTable("MEMBER_PAYROLL");
        builder.HasKey(p => new { p.MemberNo, p.UnitCode });

        builder.Property(p => p.MemberNo).HasColumnName("PAYROLL_MEMBER_NO").IsRequired();
        builder.Property(p => p.UnitCode).HasColumnName("PAYROLL_UNT_COD").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(p => p.EmployeeNo).HasColumnName("PAYROLL_EMP_NUM").IsRequired();
        builder.Property(p => p.EffectiveDate).HasColumnName("PAYROLL_EFF_DATE").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(p => p.ClosureDate).HasColumnName("PAYROLL_CLS_DATE").HasColumnType("DATETIME2(3)");
        builder.Property(p => p.Status)
            .HasColumnName("PAYROLL_STATUS")
            .HasColumnType("CHAR(1)")
            .IsRequired()
            .HasConversion(s => ((char)s).ToString(), s => (PayrollStatus)s[0]);

        builder.Ignore(p => p.DomainEvents);

        builder.HasIndex(p => new { p.MemberNo, p.Status }).HasDatabaseName("IDX_MEMBER_PAYROLL_STATUS");
    }
}

public class NomineeGuardianConfiguration : IEntityTypeConfiguration<NomineeGuardian>
{
    public void Configure(EntityTypeBuilder<NomineeGuardian> builder)
    {
        builder.ToTable("NOMINEE_GAURDIAN");
        builder.HasKey(g => new { g.TrustCode, g.NomineeMemberNo, g.NomineeSerialNo });

        builder.Property(g => g.TrustCode).HasColumnName("GN_TRUST_CODE").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(g => g.NomineeMemberNo).HasColumnName("GN_NOMINEE_MEMBER_NO").IsRequired();
        builder.Property(g => g.NomineeSerialNo).HasColumnName("GN_NOMINEE_SERIAL_NO").IsRequired();
        builder.Property(g => g.GuardianName).HasColumnName("GAURDIAN_NAME").HasMaxLength(65).IsRequired();
        builder.Property(g => g.AddressLine1).HasColumnName("GN_ADDRESS_LINE1").HasMaxLength(200);
        builder.Property(g => g.AddressLine2).HasColumnName("GN_ADDRESS_LINE2").HasMaxLength(200);
        builder.Property(g => g.AddressLine3).HasColumnName("GN_ADDRESS_LINE3").HasMaxLength(200);
        builder.Property(g => g.AddressLine4).HasColumnName("GN_ADDRESS_LINE4").HasMaxLength(200);
        builder.Property(g => g.PhoneNo).HasColumnName("GN_PHONE_NO").HasMaxLength(20);
        builder.Property(g => g.Email).HasColumnName("GN_EMAIL").HasMaxLength(100);
        builder.Property(g => g.EffectiveDate).HasColumnName("GN_EFF_DATE").HasColumnType("DATETIME2(3)");
        builder.Property(g => g.ClosureDate).HasColumnName("GN_CLS_DATE").HasColumnType("DATETIME2(3)");
        builder.Property(g => g.GuardianRelationship).HasColumnName("GAURDIAN_RELATIONSHIP").HasColumnType("CHAR(3)").IsRequired();
        builder.Property(g => g.Status).HasColumnName("GN_STATUS").HasColumnType("CHAR(1)").HasDefaultValue('A');

        builder.Ignore(g => g.DomainEvents);
    }
}

public class MemberContactConfiguration : IEntityTypeConfiguration<MemberContact>
{
    public void Configure(EntityTypeBuilder<MemberContact> builder)
    {
        builder.ToTable("MEMBER_CONTACT");
        builder.HasKey(c => c.ContactId);
        builder.Property(c => c.ContactId).HasColumnName("CONTACT_ID").UseIdentityColumn();

        builder.Property(c => c.MemberNo).HasColumnName("MEMBER_NO").IsRequired();
        builder.Property(c => c.ContactType)
            .HasColumnName("CONTACT_TYPE")
            .HasColumnType("CHAR(1)")
            .HasConversion(t => ((char)t).ToString(), s => (Domain.Enums.ContactType)s[0]);
        builder.Property(c => c.AddressLine1).HasColumnName("ADDRESS_LINE_1").HasMaxLength(200).IsRequired();
        builder.Property(c => c.AddressLine2).HasColumnName("ADDRESS_LINE_2").HasMaxLength(200);
        builder.Property(c => c.AddressLine3).HasColumnName("ADDRESS_LINE_3").HasMaxLength(200);
        builder.Property(c => c.City).HasColumnName("CITY").HasMaxLength(50).IsRequired();
        builder.Property(c => c.State).HasColumnName("STATE").HasMaxLength(50).IsRequired();
        builder.Property(c => c.PinCode).HasColumnName("PIN_CODE").HasMaxLength(10).IsRequired();
        builder.Property(c => c.Country).HasColumnName("COUNTRY").HasMaxLength(50).IsRequired();
        builder.Property(c => c.PhoneNo).HasColumnName("PHONE_NO").HasMaxLength(20);
        builder.Property(c => c.Email).HasColumnName("EMAIL").HasMaxLength(100);
        builder.Property(c => c.EffectiveDate).HasColumnName("EFF_DATE").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(c => c.ClosureDate).HasColumnName("CLS_DATE").HasColumnType("DATETIME2(3)");

        builder.Ignore(c => c.DomainEvents);

        builder.HasIndex(c => new { c.MemberNo, c.ContactType }).HasDatabaseName("IDX_MEMBER_CONTACT_MEMBER");
    }
}

public class MemberAuditLogConfiguration : IEntityTypeConfiguration<MemberAuditLog>
{
    public void Configure(EntityTypeBuilder<MemberAuditLog> builder)
    {
        builder.ToTable("MEMBER_AUDIT_LOG");
        builder.HasKey(a => a.AuditId);
        builder.Property(a => a.AuditId).HasColumnName("AUDIT_ID").UseIdentityColumn();
        builder.Property(a => a.MemberNo).HasColumnName("MEMBER_NO").IsRequired();
        builder.Property(a => a.AuditAction).HasColumnName("AUDIT_ACTION").HasMaxLength(50).IsRequired();
        builder.Property(a => a.AuditTimestamp).HasColumnName("AUDIT_TIMESTAMP").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(a => a.AuditUserId).HasColumnName("AUDIT_USER_ID").IsRequired();
        builder.Property(a => a.OldValues).HasColumnName("AUDIT_OLD_VALUES").HasColumnType("VARCHAR(MAX)");
        builder.Property(a => a.NewValues).HasColumnName("AUDIT_NEW_VALUES").HasColumnType("VARCHAR(MAX)");

        builder.HasIndex(a => new { a.MemberNo, a.AuditTimestamp }).HasDatabaseName("IDX_MEMBER_AUDIT_LOG_MEMBER");
    }
}
