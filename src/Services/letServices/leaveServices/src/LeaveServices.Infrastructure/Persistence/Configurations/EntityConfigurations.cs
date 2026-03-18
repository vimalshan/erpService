using LeaveServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveServices.Infrastructure.Persistence.Configurations;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable("LET_MAIN");
        builder.HasKey(x => x.ReqNum);
        builder.Property(x => x.ReqNum).HasColumnName("REQ_NUM").ValueGeneratedNever();
        builder.Property(x => x.FinyearSrlno).HasColumnName("FINYEAR_SRLNO").IsRequired();
        builder.Property(x => x.EmpUserId).HasColumnName("EMP_USERID").HasMaxLength(25).IsRequired();
        builder.Property(x => x.SupUserId).HasColumnName("SUP_USERID").HasMaxLength(25);
        builder.Property(x => x.ReqDate).HasColumnName("REQ_DATE");

        builder.HasMany(x => x.Details)
               .WithOne(d => d.LeaveRequest)
               .HasForeignKey(d => d.LsReqNum)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.DomainEvents);
    }
}

public class LeaveRequestDetailConfiguration : IEntityTypeConfiguration<LeaveRequestDetail>
{
    public void Configure(EntityTypeBuilder<LeaveRequestDetail> builder)
    {
        builder.ToTable("LET_SUB");
        builder.HasKey(x => x.LsSrlNum);
        builder.Property(x => x.LsReqNum).HasColumnName("LS_REQ_NUM");
        builder.Property(x => x.LsSrlNum).HasColumnName("LS_SRL_NUM").ValueGeneratedNever();
        builder.Property(x => x.LsModDat).HasColumnName("LS_MOD_DAT");
        builder.Property(x => x.LsModUser).HasColumnName("LS_MOD_USER").HasMaxLength(25);
        builder.Property(x => x.LsPrefModdev).HasColumnName("LS_PREF_MODDEV").HasMaxLength(1);
        builder.Property(x => x.LsActTaken).HasColumnName("LS_ACT_TAKEN").HasMaxLength(200);
        builder.Property(x => x.LsCrsId).HasColumnName("LS_CRS_ID");
        builder.Property(x => x.LsTrnprgBhr).HasColumnName("LS_TRNPRG_BHR").HasMaxLength(200);
        builder.Property(x => x.LsImpbenPro).HasColumnName("LS_IMPBEN_PRO").HasMaxLength(200);
        builder.Property(x => x.LsMeasureCp).HasColumnName("LS_MEASURE_CP").HasMaxLength(200);
        builder.Property(x => x.LsMidyerRevnam).HasColumnName("LS_MIDYER_REVNAM").HasMaxLength(200);
        builder.Property(x => x.LsMidyerRevdat).HasColumnName("LS_MIDYER_REVDAT").HasMaxLength(200);
        builder.Property(x => x.LsMidyerRevrem).HasColumnName("LS_MIDYER_REVREM").HasMaxLength(200);
        builder.Property(x => x.LsAnnyerRevnam).HasColumnName("LS_ANNYER_REVNAM").HasMaxLength(200);
        builder.Property(x => x.LsAnnyerRevdat).HasColumnName("LS_ANNYER_REVDAT").HasMaxLength(200);
        builder.Property(x => x.LsAnnyerRevrem).HasColumnName("LS_ANNYER_REVREM").HasMaxLength(200);
        builder.Property(x => x.LsCompDev).HasColumnName("LS_COMP_DEV");
        builder.Property(x => x.LsDomknowDev).HasColumnName("LS_DOMKNOW_DEV").HasMaxLength(255);
        builder.Property(x => x.LsDomknowDevDet).HasColumnName("LS_DOMKNOW_DEV_DET").HasMaxLength(255);
        builder.Property(x => x.LsProcesDev).HasColumnName("LS_PROCES_DEV").HasMaxLength(255);
        builder.Property(x => x.LsProcesDevDet).HasColumnName("LS_PROCES_DEV_DET").HasMaxLength(255);
        builder.Property(x => x.LsLetsubCode).HasColumnName("LS_LETSUB_CODE").HasMaxLength(1);
        builder.Property(x => x.LsRevType).HasColumnName("LS_REV_TYPE").HasMaxLength(255);

        builder.Ignore(x => x.DomainEvents);
    }
}

