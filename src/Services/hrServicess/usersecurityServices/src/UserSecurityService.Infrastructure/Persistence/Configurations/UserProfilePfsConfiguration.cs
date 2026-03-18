using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserSecurityService.Domain.Entities;

namespace UserSecurityService.Infrastructure.Persistence.Configurations;

public class UserProfilePfsConfiguration : IEntityTypeConfiguration<UserProfilePfs>
{
    public void Configure(EntityTypeBuilder<UserProfilePfs> builder)
    {
        builder.ToTable("USER_PROFILE_PFS");
        builder.HasKey(x => x.EmUsrId);

        builder.Property(x => x.EmUsrId).HasColumnName("EM_USR_ID").HasMaxLength(25).IsRequired();
        builder.Property(x => x.EmEmpNum).HasColumnName("EM_EMP_NUM").HasColumnType("DECIMAL(38,0)").IsRequired();
        builder.Property(x => x.EmUntCod).HasColumnName("EM_UNT_COD").HasMaxLength(3).IsRequired();
        builder.Property(x => x.EmNickNam).HasColumnName("EM_NICK_NAM").HasMaxLength(65).IsRequired();
        builder.Property(x => x.EmUsrTyp).HasColumnName("EM_USR_TYP").HasMaxLength(1).IsRequired();
        builder.Property(x => x.EmEmlFlg).HasColumnName("EM_EML_FLG").HasMaxLength(1).IsRequired();
        builder.Property(x => x.EmOEmlId).HasColumnName("EM_OEML_ID").HasMaxLength(65);
        builder.Property(x => x.EmPEmlId).HasColumnName("EM_PEML_ID").HasMaxLength(65);
        builder.Property(x => x.EmEffDat).HasColumnName("EM_EFF_DAT").HasColumnType("DATETIME2(3)").IsRequired();
        builder.Property(x => x.EmClsDat).HasColumnName("EM_CLS_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.EmUsrPass).HasColumnName("EM_USR_PASS").HasMaxLength(200).IsRequired();
        builder.Property(x => x.EmEmpNam).HasColumnName("EM_EMP_NAM").HasMaxLength(65);
        builder.Property(x => x.EmDobDat).HasColumnName("EM_DOB_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.EmPhtPth).HasColumnName("EM_PHT_PTH").HasMaxLength(500);
        builder.Property(x => x.EmDivNam).HasColumnName("EM_DIV_NAM").HasMaxLength(200);
        builder.Property(x => x.EmJobCod).HasColumnName("EM_JOB_COD").HasColumnType("BIGINT");
        builder.Property(x => x.EmPinNum).HasColumnName("EM_PIN_NUM").HasColumnType("DECIMAL(20,0)");
        builder.Property(x => x.EmOldNum).HasColumnName("EM_OLD_NUM").HasMaxLength(20);
        builder.Property(x => x.EmEmpDsg).HasColumnName("EM_EMP_DSG").HasMaxLength(100);
        builder.Property(x => x.EmFrsNam).HasColumnName("EM_FRS_NAM").HasMaxLength(65);
        builder.Property(x => x.EmMidNam).HasColumnName("EM_MID_NAM").HasMaxLength(65);
        builder.Property(x => x.EmLstNam).HasColumnName("EM_LST_NAM").HasMaxLength(65);
        builder.Property(x => x.EmCurBus).HasColumnName("EM_CUR_BUS").HasMaxLength(9);
        builder.Property(x => x.EmRepUnt).HasColumnName("EM_REP_UNT").HasMaxLength(3);
        builder.Property(x => x.EmCurGrd).HasColumnName("EM_CUR_GRD").HasMaxLength(10);
        builder.Property(x => x.EmProDat).HasColumnName("EM_PRO_DAT").HasColumnType("DATETIME2(3)");
        builder.Property(x => x.EmCurLoc).HasColumnName("EM_CUR_LOC").HasMaxLength(65);
        builder.Property(x => x.EmTimUnt).HasColumnName("EM_TIM_UNT").HasMaxLength(3);
        builder.Property(x => x.EmCtcAmt).HasColumnName("EM_CTC_AMT").HasColumnType("DECIMAL(19,0)");
        builder.Property(x => x.EmEmpSex).HasColumnName("EM_EMP_SEX").HasMaxLength(1);
        builder.Property(x => x.EmAppUsr).HasColumnName("EM_APP_USR").HasColumnType("DECIMAL(38,0)");
        builder.Property(x => x.EmWrkFlg).HasColumnName("EM_WRK_FLG").HasMaxLength(1);
        builder.Property(x => x.EmSigPth).HasColumnName("EM_SIG_PTH").HasMaxLength(200);
        builder.Property(x => x.EmOutlook).HasColumnName("EM_OUTLOOK").HasMaxLength(1);
        builder.Property(x => x.EmRegStatus).HasColumnName("EM_REGSTATUS").HasMaxLength(1).IsRequired();

        builder.Ignore(x => x.DomainEvents);
    }
}
