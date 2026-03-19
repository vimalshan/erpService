using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleTracking.Domain.Interfaces;

namespace VehicleTracking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobController(IBlobStorageService blobService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string containerName = "vehicle-images")
    {
        if (file.Length == 0) return BadRequest("File is empty.");

        var blobName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var uri = await blobService.UploadAsync(containerName, blobName, stream, file.ContentType);
        return Ok(new { Uri = uri, BlobName = blobName });
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string containerName, [FromQuery] string blobName)
    {
        var stream = await blobService.DownloadAsync(containerName, blobName);
        if (stream is null) return NotFound();
        return File(stream, "application/octet-stream", blobName);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string containerName, [FromQuery] string blobName)
    {
        await blobService.DeleteAsync(containerName, blobName);
        return NoContent();
    }
}
