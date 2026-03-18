using MedicineManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicineManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BlobController(IBlobStorageService blobStorage) : ControllerBase
{
    [HttpPost("upload/{containerName}")]
    public async Task<ActionResult<string>> Upload(string containerName, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("File is empty");
        using var stream = file.OpenReadStream();
        var url = await blobStorage.UploadAsync(containerName, file.FileName, stream, file.ContentType, ct);
        return Ok(new { Url = url });
    }

    [HttpGet("download/{containerName}/{fileName}")]
    public async Task<IActionResult> Download(string containerName, string fileName, CancellationToken ct)
    {
        var stream = await blobStorage.DownloadAsync(containerName, fileName, ct);
        if (stream is null) return NotFound();
        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete("{containerName}/{fileName}")]
    public async Task<IActionResult> Delete(string containerName, string fileName, CancellationToken ct)
    {
        await blobStorage.DeleteAsync(containerName, fileName, ct);
        return NoContent();
    }
}
