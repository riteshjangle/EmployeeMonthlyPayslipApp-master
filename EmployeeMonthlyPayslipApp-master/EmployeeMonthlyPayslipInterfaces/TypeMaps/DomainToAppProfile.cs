using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EmployeeMonthlyPayslipApp.Interfaces;
using EmployeeMonthlyPayslipApp.Models.Models;
using EmployeePayDetailsDomain.Interfaces;
using EmployeePayDetailsDomain.Models;

namespace EmployeePayDetailsCommon.TypeMaps
{
    public class DomainToAppProfile : Profile
    {
        public DomainToAppProfile()
        {
            // App to Domain
            CreateMap<IEmployeeDetails, EmployeeDetails>().ReverseMap();
            CreateMap<IEmployee, Employee>().ReverseMap();
            CreateMap<IPerson, Person>().ReverseMap();
            CreateMap<ISalary, Salary>().ReverseMap();

            CreateMap<IEmployeeDetails, ISalary>().ReverseMap();
            CreateMap<IEmployeeDetails, IEmployee>()
                .IncludeBase<IEmployeeDetails, IPerson>()
                //.ForMember(dest => dest.Salary, opt=> opt.MapFrom(src => Mapper.Map<IEmployeeDetails, ISalary>(src)))
                .ForMember(dest => dest.Salary,
                    opt => opt.MapFrom(src => new Salary {AnnualSalary = src.AnnualSalary, SuperRate = src.SuperRate}))
                .ForMember(dest => dest.SalarySlips, opt => opt.UseValue(new List<SalarySlip>()))
                .ReverseMap();

            // Domain to App
            CreateMap<ISalarySlip, SalarySlip>().ReverseMap();
            CreateMap<IPaySlipDetails, PaySlipDetails>().ReverseMap();
            CreateMap<ISalarySlip, IPaySlipDetails>()
                .ForMember(dest => dest.FullName, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<IEmployee, IPaySlipDetails>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.FirstName.Trim() + " " + src.LastName.Trim()))
                .ForMember(dest => dest.GrossIncome,
                    opt => opt.ResolveUsing(src => src.SalarySlips.FirstOrDefault().GrossIncome))
                .ForMember(dest => dest.IncomeTax,
                    opt => opt.ResolveUsing(src => src.SalarySlips.FirstOrDefault().IncomeTax))
                .ForMember(dest => dest.NetIncome,
                    opt => opt.ResolveUsing(src => src.SalarySlips.FirstOrDefault().NetIncome))
                .ForMember(dest => dest.Super, opt => opt.ResolveUsing(src => src.SalarySlips.FirstOrDefault().Super))
                .ForMember(dest => dest.TaxPeriod,
                    opt => opt.ResolveUsing(src => src.SalarySlips.FirstOrDefault().TaxPeriod))
                .ReverseMap()
                ;
        }
    }
}