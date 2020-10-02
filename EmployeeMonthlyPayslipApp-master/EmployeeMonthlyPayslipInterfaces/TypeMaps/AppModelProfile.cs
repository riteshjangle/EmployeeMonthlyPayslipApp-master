using System;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using EmployeeMonthlyPayslipApp.Interfaces;
using EmployeeMonthlyPayslipApp.Models;
using EmployeeMonthlyPayslipApp.Models.Models;

namespace EmployeeMonthlyPayslipInterfaces.TypeMaps
{
    public class AppModelProfile : Profile
    {
        public AppModelProfile()
        {
            CreateMap<IEmployeeDetails, EmployeeDetails>().ReverseMap();
            CreateMap<EmployeeDetailsInput, IEmployeeDetails>()
                .ForMember(dest => dest.SuperRate, opt => opt.ResolveUsing<SuperRateResolver>())
                .ReverseMap();

            CreateMap<CommandLineInputParameters, EmployeeDetailsInput>();
            CreateMap<CommandLineInputParameters, CSVParameters>();
        }
    }

    public class SuperRateResolver : IValueResolver<EmployeeDetailsInput, IEmployeeDetails, decimal>
    {
        private const string NUMBERWITHPERCENTAGEVALIDATIONREGEX = @"^\d+[.]?\d*%?$";

        public decimal Resolve(EmployeeDetailsInput source, IEmployeeDetails destination, decimal destMember,
            ResolutionContext context)
        {
            if (source.SuperPercentage == null)
            {
                Console.WriteLine("Super Percentage is null. Setting Super percentage to zero.");
                return decimal.Zero;
            }
            if (!Regex.IsMatch(source.SuperPercentage, NUMBERWITHPERCENTAGEVALIDATIONREGEX))
            {
                Console.WriteLine(
                    "SuperPercentage {0} supplied is not in valid format. Setting Super percentage to zero.",
                    source.SuperPercentage);
                return decimal.Zero;
            }
            decimal super;
            var superPercentageNumber = Regex.Split(source.SuperPercentage, "%").FirstOrDefault(x => !x.Contains("%"));
            if (!decimal.TryParse(superPercentageNumber, out super))
            {
                Console.WriteLine(
                    "SuperPercentage {0} supplied is not in valid format. It cannot be parsed into a valid decimal number. Setting Super percentage to zero.",
                    source.SuperPercentage);
                return decimal.Zero;
            }

            return super/100;
        }
    }
}