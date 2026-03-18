using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectService.Domain.Interfaces;

namespace ProjectService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController(IBlobStorageService blobStorage) : ControllerBase
{
    [HttpPost("upload/{containerName}")]
    public async Task<IActionResult> Upload(string containerName, IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
            return BadRequest("File is empty.");

        var allowedExtensions = new[] { ".pdf", ".xlsx", ".docx", ".pptx", ".png", ".jpg", ".jpeg" };
        var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.AsEnumerable().Contains(extension))
            return BadRequest("File type not allowed.");

        using var stream = file.OpenReadStream();
        var fileName = $"{Guid.NewGuid()}{extension}";
        var url = await blobStorage.UploadFileAsync(containerName, fileName, stream, file.ContentType, cancellationToken);


        return Ok(new { fileName, url });
    }

    [HttpGet("download/{containerName}/{fileName}")]
    public async Task<IActionResult> Download(string containerName, string fileName, CancellationToken cancellationToken)
    {
        var stream = await blobStorage.DownloadFileAsync(containerName, fileName, cancellationToken);
        if (stream is null) return NotFound();
        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete("{containerName}/{fileName}")]
    public async Task<IActionResult> Delete(string containerName, string fileName, CancellationToken cancellationToken)
    {
        var deleted = await blobStorage.DeleteFileAsync(containerName, fileName, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
