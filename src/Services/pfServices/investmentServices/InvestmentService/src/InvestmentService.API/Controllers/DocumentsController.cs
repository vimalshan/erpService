using InvestmentService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly IBlobStorageService _blobStorage;
    private const string ContainerName = "investment-documents";

    public DocumentsController(IBlobStorageService blobStorage) => _blobStorage = blobStorage;

    [HttpPost("upload/{investmentNo:long}")]
    public async Task<IActionResult> Upload(long investmentNo, IFormFile file)
    {
        if (file.Length == 0) return BadRequest("File is empty");

        var blobName = $"{investmentNo}/{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        var url = await _blobStorage.UploadAsync(ContainerName, blobName, stream, file.ContentType);
        return Ok(new { Url = url, BlobName = blobName });
    }

    [HttpGet("download/{*blobName}")]
    public async Task<IActionResult> Download(string blobName)
    {
        var stream = await _blobStorage.DownloadAsync(ContainerName, blobName);
        if (stream == null) return NotFound();
        return File(stream, "application/octet-stream", System.IO.Path.GetFileName(blobName));
    }

    [HttpDelete("{*blobName}")]
    public async Task<IActionResult> Delete(string blobName)
    {
        var deleted = await _blobStorage.DeleteAsync(ContainerName, blobName);
        return deleted ? Ok() : NotFound();
    }
}
