using GSTComplianceService.Domain.Common;

namespace GSTComplianceService.Domain.Entities;

public class GstServiceDetail : BaseEntity
{
    public long GstSacId { get; private set; }
    public long GstSacGstId { get; private set; }
    public string? GstSacServiceName { get; private set; }
    public string? GstSacCode { get; private set; }
    public string? GstSacRemarks { get; private set; }

    public GstMain? GstMain { get; private set; }

    private GstServiceDetail() { }

    public static GstServiceDetail Create(long gstId, string? serviceName, string? sacCode, string? remarks) =>
        new()
        {
            GstSacGstId = gstId,
            GstSacServiceName = serviceName,
            GstSacCode = sacCode,
            GstSacRemarks = remarks
        };

    public void Update(string? serviceName, string? sacCode, string? remarks)
    {
        GstSacServiceName = serviceName;
        GstSacCode = sacCode;
        GstSacRemarks = remarks;
    }
}
