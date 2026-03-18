using GSTComplianceService.Domain.Common;

namespace GSTComplianceService.Domain.Entities;

public class GstSupplier : BaseEntity
{
    public long SupplierNumber { get; private set; }
    public string SupplierName { get; private set; } = string.Empty;
    public string? EmailAddress { get; private set; }
    public string? OperatingUnit { get; private set; }
    public long? PanNo { get; private set; }

    private GstSupplier() { }

    public static GstSupplier Create(long supplierNumber, string supplierName, string? email, string? ou, long? panNo) =>
        new()
        {
            SupplierNumber = supplierNumber,
            SupplierName = supplierName,
            EmailAddress = email,
            OperatingUnit = ou,
            PanNo = panNo
        };
}
