using System;
using AutoMapper;
using EmployeeMonthlyPayslipApp.Interfaces;
using EmployeeMonthlyPayslipApp.Interfaces.TaxStructure;
using EmployeePayDetailsCommon;
using EmployeePayDetailsDomain.Interfaces;
using EmployeePayDetailsDomain.Models;

namespace EmployeePayDetailsService
{
    public class EmployeePayDetailsService
    {
        private readonly EmployeeMonthlyPayslipAppContext _employeeMonthlyPayslipAppContext;
        private readonly IEmployeeDetails _employeeDetails;
        private readonly IMapper _mapper;

        public EmployeePayDetailsService(EmployeeMonthlyPayslipAppContext employeeMonthlyPayslipAppContext,
            IEmployeeDetails employeeDetails, IMapper mapper)
        {
            _employeeMonthlyPayslipAppContext = employeeMonthlyPayslipAppContext;
            _employeeDetails = employeeDetails;
            _mapper = mapper;
        }

        public IPaySlipDetails GetPaySlip(ITaxStructure taxStructure)
        {
            IPaySlipDetails paySlipDetails = null;
            try
            {
                var employee = _mapper.Map<IEmployeeDetails, IEmployee>(_employeeDetails);
                var salarySlip = SalarySlip.CreateSalarySlip(employee.Salary, taxStructure);
                salarySlip.TaxPeriod = _employeeDetails.TaxPeriod;
                employee.SalarySlips.Add(salarySlip);
                paySlipDetails = _mapper.Map<IEmployee, IPaySlipDetails>(employee);
            }
            catch (Exception ex)
            {
                _employeeMonthlyPayslipAppContext.AddError(
                        new EmployeeMonthlyPayslipAppError("EmployeePayDetailsService:GetPaySlip",
                        new EmployeeMonthlyPayslipAppException("Error occured while processing pay slip", ex))
                    );
            }
            return paySlipDetails;
        }
    }
}