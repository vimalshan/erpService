using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TimeSheetService.Application.Commands.SubmitTimesheet;

namespace TimeSheetService.Infrastructure.Messaging.Consumers;

public record TimesheetSubmissionMessage
{
    public long TimeId { get; init; }
    public long EmployeeSysId { get; init; }
    public DateTime TimeDate { get; init; }
    public DateTime? TimeIn { get; init; }
    public DateTime? TimeOut { get; init; }
    public long TotalHours { get; init; }
    public string? Remarks { get; init; }
    public char EntryTypeCode { get; init; } = 'S';
    public long ModifiedBy { get; init; }
}

public class TimesheetSubmissionConsumer : BaseMessageConsumer<TimesheetSubmissionMessage>
{
    public TimesheetSubmissionConsumer(IServiceProvider serviceProvider, ILogger<TimesheetSubmissionConsumer> logger)
        : base(serviceProvider, logger, "timesheet.submission")
    {
    }

    protected override async Task HandleMessageAsync(TimesheetSubmissionMessage message, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new SubmitTimesheetCommand
        {
            TimeId = message.TimeId,
            EmployeeSysId = message.EmployeeSysId,
            TimeDate = message.TimeDate,
            TimeIn = message.TimeIn,
            TimeOut = message.TimeOut,
            TotalHours = message.TotalHours,
            Remarks = message.Remarks,
            EntryTypeCode = message.EntryTypeCode.ToString(),
            ModifiedBy = message.ModifiedBy
        }, cancellationToken);
    }
}
