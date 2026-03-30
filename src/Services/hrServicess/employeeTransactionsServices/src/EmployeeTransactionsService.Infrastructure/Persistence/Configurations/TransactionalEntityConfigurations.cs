using EmployeeTransactionsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeTransactionsService.Infrastructure.Persistence.Configurations;

public sealed class EmployeeMainConfiguration : IEntityTypeConfiguration<EmployeeMain>
{
    public void Configure(EntityTypeBuilder<EmployeeMain> builder)
    {
        builder.ToTable("EMPLOYEE_MAIN");
        builder.HasKey(x => x.EmpSysId);

        builder.Property(x => x.EmpSysId).HasColumnName("EMP_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpPinNo).HasColumnName("EMP_PIN_NO").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpAppDate).HasColumnName("EMP_APP_DATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.EmpAppUnit).HasColumnName("EMP_APP_UNIT").HasMaxLength(3).IsUnicode(false);
        builder.Property(x => x.EmpAppGrade).HasColumnName("EMP_APP_GRADE").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpAppPosition).HasColumnName("EMP_APP_POSITION").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpAppPositionDesc).HasColumnName("EMP_APP_POSITIONDESC").HasMaxLength(150).IsUnicode(false);
        builder.Property(x => x.EmpFrsName).HasColumnName("EMP_FRS_NAME").HasMaxLength(65).IsUnicode(false);
        builder.Property(x => x.EmpMidName).HasColumnName("EMP_MID_NAME").HasMaxLength(65).IsUnicode(false);
        builder.Property(x => x.EmpLstName).HasColumnName("EMP_LST_NAME").HasMaxLength(65).IsUnicode(false);
        builder.Property(x => x.EmpGender).HasColumnName("EMP_GENDER").HasMaxLength(1).IsUnicode(false);
        builder.Property(x => x.EmpDobRecord).HasColumnName("EMP_DOB_RECORD").HasColumnType("datetime2(3)");
        builder.Property(x => x.EmpOfferStatus).HasColumnName("EMP_OFFERSTATUS").HasMaxLength(1).IsUnicode(false);
        builder.Property(x => x.EmpOEmailId).HasColumnName("EMP_OEMAIL_ID").HasMaxLength(200).IsUnicode(false);
        builder.Property(x => x.EmpPEmailId).HasColumnName("EMP_PEMAIL_ID").HasMaxLength(200).IsUnicode(false);
        builder.Property(x => x.EmpMobileNo).HasColumnName("EMP_MOBILE_NO").HasMaxLength(65).IsUnicode(false);
        builder.Property(x => x.EmpLeadRole).HasColumnName("EMP_LEAD_ROLE").HasMaxLength(3).IsUnicode(false);
        builder.Property(x => x.EmpProbDate).HasColumnName("EMP_PROBDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.EmpProbFlag).HasColumnName("EMP_PROB_FLAG").HasMaxLength(1).IsUnicode(false);
        builder.Property(x => x.EmpConfDate).HasColumnName("EMP_CONFDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.EmpAppUnitId).HasColumnName("EMP_APPUNITID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpCreatedBy).HasColumnName("EMP_CREATEDBY").HasColumnType("decimal(22,0)");
        builder.Property(x => x.EmpCreatedOn).HasColumnName("EMP_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.EmpUpdatedBy).HasColumnName("EMP_UPDATED_BY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpUpdatedOn).HasColumnName("EMP_UPDATED_ON").HasColumnType("datetime2(3)");
    }
}

public sealed class EmployeeGradeConfiguration : IEntityTypeConfiguration<EmployeeGrade>
{
    public void Configure(EntityTypeBuilder<EmployeeGrade> builder)
    {
        builder.ToTable("EMP_GRADE");
        builder.HasKey(x => x.GradeEmpSysId);

        builder.Property(x => x.GradeEmpSysId).HasColumnName("GRADE_EMP_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.GradeTranId).HasColumnName("GRADE_TRANID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.GradeId).HasColumnName("GRADE_ID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.GradeEffDate).HasColumnName("GRADE_EFF_DATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.GradeClsDate).HasColumnName("GRADE_CLS_DATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.GradeRemarks).HasColumnName("GRADE_REMARKS").HasMaxLength(65).IsUnicode(false);
        builder.Property(x => x.GradeLivFlag).HasColumnName("GRADE_LIVFLAG").HasMaxLength(1).IsUnicode(false);
        builder.Property(x => x.GradeUpdatedBy).HasColumnName("GRADE_UPDATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.GradeUpdatedOn).HasColumnName("GRADE_UPDATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.GradeProbation).HasColumnName("GRADE_PROBATION").HasMaxLength(1).IsUnicode(false);
    }
}

public sealed class EmployeeGradeChangeConfiguration : IEntityTypeConfiguration<EmployeeGradeChange>
{
    public void Configure(EntityTypeBuilder<EmployeeGradeChange> builder)
    {
        builder.ToTable("EMP_GRADECHANGE");
        builder.HasKey(x => x.EmpGradeChangeId);

        builder.Property(x => x.EmpGradeChangeId).HasColumnName("EMP_GRADECHANGEID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpEmpSysId).HasColumnName("EMP_EMPSYSID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpOldGrade).HasColumnName("EMP_OLDGRADE").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpNewGrade).HasColumnName("EMP_NEWGRADE").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpEffDate).HasColumnName("EMP_EFFDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.EmpStatus).HasColumnName("EMP_STATUS").HasMaxLength(1).IsUnicode(false);
        builder.Property(x => x.EmpCreatedBy).HasColumnName("EMP_CREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.EmpCreatedOn).HasColumnName("EMP_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.EmpApprovedBy).HasColumnName("EMP_APPROVEDBY").HasColumnType("decimal(22,0)");
        builder.Property(x => x.EmpApprovedOn).HasColumnName("EMP_APPROVEDON").HasColumnType("datetime2(3)");
    }
}

