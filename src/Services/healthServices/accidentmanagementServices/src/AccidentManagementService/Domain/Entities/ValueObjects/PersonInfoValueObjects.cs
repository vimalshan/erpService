using System;

namespace AccidentManagementService.Domain.Entities
{
    /// <summary>
    /// Value Object for Employee Information
    /// </summary>
    public class EmployeeInfo : IEquatable<EmployeeInfo>
    {
        public string EmployeeNumber { get; private set; }
        public string EmployeeName { get; private set; }
        public string Department { get; private set; }

        private EmployeeInfo() { }

        public EmployeeInfo(string employeeNumber, string employeeName, string department)
        {
            if (string.IsNullOrWhiteSpace(employeeNumber))
                throw new ArgumentException("Employee number is required", nameof(employeeNumber));
            if (string.IsNullOrWhiteSpace(employeeName))
                throw new ArgumentException("Employee name is required", nameof(employeeName));
            if (string.IsNullOrWhiteSpace(department))
                throw new ArgumentException("Department is required", nameof(department));

            EmployeeNumber = employeeNumber;
            EmployeeName = employeeName;
            Department = department;
        }

        public override bool Equals(object? obj)
        {
            return obj is EmployeeInfo info && Equals(info);
        }

        public bool Equals(EmployeeInfo? other)
        {
            return other != null &&
                   EmployeeNumber == other.EmployeeNumber &&
                   EmployeeName == other.EmployeeName &&
                   Department == other.Department;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EmployeeNumber, EmployeeName, Department);
        }
    }

    /// <summary>
    /// Value Object for Contractor Information
    /// </summary>
    public class ContractorInfo : IEquatable<ContractorInfo>
    {
        public long ContractorId { get; private set; }
        public string ContractorName { get; private set; }

        private ContractorInfo() { }

        public ContractorInfo(long contractorId, string contractorName)
        {
            if (contractorId <= 0)
                throw new ArgumentException("Contractor ID must be greater than zero", nameof(contractorId));
            if (string.IsNullOrWhiteSpace(contractorName))
                throw new ArgumentException("Contractor name is required", nameof(contractorName));

            ContractorId = contractorId;
            ContractorName = contractorName;
        }

        public override bool Equals(object? obj)
        {
            return obj is ContractorInfo info && Equals(info);
        }

        public bool Equals(ContractorInfo? other)
        {
            return other != null &&
                   ContractorId == other.ContractorId &&
                   ContractorName == other.ContractorName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ContractorId, ContractorName);
        }
    }

    /// <summary>
    /// Value Object for Injured Person Information
    /// </summary>
    public class InjuredPersonInfo : IEquatable<InjuredPersonInfo>
    {
        public string PersonName { get; private set; }
        public EmployeeStatus EmployeeStatus { get; private set; }
        public long? SerialNumber { get; private set; }

        private InjuredPersonInfo() { }

        public InjuredPersonInfo(string personName, EmployeeStatus employeeStatus, long? serialNumber = null)
        {
            if (string.IsNullOrWhiteSpace(personName))
                throw new ArgumentException("Person name is required", nameof(personName));

            PersonName = personName;
            EmployeeStatus = employeeStatus;
            SerialNumber = serialNumber;
        }

        public override bool Equals(object? obj)
        {
            return obj is InjuredPersonInfo info && Equals(info);
        }

        public bool Equals(InjuredPersonInfo? other)
        {
            return other != null &&
                   PersonName == other.PersonName &&
                   EmployeeStatus == other.EmployeeStatus &&
                   SerialNumber == other.SerialNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PersonName, EmployeeStatus, SerialNumber);
        }
    }

    /// <summary>
    /// Enumeration for Employee Status (Staff or Contractor)
    /// </summary>
    public enum EmployeeStatus
    {
        Staff = 'S',
        Contractor = 'C'
    }
}
