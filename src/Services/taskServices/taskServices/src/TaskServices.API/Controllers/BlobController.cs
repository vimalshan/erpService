using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskServices.Infrastructure.BlobStorage;

namespace TaskServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobController : ControllerBase
{
    private readonly IBlobStorageService _blobService;
    private readonly IConfiguration _configuration;

    public BlobController(IBlobStorageService blobService, IConfiguration configuration)
    {
        _blobService = blobService;
        _configuration = configuration;
    }

    [HttpPost("upload")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
            return BadRequest("File is empty.");

        var containerName = _configuration["AzureBlobStorage:ContainerName"] ?? "task-attachments";
        var blobName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";

        using var stream = file.OpenReadStream();
        var uri = await _blobService.UploadAsync(containerName, blobName, stream, file.ContentType, cancellationToken);

        return Ok(new { Uri = uri, BlobName = blobName });
    }

    [HttpGet("download/{blobName}")]
    public async Task<IActionResult> Download(string blobName, CancellationToken cancellationToken)
    {
        var containerName = _configuration["AzureBlobStorage:ContainerName"] ?? "task-attachments";
        var stream = await _blobService.DownloadAsync(containerName, blobName, cancellationToken);

        if (stream is null)
            return NotFound();

        return File(stream, "application/octet-stream", blobName);
    }

    [HttpDelete("{blobName}")]
    public async Task<IActionResult> Delete(string blobName, CancellationToken cancellationToken)
    {
        var containerName = _configuration["AzureBlobStorage:ContainerName"] ?? "task-attachments";
        await _blobService.DeleteAsync(containerName, blobName, cancellationToken);
        return NoContent();
    }
}
