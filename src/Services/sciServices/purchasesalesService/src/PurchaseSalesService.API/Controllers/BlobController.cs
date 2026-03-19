using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurchaseSalesService.Infrastructure.Storage;

namespace PurchaseSalesService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class BlobController : ControllerBase
{
    private readonly BlobStorageService _blobService;

    public BlobController(BlobStorageService blobService) => _blobService = blobService;

    [HttpPost("upload")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10MB limit
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("No file provided.");
        await using var stream = file.OpenReadStream();
        var url = await _blobService.UploadImageAsync(file.FileName, stream, file.ContentType, ct);
        return Ok(new { url });
    }

    [HttpDelete("{blobName}")]
    public async Task<IActionResult> Delete(string blobName, CancellationToken ct)
    {
        await _blobService.DeleteImageAsync(blobName, ct);
        return NoContent();
    }
}
