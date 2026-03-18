using System;
using Xunit;
using Moq;
using PayrollServices.Domain.Entities;
using PayrollServices.Domain.ValueObjects;

namespace PayrollServices.Tests;

public class PayrollBatchTests
{
    [Fact]
    public void Create_ValidInput_ReturnsPayrollBatch()
    {
        // Arrange
        var batchId = 1L;
        var batchMonth = "2024-01";
        var createdBy = 100L;

        // Act
        var batch = PayrollBatch.Create(batchId, batchMonth, createdBy);

        // Assert
        Assert.NotNull(batch);
        Assert.Equal(batchId, batch.BatchId);
        Assert.Equal(batchMonth, batch.BatchMonth);
        Assert.Equal(createdBy, batch.CreatedBy);
    }

    [Fact]
    public void MarkAsCompleted_ValidInput_UpdatesStatus()
    {
        // Arrange
        var batch = PayrollBatch.Create(1L, "2024-01", 100L);
        var completedBy = 200L;

        // Act
        batch.MarkAsCompleted(completedBy);

        // Assert
        Assert.Equal(completedBy, batch.UpdatedBy);
        Assert.NotNull(batch.UpdatedOn);
    }
}

public class PayrollTransactionTests
{
    [Fact]
    public void Create_ValidInput_ReturnsPayrollTransaction()
    {
        // Arrange
        var transactionId = 1L;
        var employeeId = 100L;
        var batchId = 1L;
        var month = "2024-01";
        var gross = 50000m;
        var deductions = 5000m;
        var net = 45000m;
        var createdBy = 200L;

        // Act
        var transaction = PayrollTransaction.Create(transactionId, employeeId, batchId, month, gross, deductions, net, createdBy);

        // Assert
        Assert.NotNull(transaction);
        Assert.Equal(transactionId, transaction.TransactionId);
        Assert.Equal(employeeId, transaction.EmployeeSystemId);
    }
}

public class SalaryComponentsTests
{
    [Fact]
    public void Create_ValidInput_CalculatesCorrectly()
    {
        // Arrange
        var basicPay = 30000m;
        var allowances = 5000m;
        var deductions = 3000m;

        // Act
        var components = new SalaryComponents(basicPay, allowances, deductions);

        // Assert
        Assert.Equal(35000m, components.GrossSalary);
        Assert.Equal(32000m, components.NetSalary);
    }

    [Fact]
    public void Create_NegativeBasicPay_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new SalaryComponents(-1000m, 5000m, 3000m));
    }
}
