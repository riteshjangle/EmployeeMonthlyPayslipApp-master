using EmployeeMonthlyPayslipApp.Interfaces.TaxStructure;
using Newtonsoft.Json;

namespace EmployeeMonthlyPayslipApp.Models.Models.TaxStructure
{
    public class TaxStructure : ITaxStructure
    {
        [JsonConverter(typeof(InterfaceConverter<ITaxRate, TaxRate>))]
        public ITaxRate TaxRate { get; set; }
    }

    public class TaxStructureRoot
    {
        public TaxStructure taxStructure { get; set; }
    }
}