using BookingService.Domain.Common;
using BookingService.Domain.Enums;

namespace BookingService.Domain.Entities;

/// <summary>EF-mapped entity for BOOK_REQUEST table.</summary>
public class BookingRequest
{
    public decimal BkBokNum { get; set; }
    public decimal BkSrlNum { get; set; }
    public string? BkBokTyp { get; set; }
    public string? BkUsrCod { get; set; }
    public long? BkUsrNum { get; set; }
    public string? BkAdmSlf { get; set; }
    public int? BkAdmUnt { get; set; }
    public long? BkReqTyp { get; set; }
    public long? BkReqNum { get; set; }
    public long? BkModCod { get; set; }
    public string? BkPerSts { get; set; }
    public string? BkPerNam { get; set; }
    public DateTime? BkFroDat { get; set; }
    public decimal? BkFrmTim { get; set; }
    public DateTime? BkRetDat { get; set; }
    public decimal? BkRetTim { get; set; }
    public long? BkFroCit { get; set; }
    public long? BkToCit { get; set; }
    public string? BkPckFlg { get; set; }
    public string? BkFroLoc { get; set; }
    public string? BkToLoc { get; set; }
    public string? BkPerSex { get; set; }
    public long? BkDepNos { get; set; }
    public string? BkAdmRem { get; set; }
    public decimal? BkBudAmt { get; set; }
    public DateTime? BkCanDat { get; set; }
    public string? BkCanRem { get; set; }
    public string? BkCanUsr { get; set; }
    public string? BkAppSts { get; set; }
    public long? BkCnfNum { get; set; }
    public DateTime? BkAppDat { get; set; }
    public long? BkTraCls { get; set; }
    public string? BkAirCod { get; set; }
    public long? BkTrvlType { get; set; }
    public string? BkCabToFlg { get; set; }
    public string? BkCabToUnit { get; set; }
    public string? BkCabToCost { get; set; }
    public string? BkCabToAdd { get; set; }
    public string? BkCabToTrip { get; set; }
    public long? BkCabSegment { get; set; }
    public string? BkProductCode { get; set; }
    public string? BkSubaccountCode { get; set; }

}
