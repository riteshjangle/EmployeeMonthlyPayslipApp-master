using System;
using System.Linq;
using EmployeeMonthlyPayslipApp.Interfaces.TaxStructure;
using EmployeePayDetailsDomain.Interfaces;

namespace EmployeePayDetailsDomain.Models
{
    public class SalarySlip : ISalarySlip
    {
        private readonly ISalary _salary;
        private readonly ITaxStructure _taxStructure;

        private SalarySlip(ISalary salary, ITaxStructure taxStructure)
        {
            _salary = salary;
            _taxStructure = taxStructure;
            IncomeTax = CalculateIncomeTax();
        }

        public SalarySlip()
        {
        }

        public decimal GrossIncome => Math.Round(_salary.AnnualSalary/12, 0, MidpointRounding.AwayFromZero);
        public decimal NetIncome => Math.Round(GrossIncome - IncomeTax, 0, MidpointRounding.AwayFromZero);
        public string TaxPeriod { get; set; }
        public decimal IncomeTax { get; }

        public decimal Super => Math.Round(GrossIncome*_salary.SuperRate, 0, MidpointRounding.AwayFromZero);

        public static SalarySlip CreateSalarySlip(ISalary salary, ITaxStructure taxStructure)
            => new SalarySlip(salary, taxStructure);

        private decimal CalculateIncomeTax()
        {
            const decimal incomeTax = 0.00M;

            var taxSlab = _taxStructure
                .TaxRate
                .TaxSlab
                .FirstOrDefault(x => (x.MinIncome <= _salary.AnnualSalary) &&
                                     (x.MaxIncome >= _salary.AnnualSalary));
            // check if tax slab is null and throw an exception
            if (taxSlab == null) throw new NullReferenceException("Tax Slab is null");
            // no tax
            if (taxSlab.Tax == null || (taxSlab.Tax.AdditionalTaxPercentageOverMinIncome == null && taxSlab.Tax.FlatTax==null))
                return incomeTax;
            //get tax structure
            var tax = taxSlab.Tax;

            var annualTaxPartial = (_salary.AnnualSalary - taxSlab.MinIncome)*
                                   tax.AdditionalTaxPercentageOverMinIncome.Value;
            // calculate annual tax
            var annualTax = tax.FlatTax == null ? annualTaxPartial : tax.FlatTax + annualTaxPartial;
            // calculate monthly tax
            var monthlyTax = Math.Round(annualTax.Value/12, 0, MidpointRounding.AwayFromZero);

            return monthlyTax;
        }
    }
}