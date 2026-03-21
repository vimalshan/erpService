using MasterDataService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobService;
    private const string ContainerName = "stationery-images";

    public BlobStorageController(IBlobStorageService blobService) => _blobService = blobService;

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0) return BadRequest("No file provided.");

        var blobName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await _blobService.UploadAsync(ContainerName, blobName, stream, file.ContentType, cancellationToken);
        return Ok(new { url, blobName });
    }

    [HttpGet("{blobName}")]
    [AllowAnonymous]
    public async Task<IActionResult> Download(string blobName, CancellationToken cancellationToken)
    {
        var stream = await _blobService.DownloadAsync(ContainerName, blobName, cancellationToken);
        if (stream is null) return NotFound();
        return File(stream, "application/octet-stream", blobName);
    }

    [HttpDelete("{blobName}")]
    public async Task<IActionResult> Delete(string blobName, CancellationToken cancellationToken)
    {
        var deleted = await _blobService.DeleteAsync(ContainerName, blobName, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
