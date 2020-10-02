using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayDetailsCommon
{
    public class EmployeeMonthlyPayslipAppException : Exception
    {

        public EmployeeMonthlyPayslipAppException(string message) : base(message)
        {
            
        }

        public EmployeeMonthlyPayslipAppException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
