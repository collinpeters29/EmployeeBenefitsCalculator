using System;
using System.Collections.Generic;
using Api.Deductions;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Interface;
using Api.Models;
using Shouldly;
using Xunit;

namespace ApiTests.IntegrationTests;

public class DeductionRulesUnitTests : IntegrationTest
{

    [Fact]
    public void TestBaseEmployeeDeduction()
    {
        var startingCheckAmount = 10000m;
        var assertDecimalDeduction = decimal.Round((Config.EmployeeBaseDeductionAmount * 12) / Config.YearlyPaycheckDistribution,2);
        var finalAmount = startingCheckAmount - assertDecimalDeduction;
        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30),
            Paycheck = new GetEmployeePaycheckDto
            {
                GrossAmount = startingCheckAmount,
                NetAmount = startingCheckAmount
            }
        };

        BaseEmployeeDeduction BaseDeduction = new BaseEmployeeDeduction();

        List<IDeduction> deductionRules = new List<IDeduction>();
        deductionRules.Add(BaseDeduction);

        foreach (var deduction in deductionRules)
        {
            deduction.PaycheckDeduction(employee);
        }
        

        employee.Paycheck.NetAmount.ShouldBe(finalAmount);
    }

    [Fact]
    public void TestBaseDependentDeduction()
    {
        var startingCheckAmount = 10000m;
        var dependentCount = 1;
        var assertDecimalDeduction = decimal.Round((Config.DependentBaseDeductionAmount * dependentCount * 12) / Config.YearlyPaycheckDistribution,2);
        var finalAmount = startingCheckAmount - assertDecimalDeduction;
        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30),
            Paycheck = new GetEmployeePaycheckDto
            {
                GrossAmount = startingCheckAmount,
                NetAmount = startingCheckAmount
            },
            Dependents = new List<GetDependentDto>
            {
                new GetDependentDto
                {
                    Id = 1,
                    FirstName = "Spouse",
                    LastName = "Morant",
                    Relationship = Relationship.Spouse,
                    DateOfBirth = new DateTime(1998, 3, 3)

                }
            }

        };

        BaseDependentDeduction BaseDeduction = new BaseDependentDeduction();

        List<IDeduction> deductionRules = new List<IDeduction>();
        deductionRules.Add(BaseDeduction);

        foreach (var deduction in deductionRules)
        {
            deduction.PaycheckDeduction(employee);
        }


        employee.Paycheck.NetAmount.ShouldBe(finalAmount);
    }

    [Fact]
    public void TestDependentAgeDeduction()
    {
        var startingCheckAmount = 10000m;
        var dependentCount = 1;
        var assertDecimalDeduction = decimal.Round((Config.DependentOver50DeductionAmount * dependentCount * 12) / Config.YearlyPaycheckDistribution, 2);
        var finalAmount = startingCheckAmount - assertDecimalDeduction;
        var employee = new GetEmployeeDto
        {
            Id = 3,
            FirstName = "Michael",
            LastName = "Jordan",
            Salary = 143211.12m,
            DateOfBirth = new DateTime(1963, 2, 17),
            Paycheck = new GetEmployeePaycheckDto
            {
                GrossAmount = startingCheckAmount,
                NetAmount = startingCheckAmount
            },
            Dependents = new List<GetDependentDto>
            {
                new()
                {
                    Id = 4,
                    FirstName = "DP",
                    LastName = "Jordan",
                    Relationship = Relationship.DomesticPartner,
                    DateOfBirth = new DateTime(1974, 1, 2)
                },
                new()
                {
                    Id = 4,
                    FirstName = "Child",
                    LastName = "Jordan",
                    Relationship = Relationship.Child,
                    DateOfBirth = new DateTime(2000, 1, 2)
                }
            }

        };

        DependentAgeDeduction BaseDeduction = new DependentAgeDeduction();

        List<IDeduction> deductionRules = new List<IDeduction>();
        deductionRules.Add(BaseDeduction);

        foreach (var deduction in deductionRules)
        {
            deduction.PaycheckDeduction(employee);
        }


        employee.Paycheck.NetAmount.ShouldBe(finalAmount);
    }

    [Fact]
    public void TestSalaryBracketDeduction()
    {
        var startingCheckAmount = 10000m;
        var yearlySalary = 143211.12m;
        var dependentCount = 1;
        var assertDecimalDeduction = decimal.Round(((Config.EmployeeSalaryWindow2TaxPercent/ 100) * yearlySalary) / Config.YearlyPaycheckDistribution, 2);
        var finalAmount = startingCheckAmount - assertDecimalDeduction;
        var employee = new GetEmployeeDto
        {
            Id = 3,
            FirstName = "Michael",
            LastName = "Jordan",
            Salary = yearlySalary,
            DateOfBirth = new DateTime(1963, 2, 17),
            Paycheck = new GetEmployeePaycheckDto
            {
                GrossAmount = startingCheckAmount,
                NetAmount = startingCheckAmount
            },

        };

        EmployeeSalaryBacket2Deduction BaseDeduction = new EmployeeSalaryBacket2Deduction();

        List<IDeduction> deductionRules = new List<IDeduction>();
        deductionRules.Add(BaseDeduction);

        foreach (var deduction in deductionRules)
        {
            deduction.PaycheckDeduction(employee);
        }


        employee.Paycheck.NetAmount.ShouldBe(finalAmount);
    }
}

