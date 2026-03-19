using HRDocumentService.Domain.Common;

namespace HRDocumentService.Domain.Entities;

public class HRDocumentFile : BaseEntity
{
    public long FileId { get; private set; }
    public long FileDocId { get; private set; }
    public string FilePath { get; private set; } = null!;
    public string FileName { get; private set; } = null!;

    private HRDocumentFile() { }

    public static HRDocumentFile Create(long fileId, long fileDocId, string filePath, string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        return new HRDocumentFile
        {
            FileId = fileId,
            FileDocId = fileDocId,
            FilePath = filePath,
            FileName = fileName
        };
    }

    public void UpdatePath(string filePath, string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        FilePath = filePath;
        FileName = fileName;
    }
}
