using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using EmployeeMonthlyPayslipApp.Models.Models;

namespace EmployeeMonthlyPayslipApp
{
    public sealed class PaySlipDetailsMap : CsvClassMap<PaySlipDetails>
    {
        public PaySlipDetailsMap()
        {
            Map(m => m.FullName).Name("FullName").Index(0);
            Map(m => m.TaxPeriod).Name("TaxPeriod").Index(1);
            Map(m => m.GrossIncome).Name("GrossIncome").Index(2);
            Map(m => m.IncomeTax).Name("IncomeTax").Index(3);
            Map(m => m.NetIncome).Name("NetIncome").Index(4);
            Map(m => m.Super).Name("Super").Index(5);
        }
    }
}
