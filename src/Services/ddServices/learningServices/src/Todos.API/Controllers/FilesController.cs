using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todos.Infrastructure.MessageBrokers;

namespace Todos.API.Controllers;

/// <summary>
/// REST API controller for file uploads
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class FilesController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly BlobStorageConfiguration _blobConfig;
    private readonly ILogger<FilesController> _logger;

    public FilesController(IBlobStorageService blobStorageService, BlobStorageConfiguration blobConfig, ILogger<FilesController> logger)
    {
        _blobStorageService = blobStorageService;
        _blobConfig = blobConfig;
        _logger = logger;
    }

    /// <summary>
    /// Uploads a file to blob storage
    /// </summary>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FileUploadResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FileUploadResponse>> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new FileUploadResponse { Success = false, Message = "No file provided" });

        // Validate file extension
        var extension = System.IO.Path.GetExtension(file.FileName);
        if (_blobConfig.AllowedExtensions?.Length > 0 && !_blobConfig.AllowedExtensions.Contains(extension.ToLower()))
            return BadRequest(new FileUploadResponse { Success = false, Message = $"File type {extension} is not allowed" });

        // Validate file size
        var maxSizeInBytes = _blobConfig.MaxFileSizeInMB * 1024 * 1024;
        if (file.Length > maxSizeInBytes)
            return BadRequest(new FileUploadResponse { Success = false, Message = $"File size exceeds maximum allowed size of {_blobConfig.MaxFileSizeInMB} MB" });

        try
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            using var stream = file.OpenReadStream();
            var fileUrl = await _blobStorageService.UploadAsync(stream, fileName, file.ContentType, cancellationToken);

            _logger.LogInformation("File uploaded successfully: {FileName}", fileName);
            return Ok(new FileUploadResponse { Success = true, Message = "File uploaded successfully", FileUrl = fileUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file");
            return BadRequest(new FileUploadResponse { Success = false, Message = $"Error uploading file: {ex.Message}" });
        }
    }

    /// <summary>
    /// Downloads a file from blob storage
    /// </summary>
    [HttpGet("download/{fileName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var stream = await _blobStorageService.DownloadAsync(Uri.UnescapeDataString(fileName), cancellationToken);
            return File(stream, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file: {FileName}", fileName);
            return NotFound();
        }
    }

    /// <summary>
    /// Deletes a file from blob storage
    /// </summary>
    [HttpDelete("{fileName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            await _blobStorageService.DeleteAsync(Uri.UnescapeDataString(fileName), cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FileName}", fileName);
            return NotFound();
        }
    }

    /// <summary>
    /// Lists all files in blob storage
    /// </summary>
    [HttpGet("list")]
    [ProducesResponseType(typeof(FileListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<FileListResponse>> List(CancellationToken cancellationToken)
    {
        try
        {
            var files = await _blobStorageService.ListAsync(cancellationToken);
            return Ok(new FileListResponse { Success = true, Files = files.ToList() });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing files");
            return BadRequest(new FileListResponse { Success = false, Message = $"Error listing files: {ex.Message}" });
        }
    }
}

/// <summary>
/// Response model for file upload
/// </summary>
public class FileUploadResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? FileUrl { get; set; }
}

/// <summary>
/// Response model for file list
/// </summary>
public class FileListResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<string> Files { get; set; } = [];
}
