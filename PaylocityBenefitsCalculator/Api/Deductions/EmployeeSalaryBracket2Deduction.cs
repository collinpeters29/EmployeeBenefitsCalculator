using Api.Dtos.Employee;
using Api.Interface;
using Api.Models;

namespace Api.Deductions
{
    public class EmployeeSalaryBacket2Deduction : IDeduction
    {
        decimal _paycheckDeductionAmount;
        
        public string DeductionBreakdown()
        {
            return $"%{Config.EmployeeSalaryWindow2TaxPercent}(yearly salary total) Deduction for all employees Making > {Config.EmployeeSalaryWindow1Cutoff} : ${_paycheckDeductionAmount}";
        }

        public void PaycheckDeduction(GetEmployeeDto employee)
        {
            if (employee.Salary <= Config.EmployeeSalaryWindow1Cutoff) return;

            GetPayCheckDeductionAmount(employee.Salary);
            employee.Paycheck.NetAmount -= _paycheckDeductionAmount;
            employee.Paycheck.DeductionBreakdowns.Add(DeductionBreakdown());
        }

        //We need to find the yearly deduction amount and divide it by our pay check distribution to find out how much to deduct per check.
        private void GetPayCheckDeductionAmount(decimal salary)
        {

            var yearlyDeduction = salary * (Config.EmployeeSalaryWindow2TaxPercent / 100);
            _paycheckDeductionAmount = Decimal.Round(yearlyDeduction / Config.YearlyPaycheckDistribution,2);
        }
    }
}
