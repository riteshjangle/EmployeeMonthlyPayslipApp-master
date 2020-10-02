using System.Collections.Generic;

namespace EmployeePayDetailsDomain.Interfaces
{
    public interface IEmployee : IPerson
    {
        ISalary Salary { get; set; }
        IList<ISalarySlip> SalarySlips { get; set; }
    }
}