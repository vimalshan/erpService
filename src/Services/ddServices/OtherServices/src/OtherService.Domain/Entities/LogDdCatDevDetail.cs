using OtherService.Domain.Common;
using OtherService.Domain.Events;
using OtherService.Domain.Exceptions;
using OtherService.Domain.ValueObjects;

namespace OtherService.Domain.Entities;

/// <summary>
/// Aggregate Root for LOG_DD_CAT_DEV_DETAIL.
/// Tracks category development detail entries for applicants.
/// </summary>
public sealed class LogDdCatDevDetail : AggregateRoot
{
    // Backing field for EF Core
    private LogDdCatDevDetail() { }

    /// <summary>CT_APP_ID – User ID (NOT NULL, part of composite key)</summary>
    public string AppId { get; private set; } = default!;

    /// <summary>CT_APP_NUM – User Number (NOT NULL, part of composite key)</summary>
    public decimal AppNum { get; private set; }

    /// <summary>CT_REQ_NUM – Request Number</summary>
    public decimal? ReqNum { get; private set; }

    /// <summary>CT_QTN_NUM – Question Number</summary>
    public decimal? QtnNum { get; private set; }

    /// <summary>CT_ANS_SRL – Answer Serial Number</summary>
    public decimal? AnsSrl { get; private set; }

    /// <summary>CT_ENT_DAT – Entry Date</summary>
    public DateTime? EntDat { get; private set; }

    /// <summary>CT_DESC – Areas for Development</summary>
    public string? Desc { get; private set; }

    /// <summary>CT_NEED – Why do you need it?</summary>
    public string? Need { get; private set; }

    public static LogDdCatDevDetail Create(
        string appId,
        decimal appNum,
        decimal? reqNum,
        decimal? qtnNum,
        decimal? ansSrl,
        DateTime? entDat,
        string? desc,
        string? need)
    {
        ValidateAppId(appId);
        ValidateAppNum(appNum);
        ValidateStringLength(desc, 400, nameof(desc));
        ValidateStringLength(need, 400, nameof(need));

        var entity = new LogDdCatDevDetail
        {
            AppId  = appId.Trim(),
            AppNum = appNum,
            ReqNum = reqNum,
            QtnNum = qtnNum,
            AnsSrl = ansSrl,
            EntDat = entDat,
            Desc   = desc?.Trim(),
            Need   = need?.Trim()
        };

        entity.AddDomainEvent(new LogDdCatDevDetailCreatedEvent(entity));
        return entity;
    }

    public void Update(
        decimal? reqNum,
        decimal? qtnNum,
        decimal? ansSrl,
        DateTime? entDat,
        string? desc,
        string? need)
    {
        ValidateStringLength(desc, 400, nameof(desc));
        ValidateStringLength(need, 400, nameof(need));

        ReqNum = reqNum;
        QtnNum = qtnNum;
        AnsSrl = ansSrl;
        EntDat = entDat;
        Desc   = desc?.Trim();
        Need   = need?.Trim();

        AddDomainEvent(new LogDdCatDevDetailUpdatedEvent(this));
    }

    // ───── Guards ─────────────────────────────────────────────────────────

    private static void ValidateAppId(string appId)
    {
        if (string.IsNullOrWhiteSpace(appId))
            throw new DomainException("AppId (CT_APP_ID) is required.");
        if (appId.Length > 30)
            throw new DomainException("AppId (CT_APP_ID) cannot exceed 30 characters.");
    }

    private static void ValidateAppNum(decimal appNum)
    {
        if (appNum < 0)
            throw new DomainException("AppNum (CT_APP_NUM) must be non-negative.");
    }

    private static void ValidateStringLength(string? value, int maxLength, string fieldName)
    {
        if (value is not null && value.Length > maxLength)
            throw new DomainException($"{fieldName} cannot exceed {maxLength} characters.");
    }
}
