namespace HRDocumentService.Application.DTOs;

public class HRDocumentDto
{
    public long DocId { get; set; }
    public long DocNo { get; set; }
    public string DocType { get; set; } = null!;
    public long DocPayRefNo { get; set; }
    public long DocLocId { get; set; }
    public long DocUnitId { get; set; }
    public string DocRemarks { get; set; } = null!;
    public long DocUserId { get; set; }
    public string? DocRefNo { get; set; }
    public string? DocRefName { get; set; }
    public DateTime DocCreatedOn { get; set; }
    public string DocDocStatus { get; set; } = null!;
    public string DocSource { get; set; } = null!;
    public string? DocActionStatus { get; set; }
    public DateTime? DocActionTakenOn { get; set; }
    public decimal? DocActionTakenBy { get; set; }
    public string? DocFilePath { get; set; }
    public string? DocCancelFlag { get; set; }
    public decimal? DocCancelBy { get; set; }
    public DateTime? DocCancelOn { get; set; }
    public decimal? DocPayBy { get; set; }
    public string? DocRejectRemarks { get; set; }
    public List<HRDocumentFileDto> Files { get; set; } = [];
    public List<HRDocumentReceiptDto> Receipts { get; set; } = [];
}

public class HRDocumentFileDto
{
    public long FileId { get; set; }
    public long FileDocId { get; set; }
    public string FilePath { get; set; } = null!;
    public string FileName { get; set; } = null!;
}

public class HRDocumentReceiptDto
{
    public long HRRecId { get; set; }
    public long HRRecEnvId { get; set; }
    public long HRRecHRDocId { get; set; }
    public long HRRecUpdatedBy { get; set; }
    public DateTime HRRecUpdatedOn { get; set; }
}
