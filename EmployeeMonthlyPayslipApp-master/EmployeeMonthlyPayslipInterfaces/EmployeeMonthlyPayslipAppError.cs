using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayDetailsCommon
{
    public class EmployeeMonthlyPayslipAppError
    {
        private readonly string _errorMessage;
        private readonly string _errorComponent;
        private readonly EmployeeMonthlyPayslipAppException _employeeMonthlyPayslipAppException;

        public EmployeeMonthlyPayslipAppException EmployeeMonthlyPayslipAppExceptions
            => _employeeMonthlyPayslipAppException;
        public string ErrorComponent => _errorComponent;
        public string ErrorMessage => _errorMessage;

        public EmployeeMonthlyPayslipAppError(string errorMessage,string errorComponent)
        {
            _errorMessage = errorMessage;
            _errorComponent = errorComponent;
        }

        public EmployeeMonthlyPayslipAppError(string errorComponent, EmployeeMonthlyPayslipAppException employeeMonthlyPayslipAppException)
        {
            _employeeMonthlyPayslipAppException = employeeMonthlyPayslipAppException;
            _errorMessage = employeeMonthlyPayslipAppException.Message;
            _errorComponent = errorComponent;
        }

    }
}
