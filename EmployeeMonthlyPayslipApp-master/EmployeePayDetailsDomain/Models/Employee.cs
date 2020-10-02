using System.Collections.Generic;
using EmployeePayDetailsDomain.Interfaces;

namespace EmployeePayDetailsDomain.Models
{
    public class Employee : Person, IEmployee
    {
        public ISalary Salary { get; set; }
        public IList<ISalarySlip> SalarySlips { get; set; }
    }
}