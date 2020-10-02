namespace EmployeeMonthlyPayslipApp.Interfaces
{
    public interface IEmployeeDetails
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        decimal AnnualSalary { get; set; }
        string TaxPeriod { get; set; }
        decimal SuperRate { get; set; }
    }
}