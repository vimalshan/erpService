namespace RequestServices.Domain.Entities;

/// <summary>Represents REQUEST_NEW — skills attached to a new training request.</summary>
public class RequestNew
{
    public long   RequestId      { get; private set; }
    public long   SerialNumber   { get; private set; }
    public string SkillName      { get; private set; } = default!;
    public long   LevelNumber    { get; private set; }
    public string FunctionDescription { get; private set; } = default!;
    public string CategoryCode   { get; private set; } = default!;
    public string SkillType      { get; private set; } = default!;
    public string StatusCode     { get; private set; } = default!;
    public long   CourseId       { get; private set; }

    private RequestNew() { }

    public static RequestNew Create(
        long requestId, long serialNumber, string skillName,
        long levelNumber, string functionDescription, string categoryCode,
        string skillType, string statusCode, long courseId)
    {
        return new RequestNew
        {
            RequestId           = requestId,
            SerialNumber        = serialNumber,
            SkillName           = skillName,
            LevelNumber         = levelNumber,
            FunctionDescription = functionDescription,
            CategoryCode        = categoryCode,
            SkillType           = skillType,
            StatusCode          = statusCode,
            CourseId            = courseId
        };
    }
}
