using MediatR;
using TimeSheetService.Application.DTOs;

namespace TimeSheetService.Application.Commands.CreateTcProject;

public class CreateTcProjectCommand : IRequest<TcProjectDto>
{
    public long ProjectId { get; init; }
    public string ProjectName { get; init; } = string.Empty;
    public long CategoryId { get; init; }
    public DateTime EffectiveDate { get; init; }
    public long TeamId { get; init; }
    public string ListAll { get; init; } = "N";
    public long? OldProjectId { get; init; }
    public long ModifiedBy { get; init; }
}
