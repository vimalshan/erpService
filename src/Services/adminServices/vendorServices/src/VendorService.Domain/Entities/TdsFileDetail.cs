using VendorService.Domain.Common;
using VendorService.Domain.Exceptions;

namespace VendorService.Domain.Entities;

public sealed class TdsFileDetail : AggregateRoot
{
    public long FileId { get; private set; }
    public string? FileName { get; private set; }
    public string? PanNo { get; private set; }
    public string? EmailStatus { get; private set; }
    public string? FileType { get; private set; }

    private TdsFileDetail() { }

    public static TdsFileDetail Create(long fileId, string? fileName, string? panNo, string? emailStatus, string? fileType)
    {
        if (fileId <= 0) throw new VendorDomainException("File ID must be positive.");
        if (emailStatus is not null && emailStatus.Length > 1)
            throw new VendorDomainException("Email status must be a single character.");
        if (fileType is not null && fileType.Length > 3)
            throw new VendorDomainException("File type must be at most 3 characters.");

        return new TdsFileDetail
        {
            FileId = fileId,
            FileName = fileName?.Trim(),
            PanNo = panNo?.Trim(),
            EmailStatus = emailStatus?.Trim(),
            FileType = fileType?.Trim()
        };
    }

    public void UpdateEmailStatus(string emailStatus)
    {
        if (emailStatus.Length > 1) throw new VendorDomainException("Email status must be a single character.");
        EmailStatus = emailStatus;
    }
}
