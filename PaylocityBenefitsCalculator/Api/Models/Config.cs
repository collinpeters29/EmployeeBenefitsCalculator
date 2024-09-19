namespace Api.Models
{
    public static class Config
    {
        // This Class represents data that rarely changes but could change so should be stored in an appsettings or a Database "MasterSet" 
        // We would like to avoid having to make a code change just because the base Employee Deduction Amount was raised 2 dollars
        // The less things that are hardcoded the better.

        public const int YearlyPaycheckDistribution = 26;
        public const decimal EmployeeBaseDeductionAmount = 1000m;
        public const decimal DependentBaseDeductionAmount = 600m;
        public const decimal DependentOver50DeductionAmount = 200m;
        public const decimal EmployeeSalaryWindow1Cutoff = 80000m;
        public const decimal EmployeeSalaryWindow2TaxPercent =  2;
        public const int DependentAgeCutoff = 50;
    }
}
