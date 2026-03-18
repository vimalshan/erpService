using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinyearAPI.Domain.Entities
{
    /// <summary>
    /// Financial Year Master Entity
    /// Represents financial year boundaries and status
    /// </summary>
    [Table("FINYEAR_MASTER")]
    public class FinancialYearMaster
    {
        [Key]
        [Column("FY_ID")]
        public long FinancialYearId { get; set; }

        [Required]
        [Column("FY_NAME")]
        [StringLength(27)]
        public string FinancialYearName { get; set; } = string.Empty;

        [Required]
        [Column("FY_STARTDATE")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("FY_CLOSEDATE")]
        public DateTime CloseDate { get; set; }

        [Column("FY_UPDATED_BY")]
        public long UpdatedBy { get; set; }

        [Column("FY_UPDATED_ON")]
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Checks if the financial year is currently active
        /// </summary>
        public bool IsActive => DateTime.Now >= StartDate && DateTime.Now <= CloseDate;

        /// <summary>
        /// Gets the duration of the financial year in days
        /// </summary>
        public int DurationInDays => (int)(CloseDate - StartDate).TotalDays;
    }
}
