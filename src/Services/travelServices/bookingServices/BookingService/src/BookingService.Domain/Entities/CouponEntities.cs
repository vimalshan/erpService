namespace BookingService.Domain.Entities;

public class CouponRequest
{
    public long CpnReqId { get; set; }
    public DateTime? CpnReqDat { get; set; }
    public string? CpnReqUsr { get; set; }
    public long? CpnNofCpn { get; set; }
    public string? CpnArlNam { get; set; }
    public string? CpnReqRmk { get; set; }
    public long? CpnArgUnt { get; set; }
    public string? CpnApvUsr { get; set; }
    public DateTime? CpnActDat { get; set; }
    public string? CpnReqSts { get; set; }
    public string? CpnActRmk { get; set; }
    public long? CpnFlxFld1 { get; set; }
    public string? CpnFlxFld2 { get; set; }
    public DateTime? CpnFlxFld3 { get; set; }
    public string? CpnFlxFld4 { get; set; }
}

public class CouponMain
{
    public long CpnCupId { get; set; }
    public string? CpnRefId { get; set; }
    public long? CpnReqId { get; set; }
    public long? CpnNofTck { get; set; }
    public string? CpnArlNam { get; set; }
    public long? CpnCupStr { get; set; }
    public long? CpnCupEnd { get; set; }
    public DateTime? CpnVldFrm { get; set; }
    public DateTime? CpnVldTo { get; set; }
    public long? CpnCupCst { get; set; }
    public string? CpnIseRek { get; set; }
    public string? CpnUsgFlg { get; set; }
    public string? CpnUsrId { get; set; }
    public long? CpnUsrPin { get; set; }
    public string? CpnAdnUsr { get; set; }
    public string? CpnAdnUnt { get; set; }
    public DateTime? CpnIssDat { get; set; }

}

public class CouponSub
{
    public long? CpnCupId { get; set; }
    public long? CpnSrlNum { get; set; }
    public string? CpnTckNum { get; set; }
    public string? CpnUsgFlg { get; set; }
}
