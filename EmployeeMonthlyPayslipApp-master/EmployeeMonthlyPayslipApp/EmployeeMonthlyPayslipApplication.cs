using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using EmployeeMonthlyPayslipApp.Interfaces;
using EmployeeMonthlyPayslipApp.Interfaces.TaxStructure;
using EmployeeMonthlyPayslipApp.Models;
using EmployeeMonthlyPayslipApp.Models.Models;
using EmployeeMonthlyPayslipApp.Models.Models.TaxStructure;
using EmployeeMonthlyPayslipInterfaces.TypeMaps;
using EmployeePayDetailsCommon;
using Fclp;
using Fclp.Internals.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace EmployeeMonthlyPayslipApp
{
    public class EmployeeMonthlyPayslipApplication
    {
        private ILogger _logger;
        private IMapper _mapper;
        private ITaxStructure _taxStructure;
        private CSVParameters _csvParameters;
        private EmployeeDetailsInput _employeeDetailsInput;
        private EmployeeMonthlyPayslipAppContext _employeeMonthlyPayslipAppContext;
        private CommandLineInputParameters _commandLineInputParameters;
        public EmployeeMonthlyPayslipAppContext EmployeeMonthlyPayslipAppContext => _employeeMonthlyPayslipAppContext;
        public EmployeeMonthlyPayslipApplication(string[] commandLineArgs)
        {
            SetupApplication(commandLineArgs);
        }

        private void SetupApplication(string[] commandLineArgs)
        {
            SetupCommonComponents();

            ICommandLineParserResult parseResult;
            var inputArguments = SetupFluentCommandLineParser(commandLineArgs, out parseResult);

            if (parseResult.HasErrors)
            {
                _employeeMonthlyPayslipAppContext.AddError(
                       new EmployeeMonthlyPayslipAppError("EmployeeMonthlyPayslipApplication:ExtractCommandLineParametersIntoObject",
                       new EmployeeMonthlyPayslipAppException("Error occured while parsing command line arguments", new Exception(parseResult.ErrorText)))
                   );
            }

            // parse command line parameters into strongly typed objects
            ExtractCommandLineParametersIntoObject(parseResult, inputArguments);

        }

        private void SetupCommonComponents()
        {
//set up logging
            _logger = LogSetup();
            _employeeMonthlyPayslipAppContext = new EmployeeMonthlyPayslipAppContext(_logger);
            // set up type mapper
            _mapper = InitializeTypeMapper();
            // load tax structure
            _taxStructure = LoadTaxStructure();
        }

        public IEnumerable<IPaySlipDetails> RunApplication()
        {
            IList < IPaySlipDetails > paySlips = new List<IPaySlipDetails>();
            if (_commandLineInputParameters.IsInputInCSVFormat)
            {
                IList<EmployeeDetailsInput> records = null;
                using (var reader = new StreamReader(_csvParameters.InputCSVFilePath))
                using (var csv = new CsvReader(reader))
                {

                    csv.Configuration.HasHeaderRecord = true;
                    records = csv.GetRecords<EmployeeDetailsInput>().ToList();
                }
                
                records.ForEach(input =>
                {
                    var paySlipDetail = GetPaySlipDetails(input);
                    if (paySlipDetail != null) paySlips.Add(paySlipDetail);
                });

                using (var writer = new StreamWriter(Path.Combine(_csvParameters.OutputCSVDirectory, "OutputCsv.csv")))
                using (var csvWriter = new CsvWriter(writer))
                {
                    //csvWriter.Configuration.RegisterClassMap<PaySlipDetailsMap>();
                    //csvWriter.WriteHeader<IPaySlipDetails>();
                    csvWriter.Configuration.AutoMap<PaySlipDetailsMap>();
                    csvWriter.WriteRecords(paySlips);
                };
                
                
                return paySlips;
            }
            var paySlip = GetPaySlipDetails();
            if (paySlip != null) paySlips.Add(paySlip);
            return paySlips;
        }

        private IPaySlipDetails GetPaySlipDetails(EmployeeDetailsInput employeeDetailsInput =null)
        {
            if (employeeDetailsInput != null)
            {
                _employeeDetailsInput = employeeDetailsInput;
            }
            _logger.Information(
                "Employee details input : FirstName: {0}, Last Name: {1}, Annual Salary: {2}, Super rate (%): {3}, Payment Period: {4}",
                _employeeDetailsInput.FirstName, _employeeDetailsInput.LastName, _employeeDetailsInput.AnnualSalary,
                _employeeDetailsInput.SuperPercentage, _employeeDetailsInput.TaxPeriod);
            var employeeDetails = _mapper.Map<EmployeeDetailsInput, IEmployeeDetails>(_employeeDetailsInput);

            var employeePayDetailsService =
                new EmployeePayDetailsService.EmployeePayDetailsService(_employeeMonthlyPayslipAppContext, employeeDetails,
                    _mapper);
            var paySlip = employeePayDetailsService.GetPaySlip(_taxStructure);
            if (_employeeMonthlyPayslipAppContext.HasErrors)
            {
                _employeeMonthlyPayslipAppContext.LogErrors();
                return paySlip;
            }

            _logger.Information(
                "Employee monthly Pay Slip : FullName: {0},Pay Period: {1}, Gross Income: {2}, Income Tax: {3}, Net Income: {4}, Super: {5}",
                paySlip.FullName, paySlip.TaxPeriod, paySlip.GrossIncome, paySlip.IncomeTax, paySlip.NetIncome,
                paySlip.Super);
            return paySlip;
        }

        private void ExtractCommandLineParametersIntoObject(ICommandLineParserResult parseResult,
            IFluentCommandLineParser<CommandLineInputParameters> inputArguments)
        {
            _commandLineInputParameters = inputArguments.Object;
            if (_commandLineInputParameters.IsInputInCSVFormat)
                _csvParameters = _mapper.Map<CSVParameters>(_commandLineInputParameters);
            _employeeDetailsInput = _mapper.Map<EmployeeDetailsInput>(_commandLineInputParameters);
        }

        private static ILogger LogSetup()
        {
            ILogger logger;
            try
            {
                logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.ColoredConsole(
                        outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
                    .CreateLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while setting up logger. Error Message {0}, Target Site {1}, Stack Trace {2}",ex.Message,ex.TargetSite.Name,ex.StackTrace);
                throw;
            }

            return logger;
        }

        private static FluentCommandLineParser<CommandLineInputParameters> SetupFluentCommandLineParser(string[] args,
            out ICommandLineParserResult parseResult)
        {
            var inputArguments = new FluentCommandLineParser<CommandLineInputParameters>();

            inputArguments.Setup(arg => arg.FirstName)
                .As('f', "firstname");
            inputArguments.Setup(arg => arg.LastName)
                .As('l', "lastname");
            inputArguments.Setup(arg => arg.AnnualSalary)
                .As('a', "annualsalary");
            inputArguments.Setup(arg => arg.SuperPercentage)
                .As('s', "super")
                .WithDescription("Enter super rate as a percentage for e.g. 9%");
            inputArguments.Setup(arg => arg.TaxPeriod)
                .As('p', "taxperiod")
                .WithDescription("Enter monthly tax period that should appear on the pay slip. ");

            inputArguments.Setup(arg => arg.IsInputInCSVFormat)
                .As('c',"isinputcsv")
                .WithDescription(
                    "If the input is a csv file set this to true, if the put is a command line input set this to false ");

            inputArguments.Setup(arg => arg.InputCSVFilePath)
                .As('i', "inputfile");

            inputArguments.Setup(arg => arg.OutputCSVDirectory)
                .As('o', "outputdirectory");

            inputArguments.SetupHelp("h", "help", "?")
                .Callback(() => Console.WriteLine());

            parseResult = inputArguments.Parse(args);
            return inputArguments;
        }

        private static IMapper InitializeTypeMapper()
        {
            return TypeMapper.InitilizeTypeConfiguration().InitializeMapper();
        }

        private static ITaxStructure LoadTaxStructure()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            const string taxJsonFileName = "TaxRate.json";
            var taxJsonFilePath = Path.Combine(path, taxJsonFileName);
            string fileContent;
            using (var reader = new StreamReader(taxJsonFilePath))
            {
                fileContent = reader.ReadToEnd().Trim();
            }
            var taxStructureRoot = JsonConvert.DeserializeObject<TaxStructureRoot>(fileContent,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            taxStructureRoot.taxStructure.TaxRate.TaxSlab.FirstOrDefault(x => x.MaxIncome == decimal.Zero).MaxIncome =
                decimal.MaxValue;
            return taxStructureRoot.taxStructure;
        }
    }
}