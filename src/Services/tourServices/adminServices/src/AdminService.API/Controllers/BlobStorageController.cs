using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AdminService.Infrastructure.Services;

namespace AdminService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobService;
    private const string ContainerName = "admin-images";

    public BlobStorageController(IBlobStorageService blobService) => _blobService = blobService;

    [HttpPost("upload")]
    public async Task<ActionResult<string>> Upload(IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file provided.");

        var blobName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await _blobService.UploadAsync(ContainerName, blobName, stream, file.ContentType, ct);
        return Ok(new { url, blobName });
    }

    [HttpGet("download/{blobName}")]
    public async Task<ActionResult> Download(string blobName, CancellationToken ct)
    {
        var stream = await _blobService.DownloadAsync(ContainerName, blobName, ct);
        if (stream is null) return NotFound();
        return File(stream, "application/octet-stream", blobName);
    }

    [HttpDelete("{blobName}")]
    public async Task<ActionResult> Delete(string blobName, CancellationToken ct)
    {
        var deleted = await _blobService.DeleteAsync(ContainerName, blobName, ct);
        return deleted ? NoContent() : NotFound();
    }
}
