namespace MobileExpenseManagement.Domain.Entities;

/// <summary>
/// Represents an attached file for an expense
/// </summary>
public class ExpenseFile
{
    public decimal Id { get; private set; }
    public decimal ExpenseId { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string FileData { get; private set; } = string.Empty; // Base64 or blob storage path
    public long FileSize { get; private set; }
    public string ContentType { get; private set; } = string.Empty;
    public DateTime UploadedOn { get; private set; }
    public decimal UploadedBy { get; private set; }
    public string BlobStoragePath { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }

    // Navigation property
    public Expense? Expense { get; set; }

    /// <summary>
    /// Create a new expense file
    /// </summary>
    public static ExpenseFile Create(decimal expenseId, string fileName, string fileData, 
        long fileSize, string contentType, decimal uploadedBy)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required", nameof(fileName));

        if (fileSize <= 0)
            throw new ArgumentException("File size must be greater than 0", nameof(fileSize));

        if (fileSize > 50 * 1024 * 1024) // 50 MB max
            throw new ArgumentException("File size cannot exceed 50 MB", nameof(fileSize));

        var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".doc", ".docx", ".xls", ".xlsx" };
        var fileExtension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(fileExtension))
            throw new ArgumentException($"File type {fileExtension} is not allowed", nameof(fileName));

        return new ExpenseFile
        {
            Id = 0, // Will be assigned by database
            ExpenseId = expenseId,
            FileName = fileName.Trim(),
            FileData = fileData,
            FileSize = fileSize,
            ContentType = contentType,
            UploadedOn = DateTime.UtcNow,
            UploadedBy = uploadedBy,
            IsDeleted = false
        };
    }

    /// <summary>
    /// Set blob storage path
    /// </summary>
    public void SetBlobStoragePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path cannot be empty", nameof(path));

        BlobStoragePath = path;
    }

    /// <summary>
    /// Soft delete the file
    /// </summary>
    public void Delete()
    {
        IsDeleted = true;
    }
}
