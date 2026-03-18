using System;

namespace AccidentManagementService.Domain.Entities
{
    /// <summary>
    /// Represents an injury category (CATEGORY_INJURY)
    /// </summary>
    public class InjuryCategory : DomainEntity
    {
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        private InjuryCategory() { }

        public InjuryCategory(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Injury category name is required", nameof(name));

            Name = name;
            Description = description;
        }

        public void UpdateDetails(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Injury category name is required", nameof(name));

            Name = name;
            Description = description;
            UpdatedDate = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"InjuryCategory: {Name} (ID: {Id})";
        }
    }

    /// <summary>
    /// Represents the nature/type of an injury (NATURE_INJURY)
    /// </summary>
    public class InjuryNature : DomainEntity
    {
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        private InjuryNature() { }

        public InjuryNature(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Injury nature name is required", nameof(name));

            Name = name;
            Description = description;
        }

        public void UpdateDetails(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Injury nature name is required", nameof(name));

            Name = name;
            Description = description;
            UpdatedDate = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"InjuryNature: {Name} (ID: {Id})";
        }
    }

    /// <summary>
    /// Enumeration for Accident Severity Levels (ACCIDENT_SEVERITY)
    /// </summary>
    public class AccidentSeverity : DomainEntity
    {
        public string Code { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        private AccidentSeverity() { }

        public AccidentSeverity(string code, string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Severity code is required", nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Severity name is required", nameof(name));

            Code = code;
            Name = name;
            Description = description;
        }

        public void UpdateDetails(string code, string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Severity code is required", nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Severity name is required", nameof(name));

            Code = code;
            Name = name;
            Description = description;
            UpdatedDate = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"AccidentSeverity: {Name} ({Code})";
        }
    }

    /// <summary>
    /// Enumeration for Accident Status States (ACCIDENT_STATUS)
    /// </summary>
    public class AccidentStatus : DomainEntity
    {
        public string Code { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        private AccidentStatus() { }

        public AccidentStatus(string code, string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Status code is required", nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Status name is required", nameof(name));

            Code = code;
            Name = name;
            Description = description;
        }

        public void UpdateDetails(string code, string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Status code is required", nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Status name is required", nameof(name));

            Code = code;
            Name = name;
            Description = description;
            UpdatedDate = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"AccidentStatus: {Name} ({Code})";
        }
    }

    /// <summary>
    /// Represents a contractor (ACC_CONTRCT_LST)
    /// </summary>
    public class Contractor : DomainEntity
    {
        public string Name { get; private set; } = null!;
        public long ContractorId { get; private set; }
        public ContractorStatusEnum Status { get; private set; }

        private Contractor() { }

        public Contractor(string name, long contractorId, ContractorStatusEnum status = ContractorStatusEnum.Active)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Contractor name is required", nameof(name));
            if (contractorId <= 0)
                throw new ArgumentException("Contractor ID must be greater than zero", nameof(contractorId));

            Name = name;
            ContractorId = contractorId;
            Status = status;
        }

        public void UpdateDetails(string name, ContractorStatusEnum status)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Contractor name is required", nameof(name));

            Name = name;
            Status = status;
            UpdatedDate = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            Status = ContractorStatusEnum.Inactive;
            UpdatedDate = DateTime.UtcNow;
        }

        public void Activate()
        {
            Status = ContractorStatusEnum.Active;
            UpdatedDate = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"Contractor: {Name} (ID: {ContractorId})";
        }
    }

    /// <summary>
    /// Enumeration for Contractor Status
    /// </summary>
    public enum ContractorStatusEnum
    {
        Active = 'A',
        Inactive = 'I'
    }

    /// <summary>
    /// Represents an injured person (ACC_PERS_INJ)
    /// </summary>
    public class InjuredPerson : DomainEntity
    {
        public string PersonName { get; private set; } = null!;
        public long SerialNumber { get; private set; }
        public EmployeeStatus EmployeeStatus { get; private set; }

        private InjuredPerson() { }

        public InjuredPerson(string personName, long serialNumber, EmployeeStatus employeeStatus)
        {
            if (string.IsNullOrWhiteSpace(personName))
                throw new ArgumentException("Person name is required", nameof(personName));
            if (serialNumber <= 0)
                throw new ArgumentException("Serial number must be greater than zero", nameof(serialNumber));

            PersonName = personName;
            SerialNumber = serialNumber;
            EmployeeStatus = employeeStatus;
        }

        public void UpdateDetails(string personName, long serialNumber, EmployeeStatus employeeStatus)
        {
            if (string.IsNullOrWhiteSpace(personName))
                throw new ArgumentException("Person name is required", nameof(personName));
            if (serialNumber <= 0)
                throw new ArgumentException("Serial number must be greater than zero", nameof(serialNumber));

            PersonName = personName;
            SerialNumber = serialNumber;
            EmployeeStatus = employeeStatus;
            UpdatedDate = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"InjuredPerson: {PersonName} (Serial: {SerialNumber})";
        }
    }
}
