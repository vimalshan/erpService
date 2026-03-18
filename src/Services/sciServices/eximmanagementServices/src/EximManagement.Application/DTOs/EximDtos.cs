namespace EximManagement.Application.DTOs;

public class EximDataFileDto
{
    public long FileId { get; set; }
    public string FileType { get; set; } = default!;
    public string? FileName { get; set; }
    public long? OriginalCount { get; set; }
    public long? FinalCount { get; set; }
    public long? FileUploadedBy { get; set; }
    public DateTime FileUploadedOn { get; set; }
    public string? Remarks { get; set; }
    public string? FileSource { get; set; }
    public string? DataTypeCode { get; set; }
    public string? DataTypeMonth { get; set; }
}

public class CreateEximDataFileDto
{
    public long FileId { get; set; }
    public string FileType { get; set; } = default!;
    public string? FileName { get; set; }
    public long? UploadedBy { get; set; }
    public string? FileSource { get; set; }
    public string? Remarks { get; set; }
    public string? DataTypeCode { get; set; }
    public string? DataTypeMonth { get; set; }
    public string? DataXml { get; set; }
}

public class EximProductDto
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public string? ProductOracleCode { get; set; }
    public long LastUpdatedBy { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public char Status { get; set; }
}

public class CreateEximProductDto
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public string? OracleCode { get; set; }
    public long UpdatedBy { get; set; }
}

public class EximProductGroupDto
{
    public long GroupId { get; set; }
    public string GroupName { get; set; } = default!;
    public long LastUpdatedBy { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public char Status { get; set; }
}

public class EximDataExportDto
{
    public long DataId { get; set; }
    public DateTime? EximDate { get; set; }
    public long? HsCode { get; set; }
    public string? ProdDesc { get; set; }
    public string? CountryDest { get; set; }
    public string? PortDest { get; set; }
    public long? StdQty { get; set; }
    public string? StdUnit { get; set; }
    public long? FobInr { get; set; }
    public long? FobDol { get; set; }
    public string? ExpName { get; set; }
    public string? ImpName { get; set; }
    public string? ImpCountry { get; set; }
    public string? Iec { get; set; }
    public string? SbNo { get; set; }
    public string? EMonth { get; set; }
    public long? FileId { get; set; }
}

public class EximDataImportDto
{
    public long DataId { get; set; }
    public DateTime? EximDate { get; set; }
    public long? HsCode { get; set; }
    public string? ProdDesc { get; set; }
    public string? CountryOrg { get; set; }
    public string? PortDest { get; set; }
    public decimal? StdQty { get; set; }
    public string? StdUnit { get; set; }
    public decimal? FobInr { get; set; }
    public decimal? FobDol { get; set; }
    public string? ImpName { get; set; }
    public string? ExpName { get; set; }
    public string? Iec { get; set; }
    public string? BeNo { get; set; }
    public string? EMonth { get; set; }
    public long? FileId { get; set; }
}

public class EximDataQueryDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string FileType { get; set; } = default!;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
