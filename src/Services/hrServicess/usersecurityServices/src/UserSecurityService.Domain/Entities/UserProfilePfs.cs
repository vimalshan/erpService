using UserSecurityService.Domain.Common;

namespace UserSecurityService.Domain.Entities;

/// <summary>Core user profile — aggregate root for the UserSecurity bounded context.</summary>
public class UserProfilePfs : BaseEntity
{
    public string EmUsrId { get; private set; } = null!;        // VARCHAR(25) PK
    public decimal EmEmpNum { get; private set; }
    public string EmUntCod { get; private set; } = null!;       // VARCHAR(3)
    public string EmNickNam { get; private set; } = null!;      // VARCHAR(65)
    public string EmUsrTyp { get; private set; } = null!;       // VARCHAR(1)
    public string EmEmlFlg { get; private set; } = null!;       // VARCHAR(1)
    public string? EmOEmlId { get; private set; }               // Office email
    public string? EmPEmlId { get; private set; }               // Personal email
    public DateTime EmEffDat { get; private set; }
    public DateTime? EmClsDat { get; private set; }
    public string EmUsrPass { get; private set; } = null!;      // Hashed password
    public string? EmEmpNam { get; private set; }
    public DateTime? EmDobDat { get; private set; }
    public string? EmPhtPth { get; private set; }               // Photo path (Blob URL)
    public string? EmDivNam { get; private set; }
    public long? EmJobCod { get; private set; }
    public decimal? EmPinNum { get; private set; }
    public string? EmOldNum { get; private set; }
    public string? EmEmpDsg { get; private set; }
    public string? EmFrsNam { get; private set; }
    public string? EmMidNam { get; private set; }
    public string? EmLstNam { get; private set; }
    public string? EmCurBus { get; private set; }
    public string? EmRepUnt { get; private set; }
    public string? EmCurGrd { get; private set; }
    public DateTime? EmProDat { get; private set; }
    public string? EmCurLoc { get; private set; }
    public string? EmTimUnt { get; private set; }
    public decimal? EmCtcAmt { get; private set; }
    public string? EmEmpSex { get; private set; }
    public decimal? EmAppUsr { get; private set; }
    public string? EmWrkFlg { get; private set; }
    public string? EmSigPth { get; private set; }
    public string? EmOutlook { get; private set; }
    public string EmRegStatus { get; private set; } = null!;    // CHAR(1)

    private UserProfilePfs() { }

    public static UserProfilePfs Create(
        string usrId, decimal empNum, string untCod, string nickNam,
        string usrTyp, string emlFlg, DateTime effDat, string hashedPassword,
        string regStatus, string? empNam = null)
    {
        var profile = new UserProfilePfs
        {
            EmUsrId = usrId,
            EmEmpNum = empNum,
            EmUntCod = untCod,
            EmNickNam = nickNam,
            EmUsrTyp = usrTyp,
            EmEmlFlg = emlFlg,
            EmEffDat = effDat,
            EmUsrPass = hashedPassword,
            EmRegStatus = regStatus,
            EmEmpNam = empNam
        };

        profile.AddDomainEvent(new Events.UserCreatedEvent(usrId, empNum, empNam));
        return profile;
    }

    public void ChangePassword(string hashedPassword)
    {
        EmUsrPass = hashedPassword;
        AddDomainEvent(new Events.PasswordChangedEvent(EmUsrId, EmEmpNum));
    }

    public void UpdatePhotoPath(string blobUrl)
    {
        EmPhtPth = blobUrl;
    }

    public void Deactivate()
    {
        EmClsDat = DateTime.UtcNow;
        EmRegStatus = "I";
    }

    public void UpdateProfile(
        string nickNam, string? empNam, string? frsNam, string? midNam,
        string? lstNam, string? oEmlId, string? pEmlId, string? empDsg)
    {
        EmNickNam = nickNam;
        EmEmpNam = empNam;
        EmFrsNam = frsNam;
        EmMidNam = midNam;
        EmLstNam = lstNam;
        EmOEmlId = oEmlId;
        EmPEmlId = pEmlId;
        EmEmpDsg = empDsg;
    }
}
