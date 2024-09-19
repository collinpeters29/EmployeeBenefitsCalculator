using Api.Dtos.Employee;
using Api.Interface;
using Api.Models;

namespace Api.Deductions
{
    public class BaseEmployeeDeduction : IDeduction
    {
        decimal _paycheckDeductionAmount;
        
        public string DeductionBreakdown()
        {
            return $"Base Deduction for all employees : ${_paycheckDeductionAmount}";
        }

        public void PaycheckDeduction(GetEmployeeDto employee)
        {
            GetPayCheckDeductionAmount();
            employee.Paycheck.NetAmount -= _paycheckDeductionAmount;
            employee.Paycheck.DeductionBreakdowns.Add(DeductionBreakdown());
        }

        //We need to find the yearly deduction amount and divide it by our pay check distribution to find out how much to deduct per check.
        private void GetPayCheckDeductionAmount()
        {
            var yearlyDeduction = Config.EmployeeBaseDeductionAmount * 12;
            _paycheckDeductionAmount = Decimal.Round(yearlyDeduction / Config.YearlyPaycheckDistribution,2);
        }
    }
}
