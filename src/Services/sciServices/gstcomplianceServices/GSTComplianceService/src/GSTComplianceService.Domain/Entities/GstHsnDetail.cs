using GSTComplianceService.Domain.Common;

namespace GSTComplianceService.Domain.Entities;

public class GstHsnDetail : BaseEntity
{
    public long GstHsnId { get; private set; }
    public long GstHsnGstId { get; private set; }
    public string? GstHsnProductName { get; private set; }
    public string? GstHsnCode { get; private set; }
    public string? GstHsnRemarks { get; private set; }

    public GstMain? GstMain { get; private set; }

    private GstHsnDetail() { }

    public static GstHsnDetail Create(long gstId, string? productName, string? hsnCode, string? remarks) =>
        new()
        {
            GstHsnGstId = gstId,
            GstHsnProductName = productName,
            GstHsnCode = hsnCode,
            GstHsnRemarks = remarks
        };

    public void Update(string? productName, string? hsnCode, string? remarks)
    {
        GstHsnProductName = productName;
        GstHsnCode = hsnCode;
        GstHsnRemarks = remarks;
    }
}
