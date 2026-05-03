using DocumentService.Domain.Entities;
using DocumentService.Infrastructure.Data;
using DocumentService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly ILogger<DocumentsController> _logger;
    private readonly DocumentDbContext _db;

    public DocumentsController(ILogger<DocumentsController> logger, DocumentDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? auditId, [FromQuery] int? findingId, [FromQuery] int? certificateId, [FromQuery] int? contractId)
    {
        var q = _db.Documents.AsNoTracking().Where(d => !d.IsDeleted);
        if (auditId.HasValue)       q = q.Where(d => d.AuditId == auditId);
        if (findingId.HasValue)     q = q.Where(d => d.FindingId == findingId);
        if (certificateId.HasValue) q = q.Where(d => d.CertificateId == certificateId);
        if (contractId.HasValue)    q = q.Where(d => d.ContractId == contractId);

        var data = await q.OrderByDescending(d => d.UploadedDate)
            .Select(d => new DocumentDto
            {
                DocumentId   = d.DocumentId.ToString(),
                FileName     = d.FileName,
                FileSize     = d.FileSize,
                ContentType  = d.ContentType,
                UploadedDate = d.UploadedDate
            })
            .ToListAsync();

        return Ok(new ApiResponse<List<DocumentDto>> { IsSuccess = true, Message = "Success", Data = data });
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadDocument([FromQuery] string documentId)
    {
        if (!Guid.TryParse(documentId, out var id))
            return BadRequest(new ApiResponse<object> { IsSuccess = false, Message = "Invalid documentId", ErrorCode = "INVALID_ID" });

        var doc = await _db.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentId == id && !d.IsDeleted);
        if (doc is null)
            return NotFound(new ApiResponse<object> { IsSuccess = false, Message = "Document not found", ErrorCode = "NOT_FOUND" });

        _logger.LogInformation("Downloading document: {DocumentId}", documentId);
        var content = System.Text.Encoding.UTF8.GetBytes($"Document {doc.FileName} content placeholder");
        return File(content, doc.ContentType, doc.FileName);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadDocument([FromQuery] int? auditId, [FromQuery] int? findingId, [FromQuery] int? certificateId, [FromQuery] int? contractId, [FromQuery] string? category)
    {
        var files = Request.Form.Files;
        if (files.Count == 0)
            return BadRequest(new ApiResponse<object> { IsSuccess = false, Message = "No files provided", ErrorCode = "NO_FILES" });

        _logger.LogInformation("Uploading {FileCount} documents", files.Count);
        var uploaded = new List<DocumentDto>();

        foreach (var file in files)
        {
            var entity = new Document
            {
                DocumentId    = Guid.NewGuid(),
                FileName      = file.FileName,
                ContentType   = string.IsNullOrEmpty(file.ContentType) ? "application/octet-stream" : file.ContentType,
                FileSize      = file.Length,
                Category      = category,
                AuditId       = auditId,
                FindingId     = findingId,
                CertificateId = certificateId,
                ContractId    = contractId,
                UploadedBy    = User.Identity?.Name,
                UploadedDate  = DateTime.UtcNow
            };
            _db.Documents.Add(entity);
            uploaded.Add(new DocumentDto
            {
                DocumentId   = entity.DocumentId.ToString(),
                FileName     = entity.FileName,
                FileSize     = entity.FileSize,
                ContentType  = entity.ContentType,
                UploadedDate = entity.UploadedDate
            });
        }
        await _db.SaveChangesAsync();

        return Ok(new ApiResponse<List<DocumentDto>> { IsSuccess = true, Message = "Documents uploaded successfully", Data = uploaded });
    }

    [HttpDelete("DeleteDocument")]
    public async Task<IActionResult> DeleteDocument([FromQuery] string documentId, [FromQuery] int? auditId = null, [FromQuery] int? findingId = null)
    {
        if (!Guid.TryParse(documentId, out var id))
            return BadRequest(new ApiResponse<object> { IsSuccess = false, Message = "Invalid documentId", ErrorCode = "INVALID_ID" });

        var doc = await _db.Documents.FirstOrDefaultAsync(d => d.DocumentId == id);
        if (doc is null || doc.IsDeleted)
            return NotFound(new ApiResponse<object> { IsSuccess = false, Message = "Document not found", ErrorCode = "NOT_FOUND" });

        doc.IsDeleted = true;
        doc.DeletedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        _logger.LogInformation("Deleted document: {DocumentId}", documentId);

        return Ok(new ApiResponse<object> { IsSuccess = true, Message = "Document deleted successfully" });
    }

    [HttpPost("Bulkdownload")]
    public async Task<IActionResult> BulkDownloadDocuments([FromQuery] string docType, [FromBody] string[] documentIds)
    {
        var ids = documentIds.Select(s => Guid.TryParse(s, out var g) ? g : Guid.Empty).Where(g => g != Guid.Empty).ToArray();
        var count = await _db.Documents.CountAsync(d => ids.Contains(d.DocumentId) && !d.IsDeleted);
        _logger.LogInformation("Bulk downloading {DocumentCount} documents (type={DocType}, found={Found})", documentIds.Length, docType, count);

        var zipContent = System.Text.Encoding.UTF8.GetBytes($"ZIP archive placeholder ({count} of {documentIds.Length} resolved)");
        return File(zipContent, "application/zip", $"{docType}-documents.zip");
    }

    [HttpGet("ContractList")]
    public async Task<IActionResult> GetContractList()
    {
        _logger.LogInformation("Retrieving contract document list");
        var data = await _db.Documents.AsNoTracking()
            .Where(d => d.ContractId != null && !d.IsDeleted)
            .OrderByDescending(d => d.UploadedDate)
            .Select(d => new DocumentDto
            {
                DocumentId   = d.DocumentId.ToString(),
                FileName     = d.FileName,
                FileSize     = d.FileSize,
                ContentType  = d.ContentType,
                UploadedDate = d.UploadedDate
            })
            .ToListAsync();

        return Ok(new ApiResponse<List<DocumentDto>> { IsSuccess = true, Message = "Contract list retrieved successfully", Data = data });
    }

    [HttpPost("ExportContract")]
    public async Task<IActionResult> ExportContractsToExcel([FromBody] object filterCriteria)
    {
        _logger.LogInformation("Exporting contracts to Excel");
        var count = await _db.Documents.CountAsync(d => d.ContractId != null && !d.IsDeleted);
        var excelContent = System.Text.Encoding.UTF8.GetBytes($"Excel placeholder ({count} contract documents)");
        return File(excelContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "contracts.xlsx");
    }
}
