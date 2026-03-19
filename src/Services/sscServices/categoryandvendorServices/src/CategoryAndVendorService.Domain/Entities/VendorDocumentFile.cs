using CategoryAndVendorService.Domain.Common;

namespace CategoryAndVendorService.Domain.Entities;

/// <summary>
/// Entity: Vendor Document File (VENDOR_DOCFILE)
/// </summary>
public class VendorDocumentFile : Entity
{
    public long FileId { get; private set; }
    public long DocumentId { get; private set; }
    public string FileName { get; private set; } = null!;
    public string? FilePath { get; private set; }

    public VendorDocument VendorDocument { get; private set; } = null!;

    private VendorDocumentFile() { }

    public static VendorDocumentFile Create(long id, long documentId, string fileName, string? filePath = null)
    {
        return new VendorDocumentFile
        {
            FileId = id,
            DocumentId = documentId,
            FileName = fileName,
            FilePath = filePath
        };
    }

    public void UpdatePath(string path) => FilePath = path;
}
