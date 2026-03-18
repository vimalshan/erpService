using EximManagement.Domain.Common;
using EximManagement.Domain.Events;

namespace EximManagement.Domain.Entities;

/// <summary>Represents a data file uploaded into the EXIM system.</summary>
public class EximDataFile : BaseEntity
{
    public long FileId { get; private set; }
    public string FileType { get; private set; } = default!;     // IMPORT | EXPORT
    public string? FileName { get; private set; }
    public long? OriginalCount { get; private set; }
    public long? FinalCount { get; private set; }
    public long? FileUploadedBy { get; private set; }
    public DateTime FileUploadedOn { get; private set; }
    public string? Remarks { get; private set; }
    public string? FileSource { get; private set; }
    public string? DelFlag { get; private set; }
    public string? DeletedDate { get; private set; }
    public string? DeletedBy { get; private set; }
    public string? DataTypeCode { get; private set; }
    public string? DataTypeMonth { get; private set; }
    public string? DataXml { get; private set; }

    private EximDataFile() { }

    public static EximDataFile Create(
        long fileId, string fileType, string? fileName,
        long? uploadedBy, string? fileSource, string? remarks,
        string? dataTypeCode, string? dataTypeMonth, string? dataXml)
    {
        if (string.IsNullOrWhiteSpace(fileType))
            throw new ArgumentException("File type is required.", nameof(fileType));

        var file = new EximDataFile
        {
            FileId = fileId,
            FileType = fileType.ToUpperInvariant(),
            FileName = fileName,
            FileUploadedBy = uploadedBy,
            FileUploadedOn = DateTime.UtcNow,
            FileSource = fileSource,
            Remarks = remarks,
            DataTypeCode = dataTypeCode,
            DataTypeMonth = dataTypeMonth,
            DataXml = dataXml,
            DelFlag = "N"
        };

        file.AddDomainEvent(new EximDataFileUploadedEvent(file.FileId, file.FileType, DateTime.UtcNow));
        return file;
    }

    public void UpdateCounts(long originalCount, long finalCount)
    {
        OriginalCount = originalCount;
        FinalCount = finalCount;
    }

    public void SoftDelete(string deletedBy)
    {
        DelFlag = "Y";
        DeletedDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        DeletedBy = deletedBy;
        AddDomainEvent(new EximDataFileDeletedEvent(FileId, DateTime.UtcNow));
    }
}
