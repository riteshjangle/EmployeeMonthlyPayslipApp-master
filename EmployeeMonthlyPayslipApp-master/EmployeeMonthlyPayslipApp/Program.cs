namespace EmployeeMonthlyPayslipApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var employeeMonthlyPayslipApplication = new EmployeeMonthlyPayslipApplication(args);
            if (!employeeMonthlyPayslipApplication.EmployeeMonthlyPayslipAppContext.HasErrors)
            {
                employeeMonthlyPayslipApplication.RunApplication();
            }
            
        }
    }
}