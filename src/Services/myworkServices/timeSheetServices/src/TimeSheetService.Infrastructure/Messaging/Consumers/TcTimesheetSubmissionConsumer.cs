using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TimeSheetService.Application.Commands.SubmitTcTimesheet;

namespace TimeSheetService.Infrastructure.Messaging.Consumers;

public record TcTimesheetSubmissionMessage
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

public class TcTimesheetSubmissionConsumer : BaseMessageConsumer<TcTimesheetSubmissionMessage>
{
    public TcTimesheetSubmissionConsumer(IServiceProvider serviceProvider, ILogger<TcTimesheetSubmissionConsumer> logger)
        : base(serviceProvider, logger, "tc.timesheet.submission")
    {
    }

    protected override async Task HandleMessageAsync(TcTimesheetSubmissionMessage message, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new SubmitTcTimesheetCommand
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
