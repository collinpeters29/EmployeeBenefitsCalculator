using Api.Deductions;
using Api.Dtos.Employee;
using Api.Interface;
using Api.Models;

namespace Api.Service
{
    public class PayCheckService
    {
        //Add references to whatever dependencies our services need to c
        private List<IDeduction> _deductions = new List<IDeduction>();
        public PayCheckService() {
            InitializeDeductionRules();
        }

        public void InitializeDeductionRules()
        {
            _deductions.Add(new BaseEmployeeDeduction());
            _deductions.Add(new EmployeeSalaryBacket2Deduction());
            _deductions.Add(new BaseDependentDeduction());
            _deductions.Add(new DependentAgeDeduction());
        }

        //Get Our gross amount Per Check Rounded to the nearest Cent
        public GetEmployeePaycheckDto GetGrossPay(decimal employeeSalary)
        {
            var paycheckGross = Decimal.Round(employeeSalary / Config.YearlyPaycheckDistribution,2);
            var paycheck = new GetEmployeePaycheckDto
            {
                GrossAmount = paycheckGross,
                NetAmount = paycheckGross
            };

            return paycheck;
        }

        public void RunDeductions(GetEmployeeDto employee)
        {
            foreach (var deduction in _deductions)
            {
                deduction.PaycheckDeduction(employee);
            }
        }
    }
}
