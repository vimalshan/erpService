namespace EmployeeService.Shared.Messaging;

public class EmployeeEventMessage
{
    public string EventType { get; set; } = string.Empty;
    public long EmployeeId { get; set; }
    public DateTime OccurredOn { get; set; }
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, string> Attributes { get; set; } = new();
}