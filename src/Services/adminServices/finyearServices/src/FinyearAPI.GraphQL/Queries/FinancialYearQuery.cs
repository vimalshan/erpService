using FinyearAPI.GraphQL.Types;
using FinyearAPI.Application.Services;
using FinyearAPI.Domain.Entities;

namespace FinyearAPI.GraphQL.Queries
{
    /// <summary>
    /// GraphQL Query type for financial year operations
    /// </summary>
    public class FinancialYearQuery
    {
        /// <summary>
        /// Get all financial years with pagination
        /// </summary>
        public async Task<List<FinancialYearType>> GetAllFinancialYears(
            [Service] IFinancialYearService financialYearService,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var financialYears = await financialYearService.GetAllFinancialYearsAsync();
                
                return financialYears
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(fy => new FinancialYearType
                    {
                        Id = fy.FinancialYearId,
                        Name = fy.FinancialYearName,
                        StartDate = fy.StartDate,
                        EndDate = fy.CloseDate,
                        DurationInDays = (int)(fy.CloseDate - fy.StartDate).TotalDays,
                        Status = fy.IsActive ? "Active" : "Inactive",
                        IsActive = fy.IsActive,
                        UpdatedOn = fy.UpdatedOn
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error retrieving financial years: {ex.Message}");
            }
        }

        /// <summary>
        /// Get financial year by ID
        /// </summary>
        public async Task<FinancialYearType?> GetFinancialYearById(
            [Service] IFinancialYearService financialYearService,
            long id)
        {
            try
            {
                var financialYear = await financialYearService.GetFinancialYearByIdAsync(id);
                
                if (financialYear == null)
                    return null;

                return new FinancialYearType
                {
                    Id = financialYear.FinancialYearId,
                    Name = financialYear.FinancialYearName,
                    StartDate = financialYear.StartDate,
                    EndDate = financialYear.CloseDate,
                    DurationInDays = (int)(financialYear.CloseDate - financialYear.StartDate).TotalDays,
                    Status = financialYear.IsActive ? "Active" : "Inactive",
                    IsActive = financialYear.IsActive,
                    UpdatedOn = financialYear.UpdatedOn
                };
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error retrieving financial year: {ex.Message}");
            }
        }

        /// <summary>
        /// Get current active financial year
        /// </summary>
        public async Task<FinancialYearType?> GetCurrentFinancialYear(
            [Service] IFinancialYearService financialYearService)
        {
            try
            {
                var financialYear = await financialYearService.GetCurrentFinancialYearAsync();
                
                if (financialYear == null)
                    return null;

                return new FinancialYearType
                {
                    Id = financialYear.FinancialYearId,
                    Name = financialYear.FinancialYearName,
                    StartDate = financialYear.StartDate,
                    EndDate = financialYear.CloseDate,
                    DurationInDays = (int)(financialYear.CloseDate - financialYear.StartDate).TotalDays,
                    Status = financialYear.IsActive ? "Active" : "Inactive",
                    IsActive = financialYear.IsActive,
                    UpdatedOn = financialYear.UpdatedOn
                };
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error retrieving current financial year: {ex.Message}");
            }
        }

        /// <summary>
        /// Get financial year by name
        /// </summary>
        public async Task<FinancialYearType?> GetFinancialYearByName(
            [Service] IFinancialYearService financialYearService,
            string name)
        {
            try
            {
                var financialYear = await financialYearService.GetFinancialYearByNameAsync(name);
                
                if (financialYear == null)
                    return null;

                return new FinancialYearType
                {
                    Id = financialYear.FinancialYearId,
                    Name = financialYear.FinancialYearName,
                    StartDate = financialYear.StartDate,
                    EndDate = financialYear.CloseDate,
                    DurationInDays = (int)(financialYear.CloseDate - financialYear.StartDate).TotalDays,
                    Status = financialYear.IsActive ? "Active" : "Inactive",
                    IsActive = financialYear.IsActive,
                    UpdatedOn = financialYear.UpdatedOn
                };
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error retrieving financial year by name: {ex.Message}");
            }
        }

        /// <summary>
        /// Get financial years in date range
        /// </summary>
        public async Task<List<FinancialYearType>> GetFinancialYearsByDateRange(
            [Service] IFinancialYearService financialYearService,
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                var financialYears = await financialYearService.GetAllFinancialYearsAsync();
                
                return financialYears
                    .Where(fy => fy.StartDate >= startDate && fy.CloseDate <= endDate)
                    .Select(fy => new FinancialYearType
                    {
                        Id = fy.FinancialYearId,
                        Name = fy.FinancialYearName,
                        StartDate = fy.StartDate,
                        EndDate = fy.CloseDate,
                        DurationInDays = (int)(fy.CloseDate - fy.StartDate).TotalDays,
                        Status = fy.IsActive ? "Active" : "Inactive",
                        IsActive = fy.IsActive,
                        UpdatedOn = fy.UpdatedOn
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error retrieving financial years by date range: {ex.Message}");
            }
        }

        /// <summary>
        /// Get active financial years
        /// </summary>
        public async Task<List<FinancialYearType>> GetActiveFinancialYears(
            [Service] IFinancialYearService financialYearService)
        {
            try
            {
                var financialYears = await financialYearService.GetAllFinancialYearsAsync();
                
                return financialYears
                    .Where(fy => fy.IsActive)
                    .Select(fy => new FinancialYearType
                    {
                        Id = fy.FinancialYearId,
                        Name = fy.FinancialYearName,
                        StartDate = fy.StartDate,
                        EndDate = fy.CloseDate,
                        DurationInDays = (int)(fy.CloseDate - fy.StartDate).TotalDays,
                        Status = "Active",
                        IsActive = true,
                        UpdatedOn = fy.UpdatedOn
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error retrieving active financial years: {ex.Message}");
            }
        }
    }
}
