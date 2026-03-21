using TravelService.Application.DTOs;
using TravelService.Domain.Repositories;

namespace TravelService.API.GraphQL;

public class TravelQuery
{
    public async Task<TourPlanDto?> GetTourPlan(
        string id,
        [Service] ITourPlanRepository repository,
        CancellationToken cancellationToken = default)
    {
        var tp = await repository.GetByIdAsync(id, cancellationToken);
        if (tp is null) return null;

        return new TourPlanDto
        {
            Id = tp.Id,
            EmployeeSysId = tp.EmployeeSysId,
            StartDate = tp.StartDate,
            EndDate = tp.EndDate,
            Purpose = tp.Purpose,
            Status = tp.Status,
            Category = tp.Category,
            FromCityName = tp.FromCity.CityName,
            ToCityName = tp.ToCity.CityName,
            CreatedBy = tp.CreatedBy,
            CreatedOn = tp.CreatedOn
        };
    }

    public async Task<IEnumerable<TourPlanDto>> GetTourPlans(
        [Service] ITourPlanRepository repository,
        int page = 1, int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var items = await repository.GetAllAsync(page, pageSize, cancellationToken);
        return items.Select(tp => new TourPlanDto
        {
            Id = tp.Id,
            EmployeeSysId = tp.EmployeeSysId,
            StartDate = tp.StartDate,
            Status = tp.Status,
            Category = tp.Category,
            Purpose = tp.Purpose,
            FromCityName = tp.FromCity.CityName,
            ToCityName = tp.ToCity.CityName
        });
    }

    public async Task<BatchMainDto?> GetBatch(
        string id,
        [Service] IBatchRepository repository,
        CancellationToken cancellationToken = default)
    {
        var batch = await repository.GetByIdAsync(id, cancellationToken);
        if (batch is null) return null;

        return new BatchMainDto
        {
            Id = batch.Id,
            AdminId = batch.AdminId,
            Status = batch.Status,
            BatchDate = batch.BatchDate,
            TotalPayable = batch.TotalPayable
        };
    }
}

public class TravelMutation
{
    public async Task<string> ApproveTourPlan(
        string tourPlanId, string approvedBy,
        [Service] ITourPlanRepository repository,
        CancellationToken cancellationToken = default)
    {
        var tp = await repository.GetByIdAsync(tourPlanId, cancellationToken);
        if (tp is null) throw new GraphQLException($"TourPlan '{tourPlanId}' not found.");
        tp.Approve(approvedBy);
        await repository.UpdateAsync(tp, cancellationToken);
        return $"TourPlan {tourPlanId} approved by {approvedBy}.";
    }
}
