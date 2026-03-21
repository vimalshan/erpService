using EnergyService.Domain.Entities;
using EnergyService.Domain.Events;
using EnergyService.Domain.Exceptions;

namespace EnergyService.Domain.Aggregates;

public class EnergyProcessAggregate
{
    public EcProcess Process { get; }

    public EnergyProcessAggregate(EcProcess process)
    {
        Process = process ?? throw new DomainException("Process cannot be null.");
    }

    public EcReading RecordReading(string unitCode, long readingValue, long? target, string? remarks, int modifiedBy, long? previousReading)
    {
        if (Process.EcCloseFlag == "Y")
            throw new DomainException("Cannot record reading on a closed process.");

        var actualUsage = readingValue - (previousReading ?? 0);

        var reading = new EcReading
        {
            EbUnitCode = unitCode,
            EbProcessId = Process.EcProcessId,
            EbDate = DateTime.UtcNow,
            EbTarget = target,
            EbReading = readingValue,
            EbActualUsage = actualUsage,
            EbRemarks = remarks,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };

        reading.AddDomainEvent(new ReadingRecordedEvent(
            Process.EcProcessId, unitCode, readingValue, actualUsage, reading.EbDate));

        return reading;
    }

    public void GrantAccess(int employeeSysId, DateTime startDate, int modifiedBy)
    {
        if (Process.EcCloseFlag == "Y")
            throw new DomainException("Cannot grant access on a closed process.");

        var access = new EcProcessAccess
        {
            PaProcessId = Process.EcProcessId,
            PaEmpSysId = employeeSysId,
            PaStartDate = startDate,
            PaLastModifiedBy = modifiedBy,
            PaLastModifiedOn = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")
        };

        Process.ProcessAccesses.Add(access);
        access.AddDomainEvent(new ProcessAccessChangedEvent(
            Process.EcProcessId, employeeSysId, startDate, null));
    }
}
