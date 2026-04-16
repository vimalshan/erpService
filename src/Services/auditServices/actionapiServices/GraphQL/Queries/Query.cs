using ActionService.Application.DTOs;
using ActionService.Application.Queries;
using ActionService.Models;
using ActionService.Services;
using MediatR;

namespace ActionService.GraphQL.Queries
{
    public class Query
    {
        [GraphQLName("allActions")]
        public async Task<IEnumerable<ActionDto>> GetAllActions([Service] IMediator mediator)
            => await mediator.Send(new GetAllActionsQuery());

        [GraphQLName("actionById")]
        public async Task<ActionDto?> GetActionById([Service] IMediator mediator, int id)
            => await mediator.Send(new GetActionByIdQuery(id));

        [GraphQLName("actionsByEntity")]
        public async Task<IEnumerable<ActionDto>> GetActionsByEntity([Service] IMediator mediator, string entityType, int entityId)
            => await mediator.Send(new GetActionsByEntityQuery(entityType, entityId));

        [GraphQLName("actions")]
        public Task<ApiResponse<ActionsPaginationResponse>> Actions(
            [Service] IActionService actionSvc,
            List<int>? category,
            List<int>? company,
            [GraphQLName("service")] List<int>? serviceFilter,
            List<int>? site,
            bool isHighPriority,
            int pageNumber,
            int pageSize)
        {
            return actionSvc.GetActionsAsync(
                category ?? new List<int>(),
                company ?? new List<int>(),
                serviceFilter ?? new List<int>(),
                site ?? new List<int>(),
                isHighPriority,
                pageNumber,
                pageSize);
        }

        [GraphQLName("actionCategoriesFilter")]
        public Task<ApiResponse<List<ActionFilterItem>>> ActionCategoriesFilter(
            [Service] IActionService service,
            List<int>? companies,
            List<int>? services,
            List<int>? sites)
        {
            return service.GetActionCategoriesAsync(
                companies ?? new List<int>(),
                services ?? new List<int>(),
                sites ?? new List<int>());
        }

        [GraphQLName("actionCompaniesFilter")]
        public Task<ApiResponse<List<ActionFilterItem>>> ActionCompaniesFilter(
            [Service] IActionService service,
            List<int>? categories,
            List<int>? services,
            List<int>? sites)
        {
            return service.GetActionCompaniesAsync(
                categories ?? new List<int>(),
                services ?? new List<int>(),
                sites ?? new List<int>());
        }

        [GraphQLName("actionServicesFilter")]
        public Task<ApiResponse<List<ActionFilterItem>>> ActionServicesFilter(
            [Service] IActionService service,
            List<int>? companies,
            List<int>? categories,
            List<int>? sites)
        {
            return service.GetActionServicesAsync(
                companies ?? new List<int>(),
                categories ?? new List<int>(),
                sites ?? new List<int>());
        }

        [GraphQLName("actionSitesFilter")]
        public Task<ApiResponse<List<ActionSiteNode>>> ActionSitesFilter(
            [Service] IActionService service,
            List<int>? companies,
            List<int>? categories,
            List<int>? services)
        {
            return service.GetActionSitesAsync(
                companies ?? new List<int>(),
                categories ?? new List<int>(),
                services ?? new List<int>());
        }
    }
}
