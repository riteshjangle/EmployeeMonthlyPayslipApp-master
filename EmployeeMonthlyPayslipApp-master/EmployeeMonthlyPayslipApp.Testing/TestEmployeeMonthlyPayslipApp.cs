using System;
using System.IO;
using System.Linq;
using EmployeeMonthlyPayslipApp.Interfaces;
using NUnit.Framework;
using Shouldly;

namespace EmployeeMonthlyPayslipApp.Testing
{
    [TestFixture]
    public class TestEmployeeMonthlyPayslipApp
    {
        //private EmployeeDetailsInput _employeeDetailsInput = new EmployeeDetailsInput();
        private string[] _commandLineArgs;

        [TestCase("EmployeeMonthlyPaySlipApp","Hariharan","S",80000,"9%","1 March - 31 March",6667,1462,5205,600)]
        public void TestCommandLineApplication(string applicationName, string firstName, string lastName, int annualSalary,
            string superRate, string payPeriod, decimal grossIncome, decimal incomeTax, decimal netIncome, decimal super)
        {
            _commandLineArgs = new[] { applicationName, "-f=" + firstName, "-l=" + lastName, "-a=" + annualSalary, "-s=" + superRate, "-p=" + payPeriod };
            var employeeMonthlyPayslipApplication = new EmployeeMonthlyPayslipApplication(_commandLineArgs);
            var paySlipDetails = employeeMonthlyPayslipApplication.RunApplication();
            AssertSuccessPaySlipDetails(firstName, lastName, payPeriod,grossIncome,incomeTax,netIncome,super, paySlipDetails.FirstOrDefault());
        }

        private static void AssertSuccessPaySlipDetails(string firstName, string lastName, string payPeriod,decimal grossIncome,decimal incomeTax,decimal netIncome,decimal super,
            IPaySlipDetails paySlipDetails)
        {
            paySlipDetails.ShouldNotBeNull();
            paySlipDetails.FullName.ShouldBe(firstName.Trim() + " " + lastName.Trim());
            paySlipDetails.TaxPeriod.ShouldBe(payPeriod);
            paySlipDetails.GrossIncome.ShouldBe(grossIncome);
            paySlipDetails.IncomeTax.ShouldBe(incomeTax);
            paySlipDetails.NetIncome.ShouldBe(netIncome);
            paySlipDetails.Super.ShouldBe(super);
        }
        [TestCase("EmployeeMonthlyPaySlipApp",true, @"TestData\EmployeeDetailsValid.csv", @"C:\Hariharan\Projects\MYOBCodeChallenge\EmployeeMonthlyPayslipApp\EmployeeMonthlyPayslipApp\EmployeeMonthlyPayslipApp.Testing\TestData\")]
        public void TestCSVApplication(string applicationName, bool isInputInCSVFormat,string inputCSVFileRelativePath,string outputDirectory)
        {
            var inputCSVFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, inputCSVFileRelativePath);
            _commandLineArgs = new[] { applicationName, "-c=true", "-i=" + inputCSVFilePath, "-o=" + outputDirectory };
            var employeeMonthlyPayslipApplication = new EmployeeMonthlyPayslipApplication(_commandLineArgs);
            var paySlipDetails = employeeMonthlyPayslipApplication.RunApplication();
        }
    }
}
