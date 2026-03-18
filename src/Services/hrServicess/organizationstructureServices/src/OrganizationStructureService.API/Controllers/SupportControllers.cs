using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationStructureService.Infrastructure.Dapper;
using OrganizationStructureService.Infrastructure.Storage;

namespace OrganizationStructureService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class ReportController : ControllerBase
{
    private readonly OrganizationDapperQueries _queries;

    public ReportController(OrganizationDapperQueries queries) => _queries = queries;

    [HttpGet("unit-hierarchy/{businessId:decimal}")]
    public async Task<IActionResult> GetUnitHierarchy(decimal businessId)
    {
        var result = await _queries.GetUnitHierarchyAsync(businessId);
        return Ok(result);
    }

    [HttpGet("positions-by-grade/{gradeId:decimal}")]
    public async Task<IActionResult> GetPositionsByGrade(decimal gradeId)
    {
        var result = await _queries.GetActivePositionsByGradeAsync(gradeId);
        return Ok(result);
    }

    [HttpGet("sites-by-unit/{unitCode}")]
    public async Task<IActionResult> GetSitesByUnit(string unitCode)
    {
        var result = await _queries.GetSitesByUnitAsync(unitCode);
        return Ok(result);
    }

    [HttpGet("grades-by-unit/{unitCode}")]
    public async Task<IActionResult> GetGradesByUnit(string unitCode)
    {
        var result = await _queries.GetGradesByUnitAsync(unitCode);
        return Ok(result);
    }
}

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IJwtTokenService jwtTokenService) => _jwtTokenService = jwtTokenService;

    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetToken([FromBody] LoginRequest request)
    {
        // Demo: accept admin/admin for local dev — replace with real user store
        if (request.Username == "admin" && request.Password == "admin")
        {
            var token = _jwtTokenService.GenerateToken(request.Username, new[] { "Admin", "User" });
            return Ok(new TokenResponse(token));
        }
        return Unauthorized();
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class BlobController : ControllerBase
{
    private readonly IBlobStorageService _blobStorage;
    private const string ContainerName = "site-images";

    public BlobController(IBlobStorageService blobStorage) => _blobStorage = blobStorage;

    [HttpPost("upload/{fileName}")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB
    public async Task<IActionResult> Upload(string fileName, IFormFile file, CancellationToken ct)
    {
        if (file.Length == 0) return BadRequest("File is empty.");
        using var stream = file.OpenReadStream();
        var url = await _blobStorage.UploadAsync(ContainerName, fileName, stream, file.ContentType, ct);
        return Ok(new { Url = url });
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> Delete(string fileName, CancellationToken ct)
    {
        await _blobStorage.DeleteAsync(ContainerName, fileName, ct);
        return NoContent();
    }
}

public record LoginRequest(string Username, string Password);
public record TokenResponse(string Token);
