using TdsService.Domain.Common;
using TdsService.Domain.Events;
using TdsService.Domain.Exceptions;
using TdsService.Domain.ValueObjects;

namespace TdsService.Domain.Entities;

/// <summary>
/// Aggregate root representing a TDS file record.
/// Maps to the TDSFILE_DETAILS table.
/// </summary>
public sealed class TdsFile : AggregateRoot<long>
{
    public string FileName { get; private set; } = string.Empty;
    public PanNumber? PanNumber { get; private set; }
    public EmailStatus EmailStatus { get; private set; }
    public FileType? FileType { get; private set; }
    public string? BlobStorageUri { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private TdsFile() { }

    public static TdsFile Create(
        long fileId,
        string fileName,
        string? panNo,
        string? emailStatus,
        string? fileType)
    {
        var file = new TdsFile
        {
            Id = fileId,
            FileName = fileName,
            PanNumber = panNo is not null ? PanNumber.TryCreate(panNo) : null,
            EmailStatus = EmailStatusExtensions.FromDbValue(emailStatus),
            FileType = fileType is not null ? FileType.Create(fileType) : null,
            CreatedAt = DateTimeOffset.UtcNow
        };

        file.RaiseDomainEvent(new TdsFileUploadedEvent(fileId, fileName, panNo));
        return file;
    }

    public void MarkEmailSent()
    {
        if (EmailStatus == EmailStatus.Sent)
            throw new DomainException($"Email notification for file '{FileName}' has already been sent.");

        EmailStatus = EmailStatus.Sent;
        UpdatedAt = DateTimeOffset.UtcNow;

        RaiseDomainEvent(new TdsFileEmailSentEvent(Id, PanNumber?.Value));
    }

    public void SetBlobUri(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
            throw new DomainException("Blob URI cannot be empty.");

        BlobStorageUri = uri;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
