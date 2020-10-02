using EmployeeMonthlyPayslipApp.Interfaces;

namespace EmployeeMonthlyPayslipApp.Models.Models
{
    public class PaySlipDetails : IPaySlipDetails
    {
        public string FullName { get; set; }
        public string TaxPeriod { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal NetIncome { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal Super { get; set; }
    }
}