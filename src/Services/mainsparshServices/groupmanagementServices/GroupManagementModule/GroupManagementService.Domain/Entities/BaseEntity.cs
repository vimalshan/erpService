namespace GroupManagementService.Domain.Entities
{
    /// <summary>
    /// Base entity class with common properties for all entities
    /// </summary>
    public abstract class BaseEntity
    {
        public long Id { get; protected set; }
        public long CreatedBy { get; protected set; }
        public DateTime CreatedOn { get; protected set; }
        public long? UpdatedBy { get; protected set; }
        public DateTime? UpdatedOn { get; protected set; }

        protected BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
        }

        protected BaseEntity(long createdBy)
        {
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
        }
    }
}
