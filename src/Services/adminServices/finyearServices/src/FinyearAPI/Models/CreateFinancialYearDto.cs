using System.ComponentModel.DataAnnotations;

namespace FinyearAPI.Models
{
    /// <summary>
    /// DTO for creating a new financial year
    /// </summary>
    public class CreateFinancialYearDto
    {
        [Required]
        public long FinancialYearId { get; set; }

        [Required]
        [StringLength(27, MinimumLength = 1)]
        public string FinancialYearName { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime CloseDate { get; set; }

        [Required]
        public long UpdatedBy { get; set; }
    }
}
