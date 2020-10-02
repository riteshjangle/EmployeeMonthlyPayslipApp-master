using EmployeeMonthlyPayslipApp.Interfaces.TaxStructure;

namespace EmployeeMonthlyPayslipApp.Models.Models.TaxStructure
{
    public class Tax : ITax
    {
        public decimal? AdditionalTaxPercentageOverMinIncome { get; set; }
        public decimal? FlatTax { get; set; }
    }
}