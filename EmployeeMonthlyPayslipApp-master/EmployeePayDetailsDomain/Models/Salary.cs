using EmployeePayDetailsDomain.Interfaces;

namespace EmployeePayDetailsDomain.Models
{
    public class Salary : ISalary
    {
        public decimal AnnualSalary { get; set; }
        public decimal SuperRate { get; set; }
    }
}