namespace EmployeePayDetailsDomain.Interfaces
{
    public interface ISalarySlip
    {
        decimal GrossIncome { get; }
        decimal IncomeTax { get; }
        decimal NetIncome { get; }
        string TaxPeriod { get; set; }
        decimal Super { get; }
    }
}