public class LeaveEncashmentConfiguration : IEntityTypeConfiguration<LeaveEncashment>
{
    public void Configure(EntityTypeBuilder<LeaveEncashment> builder)
    {
        builder.ToTable("LEAVE_ENCASHMENT");
        builder.HasKey(x => x.EncashmentId);
        builder.Property(x => x.EncashmentId).HasColumnName("ENCASHMENT_ID").UseIdentityColumn();
        builder.Property(x => x.EmpSysId).HasColumnName("EMP_SYS_ID").IsRequired();
        builder.Property(x => x.LeaveType).HasColumnName("LEAVE_TYPE").HasMaxLength(20).IsRequired();
        builder.Property(x => x.EncashmentDays).HasColumnName("ENCASHMENT_DAYS").IsRequired();
        builder.Property(x => x.EncashmentAmount).HasColumnName("ENCASHMENT_AMOUNT").HasPrecision(19, 2).IsRequired();
        builder.Property(x => x.RequestDate).HasColumnName("REQUEST_DATE").IsRequired();
        builder.Property(x => x.EncashmentStatusCode).HasColumnName("ENCASHMENT_STATUS").HasMaxLength(1)
            .HasDefaultValue('P').IsRequired();
        builder.Ignore(x => x.EncashmentStatus);
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(x => x.ModifiedOn).HasColumnName("MODIFIED_ON");
        builder.Property(x => x.ModifiedBy).HasColumnName("MODIFIED_BY");

        builder.HasIndex(x => x.EmpSysId).HasDatabaseName("IDX_LEAVE_ENCASHMENT_EMP_ID");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class LossOfPayConfiguration : IEntityTypeConfiguration<LossOfPay>
{
    public void Configure(EntityTypeBuilder<LossOfPay> builder)
    {
        builder.ToTable("LOSS_OF_PAY");
        builder.HasKey(x => x.LopId);
        builder.Property(x => x.LopId).HasColumnName("LOP_ID").UseIdentityColumn();
        builder.Property(x => x.EmpSysId).HasColumnName("EMP_SYS_ID").IsRequired();
        builder.Property(x => x.LopDays).HasColumnName("LOP_DAYS").IsRequired();
        builder.Property(x => x.LopMonth).HasColumnName("LOP_MONTH").IsRequired();
        builder.Property(x => x.LopRemarks).HasColumnName("LOP_REMARKS").HasMaxLength(500);
        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CREATED_ON").IsRequired();
        builder.Property(x => x.ModifiedOn).HasColumnName("MODIFIED_ON");
        builder.Property(x => x.ModifiedBy).HasColumnName("MODIFIED_BY");

        builder.HasIndex(x => x.EmpSysId).HasDatabaseName("IDX_LOSS_OF_PAY_EMP_ID");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class LeaveCounterConfiguration : IEntityTypeConfiguration<LeaveCounter>
{
    public void Configure(EntityTypeBuilder<LeaveCounter> builder)
    {
        builder.ToTable("LET_COUNTERS");
        builder.HasKey(x => x.LtTypCod);
        builder.Property(x => x.LtTypCod).HasColumnName("LT_TYP_COD").HasMaxLength(3).IsRequired();
        builder.Property(x => x.LtCntNum).HasColumnName("LT_CNT_NUM");
        builder.Property(x => x.LtCntDes).HasColumnName("LT_CNT_DES").HasMaxLength(65);
        builder.Ignore(x => x.DomainEvents);
    }
}

public class LeaveModelConfiguration : IEntityTypeConfiguration<LeaveModel>
{
    public void Configure(EntityTypeBuilder<LeaveModel> builder)
    {
        builder.ToTable("LET_MODEL");
        builder.HasKey(x => new { x.LtSklCod, x.LtLvlNum });
        builder.Property(x => x.LtSklCod).HasColumnName("LT_SKL_COD");
        builder.Property(x => x.LtLvlNum).HasColumnName("LT_LVL_NUM");
        builder.Property(x => x.LtFncCod).HasColumnName("LT_FNC_COD").HasMaxLength(3).IsRequired();
        builder.Property(x => x.LtJobCod).HasColumnName("LT_JOB_COD");
        builder.Ignore(x => x.DomainEvents);
    }
}

public class LeaveSignatureIdConfiguration : IEntityTypeConfiguration<LeaveSignatureId>
{
    public void Configure(EntityTypeBuilder<LeaveSignatureId> builder)
    {
        builder.ToTable("LET_SIGID");
        builder.HasNoKey();
        builder.Property(x => x.LetSigid).HasColumnName("LET_SIGID").HasMaxLength(100);
        builder.Property(x => x.SigName).HasColumnName("SIG_NAME").HasMaxLength(50);
        builder.Property(x => x.SigDesg).HasColumnName("SIG_DESG").HasMaxLength(10);
        builder.Ignore(x => x.DomainEvents);
    }
}
