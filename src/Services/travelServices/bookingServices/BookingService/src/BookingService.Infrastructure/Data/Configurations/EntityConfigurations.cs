using BookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Infrastructure.Data.Configurations;

public class BookingRequestConfiguration : IEntityTypeConfiguration<BookingRequest>
{
    public void Configure(EntityTypeBuilder<BookingRequest> builder)
    {
        builder.ToTable("BOOK_REQUEST");
        builder.HasKey(e => e.BkBokNum);

        builder.Property(e => e.BkBokNum).HasColumnName("BK_BOK_NUM").HasColumnType("decimal(20,0)");
        builder.Property(e => e.BkSrlNum).HasColumnName("BK_SRL_NUM").HasColumnType("decimal(20,0)");
        builder.Property(e => e.BkBokTyp).HasColumnName("BK_BOK_TYP").HasMaxLength(1);
        builder.Property(e => e.BkUsrCod).HasColumnName("BK_USR_COD").HasMaxLength(25);
        builder.Property(e => e.BkUsrNum).HasColumnName("BK_USR_NUM");
        builder.Property(e => e.BkAdmSlf).HasColumnName("BK_ADM_SLF").HasMaxLength(1);
        builder.Property(e => e.BkAdmUnt).HasColumnName("BK_ADM_UNT");
        builder.Property(e => e.BkReqTyp).HasColumnName("BK_REQ_TYP");
        builder.Property(e => e.BkReqNum).HasColumnName("BK_REQ_NUM");
        builder.Property(e => e.BkModCod).HasColumnName("BK_MOD_COD");
        builder.Property(e => e.BkPerSts).HasColumnName("BK_PER_STS").HasMaxLength(1);
        builder.Property(e => e.BkPerNam).HasColumnName("BK_PER_NAM").HasMaxLength(200);
        builder.Property(e => e.BkFroDat).HasColumnName("BK_FRO_DAT");
        builder.Property(e => e.BkFrmTim).HasColumnName("BK_FRM_TIM").HasColumnType("decimal(38,0)");
        builder.Property(e => e.BkRetDat).HasColumnName("BK_RET_DAT");
        builder.Property(e => e.BkRetTim).HasColumnName("BK_RET_TIM").HasColumnType("decimal(38,0)");
        builder.Property(e => e.BkFroCit).HasColumnName("BK_FRO_CIT");
        builder.Property(e => e.BkToCit).HasColumnName("BK_TO_CIT");
        builder.Property(e => e.BkPckFlg).HasColumnName("BK_PCK_FLG").HasMaxLength(2);
        builder.Property(e => e.BkFroLoc).HasColumnName("BK_FRO_LOC").HasMaxLength(200);
        builder.Property(e => e.BkToLoc).HasColumnName("BK_TO_LOC").HasMaxLength(200);
        builder.Property(e => e.BkPerSex).HasColumnName("BK_PER_SEX").HasMaxLength(1);
        builder.Property(e => e.BkDepNos).HasColumnName("BK_DEP_NOS");
        builder.Property(e => e.BkAdmRem).HasColumnName("BK_ADM_REM").HasMaxLength(200);
        builder.Property(e => e.BkBudAmt).HasColumnName("BK_BUD_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.BkCanDat).HasColumnName("BK_CAN_DAT");
        builder.Property(e => e.BkCanRem).HasColumnName("BK_CAN_REM").HasMaxLength(200);
        builder.Property(e => e.BkCanUsr).HasColumnName("BK_CAN_USR").HasMaxLength(25);
        builder.Property(e => e.BkAppSts).HasColumnName("BK_APP_STS").HasMaxLength(1);
        builder.Property(e => e.BkCnfNum).HasColumnName("BK_CNF_NUM");
        builder.Property(e => e.BkAppDat).HasColumnName("BK_APP_DAT");
        builder.Property(e => e.BkTraCls).HasColumnName("BK_TRA_CLS");
        builder.Property(e => e.BkAirCod).HasColumnName("BK_AIR_COD").HasMaxLength(3);
        builder.Property(e => e.BkTrvlType).HasColumnName("BK_TRVL_TYPE");
        builder.Property(e => e.BkCabToFlg).HasColumnName("BK_CAB_TO_FLG").HasMaxLength(1);
        builder.Property(e => e.BkCabToUnit).HasColumnName("BK_CAB_TO_UNIT").HasMaxLength(3);
        builder.Property(e => e.BkCabToCost).HasColumnName("BK_CAB_TO_COST").HasMaxLength(25);
        builder.Property(e => e.BkCabToAdd).HasColumnName("BK_CAB_TO_ADD").HasMaxLength(500);
        builder.Property(e => e.BkCabToTrip).HasColumnName("BK_CAB_TO_TRIP").HasMaxLength(1);
        builder.Property(e => e.BkCabSegment).HasColumnName("BK_CAB_SEGMENT");
        builder.Property(e => e.BkProductCode).HasColumnName("BK_PRODUCT_CODE").HasMaxLength(25);
        builder.Property(e => e.BkSubaccountCode).HasColumnName("BK_SUBACCOUNT_CODE").HasMaxLength(25);


    }
}

public class BookingConfirmationConfiguration : IEntityTypeConfiguration<BookingConfirmation>
{
    public void Configure(EntityTypeBuilder<BookingConfirmation> builder)
    {
        builder.ToTable("BOOK_CONFIRMATION");
        builder.HasKey(e => e.BkCnfNum);

        builder.Property(e => e.BkCnfNum).HasColumnName("BK_CNF_NUM").ValueGeneratedOnAdd();
        builder.Property(e => e.BkCnfSrl).HasColumnName("BK_CNF_SRL");
        builder.Property(e => e.BkBokNum).HasColumnName("BK_BOK_NUM");
        builder.Property(e => e.BkSrlNum).HasColumnName("BK_SRL_NUM");
        builder.Property(e => e.BkReqDat).HasColumnName("BK_REQ_DAT");
        builder.Property(e => e.BkFroDat).HasColumnName("BK_FRO_DAT");
        builder.Property(e => e.BkToDat).HasColumnName("BK_TO_DAT");
        builder.Property(e => e.BkFroCit).HasColumnName("BK_FRO_CIT");
        builder.Property(e => e.BkToCit).HasColumnName("BK_TO_CIT");
        builder.Property(e => e.BkModCod).HasColumnName("BK_MOD_COD");
        builder.Property(e => e.BkFrmLoc).HasColumnName("BK_FRM_LOC").HasMaxLength(200);
        builder.Property(e => e.BkToLoc).HasColumnName("BK_TO_LOC").HasMaxLength(200);
        builder.Property(e => e.BkAirLin).HasColumnName("BK_AIR_LIN").HasMaxLength(3);
        builder.Property(e => e.BkTrlNum).HasColumnName("BK_TRL_NUM").HasMaxLength(50);
        builder.Property(e => e.BkTrlNam).HasColumnName("BK_TRL_NAM").HasMaxLength(200);
        builder.Property(e => e.BkAdmRmk).HasColumnName("BK_ADM_RMK").HasMaxLength(2000);
        builder.Property(e => e.BkTrlCls).HasColumnName("BK_TRL_CLS");
        builder.Property(e => e.BkVndCod).HasColumnName("BK_VND_COD");
        builder.Property(e => e.BkGheCod).HasColumnName("BK_GHE_COD");
        builder.Property(e => e.BkRomNum).HasColumnName("BK_ROM_NUM").HasMaxLength(10);
        builder.Property(e => e.BkPheNum).HasColumnName("BK_PHE_NUM");
        builder.Property(e => e.BkCpnCod).HasColumnName("BK_CPN_COD");
        builder.Property(e => e.BkCpnTck).HasColumnName("BK_CPN_TCK");
        builder.Property(e => e.BkStsCod).HasColumnName("BK_STS_COD").HasMaxLength(1);
        builder.Property(e => e.BkNoPer).HasColumnName("BK_NO_PER");
        builder.Property(e => e.BkDrvNam).HasColumnName("BK_DRV_NAM").HasMaxLength(50);
        builder.Property(e => e.BkTrlCst).HasColumnName("BK_TRL_CST");
        builder.Property(e => e.BkSlfCst).HasColumnName("BK_SLF_CST");
        builder.Property(e => e.BkSlfFlg).HasColumnName("BK_SLF_FLG").HasMaxLength(1);
        builder.Property(e => e.BkTckNum).HasColumnName("BK_TCK_NUM").HasMaxLength(25);
        builder.Property(e => e.BkAgnCod).HasColumnName("BK_AGN_COD");
        builder.Property(e => e.BkTrvlType).HasColumnName("BK_TRVL_TYPE");
        builder.Property(e => e.BkCabUnit).HasColumnName("BK_CAB_UNIT").HasMaxLength(3);
        builder.Property(e => e.BkCostCod).HasColumnName("BK_COST_COD").HasMaxLength(25);
        builder.Property(e => e.BkCabAdd).HasColumnName("BK_CAB_ADD").HasMaxLength(500);
        builder.Property(e => e.BkTripCod).HasColumnName("BK_TRIP_COD").HasMaxLength(1);
        builder.Property(e => e.BkCabSegment).HasColumnName("BK_CAB_SEGMENT");
        builder.Property(e => e.BkAppSts).HasColumnName("BK_APP_STS").HasMaxLength(1);
        builder.Property(e => e.BkAdmnBokdat).HasColumnName("BK_ADMN_BOKDAT");
        builder.Property(e => e.BkRegnNo).HasColumnName("BK_REGN_NO").HasMaxLength(50);
        builder.Property(e => e.BkProductCode).HasColumnName("BK_PRODUCT_CODE").HasMaxLength(25);
        builder.Property(e => e.BkSubaccountCode).HasColumnName("BK_SUBACCOUNT_CODE").HasMaxLength(25);
    }
}

public class BookingForwardUnitConfiguration : IEntityTypeConfiguration<BookingForwardUnit>
{
    public void Configure(EntityTypeBuilder<BookingForwardUnit> builder)
    {
        builder.ToTable("BOOK_FORWARD_UNIT");
        builder.HasKey(e => new { e.BkBokNum, e.BkSrlNum, e.AdmUnit });

        builder.Property(e => e.BkBokNum).HasColumnName("BK_BOK_NUM").HasColumnType("decimal(20,0)");
        builder.Property(e => e.BkSrlNum).HasColumnName("BK_SRL_NUM").HasColumnType("decimal(20,0)");
        builder.Property(e => e.AdmUnit).HasColumnName("ADM_UNIT");
        builder.Property(e => e.FwdAdmUnit).HasColumnName("FWD_ADM_UNIT");
        builder.Property(e => e.FwdDate).HasColumnName("FWD_DATE");
    }
}

public class CouponRequestConfiguration : IEntityTypeConfiguration<CouponRequest>
{
    public void Configure(EntityTypeBuilder<CouponRequest> builder)
    {
        builder.ToTable("COUPON_REQUEST");
        builder.HasKey(e => e.CpnReqId);
        builder.Property(e => e.CpnReqId).HasColumnName("CPN_REQ_ID");
        builder.Property(e => e.CpnReqDat).HasColumnName("CPN_REQ_DAT");
        builder.Property(e => e.CpnReqUsr).HasColumnName("CPN_REQ_USR").HasMaxLength(50);
        builder.Property(e => e.CpnNofCpn).HasColumnName("CPN_NOF_CPN");
        builder.Property(e => e.CpnArlNam).HasColumnName("CPN_ARL_NAM").HasMaxLength(3);
        builder.Property(e => e.CpnReqRmk).HasColumnName("CPN_REQ_RMK").HasMaxLength(500);
        builder.Property(e => e.CpnArgUnt).HasColumnName("CPN_ARG_UNT");
        builder.Property(e => e.CpnApvUsr).HasColumnName("CPN_APV_USR").HasMaxLength(50);
        builder.Property(e => e.CpnActDat).HasColumnName("CPN_ACT_DAT");
        builder.Property(e => e.CpnReqSts).HasColumnName("CPN_REQ_STS").HasMaxLength(1);
        builder.Property(e => e.CpnActRmk).HasColumnName("CPN_ACT_RMK").HasMaxLength(500);
        builder.Property(e => e.CpnFlxFld1).HasColumnName("CPN_FLX_FLD1");
        builder.Property(e => e.CpnFlxFld2).HasColumnName("CPN_FLX_FLD2").HasMaxLength(200);
        builder.Property(e => e.CpnFlxFld3).HasColumnName("CPN_FLX_FLD3");
        builder.Property(e => e.CpnFlxFld4).HasColumnName("CPN_FLX_FLD4").HasMaxLength(500);
    }
}

public class CouponMainConfiguration : IEntityTypeConfiguration<CouponMain>
{
    public void Configure(EntityTypeBuilder<CouponMain> builder)
    {
        builder.ToTable("COUPON_MAIN");
        builder.HasKey(e => e.CpnCupId);
        builder.Property(e => e.CpnCupId).HasColumnName("CPN_CUP_ID");
        builder.Property(e => e.CpnRefId).HasColumnName("CPN_REF_ID").HasMaxLength(25);
        builder.Property(e => e.CpnReqId).HasColumnName("CPN_REQ_ID");
        builder.Property(e => e.CpnNofTck).HasColumnName("CPN_NOF_TCK");
        builder.Property(e => e.CpnArlNam).HasColumnName("CPN_ARL_NAM").HasMaxLength(3);
        builder.Property(e => e.CpnCupStr).HasColumnName("CPN_CUP_STR");
        builder.Property(e => e.CpnCupEnd).HasColumnName("CPN_CUP_END");
        builder.Property(e => e.CpnVldFrm).HasColumnName("CPN_VLD_FRM");
        builder.Property(e => e.CpnVldTo).HasColumnName("CPN_VLD_TO");
        builder.Property(e => e.CpnCupCst).HasColumnName("CPN_CUP_CST");
        builder.Property(e => e.CpnIseRek).HasColumnName("CPN_ISE_REK").HasMaxLength(500);
        builder.Property(e => e.CpnUsgFlg).HasColumnName("CPN_USG_FLG").HasMaxLength(1);
        builder.Property(e => e.CpnUsrId).HasColumnName("CPN_USR_ID").HasMaxLength(50);
        builder.Property(e => e.CpnUsrPin).HasColumnName("CPN_USR_PIN");
        builder.Property(e => e.CpnAdnUsr).HasColumnName("CPN_ADN_USR").HasMaxLength(50);
        builder.Property(e => e.CpnAdnUnt).HasColumnName("CPN_ADN_UNT").HasMaxLength(5);
        builder.Property(e => e.CpnIssDat).HasColumnName("CPN_ISS_DAT");


    }
}

public class CouponSubConfiguration : IEntityTypeConfiguration<CouponSub>
{
    public void Configure(EntityTypeBuilder<CouponSub> builder)
    {
        builder.ToTable("COUPON_SUB");
        builder.HasNoKey();
        builder.Property(e => e.CpnCupId).HasColumnName("CPN_CUP_ID");
        builder.Property(e => e.CpnSrlNum).HasColumnName("CPN_SRL_NUM");
        builder.Property(e => e.CpnTckNum).HasColumnName("CPN_TCK_NUM").HasMaxLength(20);
        builder.Property(e => e.CpnUsgFlg).HasColumnName("CPN_USG_FLG").HasMaxLength(1);
    }
}

public class CabPickConfiguration : IEntityTypeConfiguration<CabPick>
{
    public void Configure(EntityTypeBuilder<CabPick> builder)
    {
        builder.ToTable("CABPICK");
        builder.HasNoKey();
        builder.Property(e => e.CityFrom).HasColumnName("CITYFROM").HasMaxLength(25);
        builder.Property(e => e.CityTo).HasColumnName("CITYTO").HasMaxLength(25);
        builder.Property(e => e.PickFlag).HasColumnName("PICKFLAG").HasMaxLength(4);
    }
}

public class RoomAvailTempConfiguration : IEntityTypeConfiguration<RoomAvailTemp>
{
    public void Configure(EntityTypeBuilder<RoomAvailTemp> builder)
    {
        builder.ToTable("ROOMAVAIL_TEMP");
        builder.HasNoKey();
        builder.Property(e => e.BkGhcode).HasColumnName("BK_GHCODE");
        builder.Property(e => e.BkRoomno).HasColumnName("BK_ROOMNO").HasMaxLength(10);
        builder.Property(e => e.BkFrodat).HasColumnName("BK_FRODAT");
        builder.Property(e => e.BkTodat).HasColumnName("BK_TODAT");
        builder.Property(e => e.TotalHrOccupied).HasColumnName("TOTALHR_OCCUPIED");
    }
}
