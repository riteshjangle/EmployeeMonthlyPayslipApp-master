using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeMonthlyPayslipApp.Interfaces;
using Serilog;

namespace EmployeePayDetailsCommon
{
    public class EmployeeMonthlyPayslipAppContext
    {
        private readonly ILogger _logger;
        private IList<EmployeeMonthlyPayslipAppError> _employeeMonthlyPayslipAppError;
        public IList<EmployeeMonthlyPayslipAppError> EmployeeMonthlyPayslipAppError => _employeeMonthlyPayslipAppError;

        public IPaySlipDetails PaySlipDetails { get; set; }

        public bool HasErrors => _employeeMonthlyPayslipAppError.Count > 0;

        public EmployeeMonthlyPayslipAppContext(ILogger logger)
        {
            _logger = logger;
            _employeeMonthlyPayslipAppError = new List<EmployeeMonthlyPayslipAppError>();
        }

        public void AddError(EmployeeMonthlyPayslipAppError employeeMonthlyPayslipAppError)
        {
            _employeeMonthlyPayslipAppError.Add(employeeMonthlyPayslipAppError);
        }

        public void LogErrors()
        {
            _logger.Error("Following Errors occured while processing the application");
            foreach (var employeeMonthlyPayslipAppError in _employeeMonthlyPayslipAppError)
            {
                _logger.Error("Errors : {@EmployeeMonthlyPayslipAppError}", employeeMonthlyPayslipAppError);
            }
        }
    }
}