public sealed class EmployeeProbationConfiguration : IEntityTypeConfiguration<EmployeeProbation>
{
    public void Configure(EntityTypeBuilder<EmployeeProbation> builder)
    {
        builder.ToTable("AA_EMP_PROBATION");
        builder.HasKey(x => x.ProbId);

        builder.Property(x => x.ProbId).HasColumnName("PROB_ID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.ProbEmpSysId).HasColumnName("PROB_EMP_SYSID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.ProbDueDate).HasColumnName("PROB_DUEDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.ProbDdRequestNo).HasColumnName("PROB_DDREQUESTNO").HasColumnType("decimal(38,0)");
        builder.Property(x => x.ProbFinStatus).HasColumnName("PROB_FINSTATUS").HasMaxLength(1).IsUnicode(false);
        builder.Property(x => x.ProbReviewDate).HasColumnName("PROB_REVIEWDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.ProbNxtReviewDate).HasColumnName("PROB_NXTREVIEWDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.ProbConfDate).HasColumnName("PROB_CONFDATE").HasColumnType("datetime2(3)");
    }
}

public sealed class AlertGroupConfiguration : IEntityTypeConfiguration<AlertGroup>
{
    public void Configure(EntityTypeBuilder<AlertGroup> builder)
    {
        builder.ToTable("ALERTGRP_MASTER");
        builder.HasKey(x => x.AlgrpId);

        builder.Property(x => x.AlgrpId).HasColumnName("ALGRP_ID").HasColumnType("decimal(22,0)");
        builder.Property(x => x.AlgrpName).HasColumnName("ALGRP_NAME").HasMaxLength(100).IsUnicode(false);
        builder.Property(x => x.AlgrpType).HasColumnName("ALGRP_TYPE").HasMaxLength(1).IsUnicode(false);
        builder.Property(x => x.AlgrpCreatedBy).HasColumnName("ALGRP_CREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AlgrpCreatedOn).HasColumnName("ALGRP_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.AlgrpModifiedBy).HasColumnName("ALGRP_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AlgrpModifiedOn).HasColumnName("ALGRP_MODIFIEDON").HasColumnType("datetime2(3)");

        builder.HasMany(x => x.Members)
            .WithOne()
            .HasForeignKey(x => x.AlmapGrpid)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class AlertGroupEmployeeMapConfiguration : IEntityTypeConfiguration<AlertGroupEmployeeMap>
{
    public void Configure(EntityTypeBuilder<AlertGroupEmployeeMap> builder)
    {
        builder.ToTable("ALERTGRP_EMPMAP");
        builder.HasKey(x => x.AlmapId);

        builder.Property(x => x.AlmapId).HasColumnName("ALMAP_ID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AlmapGrpid).HasColumnName("ALMAP_GRPID").HasColumnType("decimal(22,0)");
        builder.Property(x => x.AlmapEmpSysId).HasColumnName("ALMAP_EMPSYSID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AlmapEmailId).HasColumnName("ALMAP_EMAILID").HasMaxLength(100).IsUnicode(false);
        builder.Property(x => x.AlmapOrgId).HasColumnName("ALMAP_ORGID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AlmapUnitId).HasColumnName("ALMAP_UNITID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AlmapCalendarId).HasColumnName("ALMAP_CALENDARID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AlmapEffDate).HasColumnName("ALMAP_EFFDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.AlmapClsDate).HasColumnName("ALMAP_CLSDATE").HasColumnType("datetime2(3)");
        builder.Property(x => x.AlmapCreatedBy).HasColumnName("ALMAP_CREATEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AlmapCreatedOn).HasColumnName("ALMAP_CREATEDON").HasColumnType("datetime2(3)");
        builder.Property(x => x.AlmapModifiedBy).HasColumnName("ALMAP_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.AlmapModifiedOn).HasColumnName("ALMAP_MODIFIEDON").HasColumnType("datetime2(3)");
    }
}

public sealed class StationeryItemImageConfiguration : IEntityTypeConfiguration<StationeryItemImage>
{
    public void Configure(EntityTypeBuilder<StationeryItemImage> builder)
    {
        builder.ToTable("STATIONERY_ITEM_IMAGE");
        builder.HasKey(x => x.ImageId);

        builder.Property(x => x.ImageId).HasColumnName("IMAGE_ID");
        builder.Property(x => x.ItemReference).HasColumnName("ITEM_REFERENCE").HasMaxLength(50).IsUnicode(false);
        builder.Property(x => x.BlobName).HasColumnName("BLOB_NAME").HasMaxLength(300).IsUnicode(false);
        builder.Property(x => x.ContentType).HasColumnName("CONTENT_TYPE").HasMaxLength(100).IsUnicode(false);
        builder.Property(x => x.UploadedBy).HasColumnName("UPLOADED_BY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.UploadedOnUtc).HasColumnName("UPLOADED_ON_UTC").HasColumnType("datetime2(3)");
    }
}