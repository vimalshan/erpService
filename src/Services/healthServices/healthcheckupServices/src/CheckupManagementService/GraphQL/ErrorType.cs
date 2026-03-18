using HotChocolate.Types;

namespace CheckupManagementService.GraphQL;

/// <summary>
/// GraphQL type for error responses
/// </summary>
public class ErrorInfo
{
    public string Message { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int? StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// GraphQL Object Type for Error responses
/// </summary>
public class ErrorInfoType : ObjectType<ErrorInfo>
{
    protected override void Configure(IObjectTypeDescriptor<ErrorInfo> descriptor)
    {
        descriptor
            .Description("Error information returned from API");

        descriptor
            .Field(f => f.Message)
            .Description("Error message");

        descriptor
            .Field(f => f.Code)
            .Description("Error code");

        descriptor
            .Field(f => f.StatusCode)
            .Description("HTTP status code");

        descriptor
            .Field(f => f.Timestamp)
            .Description("Timestamp when the error occurred");
    }
}
