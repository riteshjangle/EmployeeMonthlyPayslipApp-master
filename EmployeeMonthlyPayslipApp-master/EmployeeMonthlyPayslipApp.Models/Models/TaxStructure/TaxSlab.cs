using EmployeeMonthlyPayslipApp.Interfaces.TaxStructure;
using Newtonsoft.Json;

namespace EmployeeMonthlyPayslipApp.Models.Models.TaxStructure
{
    public class TaxSlab : ITaxSlab
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public decimal MaxIncome { get; set; }

        public decimal MinIncome { get; set; }

        [JsonConverter(typeof(InterfaceConverter<ITax, Tax>))]
        public ITax Tax { get; set; }
    }
}