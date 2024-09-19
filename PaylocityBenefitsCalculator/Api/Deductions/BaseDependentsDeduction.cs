using Api.Dtos.Employee;
using Api.Interface;
using Api.Models;

namespace Api.Deductions
{
    public class BaseDependentDeduction : IDeduction
    {
        decimal _paycheckDeductionAmount;
        int dependentCount;
        public string DeductionBreakdown()
        {
            return $"Base Deduction for {dependentCount} Dependents : ${_paycheckDeductionAmount}";
        }

        public void PaycheckDeduction(GetEmployeeDto employee)
        {
            if (employee.Dependents.Count == 0) return;
            dependentCount = employee.Dependents.Count;
            GetPayCheckDeductionAmount();
            employee.Paycheck.NetAmount -= _paycheckDeductionAmount;
            employee.Paycheck.DeductionBreakdowns.Add(DeductionBreakdown());
        }

         //We need to multiple the base dependent deduction by the number of dependents.
         //Then convert it to a yearly deduction amount and divide it by our pay check distribution to find out how much to deduct per check.
        private void GetPayCheckDeductionAmount()
        {
            var yearlyDeduction = (Config.DependentBaseDeductionAmount * dependentCount) * 12;
            _paycheckDeductionAmount = Decimal.Round(yearlyDeduction / Config.YearlyPaycheckDistribution,2);
        }
    }
}
