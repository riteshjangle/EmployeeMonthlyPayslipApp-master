namespace EmployeeMonthlyPayslipApp.Interfaces.TaxStructure
{
    public interface ITaxSlab
    {
        decimal MaxIncome { get; set; }
        decimal MinIncome { get; set; }
        ITax Tax { get; set; }
    }
}