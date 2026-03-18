using MasterService.Domain.Common;
using MasterService.Domain.Events;

namespace MasterService.Domain.Entities;

/// <summary>Aggregate: JOB_MAST</summary>
public sealed class JobMaster : AggregateRoot
{
    public long JobCode { get; private set; }
    public string JobName { get; private set; } = string.Empty;
    public string CategoryCode { get; private set; } = string.Empty;
    public long? SerialNumber { get; private set; }

    private JobMaster() { }

    public static JobMaster Create(long jobCode, string jobName, string categoryCode, long? serialNumber = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jobName);
        ArgumentException.ThrowIfNullOrWhiteSpace(categoryCode);
        if (jobCode <= 0) throw new ArgumentException("JobCode must be positive.", nameof(jobCode));

        var job = new JobMaster
        {
            JobCode = jobCode,
            JobName = jobName.Trim(),
            CategoryCode = categoryCode.Trim().ToUpper(),
            SerialNumber = serialNumber
        };

        job.AddDomainEvent(new JobCreatedEvent(job.JobCode, job.JobName, job.CategoryCode));
        return job;
    }

    public void Update(string jobName, string categoryCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jobName);
        ArgumentException.ThrowIfNullOrWhiteSpace(categoryCode);
        JobName = jobName.Trim();
        CategoryCode = categoryCode.Trim().ToUpper();
    }
}
