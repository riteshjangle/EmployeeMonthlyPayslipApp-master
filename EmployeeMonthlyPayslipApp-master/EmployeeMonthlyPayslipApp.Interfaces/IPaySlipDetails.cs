namespace EmployeeMonthlyPayslipApp.Interfaces
{
    public interface IPaySlipDetails
    {
        string FullName { get; set; }
        string TaxPeriod { get; set; }
        decimal GrossIncome { get; set; }
        decimal NetIncome { get; set; }
        decimal IncomeTax { get; set; }
        decimal Super { get; set; }
    }
}