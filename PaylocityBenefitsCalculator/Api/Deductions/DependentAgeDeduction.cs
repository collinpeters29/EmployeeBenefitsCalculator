using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Interface;
using Api.Models;

namespace Api.Deductions
{
    public class DependentAgeDeduction : IDeduction
    {
        decimal _paycheckDeductionAmount;
        int dependentCount;
        public string DeductionBreakdown()
        {
            return $"Deduction for {dependentCount} Dependents over {Config.DependentAgeCutoff} : ${_paycheckDeductionAmount}";
        }

        public void PaycheckDeduction(GetEmployeeDto employee)
        {
            dependentCount = SetDependentAgeEligibilityCount(employee.Dependents);

            if (dependentCount == 0) return;

            GetPayCheckDeductionAmount();
            employee.Paycheck.NetAmount -= _paycheckDeductionAmount;
            employee.Paycheck.DeductionBreakdowns.Add(DeductionBreakdown());
        }

        private int SetDependentAgeEligibilityCount(ICollection<GetDependentDto> dependents)
        {
            return dependents.Where(e => e.IsOverDependentCutoff == true).ToList().Count;
        }

        //We need to multiple the base dependent deduction by the number of dependents.
        //Then convert it to a yearly deduction amount and divide it by our pay check distribution to find out how much to deduct per check.
        private void GetPayCheckDeductionAmount()
        {
            var dependentYearlyDeduction = (Config.DependentOver50DeductionAmount * dependentCount) * 12;
            _paycheckDeductionAmount = Decimal.Round(dependentYearlyDeduction / Config.YearlyPaycheckDistribution,2);
        }
    }
}
