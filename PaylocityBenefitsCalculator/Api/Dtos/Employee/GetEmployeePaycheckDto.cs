namespace Api.Dtos.Employee;

public class GetEmployeePaycheckDto
{
    public decimal GrossAmount { get; set; }
    public decimal NetAmount { get; set; }
    public ICollection<string> DeductionBreakdowns { get; set; } = new List<string>();
}
