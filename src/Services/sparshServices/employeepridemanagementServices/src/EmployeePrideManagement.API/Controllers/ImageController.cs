using EmployeePrideManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePrideManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ImageController : ControllerBase
{
    private readonly IBlobStorageService _blobService;

    public ImageController(IBlobStorageService blobService)
    {
        _blobService = blobService;
    }

    [HttpPost("upload")]
    [ProducesResponseType(typeof(ImageUploadResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
            return BadRequest("No file provided.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Invalid file type. Allowed: jpeg, png, gif, webp.");

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File size exceeds 5MB limit.");

        using var stream = file.OpenReadStream();
        var blobName = await _blobService.UploadImageAsync(stream, file.FileName, file.ContentType, cancellationToken);
        var url = await _blobService.GetImageUrlAsync(blobName, cancellationToken);

        return Ok(new ImageUploadResponse { BlobName = blobName, Url = url });
    }

    [HttpDelete("{*blobName}")]
    public async Task<IActionResult> Delete(string blobName, CancellationToken cancellationToken)
    {
        await _blobService.DeleteImageAsync(blobName, cancellationToken);
        return NoContent();
    }
}

public class ImageUploadResponse
{
    public string BlobName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
