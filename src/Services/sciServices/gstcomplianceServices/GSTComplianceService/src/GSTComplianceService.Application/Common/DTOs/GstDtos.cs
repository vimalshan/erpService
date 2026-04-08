namespace GSTComplianceService.Application.Common.DTOs;

public record GstMainDto(
    long GstId,
    string? GstType,
    string GstPanNo,
    string? GstEmailId,
    string? GstMobileNo,
    DateTime GstCreatedOn,
    DateTime? GstModifiedOn,
    string? GstVendorName,
    string? GstVendAddLine1,
    string? GstVendCity,
    string? GstVendState,
    string? GstVendPincode,
    int? GstRegistrationType,
    string? GstContactName,
    string? GstContactEmailId,
    string? GstContactMobileNo,
    string? GstRemarks,
    string? GstStatus,
    string GstDigitalFlag,
    string? GstGstnCopy,
    List<GstHsnDetailDto>? HsnDetails = null,
    List<GstServiceDetailDto>? ServiceDetails = null,
    List<GstStateRegDetailDto>? StateRegDetails = null
);

public record GstHsnDetailDto(
    long GstHsnId,
    long GstHsnGstId,
    string? GstHsnProductName,
    string? GstHsnCode,
    string? GstHsnRemarks
);

public record GstServiceDetailDto(
    decimal GstSacId,
    decimal GstSacGstId,
    string? GstSacServiceName,
    string? GstSacCode,
    string? GstSacRemarks
);

public record GstStateRegDetailDto(
    long GstTinId,
    long GstId,
    string? GstState,
    string? GstAddress,
    string? GstGstinNo,
    string? GstTinNo,
    string? GstContactPerson,
    string? GstEmailId,
    string? GstMobileNo,
    string? GstRemarks
);

public record PagedResult<T>(
    IEnumerable<T> Items,
    int Page,
    int PageSize,
    long TotalCount
)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
