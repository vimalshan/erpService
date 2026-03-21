using LookupService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LookupService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StationeryImagesController(IBlobStorageService blobService) : ControllerBase
{
    private const string ContainerName = "stationery-images";

    [HttpPost("{itemName}")]
    public async Task<IActionResult> Upload(string itemName, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("File is empty");

        var blobName = $"{itemName}/{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await blobService.UploadAsync(ContainerName, blobName, stream, file.ContentType, ct);
        return Ok(new { Url = url, BlobName = blobName });
    }

    [HttpGet("{*blobName}")]
    public async Task<IActionResult> Download(string blobName, CancellationToken ct)
    {
        var stream = await blobService.DownloadAsync(ContainerName, blobName, ct);
        if (stream is null) return NotFound();
        return File(stream, "application/octet-stream", System.IO.Path.GetFileName(blobName));
    }

    [HttpDelete("{*blobName}")]
    public async Task<IActionResult> Delete(string blobName, CancellationToken ct)
        => await blobService.DeleteAsync(ContainerName, blobName, ct) ? NoContent() : NotFound();
}
