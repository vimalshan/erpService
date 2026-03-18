using FinyearAPI.GraphQL.Types;
using FinyearAPI.Application.Services;
using FinyearAPI.Application.DTOs;
using FinyearAPI.Domain.Entities;

namespace FinyearAPI.GraphQL.Mutations
{
    /// <summary>
    /// GraphQL Mutation type for financial year operations
    /// </summary>
    public class FinancialYearMutation
    {
        /// <summary>
        /// Create a new financial year
        /// </summary>
        public async Task<FinancialYearMutationPayload> CreateFinancialYear(
            [Service] IFinancialYearService financialYearService,
            CreateFinancialYearInput input)
        {
            try
            {
                var dto = new CreateFinancialYearDto
                {
                    FinancialYearId = input.Id,
                    FinancialYearName = input.Name,
                    StartDate = input.StartDate,
                    CloseDate = input.EndDate,
                    UpdatedBy = 1 // GraphQL user
                };

                var result = await financialYearService.CreateFinancialYearAsync(dto);

                return new FinancialYearMutationPayload
                {
                    Success = true,
                    Message = "Financial year created successfully",
                    FinancialYear = new FinancialYearType
                    {
                        Id = result.FinancialYearId,
                        Name = result.FinancialYearName,
                        StartDate = result.StartDate,
                        EndDate = result.CloseDate,
                        DurationInDays = (int)(result.CloseDate - result.StartDate).TotalDays,
                        Status = result.IsActive ? "Active" : "Inactive",
                        IsActive = result.IsActive,
                        UpdatedOn = result.UpdatedOn
                    }
                };
            }
            catch (Exception ex)
            {
                return new FinancialYearMutationPayload
                {
                    Success = false,
                    Message = "Failed to create financial year",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Update an existing financial year
        /// </summary>
        public async Task<FinancialYearMutationPayload> UpdateFinancialYear(
            [Service] IFinancialYearService financialYearService,
            UpdateFinancialYearInput input)
        {
            try
            {
                var dto = new UpdateFinancialYearDto
                {
                    FinancialYearName = input.Name,
                    StartDate = input.StartDate,
                    CloseDate = input.EndDate,
                    UpdatedBy = 1 // GraphQL user
                };

                var result = await financialYearService.UpdateFinancialYearAsync(input.Id, dto);

                return new FinancialYearMutationPayload
                {
                    Success = true,
                    Message = "Financial year updated successfully",
                    FinancialYear = new FinancialYearType
                    {
                        Id = result.FinancialYearId,
                        Name = result.FinancialYearName,
                        StartDate = result.StartDate,
                        EndDate = result.CloseDate,
                        DurationInDays = (int)(result.CloseDate - result.StartDate).TotalDays,
                        Status = result.IsActive ? "Active" : "Inactive",
                        IsActive = result.IsActive,
                        UpdatedOn = result.UpdatedOn
                    }
                };
            }
            catch (Exception ex)
            {
                return new FinancialYearMutationPayload
                {
                    Success = false,
                    Message = "Failed to update financial year",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Close a financial year
        /// </summary>
        public async Task<FinancialYearMutationPayload> CloseFinancialYear(
            [Service] IFinancialYearService financialYearService,
            long id)
        {
            try
            {
                var financialYear = await financialYearService.GetFinancialYearByIdAsync(id);
                
                if (financialYear == null)
                {
                    return new FinancialYearMutationPayload
                    {
                        Success = false,
                        Message = "Financial year not found",
                        Errors = new List<string> { $"No financial year found with ID: {id}" }
                    };
                }

                var dto = new UpdateFinancialYearDto
                {
                    FinancialYearName = financialYear.FinancialYearName,
                    StartDate = financialYear.StartDate,
                    CloseDate = financialYear.CloseDate,
                    UpdatedBy = 1 // GraphQL user
                };

                var result = await financialYearService.UpdateFinancialYearAsync(id, dto);

                return new FinancialYearMutationPayload
                {
                    Success = true,
                    Message = "Financial year closed successfully",
                    FinancialYear = new FinancialYearType
                    {
                        Id = result.FinancialYearId,
                        Name = result.FinancialYearName,
                        StartDate = result.StartDate,
                        EndDate = result.CloseDate,
                        DurationInDays = (int)(result.CloseDate - result.StartDate).TotalDays,
                        Status = result.IsActive ? "Active" : "Inactive",
                        IsActive = result.IsActive,
                        UpdatedOn = result.UpdatedOn
                    }
                };
            }
            catch (Exception ex)
            {
                return new FinancialYearMutationPayload
                {
                    Success = false,
                    Message = "Failed to close financial year",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Delete a financial year
        /// </summary>
        public async Task<FinancialYearMutationPayload> DeleteFinancialYear(
            [Service] IFinancialYearService financialYearService,
            long id)
        {
            try
            {
                var result = await financialYearService.DeleteFinancialYearAsync(id);

                if (!result)
                {
                    return new FinancialYearMutationPayload
                    {
                        Success = false,
                        Message = "Financial year not found",
                        Errors = new List<string> { $"No financial year found with ID: {id}" }
                    };
                }

                return new FinancialYearMutationPayload
                {
                    Success = true,
                    Message = "Financial year deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new FinancialYearMutationPayload
                {
                    Success = false,
                    Message = "Failed to delete financial year",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        /// <summary>
        /// Activate a financial year
        /// </summary>
        public async Task<FinancialYearMutationPayload> ActivateFinancialYear(
            [Service] IFinancialYearService financialYearService,
            long id)
        {
            try
            {
                var financialYear = await financialYearService.GetFinancialYearByIdAsync(id);
                
                if (financialYear == null)
                {
                    return new FinancialYearMutationPayload
                    {
                        Success = false,
                        Message = "Financial year not found",
                        Errors = new List<string> { $"No financial year found with ID: {id}" }
                    };
                }

                var dto = new UpdateFinancialYearDto
                {
                    FinancialYearName = financialYear.FinancialYearName,
                    StartDate = financialYear.StartDate,
                    CloseDate = financialYear.CloseDate,
                    UpdatedBy = 1 // GraphQL user
                };

                var result = await financialYearService.UpdateFinancialYearAsync(id, dto);

                return new FinancialYearMutationPayload
                {
                    Success = true,
                    Message = "Financial year activated successfully",
                    FinancialYear = new FinancialYearType
                    {
                        Id = result.FinancialYearId,
                        Name = result.FinancialYearName,
                        StartDate = result.StartDate,
                        EndDate = result.CloseDate,
                        DurationInDays = (int)(result.CloseDate - result.StartDate).TotalDays,
                        Status = result.IsActive ? "Active" : "Inactive",
                        IsActive = result.IsActive,
                        UpdatedOn = result.UpdatedOn
                    }
                };
            }
            catch (Exception ex)
            {
                return new FinancialYearMutationPayload
                {
                    Success = false,
                    Message = "Failed to activate financial year",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
