using Api.Dtos.Employee;

namespace Api.Interface
{
    public interface IDeduction
    {        
        void PaycheckDeduction(GetEmployeeDto employee);
        string DeductionBreakdown();
    }
}
