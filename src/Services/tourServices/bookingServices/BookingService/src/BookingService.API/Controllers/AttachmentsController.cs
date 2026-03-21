using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookingService.Domain.Interfaces;

namespace BookingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttachmentsController(IBlobStorageService blobStorage) : ControllerBase
{
    private const string ContainerName = "booking-attachments";

    [HttpPost("upload")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0)
            return BadRequest("File is empty");

        var fileName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        await using var stream = file.OpenReadStream();
        var url = await blobStorage.UploadAsync(ContainerName, fileName, stream, file.ContentType, ct);

        return Ok(new { fileName, url });
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> Download(string fileName, CancellationToken ct)
    {
        var stream = await blobStorage.DownloadAsync(ContainerName, fileName, ct);
        if (stream is null)
            return NotFound();

        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete("{fileName}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string fileName, CancellationToken ct)
    {
        await blobStorage.DeleteAsync(ContainerName, fileName, ct);
        return NoContent();
    }
}
