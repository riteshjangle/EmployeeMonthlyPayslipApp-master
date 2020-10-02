using System.Collections.Generic;
using EmployeeMonthlyPayslipApp.Interfaces.TaxStructure;
using Newtonsoft.Json;

namespace EmployeeMonthlyPayslipApp.Models.Models.TaxStructure
{
    public class TaxRate : ITaxRate
    {
        [JsonConverter(typeof(InterfaceConverter<IEnumerable<ITaxSlab>, List<TaxSlab>>))]
        public IEnumerable<ITaxSlab> TaxSlab { get; set; }

        public int TaxYearEnd { get; set; }
        public int TaxYearStart { get; set; }
    }
}