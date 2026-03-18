using EximManagement.Application.Interfaces;
using EximManagement.Infrastructure.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EximManagement.Functions;

/// <summary>
/// Azure Function that runs every day at midnight to clean up old soft-deleted data files.
/// </summary>
public class EximCleanupFunction(
    EximDbContext dbContext,
    IMessagePublisher publisher,
    ILogger<EximCleanupFunction> logger)
{
    [Function("EximDailyCleanup")]
    public async Task RunDailyCleanup(
        [TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo,
        CancellationToken ct)
    {
        logger.LogInformation("EXIM Daily Cleanup started at {Timestamp}", DateTime.UtcNow);

        var cutoff = DateTime.UtcNow.AddDays(-30);
        var staleFiles = await dbContext.EximDataFiles
            .Where(f => f.DelFlag == "Y" && f.FileUploadedOn < cutoff)
            .ToListAsync(ct);

        if (staleFiles.Any())
        {
            dbContext.EximDataFiles.RemoveRange(staleFiles);
            var count = await dbContext.SaveChangesAsync(ct);

            logger.LogInformation("Purged {Count} stale EXIM data files.", count);

            await publisher.PublishAsync(
                new { PurgedCount = count, Timestamp = DateTime.UtcNow },
                "exim.cleanup.completed", ct);
        }
        else
        {
            logger.LogInformation("No stale EXIM data files to purge.");
        }
    }
}

/// <summary>
/// Azure Function that processes incoming EXIM data files uploaded to Blob Storage.
/// </summary>
public class EximBlobProcessorFunction(
    IMessagePublisher publisher,
    ILogger<EximBlobProcessorFunction> logger)
{
    [Function("EximBlobProcessor")]
    public async Task ProcessBlobAsync(
        [BlobTrigger("exim-documents/{name}")] Stream blob,
        string name,
        CancellationToken ct)
    {
        logger.LogInformation("EXIM Blob trigger: Processing file '{Name}', size={Size}", name, blob.Length);

        try
        {
            // Determine file type from name
            var fileType = name.Contains("export", StringComparison.OrdinalIgnoreCase) ? "EXPORT" : "IMPORT";

            await publisher.PublishAsync(
                new { FileName = name, FileType = fileType, Size = blob.Length, Timestamp = DateTime.UtcNow },
                "exim.file.blob.received", ct);

            logger.LogInformation("EXIM Blob '{Name}' queued for processing.", name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process EXIM blob '{Name}'", name);
            throw;
        }
    }
}

/// <summary>Place-holder BlobTrigger attribute for non-Azure-Functions-enabled compilation.</summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class BlobTriggerAttribute(string path, string connection = "") : Attribute
{
    public string Path { get; } = path;
    public string Connection { get; } = connection;
}
