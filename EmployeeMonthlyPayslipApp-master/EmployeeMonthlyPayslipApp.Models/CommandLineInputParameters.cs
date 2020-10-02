namespace EmployeeMonthlyPayslipApp.Models
{
    public class CommandLineInputParameters
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AnnualSalary { get; set; }
        public string TaxPeriod { get; set; }
        public string SuperPercentage { get; set; }
        public bool IsInputInCSVFormat { get; set; }

        public string InputCSVFilePath { get; set; }
        public string OutputCSVDirectory { get; set; }
    }

    public class CSVParameters
    {
        public string InputCSVFilePath { get; set; }
        public string OutputCSVDirectory { get; set; }
    }
}