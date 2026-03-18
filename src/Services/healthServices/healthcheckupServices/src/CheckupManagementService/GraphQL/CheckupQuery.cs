using HotChocolate;
using MediatR;
using CheckupManagementService.Application.Queries;
using CheckupManagementService.DTOs;

namespace CheckupManagementService.GraphQL;

/// <summary>
/// GraphQL Query types for Checkup Management
/// </summary>
[QueryType]
public class CheckupQuery
{
    /// <summary>
    /// Get all checkups paginated
    /// </summary>
    public async Task<GetCheckupsResponse> GetCheckups(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 10,
        string? status = null,
        string? employeeNumber = null,
        string? checkupType = null)
    {
        var query = new GetCheckupsQuery 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize,
            Status = status,
            EmployeeNumber = employeeNumber,
            CheckupType = checkupType
        };
        var result = await mediator.Send(query);
        return result ?? new GetCheckupsResponse();
    }

    /// <summary>
    /// Get checkup by ID
    /// </summary>
    public async Task<CheckupMasterDto?> GetCheckupById(
        [Service] IMediator mediator,
        string checkupMasterId)
    {
        var query = new GetCheckupByIdQuery { CheckupMasterId = checkupMasterId };
        return await mediator.Send(query);
    }

    /// <summary>
    /// Get checkups by employee
    /// </summary>
    public async Task<GetCheckupsResponse> GetCheckupsByEmployee(
        [Service] IMediator mediator,
        string employeeNumber,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = new GetCheckupsByEmployeeQuery 
        { 
            EmployeeNumber = employeeNumber, 
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var result = await mediator.Send(query);
        return result ?? new GetCheckupsResponse();
    }

    /// <summary>
    /// Get health examination by ID
    /// </summary>
    public async Task<HealthMainDto?> GetHealthExamination(
        [Service] IMediator mediator,
        string healthId)
    {
        var query = new GetHealthExaminationQuery { HealthId = healthId };
        return await mediator.Send(query);
    }

    /// <summary>
    /// Get test masters
    /// </summary>
    public async Task<GetTestMastersResponse> GetTestMasters(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 10,
        bool? isActive = null,
        string? category = null)
    {
        var query = new GetTestMastersQuery 
        { 
            PageNumber = pageNumber, 
            PageSize = pageSize,
            IsActive = isActive,
            Category = category
        };
        var result = await mediator.Send(query);
        return result ?? new GetTestMastersResponse();
    }

    /// <summary>
    /// Get health check card
    /// </summary>
    public async Task<HealthCheckCardDto?> GetHealthCheckCard(
        [Service] IMediator mediator,
        string cardNumber)
    {
        var query = new GetHealthCheckCardQuery { CardNumber = cardNumber };
        return await mediator.Send(query);
    }

    /// <summary>
    /// Get checkup status report
    /// </summary>
    public async Task<CheckupManagementService.Application.Queries.CheckupStatusReportDto> GetCheckupStatusReport(
        [Service] IMediator mediator,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        var query = new GetCheckupStatusReportQuery { FromDate = fromDate, ToDate = toDate };
        return await mediator.Send(query) ?? new CheckupManagementService.Application.Queries.CheckupStatusReportDto();
    }
}
