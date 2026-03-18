using ErrorLoggingService.Application.DTOs;
using ErrorLoggingService.Domain.Entities;

namespace ErrorLoggingService.API.GraphQL;

[GraphQLName("ErrorLog")]
public class ErrorLogType
{
    public int Id { get; set; }
    public string? ErrorMessage { get; set; }
    public string? StoredProcedureName { get; set; }
    public int? ErrorReference { get; set; }
    public DateTime? ErrorDate { get; set; }

    public static ErrorLogType FromDto(ErrorLogDto dto) => new()
    {
        Id = dto.Id,
        ErrorMessage = dto.ErrorMessage,
        StoredProcedureName = dto.StoredProcedureName,
        ErrorReference = dto.ErrorReference,
        ErrorDate = dto.ErrorDate
    };
}
