using AuditService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditService.Infrastructure.Data.Configurations;

public class AuditObservationConfiguration : IEntityTypeConfiguration<AuditObservation>
{
    public void Configure(EntityTypeBuilder<AuditObservation> builder)
    {
        builder.ToTable("AUDIT_OBSERVATION");
        builder.HasKey(x => x.ObvId);
        builder.Property(x => x.ObvId).HasColumnName("OBV_ID").ValueGeneratedNever();
        builder.Property(x => x.ObvAuditId).HasColumnName("OBV_AUDITID").IsRequired();
        builder.Property(x => x.ObvTitle).HasColumnName("OBV_TITLE").HasMaxLength(200).IsRequired();
        builder.Property(x => x.ObvDescription).HasColumnName("OBV_DESCRIPTION").HasMaxLength(2000).IsRequired();
        builder.Property(x => x.ObvRisk).HasColumnName("OBV_RISK").HasMaxLength(1).IsRequired();
        builder.Property(x => x.ObvAuditee).HasColumnName("OBV_AUDITEE").IsRequired();
        builder.Property(x => x.ObvEsc1).HasColumnName("OBV_ESC1").IsRequired();
        builder.Property(x => x.ObvEsc2).HasColumnName("OBV_ESC2").IsRequired();
        builder.Property(x => x.ObvManComments).HasColumnName("OBV_MANCOMMENTS").HasMaxLength(2000).IsRequired();
        builder.Property(x => x.ObvImplication).HasColumnName("OBV_IMPLICATION").HasMaxLength(2000);
        builder.Property(x => x.ObvStatus).HasColumnName("OBV_STATUS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.ObvOrgDueDate).HasColumnName("OBV_ORGDUEDATE").IsRequired();
        builder.Property(x => x.ObvOrgRev1Date).HasColumnName("OBV_ORGREV1DATE");
        builder.Property(x => x.ObvOrgRev2Date).HasColumnName("OBV_ORGREV2DATE");
        builder.Property(x => x.ObvDelay1Remarks).HasColumnName("OBV_DELAY1REMARKS").HasMaxLength(2000);
        builder.Property(x => x.ObvDelay2Remarks).HasColumnName("OBV_DELAY2REMARKS").HasMaxLength(2000);
        builder.Property(x => x.ObvCreatedBy).HasColumnName("OBV_CREATEDBY").IsRequired();
        builder.Property(x => x.ObvCreatedOn).HasColumnName("OBV_CREATEDON").IsRequired();
        builder.Property(x => x.ObvModifiedBy).HasColumnName("OBV_MODIFIEDBY").IsRequired();
        builder.Property(x => x.ObvModifiedOn).HasColumnName("OBV_MODIFIEDON").IsRequired();
        builder.Property(x => x.ObvCompletedOn).HasColumnName("OBV_COMPLETEDON").IsRequired();
        builder.Property(x => x.ObvLocation).HasColumnName("OBV_LOCATION").HasMaxLength(200).IsRequired();
        builder.Property(x => x.ObvAuditorName).HasColumnName("OBV_AUDITORNAME").HasMaxLength(200).IsRequired();
        builder.Property(x => x.ObvRemarks).HasColumnName("OBV_REMARKS").HasMaxLength(200).IsRequired();
        builder.Property(x => x.ObvAppStatus).HasColumnName("OBV_APPSTATUS").HasMaxLength(1);
        builder.Property(x => x.ObvEntryStatus).HasColumnName("OBV_ENTRYSTATUS").HasMaxLength(1);
        builder.Property(x => x.ObvRepeatFlag).HasColumnName("OBV_REPEATFLAG").HasMaxLength(1);
        builder.Property(x => x.ObvDupFlag).HasColumnName("OBV_DUPFLAG").HasMaxLength(1);
        builder.Property(x => x.ObvProcess).HasColumnName("OBV_PROCESS").HasColumnType("decimal(38,0)");

        builder.Ignore(x => x.DomainEvents);
    }
}
