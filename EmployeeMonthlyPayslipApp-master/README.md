# EmployeeMonthlyPayslipApp
Code Challenge

- *Details :* This application takes in Employee details, annualr salary, super rate and tax period information and calculates the monthly income of the employee along with the income tax, super etc. It is a designed in a layered architecture to demonstate the application of basic software design principles.
- *Libraries used :* 
 - Automapper
 - FluentCommandLineParser
 - Newtonson.Json

- *Usage :* 
 Download the source code and restore the nuget packages.
 Build the solution using VisualStudio / MSBuild.
 Run the executable from the built folder "EmployeeMonthlyPayslipApp.exe" in command prompt with appropriate arguments

This is a console application which take following arguments
```
 EmployeeMonthlyPayslipApp -f=Hariharan -l=S -a=80000 -s=9% -p="1 March - 31 March"
```
 or
```
EmployeeMonthlyPayslipApp -firstname=Hariharan -lastname=S -annualpay=80000 -super=9% -taxperiod="1 March - 31 March"
```
or *csv file input* (coming soon)
```
 EmployeeMonthlyPayslipApp -c=true -i=<csv file path> -o=<output directory>
```
