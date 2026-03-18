using AlertsNotifications.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlertsNotifications.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly IConfiguration _configuration;

    public BlobStorageController(IBlobStorageService blobStorageService, IConfiguration configuration)
    {
        _blobStorageService = blobStorageService;
        _configuration = configuration;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file provided.");

        var containerName = _configuration["AzureBlobStorage:ContainerName"] ?? "circular-documents";
        var fileName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";

        using var stream = file.OpenReadStream();
        var url = await _blobStorageService.UploadFileAsync(containerName, fileName, stream, file.ContentType);

        return Ok(new { FileName = fileName, Url = url });
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> Download(string fileName)
    {
        var containerName = _configuration["AzureBlobStorage:ContainerName"] ?? "circular-documents";
        var stream = await _blobStorageService.DownloadFileAsync(containerName, fileName);

        if (stream is null)
            return NotFound();

        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> Delete(string fileName)
    {
        var containerName = _configuration["AzureBlobStorage:ContainerName"] ?? "circular-documents";
        var result = await _blobStorageService.DeleteFileAsync(containerName, fileName);

        return result ? NoContent() : NotFound();
    }
}
