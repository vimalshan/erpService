using System;

namespace AccidentManagementService.Domain.Entities
{
    /// <summary>
    /// Base class for all domain entities with common properties
    /// </summary>
    public abstract class DomainEntity
    {
        public long Id { get; protected set; }
        public Guid Guid { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; protected set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; protected set; }

        protected DomainEntity()
        {
            Guid = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
            IsDeleted = false;
        }

        public virtual void Delete()
        {
            IsDeleted = true;
            UpdatedDate = DateTime.UtcNow;
        }

        public virtual void Restore()
        {
            IsDeleted = false;
            UpdatedDate = DateTime.UtcNow;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not DomainEntity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            // For unsaved entities, use reference equality
            if (Id == 0 && other.Id == 0)
                return ReferenceEquals(this, other);

            // For saved entities, use ID equality
            return Id == other.Id && GetType() == other.GetType();
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }

        public static bool operator ==(DomainEntity? left, DomainEntity? right)
        {
            if (left is null && right is null)
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(DomainEntity? left, DomainEntity? right)
        {
            return !(left == right);
        }
    }
}
