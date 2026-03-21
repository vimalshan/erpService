namespace BookingService.Domain.Entities;

/// <summary>EF-mapped entity for BOOK_CONFIRMATION table.</summary>
public class BookingConfirmation
{
    public long BkCnfNum { get; set; }
    public long BkCnfSrl { get; set; }
    public long? BkBokNum { get; set; }
    public long? BkSrlNum { get; set; }
    public DateTime? BkReqDat { get; set; }
    public DateTime? BkFroDat { get; set; }
    public DateTime? BkToDat { get; set; }
    public long? BkFroCit { get; set; }
    public long? BkToCit { get; set; }
    public long BkModCod { get; set; }
    public string? BkFrmLoc { get; set; }
    public string? BkToLoc { get; set; }
    public string? BkAirLin { get; set; }
    public string? BkTrlNum { get; set; }
    public string? BkTrlNam { get; set; }
    public string? BkAdmRmk { get; set; }
    public long? BkTrlCls { get; set; }
    public long? BkVndCod { get; set; }
    public long? BkGheCod { get; set; }
    public string? BkRomNum { get; set; }
    public long? BkPheNum { get; set; }
    public long? BkCpnCod { get; set; }
    public long? BkCpnTck { get; set; }
    public string? BkStsCod { get; set; }
    public long? BkNoPer { get; set; }
    public string? BkDrvNam { get; set; }
    public long? BkTrlCst { get; set; }
    public long? BkSlfCst { get; set; }
    public string? BkSlfFlg { get; set; }
    public string? BkTckNum { get; set; }
    public long? BkAgnCod { get; set; }
    public long? BkTrvlType { get; set; }
    public string? BkCabUnit { get; set; }
    public string? BkCostCod { get; set; }
    public string? BkCabAdd { get; set; }
    public string? BkTripCod { get; set; }
    public long? BkCabSegment { get; set; }
    public string? BkAppSts { get; set; }
    public DateTime? BkAdmnBokdat { get; set; }
    public string? BkRegnNo { get; set; }
    public string? BkProductCode { get; set; }
    public string? BkSubaccountCode { get; set; }

}
